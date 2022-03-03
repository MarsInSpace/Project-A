using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Feedbacks : MonoBehaviour
{
    bool showButtons = false;
    Feedback[] addedFeedbacks;
    [SerializeField] 

    void Awake()
    {
        addedFeedbacks = GetComponents<Feedback>();
    }

    public void PlayFeedbacks(Vector3 _position)
    {
        foreach (Feedback _f in addedFeedbacks)
        {
            if (_f)
            {
                _f.PlayFeedback(_position);
            }
        }
    }
    void AddFeedback(string _feedbackType)
    {
        Type _type = Type.GetType(_feedbackType + ",Assembly-CSharp");
        if (_type != null)
        {
           gameObject.AddComponent(Type.GetType(_feedbackType + ",Assembly-CSharp"));
        }
        else
        {
            Debug.LogError("There is no feedback Component called: "+ _feedbackType);
        }
    }
    
    #region Editor

#if UNITY_EDITOR
[CustomEditor(typeof(Feedbacks))]
    public class FeedbacksEditor: Editor
    {
        Feedbacks feedbacks;
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.LabelField("add any Feedback from the various feedback types below");
            
            feedbacks = (Feedbacks)target;

            feedbacks.showButtons = EditorGUILayout.Foldout(feedbacks.showButtons, "Feedback Types", true);

            if (feedbacks.showButtons)
            {
                var _enumValues = Enum.GetValues(typeof(FeedbackTypes));
                for (int i = 0; i < _enumValues.Length; i++)
                {
                    if (GUILayout.Button("Add a "+ _enumValues.GetValue(i) + '!'))
                    {
                        feedbacks.AddFeedback(_enumValues.GetValue(i).ToString());
                    }
                }
            }
            
          
            serializedObject.ApplyModifiedProperties();
        }
        
    }
#endif
#endregion
}

public enum FeedbackTypes
{
    FB_Sound,
    FB_ParticleEffect,
    FB_MaterialChange,
    FB_CameraShake,
    FB_CameraZoom,
    FB_Distortion,
    FB_ChromaticAberation,
    FB_Bloom,
    FB_ColorAdjustment,
    FB_MotionBlur,
    FB_Vibration,
    FB_VibrationPulse,
    FB_VibrationLinear,
    FB_PopUpScore,
    FB_TimeManipulation,
    FB_ChangePitch
}


