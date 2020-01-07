//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Extensions.NHibernate;
using MateSharp.Framework.Helpers;
using MateSharp.Framework.Models;
using MateSharp.RoleClaim.Domain.Contracts.Repository;
using MateSharp.RoleClaim.Domain.Dtos;
using MateSharp.RoleClaim.Domain.Entities;
using MateSharp.RoleClaim.Repository.Relational.NHibernate;
using Meti.Domain.Services;
using Meti.Infrastructure.Emails;
using Meti.Infrastructure.Security;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;

namespace Meti.Application.Services
{
    public class AccountService : ServiceNHibernateBase, IServiceNHibernateBase, IAccountService
    {
        #region Private fields

        private readonly IUserRepository _userRepository;
        private const int passwordGeneratorLength = 8;
        private readonly string appUrl = ConfigurationManager.AppSettings["AppUrl"].ToString();

        #endregion Private fields

        #region Costructors

        public AccountService(ISession session)
            : base(session)
        {
            _userRepository = new UserRepository(session);
        }

        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion Costructors

        public OperationResult<string> ChangePassword(string username, string newPassword, string oldPassword)
        {
            throw new NotImplementedException();
        }

        public void IncreaseAccessCountFail(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return;

            using (var transaction = Session.ExecuteInTransaction())
            {
                User user = Session.QueryOver<User>()
                    .WhereRestrictionOn(e => e.UserName).IsInsensitiveLike(username)
                    .SingleOrDefault();

                if (user == null) return;
                user.AccessFailedCount = user.AccessFailedCount + 1;

                var vResults = ValidateEntity(user);

                if (!vResults.Any())
                {
                    _userRepository.Save(user);
                    transaction.ExecuteCommit();
                }
                else
                {
                    transaction.ExecuteRollback();
                }
            }
        }

        public OperationResult<string> Login(string username, string password)
        {
            //Validazione degli argomenti
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException("username is null");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password is null");
            }

            //Definisco i risultati di ritorno
            OperationResult<string> oResults = new OperationResult<string>();

            //Eseguo in transazione
            using (var transaction = Session.ExecuteInTransaction())
            {
                //Recupero l'utente
                User user = Session.QueryOver<User>()
                    .WhereRestrictionOn(e => e.UserName).IsInsensitiveLike(username)
                    .Where(e => e.Password == EncryptPassword.EncryptWithSha256(password))
                    .SingleOrDefault();

                //Check su username
                if (user == null)
                {
                    var vResults = new List<ValidationResult> { new ValidationResult("Utente non trovato.") };
                    oResults.ConcatValidationResults(vResults);
                    oResults.ReturnedValue = "Utente o password invalidi";
                    IncreaseAccessCountFail(username);
                    transaction.ExecuteCommit();
                    return oResults;
                }

                //Aggiorno i campi utente
                user.AccessFailedCount = 0;

                //Eseguo la validazione logica
                var userValidation = ValidateEntity(user);
                oResults.ConcatValidationResults(userValidation);

                //Valuto il salvataggio
                if (!oResults.HasErrors())
                {
                    _userRepository.Save(user);
                    transaction.ExecuteCommit();
                }
                else
                {
                    transaction.ExecuteRollback();
                }
            }
            return oResults;
        }

        public OperationResult<object> Register(UserDto userDto, IList<Role> roles)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            using (var transaction = Session.ExecuteInTransaction())
            {
                var emailExist = _userRepository.Count(e => e.Email == userDto.Email) > 0;
                if (emailExist) {
                    OperationResult<object> oResult = new OperationResult<object>();
                    oResult.ValidationResults.Add(new ValidationResult(string.Format("L'email è già presente nel sistema: {0}", userDto.Email)));
                    return oResult;
                }

                string passwordTemp = PasswordGenerator.GenerateRandom(passwordGeneratorLength);
                User newUser = new User
                {
                    Firstname = userDto.Firstname,
                    Surname = userDto.Surname,
                    Email = userDto.Email,
                    AccessFailedCount = 0,
                    ImgFilePath = userDto.ImgFilePath,
                    UserName = userDto.Username,
                    Password = EncryptPassword.EncryptWithSha256(passwordTemp),
                    Roles = roles
                };

                OperationResult<object> oResults = new OperationResult<object>();

                //Eseguo la validazione logica
                oResults.ConcatValidationResults(ValidateEntity(newUser));

                //valuto il salvataggio
                if (!oResults.HasErrors())
                {
                    _userRepository.Save(newUser);

                    transaction.ExecuteCommit();
                    if (!string.IsNullOrWhiteSpace(newUser.Email))
                        NoticeRegisterViaEmail(newUser, passwordTemp);
                }
                else
                {
                    transaction.ExecuteRollback();
                }
                return oResults;
            }
        }

        public OperationResult<object> UpdateAccount(UserDto userDto, IList<Role> roles)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            using (var transaction = Session.ExecuteInTransaction())
            {
                User newUser = _userRepository.Load(userDto.Id);

                if (userDto.Email != newUser.Email)
                {
                    var emailExist = _userRepository.Count(e => e.Email == userDto.Email) > 0;
                    if (emailExist)
                    {
                        OperationResult<object> oResult = new OperationResult<object>();
                        oResult.ValidationResults.Add(new ValidationResult(string.Format("L'email è già presente nel sistema: {0}", userDto.Email)));
                        return oResult;
                    }
                }

                newUser.Firstname = userDto.Firstname;
                newUser.Surname = userDto.Surname;
                newUser.Email = userDto.Email;
                newUser.ImgFilePath = userDto.ImgFilePath;
                newUser.UserName = userDto.Username;

                OperationResult<object> oResults = new OperationResult<object>();

                //Eseguo la validazione logica
                oResults.ConcatValidationResults(ValidateEntity(newUser));

                //valuto il salvataggio
                if (!oResults.HasErrors())
                {
                    _userRepository.Save(newUser);

                    transaction.ExecuteCommit();
                }
                else
                {
                    transaction.ExecuteRollback();
                }
                return oResults;
            }
        }

        public OperationResult<object> ResetPassword(string username)
        {
            //Validazione degli argomenti
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException("Username is null");
            }

            //Definisco i risultati di ritorno
            OperationResult<object> oResults = new OperationResult<object>();

            //Eseguo in transazione
            using (var transaction = Session.ExecuteInTransaction())
            {
                //Recupero l'utente
                User user = Session.QueryOver<User>()
                                 .WhereRestrictionOn(e => e.UserName).IsInsensitiveLike(username)
                                 .SingleOrDefault();

                if (user == null)
                    throw new ArgumentNullException("User undefined");

                //Eseguo la generazione di una password temporanea
                string passwordTemp = PasswordGenerator.GenerateRandom(passwordGeneratorLength);
                user.Password = EncryptPassword.EncryptWithSha256(passwordTemp);

                //Eseguo la validazione logica dell'entità
                oResults.ValidationResults = ValidateEntity(user);

                //Valuto il salvataggio
                if (!oResults.HasErrors())
                {
                    _userRepository.Save(user);
                    transaction.ExecuteCommit();

                    oResults.ReturnedValue = passwordTemp;
                    try
                    {
                        NoticeRegisterViaEmail(user, passwordTemp);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    transaction.ExecuteRollback();
                }
            }

            //Ritorno i risultati
            return oResults;
        }

        private void NoticeRegisterViaEmail(User user, string passwordTemp)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Email))
                return;
            string mailFrom = ConfigurationManager.AppSettings["Email_From"];
            string mailSubject = ConfigurationManager.AppSettings["Email_Register_Subject"];
            mailSubject = "Semprevicini - Nuova password di accesso";
            IList<string> tos = new List<string> { user.Email };
            string emailsBody = new RazorCredentialTemplate(user.Firstname, user.Surname, user.UserName, passwordTemp, appUrl).Result;
            EmailHelper.SendMail(mailFrom, tos, null, null, mailSubject, emailsBody, null);
        }
    }
}