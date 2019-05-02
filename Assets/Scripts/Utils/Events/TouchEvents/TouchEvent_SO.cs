using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TouchEvent_SO : ScriptableObject
{
    private List<TouchEventListiner> listeners = new List<TouchEventListiner>();

    public void Rais(Vector3 beginTouchPosition, TouchRotateDirction swipeDirction)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(beginTouchPosition, swipeDirction);
        }
    }
    public void RegisterListener(TouchEventListiner listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }
    public void UnregisterListener(TouchEventListiner listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

}
