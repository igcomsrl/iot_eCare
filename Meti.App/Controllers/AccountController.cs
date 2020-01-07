//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Helpers;
using MateSharp.RoleClaim.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Meti.App.Controllers
{
    [Authorize]
    public class AccountController : ApiController
    {
        #region Private Fields

        private readonly IAuthorizeService _authorizeService;

        #endregion Private Fields

        #region Costructors
        
        public AccountController(IAuthorizeService authorizeService)
        {
            _authorizeService = authorizeService;
        }

        #endregion Costructors

        #region Api

        [HttpGet]
        public HttpResponseMessage GetProfile()
        {
            //Verifico se l'utente che esegue la richiesta è autorizzato
            bool isAuth = IdentityHelper.IsAuthenticated();

            //Se il client non è autorizzato, lo notifico
            if (!isAuth)
                Request.CreateResponse(HttpStatusCode.Unauthorized);

            string username = IdentityHelper.GetUsername();

            IList<Claim> claims = IdentityHelper.FetchClaims().ToList();

            //Compongo il dto in uscita
            object dto = new
            {
                IsAuth = isAuth,
                Claims = claims,
                // ImgProfilePath = !string.IsNullOrWhiteSpace(picturePath) ? Url.Content(picturePath) : string.Empty,
                ImgProfilePath = String.Empty,
                Username = username,
                Email = IdentityHelper.GetEmail()

            };

            //ritorno il contesto di validazione
            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }

        #endregion Api

        #region Dispose Pattern

        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                //Rilascio le risorse locali
                _authorizeService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}