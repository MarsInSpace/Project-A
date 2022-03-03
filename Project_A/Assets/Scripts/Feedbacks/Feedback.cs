using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public abstract class Feedback : MonoBehaviour
{
    public abstract void PlayFeedback(Vector3 _position);


    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(Feedback), true)]
    public class FeedbackEditor : Editor
    {
        Feedback feedbackItem;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            feedbackItem = (Feedback) target;
            EditorGUILayout.Space();
            if (GUILayout.Button("Delete Feedback"))
            {
                DestroyImmediate(feedbackItem);
            }

            if (feedbackItem)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
    #endregion
    
}
