using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ACTools.Saving
{
    public static class LoadData
    {
        /// <summary> Loads game data from a binary file. </summary>
        /// <typeparam name="T"> Type of data being loaded. </typeparam>
        /// <param name="projectName"> The name of your project. This should ideally be the same for all save files for this game for convenience. </param>
        /// <param name="dataName"> A name for the data. This should have been used when saving the data. </param>
        /// <returns> Returns the data if it exists. If it doesn't, returns the default of that type. </returns>
        public static T FromBinaryFile<T>(string projectName, string dataName)
        {
            string path = Application.persistentDataPath + "/" + projectName + "." + dataName;

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                T data = (T)formatter.Deserialize(stream);
                stream.Close();

                return data;
            }
            else
            {
                Debug.LogWarning("No file exists at " + path + ".\nReturning default of " + typeof(T).ToString() + ".");
                return default;
            }
        }
    }
}