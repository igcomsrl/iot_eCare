//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Helpers.NHibernate;
using Meti.Maintenance.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Maintenance
{
    class Program
    {

        private static string GetConnectionInfo()
        {
            string connectionStringName = NHibernateHelper.Configuration.GetProperty(NHibernate.Cfg.Environment.ConnectionStringName);
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            //Init NHibernate

            NHibernateHelper.InitNHibernate();
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

            while (true)
            {
                string connectionInfo = GetConnectionInfo();

                ConsoleColorHelper.WritelineWithColor("Meti Mantainance Console\n", ConsoleColor.Green);
                ConsoleColorHelper.WritelineWithColor(string.Format("Connection string: {0}\n", connectionInfo), ConsoleColor.Yellow);
                //Console.WriteLine(string.Format("1 - {0} \n2 - {1} \n3 - {2} \n4 - {3} \n5 - {4} \n",
                //    "Pulisci console",
                //    "Genera schema del database",
                //    "Aggiorna lo schema del database",
                //    "Cancella lo schema del database",
                //    //"Genera un numero casuale di allarmi di test",
                //    "Genera utenti, ruoli e claims di base"));
                Console.WriteLine(string.Format("2 - {0} \n5 - {1}",
                    "Genera schema del database",
                    "Genera utenti, ruoli e claims di base"));
                var operation = Console.ReadLine();
                switch (operation)
                {
                    case "1":
                        //Console.Clear();
                        //break;
                    case "2":
                        NHibernateHelper.GenerateSchema();
                        break;
                    case "3":
                        //NHibernateHelper.UpdateSchema();
                        //break;
                    case "4":
                       // NHibernateHelper.DropSchema();
                        break;
                  //  case "5":
                        //SampleDataGenerator generator = new SampleDataGenerator(NHibernateHelper.SessionFactory.OpenSession());
                        //generator.GenerateAlarmFired();
                        break;
                    case "5":
                        RoleClaimGenerator roleClaimGenerator = new RoleClaimGenerator();
                        roleClaimGenerator.GenerateUserRoleAndClaim();
                        break;
                    //case "7":
                    //    ProcessInstanceQuery processInstanceQuery = new ProcessInstanceQuery();
                    //    processInstanceQuery.SyncProcessWithProcessInstance();
                    //    break;

                }
            }
        }
    }
}
