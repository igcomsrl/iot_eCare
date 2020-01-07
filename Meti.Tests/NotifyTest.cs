//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using Meti.Infrastructure.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace Meti.Tests
{
    [TestClass]
    public class NotifyTest
    {
        private readonly string _smsTotalConnectUrl = ConfigurationManager.AppSettings["SmsTotalConnectUrl"];
        private readonly string _smsTotalConnectLogin = ConfigurationManager.AppSettings["SmsTotalConnectLogin"];
        private readonly string _smsTotalConnectPassword = ConfigurationManager.AppSettings["SmsTotalConnectPassword"];
        private readonly string _smsTotalConnectRoute = ConfigurationManager.AppSettings["SmsTotalConnectRoute"];
        private readonly string _smsTotalConnectFrom = ConfigurationManager.AppSettings["SmsTotalConnectFrom"];

        [TestMethod]
        public void TestThatSmsIsInviated()
        {
            var message = "Ciao Marco, sto funzionando.";
            var phoneNumber = "3339602189";

            var baseAddress = _smsTotalConnectUrl;
            var endpoint = string.Format("?username={0}&password={1}&route{2}&from={3}&to={4}&message={5}",
                _smsTotalConnectLogin,
                _smsTotalConnectPassword,
                _smsTotalConnectRoute,
                _smsTotalConnectFrom,
                phoneNumber,
                message);

            var response = HttpClientHelper.Get(baseAddress, endpoint, true).Result;

            if (!response.IsStatusSuccessCode)
            {
                Assert.Fail();
            }
            else
            {
                Assert.IsTrue(true);
            }
        }
    }
}