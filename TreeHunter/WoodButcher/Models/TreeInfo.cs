using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WoodButcher.Models
{
    [Serializable]
    public class TreeInfo
    {
        [JsonPropertyName(name:"id")]
        public int ID { get; set; }

        [JsonPropertyName(name:"plz")]
        public int PLZ { get; set; }

        [JsonPropertyName(name:"ortsteil")]
        public string Ortsteil { get; set; }

        [JsonPropertyName(name:"strasse")]
        public string Strasse { get; set; }

        [JsonProperty("hausnr")]
        public string HausNummer { get; set; }

        [JsonProperty("baumnr")]
        public string BaumNummer { get; set; }

        [JsonProperty("gattung")]
        public string Gattung { get; set; }

        [JsonProperty("faellgrund")]
        public string FaellGrund { get; set; }
    
        [JsonProperty("faelldatum")]
        public string Datum { get; set; }

        [JsonProperty("rechtswert_utm33")]
        public float Rechtswert { get; set; }
    
        [JsonProperty("hochwert_utm33")]
        public float Hochwert { get; set; }

        [JsonProperty("geograf_rechtswert")]
        public float GeoRechtswert { get; set; }

        [JsonProperty("geograf_hochwert")]
        public float GeoHochwert { get; set; }
    }

    public class TreeInfos
    {
        public List<TreeInfo> infos { get; set; }
    }
}
