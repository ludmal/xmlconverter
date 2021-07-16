
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace XmlConfigUnity
{
    public class Types : List<Type>
    {

    }

    public class Type
    {
        public Type()
        {
            this.LifeTimes = new List<LifeTime>();
            this.TypeConfig = new TypeConfig();
        }
        public string name { get; set; }
        public string type { get; set; }
        public string mapTo { get; set; }
        public List<LifeTime> LifeTimes { get; set; }
        public TypeConfig TypeConfig { get; set; }
    }

    public class LifeTime
    {
        public string type { get; set; }
    }

    public class TypeConfig
    {
        public TypeConfig()
        {
            this.Constructor = new List<Constructor>();
        }
        public string extensionType { get; set; }
        public List<Constructor> Constructor { get; set; }
    }

    public class Constructor
    {
        public Constructor()
        {
            this.Param = new List<Param>();
        }
        public List<Param> Param { get; set; }
    }

    public class Param
    {
        public Param()
        {
            this.Dependencies = new List<Dependency>();
            this.Values = new List<Value>();
        }
        public string name { get; set; }
        public string parameterType { get; set; }
        public List<Dependency> Dependencies { get; set; }
        public List<Value> Values { get; set; }
    }

    public class Value
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class Dependency
    {
        public string name { get; set; }
    }
}
