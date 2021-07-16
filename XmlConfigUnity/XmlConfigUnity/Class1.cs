using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlConfigUnity
{
    public class Unity
    {
        public Unity()
        {
            this.containers = new Containers();
            this.typeAliases = new TypeAliases();
        }

        public TypeAliases typeAliases { get; set; }
        public Containers containers { get; set; }
        private XDocument _doc;
        public void Load(XDocument doc)
        {
            _doc = doc;
            LoadTypeAliases();
            LoadContainers();
        }

        private void LoadTypeAliases()
        {
            foreach (var s in GetElements("typeAliases"))
            {
                this.typeAliases = new TypeAliases();
                var elements = s.Elements();
                foreach (var element in elements)
                {
                    this.typeAliases.Add(new TypeAlias()
                    {
                        alias = element.Attributes().FirstOrDefault(_ => _.Name == "alias")?.Value,
                        type = element.Attributes().FirstOrDefault(_ => _.Name == "type")?.Value
                    });
                }
            }
        }

        private void LoadInstances()
        {
            var eContainers = _doc.Descendants().Where(_ => _.Name == "container").ToList();
            foreach (var s in eContainers)
            {
                var eInstances = s.Elements("instances");
                foreach (var eInstance in eInstances)
                {
                       
                }
            }
        }

        private IEnumerable<XElement> GetElements(string name)
        {
            return _doc.Descendants().Where(_ => _.Name == name);
        }

        private void LoadContainers()
        {
            var eContainers = _doc.Descendants().Where(_ => _.Name == "container").ToList();
            foreach (var sContainer in eContainers)
            {
                var container = new Container();

                //Instances
                var eInstances = sContainer.Elements("instances");
                foreach (var eInstance in eInstances)
                {
                    container.Instances = new Instances();
                }

                //Extensions 
                var eExtensions = eContainers.Elements("extensions");
                foreach (var eExtension in eExtensions)
                {
                    var eAdds = eExtensions.Elements("add");
                    foreach (var eAdd in eAdds)
                    {
                        var add = new Add();
                        add.name = eAdd.Attributes().FirstOrDefault(_ => _.Name == "name")?.Value;
                        add.type = eAdd.Attributes().FirstOrDefault(_ => _.Name == "type")?.Value;
                        container.Extensions.Add(add);
                    }
                }

                //Extensions Configs
                var eExtensionConfigs = eContainers.Elements("extensionConfig");
                foreach (var eExtensionConfig in eExtensionConfigs)
                {
                    var extensionConfig = new ExtensionConfig();
                    var eAdds = eExtensionConfig.Elements("add");
                    foreach (var eAdd in eAdds)
                    {
                        var add = new Add();
                        add.name = eAdd.Attributes().FirstOrDefault(_ => _.Name == "name")?.Value;
                        add.type = eAdd.Attributes().FirstOrDefault(_ => _.Name == "type")?.Value;

                        var eInteceptors = eAdd.Elements("interceptors").Elements("interceptor");
                        foreach (var eInteceptor in eInteceptors)
                        {
                            var inteceptor = new Interceptor();
                            inteceptor.type = eInteceptor.Attributes().FirstOrDefault(_ => _.Name == "type")?.Value;
                            inteceptor.name = eInteceptor.Attributes().FirstOrDefault(_ => _.Name == "name")?.Value;
                            var eKeys = eInteceptor.Elements("key");
                            foreach (var eKey in eKeys)
                            {
                                var key = new Key();
                                key.type = eKey.Attributes().FirstOrDefault(_ => _.Name == "type")?.Value;
                                inteceptor.Keys.Add(key);
                            }
                            add.Interceptors.Add(inteceptor);
                        }

                        var ePolicies = eAdd.Elements("policies").Elements("policy");
                        foreach (var ePolicy in ePolicies)
                        {
                            var policy = new Policy();
                            policy.name = ePolicy.Attributes().FirstOrDefault(_ => _.Name == "name")?.Value;
                            var eMatchingRules = ePolicy.Elements("matchingRules").Elements("matchingRule");
                            foreach (var eMatchingRule in eMatchingRules)
                            {
                                var matchingRule = new MatchingRule();
                                matchingRule.name = eMatchingRule.Attributes().FirstOrDefault(_ => _.Name == "name")?.Value;
                                matchingRule.type = eMatchingRule.Attributes().FirstOrDefault(_ => _.Name == "type")?.Value;

                                var eInjections = eMatchingRule.Elements("injection");
                                foreach (var eInjection in eInjections)
                                {
                                    var injection = new Injection();
                                    var eConstructors = eInjection.Elements("constructor");
                                    foreach (var eConstructor in eConstructors)
                                    {
                                        var constructor = new Constructor();
                                        var eParams = eConstructor.Elements("param");
                                        foreach (var eParam in eParams)
                                        {
                                            var param = new Param();
                                            param.name = eParam.Attributes().FirstOrDefault(_ => _.Name == "name")?.Value;
                                            param.parameterType = eParam.Attributes().FirstOrDefault(_ => _.Name == "parameterType")?.Value;

                                            var eValues = eParam.Elements("value");
                                            foreach (var eValue in eValues)
                                            {
                                                var value = new Value();
                                                value.type = eValue.Attributes().FirstOrDefault(_ => _.Name == "type")?.Value;
                                                value.value = eValue.Attributes().FirstOrDefault(_ => _.Name == "value")?.Value;
                                                param.Values.Add(value);
                                            }

                                            constructor.Param.Add(param);
                                        }

                                        injection.Constructors.Add(constructor);
                                    }
                                    matchingRule.Injections.Add(injection);
                                }
                                policy.MatchingRules.Add(matchingRule);
                            }

                            var eCallHandlers = ePolicy.Elements("callHandlers").Elements("callHandlers");
                            foreach (var eCallHandler in eCallHandlers)
                            {
                                var callHandler = new CallHandler();
                                callHandler.name = eCallHandler.Attributes().FirstOrDefault(_ => _.Name == "name")?.Value;
                                callHandler.type = eCallHandler.Attributes().FirstOrDefault(_ => _.Name == "type")?.Value;

                                var eInjections = eCallHandler.Elements("injection");
                                foreach (var eInjection in eInjections)
                                {
                                    var injection = new Injection();
                                    var eConstructors = eInjection.Elements("constructor");
                                    foreach (var eConstructor in eConstructors)
                                    {
                                        var constructor = new Constructor();
                                        var eParams = eConstructor.Elements("param");
                                        foreach (var eParam in eParams)
                                        {
                                            var param = new Param();
                                            param.name = eParam.Attributes().FirstOrDefault(_ => _.Name == "name")?.Value;
                                            param.parameterType = eParam.Attributes().FirstOrDefault(_ => _.Name == "parameterType")?.Value;

                                            var eValues = eParam.Elements("value");
                                            foreach (var eValue in eValues)
                                            {
                                                var value = new Value();
                                                value.type = eValue.Attributes().FirstOrDefault(_ => _.Name == "type")?.Value;
                                                value.value = eValue.Attributes().FirstOrDefault(_ => _.Name == "value")?.Value;
                                                param.Values.Add(value);
                                            }

                                            constructor.Param.Add(param);
                                        }

                                        injection.Constructors.Add(constructor);
                                    }

                                    var eProperties = eInjection.Elements("property");
                                    foreach (var ePropertie in eProperties)
                                    {
                                        var property = new Property();
                                        property.name = ePropertie.Attributes().FirstOrDefault(_ => _.Name == "name")?.Value;
                                        property.propertyType = ePropertie.Attributes().FirstOrDefault(_ => _.Name == "propertyType")?.Value;

                                        var eValues = ePropertie.Elements("value");
                                        foreach (var eValue in eValues)
                                        {
                                            var value = new Value();
                                            value.type = eValue.Attributes().FirstOrDefault(_ => _.Name == "name")?.Value;
                                            value.value = eValue.Attributes().FirstOrDefault(_ => _.Name == "value")?.Value;
                                            property.Values.Add(value);
                                        }

                                        injection.Properties.Add(property);
                                    }
                                    callHandler.Injections.Add(injection);
                                }

                                policy.CallHandlers.Add(callHandler);
                            }
                            add.Policies.Add(policy);
                        }

                        extensionConfig.Add.Add(add);
                    }

                    container.ExtensionConfig = extensionConfig;
                }

                //Types
                var etypes = sContainer.Elements("types").Elements("type").ToList();
                foreach (var etype in etypes)
                {
                    var type = new Type
                    {
                        type = etype.Attributes().FirstOrDefault(_ => _.Name == "type")?.Value,
                        mapTo = etype.Attributes().FirstOrDefault(_ => _.Name == "mapTo")?.Value
                    };

                    var elifetimes = etype.Elements("lifetime");
                    foreach (var elifetime in elifetimes)
                    {
                        type.LifeTimes.Add(new LifeTime()
                        {
                            type = elifetime.Attributes().FirstOrDefault(_ => _.Name == "type")?.Value
                        });
                    }

                    var etypeConfigs = etype.Elements("typeConfig").ToList();
                    foreach (var etypeConfig in etypeConfigs)
                    {
                        var typeConfig = new TypeConfig();
                        typeConfig.extensionType = etypeConfig.Attributes().FirstOrDefault(_ => _.Name == "extensionType").Value;
                        var eConstructors = etypeConfig.Elements("constructor");
                        foreach (var eConstructor in eConstructors)
                        {
                            var constructor = new Constructor();
                            var eParams = eConstructor.Elements("param");
                            foreach (var eParam in eParams)
                            {
                                var param = new Param
                                {
                                    name = eParam.Attributes().FirstOrDefault(_ => _.Name == "name")?.Value,
                                    parameterType = eParam.Attributes()
                                        .FirstOrDefault(_ => _.Name == "parameterType")?.Value
                                };

                                var eDependencies = eParam.Elements("dependency");
                                foreach (var eDependency in eDependencies)
                                {
                                    var dependency = new Dependency()
                                    {
                                        name = eDependency.Attributes("name").FirstOrDefault()?.Value
                                    };

                                    param.Dependencies.Add(dependency);
                                }

                                var eValues = eParam.Elements("value");
                                foreach (var eValue in eValues)
                                {
                                    var value = new Value()
                                    {
                                        type = eValue.Attributes("type").FirstOrDefault()?.Value,
                                        value = eValue.Attributes("value").FirstOrDefault()?.Value
                                    };

                                    param.Values.Add(value);
                                }

                                constructor.Param.Add(param);
                            }

                            typeConfig.Constructor.Add(constructor);
                        }

                        type.TypeConfig = typeConfig;
                    }
                    container.Types.Add(type);
                }

                this.containers.Add(container);
            }
        }

        private void LoadTypes()
        {
            foreach (var s in GetElements("types"))
            {
                this.containers = new Containers();
                var elements = s.Elements();
                foreach (var element in elements)
                {
                    this.containers.Add(new Container()
                    {
                    });
                }
            }
        }


    }

    public class TypeAliases : List<TypeAlias>
    {

    }
    public class TypeAlias
    {
        public string alias { get; set; }
        public string type { get; set; }
    }

    public class Containers :List<Container>
    {

    }

    public class Container
    {
        public Container()
        {
            this.Types = new Types();
            this.Instances = new Instances();
            this.Extensions = new Extensions();
            this.ExtensionConfig = new ExtensionConfig();
        }

        public Types Types { get; set; }
        public Instances Instances { get; set; }
        public Extensions Extensions { get; set; }
        public ExtensionConfig ExtensionConfig { get; set; }
    }

    public class Instances
    {

    }

    public class Extensions : List<Add>
    {

    }

    public class ExtensionConfig
    {
        public ExtensionConfig()
        {

            this.Add = new List<Add>();
        }
        public List<Add> Add { get; set; }
    }

    public class Add
    {
        public Add()
        {
            this.Interceptors = new Interceptors();
            this.Policies = new Policies();
        }

        public string name { get; set; }
        public string type { get; set; }

        public Interceptors Interceptors { get; set; }
        public Policies Policies { get; set; }
    }

    public class Interceptors : List<Interceptor>
    {
    }

    public class Interceptor
    {
        public Interceptor()
        {
            this.Keys = new List<Key>();
        }
        public string name { get; set; }
        public string type { get; set; }
        public List<Key> Keys { get; set; }
    }

    public class Key
    {
        public string type { get; set; }
    }

    public class Policies : List<Policy>
    {

    }

    public class CallHandlers : List<CallHandler>
    {

    }

    public class CallHandler
    {
        public string name { get; set; }
        public string type { get; set; }
        public List<Injection> Injections { get; set; }
    }

    public class Policy
    {
        public Policy()
        {

            this.CallHandlers = new CallHandlers();
            this.MatchingRules = new MatchingRules();
        }
        public string name { get; set; }
        public MatchingRules MatchingRules { get; set; }
        public CallHandlers CallHandlers { get; set; }
    }

    public class Property
    {
        public string name { get; set; }
        public string propertyType { get; set; }
        public List<Value> Values { get; set; }
    }

    public class MatchingRules : List<MatchingRule>
    {

    }

    public class MatchingRule
    {
        public MatchingRule()
        {
            this.Injections = new List<Injection>();
        }

        public string name { get; set; }
        public string type { get; set; }
        public List<Injection> Injections { get; set; }
    }

    public class Injection
    {
        public Injection()
        {
            this.Constructors = new List<Constructor>();
            this.Properties = new List<Property>();
        }

        public List<Constructor> Constructors { get; set; }
        public List<Property> Properties { get; set; }
    }


}
