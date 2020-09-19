using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using ACTools.General;

namespace ACTools
{
    namespace DataManager
    {
        [Serializable]
        public class DataManagerSettings : ScriptableObject
        {
            public static string MyName => "DataManagerSettings";
            public static string MyFolderPath => "Assets/_ACTools/_Data Manager/Editor";
            public static string MyPath => MyFolderPath + "/" + MyName + ".asset";

            [SerializeField]
            private List<DataManagerSettingsEntry> entriesList;
            public DataManagerSettingsEntry[] EntriesArray => entriesList.ToArray();

            /// <summary> Initalizes the entries list if it is null. </summary>
            public void Initialize()
            {
                if (entriesList == null)
                    entriesList = new List<DataManagerSettingsEntry>();
            }

            /// <summary> Adds a new entry to the private list. </summary>
            /// <param name="entry"> The entry to add. </param>
            public void AddEntry(DataManagerSettingsEntry entry)
            {
                EditorUtility.SetDirty(this);
                entriesList.Add(entry);
                entriesList.Sort(entry);
            }

            /// <summary> Checks if a given entry is within the entriesList. </summary>
            /// <param name="entry"> The entry to check for in entriesList. </param>
            /// <returns> Returns entriesList.Contains(entry). </returns>
            public bool Contains(DataManagerSettingsEntry entry)
            {
                return entriesList.Contains(entry);
            }

            /// <summary> Changes the visiblity of the given entry. </summary>
            /// <param name="newVisiblity"> New visiblity for the given entry. </param>
            public void ChangeVisiblityOf(DataManagerSettingsEntry entry, VisiblityToManager newVisiblity)
            {
                int index = entriesList.IndexOf(entry);
                DataManagerSettingsEntry[] array = EntriesArray;
                array[index].visiblity = newVisiblity;

                EditorUtility.SetDirty(this);
                entriesList = array.ToList();
            }
            
            /// <summary> Removes a new entry from the private list. </summary>
            /// <param name="entry"> The entry to remove. </param>
            /// <returns> Returns true if the entry is removed. </returns>
            public bool RemoveEntry(DataManagerSettingsEntry entry)
            {
                EditorUtility.SetDirty(this);
                return entriesList.Remove(entry);
            }

            /// <summary> Resets the entriesList. </summary>
            public void ResetEntriesList()
            {
                entriesList = new List<DataManagerSettingsEntry>();
            }
        }

        [Serializable]
        public class DataManagerSettingsEntry : IComparer<DataManagerSettingsEntry>
        {
            public string typeAsString;
            public string assemblyName;
            public VisiblityToManager visiblity = VisiblityToManager.Visible;

            /// <summary> Constructs a new entry to be added to the dmdEntries list. </summary>
            /// <param name="givenAssemblyName"> Assembly for this type of entry. </param>
            /// <param name="givenTypeAsString"> The type of the new entry as a string. Case sensitive. (e.g. ScriptableObject). </param>
            public DataManagerSettingsEntry(string givenAssemblyName, string givenTypeAsString)
            {
                assemblyName = givenAssemblyName;
                typeAsString = givenTypeAsString;
            }

            // From IComparer interface.
            public int Compare(DataManagerSettingsEntry x, DataManagerSettingsEntry y)
            {
                int typeComparison = x.typeAsString.CompareTo(y.typeAsString);

                if (typeComparison == 0)
                    return x.assemblyName.CompareTo(y.assemblyName);

                return typeComparison;
            }
        }

        [Serializable]
        public enum VisiblityToManager
        {
            Visible,
            NotVisible,
            NotFound
        }
    }
}