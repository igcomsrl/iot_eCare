//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System.Web.Http;
using System.Net;
using System.Net.Http;
using Meti.Domain.Services;
using Meti.Infrastructure.Configurations;
using MateSharp.Framework.Filters;
using AutoMapper;
using Meti.Application.Dtos.File;
using MateSharp.Framework.Helpers;
using System.IO;
using System.Threading.Tasks;
using Meti.App.Filters;

namespace Meti.App.Controllers
{
    [Authorize]
    [CatchLogException]
    public class FileController : ApiController
    {
        #region Private Fields

        private readonly IFileService _fileService;

        #endregion Private Fields

        #region Costructors


        public FileController(IFileService FileService)
        {
            _fileService = FileService;
        }

        #endregion Costructors

        #region Api

    
        #endregion Api

        #region Dispose Pattern

        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                _fileService.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }

        #endregion Dispose Pattern
    }
}