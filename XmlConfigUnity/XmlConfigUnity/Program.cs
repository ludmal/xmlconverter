using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XmlConfigUnity
{
    class Program
    {
        static void Main(string[] args)
        {

            var xmlfile = @"C:\projects\leica\unity_file\Unity.client.Config";

            var doc = XDocument.Load(xmlfile);
            //doc.Elements().Select(_ => _.Name).ToList().ForEach(Console.WriteLine);

            var items = new List<string>();
            var dic = new Dictionary<string, string>();

            var unity = new Unity();
            unity.Load(doc);

            Console.WriteLine(unity);

            var totalTypeAlias = unity.typeAliases.Count;
            var totalContainers = unity.containers.Count;
            var totalTypes = unity.containers.FirstOrDefault()?.Types.Count;
            var container = unity.containers.FirstOrDefault();
            var instances = container.Instances != null ? 1 : 0 ;
            var extensions = container.Extensions.Count;
            var extensionConfig = container.ExtensionConfig != null ? 1 : 0;
            var adds = container.Extensions.Count;
            var inteceptors = container.ExtensionConfig.Add.Sum(_ => _.Interceptors.Count);
            var policies = container.ExtensionConfig.Add.Sum(_ => _.Policies.Count);

            Console.WriteLine($"totalTypeAlias => {totalTypeAlias}");
            Console.WriteLine($"totalContainers => {totalContainers}");
            Console.WriteLine($"totalTypes => {totalTypes}");
            Console.WriteLine($"instances => {instances}");
            Console.WriteLine($"extensions => {extensions}");
            Console.WriteLine($"extensionConfig => {extensionConfig}");
            Console.WriteLine($"adds => {adds}");
            Console.WriteLine($"inteceptors => {inteceptors}");
            Console.WriteLine($"policies => {policies}");
        }
        
    }


   

}
