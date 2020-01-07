//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Helpers.NHibernate;
using MateSharp.RoleClaim.Domain.Entities;
using MateSharp.RoleClaim.Domain.Services;
using Meti.Infrastructure.Security;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Maintenance.Database
{
    public class RoleClaimGenerator
    {
        public RoleClaimGenerator()
        {
        }

        private IList<Role> BuildRoles(IList<Claim> claims)
        {
            IList<Role> roles = new List<Role>();

            Role role1 = new Role();
            role1.Name = "Medico";
            role1.Description = "";
            role1.Claims = claims.Where(e=>e.Name == "IsMedico").ToList();

            Role role2 = new Role();
            role2.Name = "Centrale Operativa";
            role2.Description = "";
            role2.Claims = claims.Where(e => e.Name == "IsCentraleOperativa").ToList();

            Role role3 = new Role();
            role3.Name = "Configuratore";
            role3.Description = "";
            role3.Claims = claims.Where(e => e.Name == "IsConfiguratore").ToList();

            Role role4 = new Role();
            role4.Name = "Admin";
            role4.Description = "";
            role4.Claims = claims.Where(e => e.Name == "IsConfiguratore" ||
                e.Name == "IsCentraleOperativa" ||
                e.Name == "IsMedico")
                .ToList();

            Role role5 = new Role();
            role5.Name = "Cliente";
            role5.Description = "";

            roles.Add(role1);
            roles.Add(role2);
            roles.Add(role3);
            roles.Add(role4);
            roles.Add(role5);

            return roles;
        }

        private IList<Claim> BuildClaims()
        {
            IList<Claim> claims = new List<Claim>();

            Claim claim1 = new Claim();
            claim1.Name = "IsMedico";
            claim1.Description = "";

            Claim claim2 = new Claim();
            claim2.Name = "IsCentraleOperativa";
            claim2.Description = "";

            Claim claim3 = new Claim();
            claim3.Name = "IsConfiguratore";
            claim3.Description = "";

            claims.Add(claim1);
            claims.Add(claim2);
            claims.Add(claim3);

            using (var session = NHibernateHelper.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var claim in claims)
                    {
                        session.SaveOrUpdate(claim);
                    }

                    transaction.Commit();
                }
            }

            return claims;
        }

        private IList<User> BuildUsers(IList<Role> roles)
        {
            IList<User> users = new List<User>();

            User user1 = new User();
            user1.Firstname = "Igcom";
            user1.Surname = "Igcom";
            user1.UserName = "Igcom";
            user1.Email = "marco.broccio@igcom.it";
            user1.Password = EncryptPassword.EncryptWithSha256("Igcom2019");
            user1.AccessFailedCount =0;
            user1.Roles = roles.Where(e => e.Name == "Admin").ToList();

            //User user2 = new User();
            //user2.Firstname = "Smartme";
            //user2.Surname = "Smartme";
            //user2.UserName = "Smartme";
            //user2.Email = "carmelo@smartme.io";
            //user2.Password = EncryptPassword.EncryptWithSha256("Smartme2019");
            //user2.AccessFailedCount = 0;
            //user2.Roles = roles.Where(e => e.Name == "Admin").ToList();

            User user3 = new User();
            user3.Firstname = "Admin";
            user3.Surname = "Admin";
            user3.UserName = "Admin";
            user3.Email = "leonardobertini2008@gmail.com";
            user3.Password = EncryptPassword.EncryptWithSha256("Leonardo2019");
            user3.AccessFailedCount = 0;
            user3.Roles = roles.Where(e => e.Name == "Admin").ToList(); 

            User user4 = new User();
            user4.Firstname = "Profilo centrale operativa";
            user4.Surname = "Profilo centrale operativa";
            user4.UserName = "ProfiloCentraleOperativa";
            user4.Email = "leonardobertini2008@gmail.com";
            user4.Password = EncryptPassword.EncryptWithSha256("Igcom2019");
            user4.AccessFailedCount = 0;
            user4.Roles = roles.Where(e=>e.Name == "Centrale Operativa").ToList();

            User user5 = new User();
            user5.Firstname = "Profilo Medico";
            user5.Surname = "Profilo Medico";
            user5.UserName = "ProfiloMedico";
            user5.Email = "leonardobertini2008@gmail.com";
            user5.Password = EncryptPassword.EncryptWithSha256("Medico2019");
            user5.AccessFailedCount = 0;
            user5.Roles = roles.Where(e => e.Name == "Medico").ToList();

            //User user6 = new User();
            //user6.Firstname = "Profilo Cliente";
            //user6.Surname = "Profilo Cliente";
            //user6.UserName = "ProfiloCliente";
            //user6.Email = "leonardobertini2008@gmail.com";
            //user6.Password = EncryptPassword.EncryptWithSha256("Cliente2019");
            //user6.AccessFailedCount = 0;
            //user6.Roles = roles.Where(e => e.Name == "Profilo Cliente").ToList();

            //users.Add(user1);
            //users.Add(user2);
            users.Add(user3);
            //users.Add(user4);
            //users.Add(user5);
            //users.Add(user6);

            return users;
        }

        public void GenerateUserRoleAndClaim()
        {
            IList<Claim> claims = new List<Claim>();
            IList<Role> roles = new List<Role>();
            IList<User> users = new List<User>();

            try
            {
                claims = BuildClaims();
                roles = BuildRoles(claims);
                users = BuildUsers(roles);
            }
            catch (Exception ex)
            {
                //Continuo anche se ci sono errori
            }

            using (var session = NHibernateHelper.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var role in roles)
                    {
                        session.SaveOrUpdate(role);
                    }

                    transaction.Commit();
                }
            }

            using (var session = NHibernateHelper.SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var user in users)
                    {
                        session.SaveOrUpdate(user);
                    }

                    transaction.Commit();
                }
            }

        }
    }


}
