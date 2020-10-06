using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ACTools
{
    namespace DataManager
    {
        public class DataManagerWindow : EditorWindow
        {
            public static bool DataTypeSelected { get; private set; } = false;
            public static bool ItemSelected { get; private set; } = false;
            
            private VisualElement leftColumnItems;
            private VisualElement rightColumnItems;

            List<DataManagerSettingsEntry> visibleEntries;

            DataManagerSettingsEntry selectedDataEntry;
            List<DataType> selectedDataObjects;

            private ListView leftList;
            private ListView rightList;

            private Button CheckForTypesButton;

            [MenuItem("ACTools/Data Manager/Data Manager Window")]
            public static void ShowWindow()
            {
                DataManagerWindow wnd = GetWindow<DataManagerWindow>();
                wnd.titleContent = new GUIContent("Data Manager");

                wnd.minSize = new Vector2(400f, 180f);
            }

            public void OnEnable()
            {
                #region UXML and USS set-up
                // Each editor window contains a root VisualElement object
                VisualElement root = rootVisualElement;

                // Import UXML and USS
                VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_ACTools/_Data Manager/Editor/DataManagerWindow.uxml");
                StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_ACTools/_Data Manager/Editor/DataManagerWindow.uss");

                // Adds style sheet to root.
                root.styleSheets.Add(styleSheet);

                // Clones the visual tree and adds it to the root.
                VisualElement tree = visualTree.CloneTree();
                root.Add(tree);
                #endregion

                #region Mini-Row
                // Sets up the add fields, buttons, and events.
                CheckForTypesButton = (Button)tree.hierarchy.ElementAt(0).ElementAt(0);
                CheckForTypesButton.clickable.clicked += CheckForTypesAction;
                #endregion

                #region Main Row
                // Sets selected values to null;
                DataTypeSelected = false;
                ItemSelected = false;

                // Gets the main row element.
                VisualElement row = tree.hierarchy.ElementAt(2);

                #region Left Column
                // Gets the left column.
                leftColumnItems = row.ElementAt(0);

                DrawLeftColumnItems();
                #endregion

                #region Right Column
                // Gets the middle column.
                rightColumnItems = row.ElementAt(1);
                
                DrawRightColumnItems();
                #endregion

                #endregion
            }

            /// <summary> Draws the left column from the avaliable types. </summary>
            private void DrawLeftColumnItems()
            {
                // Clears out old visual elements.
                leftColumnItems.Clear();
                DataManagerDataTypeFinder.CheckForNewDataTypes();

                #region Sets up ListView.
                visibleEntries = new List<DataManagerSettingsEntry>();
                foreach (DataManagerSettingsEntry entry in DataManagerSettingsAccessor.Settings.EntriesArray)
                {
                    if (entry.visiblity == VisiblityToManager.Visible)
                        visibleEntries.Add(entry);
                }

                Label itemLabel = new Label();
                itemLabel.AddToClassList("list_item");
                Func<VisualElement> makeItem = () => new Label(); // Using the itemLabel cause the list to not add the first result for some reason.

                Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = visibleEntries.ToArray()[i].typeAsString;

                const int itemHeight = 20;

                leftList = new ListView(visibleEntries, itemHeight, makeItem, bindItem);
                leftList.AddToClassList("list");
                leftList.selectionType = SelectionType.Single;
                leftList.onSelectionChanged += CheckForSelectedType;
                
                leftColumnItems.Add(leftList);
                #endregion
            }

            /// <summary> Draws the right column based on the currently selected type. </summary>
            private void DrawRightColumnItems()
            {
                // Clears out old visual elements.
                rightColumnItems.Clear();

                if (DataTypeSelected)
                {
                    #region Sets up List.

                    selectedDataEntry = visibleEntries.ToArray()[leftList.selectedIndex];
                    Type stringToType = Type.GetType(selectedDataEntry.assemblyName);

                    List<UnityEngine.Object> objectArray = FolderUtility.FindAssetsByType(stringToType);
                    selectedDataObjects = new List<DataType>();

                    foreach (UnityEngine.Object obj in objectArray)
                    {
                        selectedDataObjects.Add((DataType) obj);
                    }

                    #endregion

                    #region Sets up ListView.

                    Label itemLabel = new Label();
                    itemLabel.AddToClassList("list_item");
                    Func<VisualElement> makeItem = () => new Label(); // Using the itemLabel cause the list to not add the first result for some reason.

                    Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = selectedDataObjects.ToArray()[i].name;
                     
                    const int itemHeight = 20;

                    rightList = new ListView(selectedDataObjects, itemHeight, makeItem, bindItem);
                    rightList.AddToClassList("list");
                    rightList.selectionType = SelectionType.Single;
                    rightList.onSelectionChanged += CheckForSelectedObject;

                    rightColumnItems.Add(rightList);
                    #endregion
                }
            }

            /// <summary> Shows the selected item in the inspector. </summary>
            private void DrawSelectedItemInInspector()
            {
                if (DataTypeSelected && ItemSelected)
                {
                    DataType selectedDataObject = selectedDataObjects.ToArray()[rightList.selectedIndex];
                    Type selectedDataObjectType = Type.GetType(selectedDataObject.AssemblyName);

                    string[] guids = AssetDatabase.FindAssets(selectedDataObject.name);
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(path, selectedDataObjectType);

                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = asset;
                }
            }

            /// <summary> Checks when a new type has been selected. </summary>
            /// <param name="target"> Target of the new selection. </param>
            private void CheckForSelectedType(object target)
            {
                DataTypeSelected = true;
                ItemSelected = false;

                DrawRightColumnItems();
            }
            
            /// <summary> Checks when a new item has been selected. </summary>
            /// <param name="target"> Target of the new selection. </param>
            private void CheckForSelectedObject(object target)
            {
                ItemSelected = true;

                DrawSelectedItemInInspector();
            }
            
            /// <summary> Called when the CheckForTypes button is pressed. </summary>
            private void CheckForTypesAction()
            {
                DataTypeSelected = false;
                ItemSelected = false;

                DataManagerDataTypeFinder.CheckForNewDataTypes();

                DrawLeftColumnItems();
                DrawRightColumnItems();
            }
        }
    }
}