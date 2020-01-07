//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Helpers;
using Meti.Domain.Models;
using Meti.Domain.Repository;
using Meti.Domain.ValueObjects;
using Meti.Infrastructure.Repository;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Maintenance.Database
{
    public class SampleDataGenerator
    {
        private readonly ISession _session;
        private readonly IProcessInstanceRepository _processInstanceRepository;
        private readonly IAlarmFiredRepository _alarmFiredRepository;

        public SampleDataGenerator(ISession session)
        {
            _session = session;
            _processInstanceRepository = new ProcessInstanceRepository(_session);
            _alarmFiredRepository = new AlarmFiredRepository(_session);
        }

        public void GenerateAlarmFired()
        {
            using (var transaction = _session.BeginTransaction())
            {
                var processInstances = _processInstanceRepository.Fetch(null, null);

                Random random = new Random();
                var numbersOfProcessInstance = random.Next(1, processInstances.Count());
                var numbersOfAlarms = random.Next(5, 10);

                for (var j = 0; j < numbersOfProcessInstance; j++)
                {
                    var processInstance = processInstances[j];

                    for (var i = 0; i < numbersOfAlarms; i++)
                    {
                        AlarmFired entity = new AlarmFired();
                        entity.AlarmColor = (AlarmColor)random.Next(1, 3);
                        entity.IsActive = true;
                        entity.Parameter = processInstance.Process.Parameters[random.Next(0, processInstance.Process.Parameters.Count)];
                        entity.Result = random.Next(50, 250).ToString();
                        entity.ProcessInstance = processInstance;

                        _alarmFiredRepository.Save(entity);
                    }
                }
                transaction.Commit();

            }
        }
    }
}
