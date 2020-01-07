//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using System.Linq;
using Meti.Application.Dtos.AlarmFired;
using Meti.Application.Services;
using Meti.Domain.Models;
using Meti.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Meti.Tests
{
    [TestClass]
    public class AlarmFiredTest : TestBase
    {
        [TestMethod]
        public void TestThatResolvingNotesWasPopulated()
        {
            AlarmFiredEditDto dto = new AlarmFiredEditDto();
            dto.AlarmColor = Domain.ValueObjects.AlarmColor.Red;
            dto.AlarmId = _session.QueryOver<Alarm>().List().First().Id;
            dto.Fall = false;
            dto.FallNoResponse = false;
            dto.ParameterId = _session.QueryOver<Parameter>().List().First().Id;
            dto.ProcessInstanceId = _session.QueryOver<ProcessInstance>().List().First().Id;
            dto.ResolvingNotes = "test";
            dto.Result = "Test risultato";

            IAlarmFiredService service = new AlarmFiredService(_session);
            var oresult = service.CreateAlarmFired(dto);

        }
    }
}
