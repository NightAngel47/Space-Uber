using UnityEngine;

namespace ACTools
{
    namespace DataManager
    {
        public abstract class DataType : ScriptableObject
        {
            public abstract string AssemblyName { get; }
            public abstract string TypeAsString { get; }
        }
    }
}