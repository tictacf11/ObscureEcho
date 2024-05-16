using UnityEditor;
using UnityEditor.UI;
using static UnityEngine.UI.GridLayoutGroup;

[CustomEditor(typeof(DynamicGridLayoutGroup), true)]
[CanEditMultipleObjects]
public class DynamicGridLayoutGroupEditor : Editor
{
    SerializedProperty m_Padding;
    SerializedProperty m_Spacing;
    SerializedProperty m_StartCorner;
    SerializedProperty m_StartAxis;
    SerializedProperty m_ChildAlignment;
    SerializedProperty m_CellAspectRatio;
    SerializedProperty m_Columns;
    SerializedProperty m_Rows;

    protected virtual void OnEnable()
    {
        m_Padding = serializedObject.FindProperty("m_Padding");
        m_Spacing = serializedObject.FindProperty("m_Spacing");
        m_StartCorner = serializedObject.FindProperty("m_StartCorner");
        m_StartAxis = serializedObject.FindProperty("m_StartAxis");
        m_ChildAlignment = serializedObject.FindProperty("m_ChildAlignment");
        m_Columns = serializedObject.FindProperty("m_Columns");
        m_Rows = serializedObject.FindProperty("m_Rows");
        m_CellAspectRatio = serializedObject.FindProperty("m_CellAspectRatio");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_Padding, true);
        EditorGUILayout.PropertyField(m_Spacing, true);
        EditorGUILayout.PropertyField(m_StartCorner, true);
        EditorGUILayout.PropertyField(m_StartAxis, true);
        EditorGUILayout.PropertyField(m_ChildAlignment, true);
        EditorGUILayout.PropertyField(m_Columns, true);
        EditorGUILayout.PropertyField(m_Rows, true);
        EditorGUILayout.PropertyField(m_CellAspectRatio, true);
        serializedObject.ApplyModifiedProperties();
    }
}
