using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        serializedObject.Update();

        Player player = (Player)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("rb"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("overheatMaterial"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxSpeed"));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("unstopableEffect"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("unstoppableSpeed"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("unstoppableTrail"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("unstoppableTrailMat"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("normalTrailMat"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("deadlyPlatform"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("dangerFeedbacks"));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nearMissFeedbacks"));

        

        EditorGUILayout.PropertyField(serializedObject.FindProperty("aimObject"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("unstopableEffect"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("guns"));



        serializedObject.ApplyModifiedProperties();
    }   
}
