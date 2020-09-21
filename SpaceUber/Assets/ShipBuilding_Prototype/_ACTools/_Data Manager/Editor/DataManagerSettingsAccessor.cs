using UnityEditor;

namespace ACTools
{
    namespace DataManager
    {
        public static class DataManagerSettingsAccessor
        {
            private static DataManagerSettings settings;
            public static DataManagerSettings Settings => settings != null ? settings : GetOrCreateSettings();

            /// <summary> Gets the settings for the data manager, or creates them if they don't exist. </summary>
            /// <returns> Returns the data manager settings object. </returns>
            internal static DataManagerSettings GetOrCreateSettings()
            {
                settings = AssetDatabase.LoadAssetAtPath<DataManagerSettings>(DataManagerSettings.MyPath);
                if (settings == null)
                {
                    settings = ScriptableObjectUtility.CreateAsset<DataManagerSettings>(DataManagerSettings.MyFolderPath);
                    settings.name = AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(settings), DataManagerSettings.MyName);
                    settings.Initialize();
                }
                return settings;
            }

            /// <summary> Gets a serialized version of the data manager settings object. </summary>
            /// <returns> Returns the serialized settings object. </returns>
            internal static SerializedObject GetSerializedSettings()
            {
                return new SerializedObject(Settings);
            }
        }
    }
}