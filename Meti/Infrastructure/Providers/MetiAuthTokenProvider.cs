//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Helpers.NHibernate;
using MateSharp.RoleClaim.Domain.Contracts.Repository;
using MateSharp.RoleClaim.Domain.Entities;
using MateSharp.RoleClaim.Repository.Relational.NHibernate;
using Meti.Application.Services;
using Meti.Domain.Services;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Infrastructure.Providers
{
    public class MetiAuthTokenProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// Called to validate that the origin of the request is a registered "client_id", and that the correct credentials for that client are
        /// present on the request. If the web application accepts Basic authentication credentials,
        /// context.TryGetBasicCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request header. If the web
        /// application accepts "client_id" and "client_secret" as form encoded POST parameters,
        /// context.TryGetFormCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request body.
        /// If context.Validated is not called the request will not proceed further.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //Valido e proseguo
            context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Called when a request to the Token endpoint arrives with a "grant_type" of "password". This occurs when the user has provided name and password
        /// credentials directly into the client application's user interface, and the client application is using those to acquire an "access_token" and
        /// optional "refresh_token". If the web application supports the
        /// resource owner credentials grant type it must validate the context.Username and context.Password as appropriate. To issue an
        /// access token the context.Validated must be called with a new ticket containing the claims about the resource owner which should be associated
        /// with the access token. The application should take appropriate measures to ensure that the endpoint isn’t abused by malicious callers.
        /// The default behavior is to reject this grant type.
        /// See also http://tools.ietf.org/html/rfc6749#section-4.3.2
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            IList<MateSharp.RoleClaim.Domain.Entities.Claim> userClaims = null;
            User user = null;

            var session = NHibernateHelper.SessionFactory.GetCurrentSession();

            //Verifico le logiche di business di autenticazione
            using (IAccountService accountService = new AccountService(session))
            {
                var oResults = accountService.Login(context.UserName, context.Password);

                if (oResults.HasErrors())
                {
                    context.SetError("invalid_login", Newtonsoft.Json.JsonConvert.SerializeObject(
                        new
                        {
                            errorMessages = oResults.GetValidationErrors(),
                            loginStatus = oResults.ReturnedValue
                        }));
                    return Task.FromResult<object>(null);
                }

                //Recupero le informazioni riguardo l'utente
                using (IClaimRepository claimRepository = new ClaimRepository(session))
                {
                    userClaims = claimRepository.FetchUserClaims(context.UserName);
                }

                user = session.QueryOver<User>().WhereRestrictionOn(e => e.UserName).IsInsensitiveLike(context.UserName).SingleOrDefault();
            }

            //Costruisco l'identità dell'utente
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new System.Security.Claims.Claim("LoggedTime", new DateTime().ToString("dd-MM-yyyy mm:hh")));
            identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new System.Security.Claims.Claim("Email", user.Email));
            identity.AddClaim(new System.Security.Claims.Claim("Surname", user.Surname));
            identity.AddClaim(new System.Security.Claims.Claim("Firstname", user.Firstname));
            if (userClaims != null && userClaims.Count > 0)
                foreach (var claim in userClaims)
                    identity.AddClaim(new System.Security.Claims.Claim(claim.Description, claim.Name));//ValueType = description Value= name

            //Parametri di authenticazione di ritorno
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { "as:client_id", context.ClientId ?? string.Empty },
                    { "userName", context.UserName },
                    { "userId", user.Id.ToString() },
                    { "email", user.Email  },
                });

            //Compongo il ticket
            var ticket = new AuthenticationTicket(identity, props);

            //Valido il ticket con il motore di oauth
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Called when a request to the Token endpoint arrives with a "grant_type" of "refresh_token". This occurs if your application has issued a "refresh_token"
        /// along with the "access_token", and the client is attempting to use the "refresh_token" to acquire a new "access_token", and possibly a new "refresh_token".
        /// To issue a refresh token the an Options.RefreshTokenProvider must be assigned to create the value which is returned. The claims and properties
        /// associated with the refresh token are present in the context.Ticket. The application must call context.Validated to instruct the
        /// Authorization Server middleware to issue an access token based on those claims and properties. The call to context.Validated may
        /// be given a different AuthenticationTicket or ClaimsIdentity in order to control which information flows from the refresh token to
        /// the access token. The default behavior when using the OAuthAuthorizationServerProvider is to flow information from the refresh token to
        /// the access token unmodified.
        /// See also http://tools.ietf.org/html/rfc6749#section-6
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Il refresh token è stato rilasciato ad un clientId diverso.");
                return Task.FromResult<object>(null);
            }

            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Called at the final stage of a successful Token endpoint request. An application may implement this call in order to do any final
        /// modification of the claims being used to issue access or refresh tokens. This call may also be used in order to add additional
        /// response parameters to the Token endpoint's json response body.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            var claims = context.Identity.Claims;

            if (claims.Count(e => e.Value == "ManageAreaPaziente") > 0)
                context.Properties.ExpiresUtc = DateTime.UtcNow.AddYears(1);

            return base.TokenEndpoint(context);
        }

        /// <summary>
        /// Called to determine if an incoming request is treated as an Authorize or Token
        /// endpoint. If Options.AuthorizeEndpointPath or Options.TokenEndpointPath
        /// are assigned values, then handling this event is optional and context.IsAuthorizeEndpoint and context.IsTokenEndpoint
        /// will already be true if the request path matches.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.IsTokenEndpoint && context.Request.Method == "OPTIONS")
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "authorization" });
                context.RequestCompleted();
                return Task.FromResult(0);
            }

            return base.MatchEndpoint(context);
        }
    }
}
