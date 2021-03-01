using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ACTools.Saving
{
    public static class SaveData
    {
        /// <summary> Saves game data to a binary file. </summary>
        /// <typeparam name="T"> Type of data being saved. </typeparam>
        /// <param name="projectName"> The name of your project. This should ideally be the same for all save files for this game for convenience. </param>
        /// <param name="dataName"> A name for the data. This is be apart of the path for saving it. Make sure it's unique. </param>
        /// <param name="data"> Data to be saved. </param>
        public static void ToBinaryFile<T>(string projectName, string dataName, T data)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + "/" + projectName + "." + dataName;
            FileStream stream = new FileStream(path, FileMode.Create);


            formatter.Serialize(stream, data);
            stream.Close();
        }
    }
}