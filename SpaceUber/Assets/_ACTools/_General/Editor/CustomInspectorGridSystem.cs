using UnityEngine;
using UnityEditor;
using ACTools.General;

[CustomEditor(typeof(GridSystem))]
public class CustomInspectorGridSystem : Editor
{
    public static bool showGizmoSettings = false;

    private GridSystem gridSystem;
    private SerializedObject targetObj;

    private SerializedProperty size;
    private SerializedProperty axis1;
    private SerializedProperty axis2;

    private Vector3 testValue = new Vector3();

    private SerializedProperty gizmoColor;
    private SerializedProperty alpha;
    private SerializedProperty gizmoSize;
    private SerializedProperty gizmoLength;

    private void OnEnable()
    {
        gridSystem = (GridSystem)target;
        targetObj = new SerializedObject(target);

        size = targetObj.FindProperty("size");
        axis1 = targetObj.FindProperty("axis1");
        axis2 = targetObj.FindProperty("axis2");

        gizmoColor = targetObj.FindProperty("gizmoColor");
        alpha = targetObj.FindProperty("alpha");
        gizmoSize = targetObj.FindProperty("gizmoSize");
        gizmoLength = targetObj.FindProperty("gizmoLength");
    }

    public override void OnInspectorGUI()
    {
        targetObj.Update();

        gridSystem.transform.localEulerAngles = Vector3.zero;
        gridSystem.transform.localScale = Vector3.one;

        EditorGUILayout.PropertyField(size);
        EditorGUILayout.PropertyField(axis1);
        EditorGUILayout.PropertyField(axis2);


        EditorGUILayout.Space();


        EditorGUILayout.BeginHorizontal();

        if (!gridSystem.IsAlongAxis(GridAxes.x))
            testValue.With(gridSystem.transform.position.x);
        if (!gridSystem.IsAlongAxis(GridAxes.y))
            testValue.With(null, gridSystem.transform.position.y);
        if (!gridSystem.IsAlongAxis(GridAxes.z))
            testValue.With(null, null, gridSystem.transform.position.z);

        testValue = EditorGUILayout.Vector3Field("Test Point", testValue);

        GUIStyle horizontalButtonStyles = new GUIStyle(EditorStyles.miniButton);
        horizontalButtonStyles.margin = new RectOffset(6, 6, 18, 0);
        horizontalButtonStyles.fontStyle = FontStyle.Bold;

        if (GUILayout.Button("Test Point On Grid", horizontalButtonStyles))
        {
            Vector3 gridPoint = gridSystem.GetNearestPointOnGrid(testValue);
            Debug.Log("Test Value: " + testValue);
            Debug.Log("Grid Value: " + gridPoint);
            DebugExtensions.DrawCube(gridPoint, size.floatValue, gizmoColor.colorValue, 5f, true);
        }

        EditorGUILayout.EndHorizontal();


        EditorGUILayout.Space();


        GUIStyle gizmoStyles = new GUIStyle(EditorStyles.foldout);
        gizmoStyles.fontStyle = FontStyle.Bold;

        showGizmoSettings = EditorGUILayout.Foldout(showGizmoSettings, "Gizmo Settings", true, gizmoStyles);

        if (showGizmoSettings)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(gizmoColor);
            EditorGUILayout.PropertyField(alpha);
            EditorGUILayout.PropertyField(gizmoSize);
            EditorGUILayout.PropertyField(gizmoLength);
            EditorGUI.indentLevel--;
        }

        targetObj.ApplyModifiedProperties();
    }
}