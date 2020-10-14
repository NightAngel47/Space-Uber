using UnityEditor;

namespace ACTools
{
    namespace DataManager
    {
        public class CreateNewDataType
        {
            private static string templatePath = "Assets/_ACTools/_Data Manager/Editor/NewDataTypeTemplate.cs.txt";
            private static string defaultName = "NewDataType.cs";

            [MenuItem("ACTools/Data Manager/Create New DataType")]
            public static void CreateDataType()
            {
                ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, defaultName);
            }
        }
    }
}