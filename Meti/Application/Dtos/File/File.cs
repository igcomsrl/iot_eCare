//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019

//Concesso in licenza a norma dell'EUPL, versione 1.2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Meti.Application.Dtos.File
{
    public class FileDto
    {
        public Guid? Id { get; set; }
        public string FilepathVirtual { get; set; }
        public string FilepathPhysical { get; set; }   
        public string FilepathBodypart { get; set; }
        public string FileUrl
        {
            get {
                if (!string.IsNullOrWhiteSpace(FilepathVirtual))
                    return HttpContext.Current.Request.UrlReferrer.AbsoluteUri + FilepathVirtual.Replace("~/", "/");
                else
                    return string.Empty;
            }
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public Guid? RegistryId { get; set; }
    }
}
