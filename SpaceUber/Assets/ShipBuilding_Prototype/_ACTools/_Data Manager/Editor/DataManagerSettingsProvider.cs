using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ACTools
{
    namespace DataManager
    {
        class DataManagerSettingsProvider
        {
            private static Button button = null;
            private static ListView listView = null;
            private static VisualElement currentTarget = null;

            private static string ToHideText => "Hide Data Type from Data Manager";
            private static string ToShowText => "Show Data Type to Data Manager";

            /// <summary> Updates the button's status and text. </summary>
            /// <param name="notUsed"> Selected object. It isn't used. </param>
            private static void UpdateButton(object notUsed)
            {
                button.SetEnabled(true);

                currentTarget = listView.ElementAt(listView.selectedIndex);
                if (currentTarget.enabledSelf)
                {
                    DataManagerSettingsAccessor.Settings.ChangeVisiblityOf(DataManagerSettingsAccessor.Settings.EntriesArray[listView.selectedIndex],
                                                                           VisiblityToManager.Visible);
                    button.text = ToHideText;
                }
                else
                {
                    DataManagerSettingsAccessor.Settings.ChangeVisiblityOf(DataManagerSettingsAccessor.Settings.EntriesArray[listView.selectedIndex],
                                                                           VisiblityToManager.NotVisible);
                    button.text = ToShowText;
                }
            }
            
            /// <summary> Updates the current status of the target. </summary>
            private static void SwitchTargetEnabledStatus()
            {
                currentTarget.SetEnabled(!currentTarget.enabledSelf);
                UpdateButton(null);
            }
            
            //[SettingsProvider]
            public static SettingsProvider CreateDataManagerSettings()
            {
                DataManagerDataTypeFinder.CheckForNewDataTypes();

                currentTarget = null;
                
                SettingsProvider provider = new SettingsProvider("ACTools/Data Manager", SettingsScope.User)
                {
                    activateHandler = (searchContext, rootElement) =>
                    {
                        SerializedObject serializedSettings = DataManagerSettingsAccessor.GetSerializedSettings();

                        #region UXML and USS set-up
                        // Import UXML and USS
                        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_ACTools/_Data Manager/Editor/DataManagerSettingsProvider.uxml");
                        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_ACTools/_Data Manager/Editor/DataManagerSettingsProvider.uss");

                        // Adds style sheet to root.
                        rootElement.styleSheets.Add(styleSheet);

                        // Clones the visual tree and adds it to the root.
                        VisualElement tree = visualTree.CloneTree();
                        rootElement.Add(tree);
                        #endregion

                        #region Sets up button.
                        if (button == null)
                            button = (Button) tree.hierarchy.ElementAt(1);
                        button.SetEnabled(false);
                        button.clickable.clicked += SwitchTargetEnabledStatus;
                        #endregion

                        #region Sets up ListView.
                        Func<VisualElement> makeItem = () => new Label();

                        Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = DataManagerSettingsAccessor.Settings.EntriesArray[i].typeAsString;

                        const int itemHeight = 16;

                        listView = new ListView(DataManagerSettingsAccessor.Settings.EntriesArray, itemHeight, makeItem, bindItem);
                        listView.AddToClassList("list");
                        listView.selectionType = SelectionType.Single;
                        listView.onSelectionChanged += UpdateButton;

                        rootElement.Add(listView);
                        #endregion
                    },

                    // Populate the search keywords to enable smart search filtering and label highlighting.
                    keywords = new HashSet<string>(new[] { "ACTools", "Data", "Manager", "Data Manager" })
                };
                return provider;
            }
        }
    }
}