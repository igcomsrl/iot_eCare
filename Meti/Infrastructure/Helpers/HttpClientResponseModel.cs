//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Meti.Infrastructure.Helpers
{
    public class HttpClientResponseModel
    {
        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        /// <value>The HTTP status code.</value>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>The response.</value>
        public object Response { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is status success code.
        /// </summary>
        /// <value><c>true</c> if this instance is status success code; otherwise, <c>false</c>.</value>
        public bool IsStatusSuccessCode { get; set; }
    }
}
