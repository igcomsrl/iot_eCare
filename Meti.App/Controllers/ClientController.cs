//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System.Configuration;
using System.Web.Mvc;

namespace Meti.App.Controllers
{
    public class ClientController : Controller
    {
        public ActionResult Dashboard()
        {
            ViewData["apiEndPoint"] = Url.Content("~");
            ViewData["zeusEndpoint"] = Url.Content("~");            
            ViewData["isDebug"] = ConfigurationManager.AppSettings["isDebug"];
            ViewData["alarmFiredPooling"] = ConfigurationManager.AppSettings["alarmFiredPooling"];
            ViewData["alarmFiredSoundServerPath"] = ConfigurationManager.AppSettings["alarmFiredSoundServerPath"];
            ViewData["enableAlarmFiredSound"] = ConfigurationManager.AppSettings["enableAlarmFiredSound"];
            ViewData["noderedConsoleUrl"] = ConfigurationManager.AppSettings["noderedConsoleUrl"];
            ViewData["grafanaConsoleUrl"] = ConfigurationManager.AppSettings["grafanaConsoleUrl"];
            ViewData["grafanaConsoleRefreshTime"] = ConfigurationManager.AppSettings["grafanaConsoleRefreshTime"];
            ViewData["artificialIntelligenceUrl"] = ConfigurationManager.AppSettings["artificialIntelligenceUrl"];
            ViewData["softwareVersion"] = ConfigurationManager.AppSettings["softwareVersion"];
            
            return View();
        }

        //[Route("Ionic/www")]
        //public ActionResult Ionic()
        //{
        //    return View();
        //}
    }
}