using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IntegerEvent_SO : ScriptableObject
{
    private List<IntegerEventListener> listeners = new List<IntegerEventListener>();

    public void Rais(int intToSend)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(intToSend);
        }
    }
    public void RegisterListener(IntegerEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }
    public void UnregisterListener(IntegerEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}
