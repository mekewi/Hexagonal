using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchEventListiner : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public TouchEvent_SO gameEvent;

    [Tooltip("Response to Invoke When Evemt is raised.")]
    public TouchEvent Response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }
    public void OnEventRaised(Vector3 beginTouchPosition, TouchRotateDirction swipeDirction)
    {
        Response.Invoke(beginTouchPosition, swipeDirction);
    }
}
