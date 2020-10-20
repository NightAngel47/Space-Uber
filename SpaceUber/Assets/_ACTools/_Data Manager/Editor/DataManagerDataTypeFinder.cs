using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ACTools
{
    namespace DataManager
    {
        public static class DataManagerDataTypeFinder
        {
            public static void CheckForNewDataTypes()
            {
                // Doesnt find type if no assets of it exist.
                List<DataType> allDataTypeAssets = FolderUtility.FindAssetsByType<DataType>();
                List<DataManagerSettingsEntry> possibleEntries = new List<DataManagerSettingsEntry>();
                List<DataManagerSettingsEntry> entriesToAdd = new List<DataManagerSettingsEntry>();

                DataManagerSettingsAccessor.Settings.ResetEntriesList();
                
                // Checks the all the different kinds of data types
                foreach (DataType asset in allDataTypeAssets)
                {
                    DataManagerSettingsEntry newEntry = new DataManagerSettingsEntry(asset.AssemblyName, asset.TypeAsString);
                    bool isAlreadyPossibleEntry = false;

                    // Checks the list of possible entries for duplicates.
                    foreach (DataManagerSettingsEntry possibleEntry in possibleEntries)
                    {
                        if (newEntry.Compare(newEntry, possibleEntry) == 0)
                        {
                            isAlreadyPossibleEntry = true;
                            break;
                        }
                    }

                    // Adds to possible entries if no duplicates exist.
                    if (!isAlreadyPossibleEntry)
                        possibleEntries.Add(newEntry);
                }

                // Checks the list of possible entries against the list of existing entries to see if any should be added. 
                foreach (DataManagerSettingsEntry possibleEntry in possibleEntries)
                {
                    bool isAlreadyEntry = false;

                    // Comparing possible entries against existing entries.
                    foreach (DataManagerSettingsEntry entry in DataManagerSettingsAccessor.Settings.EntriesArray)
                    {
                        if (possibleEntry.Compare(possibleEntry, entry) == 0)
                        {
                            isAlreadyEntry = true;
                            break;
                        }
                    }

                    // Adds possible entries to the add list if they're no duplicates already existing.
                    if (!isAlreadyEntry)
                        entriesToAdd.Add(possibleEntry);
                }

                // Adds the new entries to the settings list.
                foreach (DataManagerSettingsEntry entry in entriesToAdd)
                    DataManagerSettingsAccessor.Settings.AddEntry(entry);
            }
        }
    }
}