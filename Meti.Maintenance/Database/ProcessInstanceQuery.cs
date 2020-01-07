//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Helpers.NHibernate;
using Meti.Domain.Models;
using Meti.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Maintenance.Database
{
    public class ProcessInstanceQuery
    {
        public void SyncProcessWithProcessInstance()
        {
            using (var session = NHibernateHelper.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    //Recupero i processi da travasare
                    var processList = session.QueryOver<Process>().Where(e=>e.ProcessType == ProcessType.Template).List(); 
                    var processInstanceList = session.QueryOver<ProcessInstance>().List();

                    foreach (var process in processList)
                    {
                        foreach (var processInstance in processInstanceList)
                        {
                            if (processInstance.Process.Name == process.Name)
                            {
                                foreach (var parameter in process.Parameters)
                                {
                                    foreach (var alarm in parameter.Alarms)
                                    {
                                        foreach (var parameterInstance in processInstance.Process.Parameters)
                                        {
                                            var alarmsIstance = parameterInstance.Alarms;
                                            foreach (var alarmInstance in alarmsIstance)
                                            {
                                                if (alarmInstance.Name == alarm.Name && alarm.Name != null)
                                                {
                                                    alarmInstance.HelpMessage = alarm.HelpMessage;
                                                    session.SaveOrUpdate(alarmInstance);
                                                }
                                                
                                            }
                                        }
                                        
                                    }
                                    
                                }
                            }
                        }
                    }

                    
                    transaction.Commit();
                }
            }
        }
    }
}
