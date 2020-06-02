using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Ubiety.Dns.Core;
using WoodButcher.Request.Models;

namespace WoodenButcher.Request
{
    public class RequestHandler<T>
    {
        private string _url = "https://www.berlin.de/ba-steglitz-zehlendorf/politik-und-verwaltung/aemter/strassen-und-gruenflaechenamt/gruenflaechen/baeume/baumfaellungen/index.php/index/all.json?q=";

        /* Contructor */ 
        public RequestHandler() { }
        
        public List<T> GetRequestResult()
        {
            // Grab data from the tree json.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.ContentType = "json/text";
            request.Method = "POST";

            var response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var r = reader.ReadToEnd();
            var json = JObject.Parse(r);
            var token = json["index"].ToString();
            
            var falledTrees = JsonConvert.DeserializeObject<List<T>>(token);
            return falledTrees;
        }
    }
}
