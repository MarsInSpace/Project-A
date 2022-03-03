
using UnityEditor;

[CustomEditor(typeof(Grapple_Gun))]
public class GrappleEditor : Editor
{
    public override void OnInspectorGUI()
    {
       // base.OnInspectorGUI();
        serializedObject.Update();

        Grapple_Gun grapple = (Grapple_Gun)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("gunVariables").FindPropertyRelative("muzzle"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("whatIsGrappable"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lineRenderer"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pointer"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speedMultiplier"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hasLaser"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("swingAudio"));


        EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxHookRange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minHookRange"));
 
         EditorGUILayout.PropertyField(serializedObject.FindProperty("jointVariables"));

        

        serializedObject.ApplyModifiedProperties();
    }
}
