using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Timing
{  
    List<UnityAction> actions;
    List<float> actionDelays;

    static Timing m_instance;
    public static Timing Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new Timing();
                m_instance.Init();
            }
            return m_instance;
        }
    }

    void Init()
    {
        actions = new List<UnityAction>();
        actionDelays = new List<float>();
    }

    public void Run()
    {
        for (int i = 0; i < actions.Count; i++)
        {

            try
            {
                actionDelays[i] -= Time.deltaTime;
                if (actionDelays[i] <= 0)
                {
                    actions[i].Invoke();
                    actions.RemoveAt(i);
                    actionDelays.RemoveAt(i);
                }
            }
            catch (System.Exception)
            {
                actions.RemoveAt(i);
                actionDelays.RemoveAt(i);
            }
          
        }
    }

    public void DoAfterDelay(UnityAction _action, float _delay)
    {
        actions.Add(_action);
        actionDelays.Add(_delay);
    }
}
