//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System.Web.Http;

namespace Meti.App.Controllers
{
    [Authorize]
    public class ConfigurationController : ApiController
    {
        #region Costructors

        public ConfigurationController()
        {
        }

        #endregion Costructors

        #region Api

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok();
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