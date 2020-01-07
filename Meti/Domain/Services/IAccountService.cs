//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Interfaces;
using MateSharp.Framework.Models;
using MateSharp.RoleClaim.Domain.Dtos;
using MateSharp.RoleClaim.Domain.Entities;
using Meti.Application.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Domain.Services
{
    public interface IAccountService : IServiceNHibernateBase
    {
        /// <summary>
        /// Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>IList&lt;OperationResult&lt;System.Object&gt;&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        OperationResult<string> Login(string username, string password);

        /// <summary>
        /// Registers the specified user dto.
        /// </summary>
        /// <param name="userDto">The user dto.</param>
        /// <returns>IList&lt;OperationResult&lt;System.Object&gt;&gt;.</returns>
        OperationResult<object> Register(UserDto userDto, IList<Role> roles);

        ///// <summary>
        ///// Invita un amico a condividere informazioni sui parametri vitali
        ///// </summary>
        ///// <param name="dto"></param>
        ///// <returns></returns>
        //OperationResult<object> InviteFriend(InviteFriendDto dto);

        OperationResult<object> UpdateAccount(UserDto userDto, IList<Role> roles);

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <returns>IList&lt;OperationResult&lt;System.Object&gt;&gt;.</returns>
        OperationResult<string> ChangePassword(string username, string newPassword, string oldPassword);

        /// <summary>
        /// Resets the passowrd.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>IList&lt;OperationResult&lt;System.Object&gt;&gt;.</returns>
        OperationResult<object> ResetPassword(string username);

        /// <summary>
        /// Increases the access count fail.
        /// </summary>
        /// <returns>IList&lt;ValidationResult&gt;.</returns>
        void IncreaseAccessCountFail(string username);
    }
}
