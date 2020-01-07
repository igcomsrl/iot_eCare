//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using MateSharp.Framework.Domain.Services;
using MateSharp.Framework.Extensions.NHibernate;
using MateSharp.Framework.Helpers;
using MateSharp.Framework.Models;
using Meti.Application.Dtos.File;
using Meti.Domain.Models;
using Meti.Domain.Repository;
using Meti.Domain.Services;
using Meti.Infrastructure.Repository;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using File = Meti.Domain.Models.File;

namespace Meti.Application.Services
{
    public class FileService : ServiceNHibernateBase, IFileService
    {
        #region Private Fields

        private readonly IFileRepository _fileRepository;
        private readonly IRegistryRepository _registryRepository;
        private readonly string _registryDocumentsPath;

        #endregion Private Fields

        #region Costructors

        public FileService(ISession session): base(session)
        {
            _fileRepository = new FileRepository(session);
            _registryRepository = new RegistryRepository(session);
        }

        public FileService(IFileRepository FileRepository, IRegistryRepository registryRepository)
        {
            _fileRepository = FileRepository;
            _registryRepository = registryRepository;
            _registryDocumentsPath = ConfigurationManager.AppSettings["registryDocumentsPath"];
        }

        #endregion Costructors

        /// <summary>
        /// Creates the File.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IList&lt;ValidationResult&gt;.</returns>
        public OperationResult<File> CreateFile(FileDto dto, Guid? registryId)
        {
            //Validazione degli argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (!registryId.HasValue) throw new ArgumentNullException(nameof(registryId));

            //Dichiaro la lista di risultati di ritorno
            IList<ValidationResult> vResults = new List<ValidationResult>();

            //Eseguo in transazione
            using (var transaction = Session.ExecuteInTransaction())
            {
                //Definisco l'entità
                File entity = new File();
                entity.Name = dto.Name;
                entity.Size = dto.Size;
                entity.Type = dto.Type;
                entity.FilepathVirtual = dto.FilepathVirtual;
                entity.FilepathPhysical = dto.FilepathPhysical;
                var documentDirectory = CreateFileDirectory();
                entity.FilepathVirtual = string.Format("{0}/{1}", documentDirectory, entity.Name);
                entity.FilepathPhysical = HttpContext.Current.Server.MapPath(entity.FilepathVirtual);

                //Eseguo la validazione logica
                vResults = ValidateEntity(entity);

                if (!vResults.Any())
                {
                    //Salvataggio su db

                    WriteDocumentToDisk(documentDirectory, dto.FilepathBodypart, entity.Name);

                    Registry registry = _registryRepository.Load(registryId);
                    registry.Files.Add(entity);

                    vResults = ValidateEntity(registry);
                    if (!vResults.Any())
                    {
                        _registryRepository.Save(registry);
                    }
                    //Commit Esplicito
                    transaction.ExecuteCommit();


                }
                else
                {
                    transaction.ExecuteRollback();
                }

                //Ritorno i risultati
                return new OperationResult<File>
                {
                    ReturnedValue = entity,
                    ValidationResults = vResults
                };
            }
        }

        /// <summary>
        /// Deletes the File.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IList&lt;ValidationResult&gt;.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public OperationResult<File> DeleteFile(FileDto dto, Guid? registryId)
        {
            //Validazione degli argomenti
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            using (var transaction = Session.ExecuteInTransaction())
            {
                //Recupero l'entità tramite l'id
                File entity = _fileRepository.Load(dto.Id);

                //Eseguo la validazione logica
                IList<ValidationResult> vResults = ValidateEntity(entity);

                if (!vResults.Any())
                {
                    var registry = _registryRepository.Load(registryId);
                    registry.Files.Remove(entity);

                    _registryRepository.Save(registry);
                    //Eseguo la cancellazione logica     

                    var filepathFisical = HttpContext.Current.Server.MapPath(entity.FilepathVirtual);
                    if (System.IO.File.Exists(filepathFisical))
                        System.IO.File.Delete(filepathFisical);

                    //Commit Esplicito
                    transaction.ExecuteCommit();
                }
                else
                {
                    transaction.ExecuteRollback();
                }

                //Ritorno i risultati
                return new OperationResult<File>
                {
                    ReturnedValue = entity,
                    ValidationResults = vResults
                };
            }
        }

        /// <summary>
        /// Crea la directory virtuale legata al nosologico: "~/Uploads/RicoveroFile/1000259"
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        private string CreateFileDirectory()
        {
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(_registryDocumentsPath));

            return _registryDocumentsPath;
        }

        public void WriteDocumentToDisk(string virtualFilePath, string physicalFilepath, string filename)
        {
            //Validazione degli argomenti
            if (string.IsNullOrWhiteSpace(virtualFilePath)) throw new ArgumentNullException(nameof(virtualFilePath));
            if (string.IsNullOrWhiteSpace(physicalFilepath)) throw new ArgumentNullException(nameof(physicalFilepath));

            string virtualFilePathLocation = virtualFilePath;

            MultipartHelper.WriteFromMultipart(filename, physicalFilepath, virtualFilePathLocation);

        }
   

     
        #region Dispose Pattern

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool isDisposing)
        {
            //Se sto facendo la dispose
            if (isDisposing)
            {
                //Rilascio le risorse locali
                _fileRepository.Dispose();
            }

            //Chiamo il metodo base
            base.Dispose(isDisposing);
        }





        #endregion Dispose Pattern
    }
}
