//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using Igcom.Sms.Enums;
using Igcom.Sms.Helpers;
using Igcom.Sms.Models;
using MateSharp.Framework.Helpers;
using Meti.App.Filters;
using Meti.Infrastructure.Configurations;
using Meti.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Meti.App.Controllers
{
    [Authorize]
    [CatchLogException]
    public class NotifyController : ApiController
    {
        #region Private Fields

        private readonly bool _isSmsServiceActive = Convert.ToBoolean(ConfigurationManager.AppSettings["isSmsServiceActive"]);
        //private readonly string _smsUser = ConfigurationManager.AppSettings["smsUser"];
        //private readonly string _smsPassword = ConfigurationManager.AppSettings["smsPassword"];
        private readonly string _smsTotalConnectUrl = ConfigurationManager.AppSettings["SmsTotalConnectUrl"];
        private readonly string _smsTotalConnectLogin = ConfigurationManager.AppSettings["SmsTotalConnectLogin"];
        private readonly string _smsTotalConnectPassword = ConfigurationManager.AppSettings["SmsTotalConnectPassword"];
        private readonly string _smsTotalConnectRoute = ConfigurationManager.AppSettings["SmsTotalConnectRoute"];
        private readonly string _smsTotalConnectFrom = ConfigurationManager.AppSettings["SmsTotalConnectFrom"];
        private readonly string _centraleOperativaPhoneNumber = ConfigurationManager.AppSettings["centraleOperativaPhoneNumber"];
        

        #endregion Private Fields

        #region Costructors

        public NotifyController()
        {
        }

        #endregion Costructors

        #region Api

        [HttpGet]
        public IHttpActionResult SendSms(string phoneNumber, string message)
        {
            if (!_isSmsServiceActive)
            {
                return BadRequest("Servizio SMS disabilitato.");
            }

            try
            {
                message = message + " Centrale Operativa: " + _centraleOperativaPhoneNumber;

                var baseAddress = _smsTotalConnectUrl;
                var endpoint = string.Format("?username={0}&password={1}&route={2}&from={3}&to={4}&message={5}",
                    _smsTotalConnectLogin,
                    _smsTotalConnectPassword,
                    _smsTotalConnectRoute,
                    _smsTotalConnectFrom,
                    phoneNumber,
                    message);
                
                var response = HttpClientHelper.Get(baseAddress, endpoint, true).Result;

                if (!response.IsStatusSuccessCode)
                {
                    Log4NetConfig.ApplicationLog.Error(string.Format("Errore durante l'invio dell'sms. Status http response: {0}. Messaggio: {1}",
                        response.HttpStatusCode,
                        response.Response));

                    return BadRequest("Invio SMS fallito");
                }
                
            }
            catch (Exception ex)
            {

                Log4NetConfig.ApplicationLog.Error(string.Format("Errore durante l'invio dell'sms {0}",
                    ex.Message));
            }
            
            //ritorno il contesto di validazione
            return Ok();
        }

        [HttpGet]
        public HttpResponseMessage SendEmail(string from, [FromUri]IList<string> to, [FromUri]IList<string> ccn , [FromUri]IList<string> cc , string subject , string body)
        {
            from = ConfigurationManager.AppSettings["Email_From"];

            Log4NetConfig.ApplicationLog.Debug(string.Format("Parametri email. from: {0}, to: {1}, ccn: {2}, cc: {3}, subject: {4}, body: {5}",
                from,
                to[0],
                ccn[0],
                cc[0],
                subject,
                body));

            try
            {
                EmailHelper.SendMail(from, to, ccn, cc, subject, body, null);
            }
            catch (Exception ex)
            {

                Log4NetConfig.ApplicationLog.Error(string.Format("Errore durante l'invio delle email {0}",
                    ex.Message));
            }
            

            //ritorno il contesto di validazione
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        #endregion Api

        #region Dispose Pattern

        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                //Rilascio le risorse locali
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}