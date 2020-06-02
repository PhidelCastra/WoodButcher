using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using WoodButcher.Models;
using WoodenButcher.Request;

namespace WoodButcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var requestHandler = new RequestHandler<TreeInfo>();
            var falledTrees = requestHandler.GetRequestResult();

            var diff = falledTrees.Select(tree => tree.FaellGrund).ToList();
            var bestReg = falledTrees.Where(tree => tree.FaellGrund == "Bestandsregulierung").ToList();

            Console.WriteLine($"Count: {bestReg.Count}\n");
            bestReg.ForEach(tree =>
            {
                Console.WriteLine($"Baumnummer: {tree.BaumNummer}");
                Console.WriteLine($"Adresse: {tree.Strasse} {tree.HausNummer}, {tree.PLZ} {tree.Ortsteil}");
                Console.WriteLine($"Datum: {tree.Datum}\n");
            });
        }
    }
}
