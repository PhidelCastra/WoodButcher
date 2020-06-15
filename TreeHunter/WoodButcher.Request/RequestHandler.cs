using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace WoodButcher.Request
{
    public class RequestHandler<T>
    {
        /// <summary>
        /// URL for request data.
        /// </summary>
        private string _url = "https://www.berlin.de/ba-steglitz-zehlendorf/politik-und-verwaltung/aemter/strassen-und-gruenflaechenamt/gruenflaechen/baeume/baumfaellungen/index.php/index/all.json?q=";

        /* Contructor */
        public RequestHandler() { }

        /// <summary>
        /// Gets a adata set from the URL and prepare a List of corresponding objects.
        /// </summary>
        /// <returns>List with objects of generic model T.</returns>
        public List<T> GetRequestResult()
        {
            // Grab data.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.ContentType = "json/text";
            request.Method = "POST";

            var response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var stringResult = reader.ReadToEnd();
            var jsonResult = JObject.Parse(stringResult);
            var readFrom = jsonResult["index"].ToString();

            var falledTrees = JsonConvert.DeserializeObject<List<T>>(readFrom);
            return falledTrees;
        }
    }
}
