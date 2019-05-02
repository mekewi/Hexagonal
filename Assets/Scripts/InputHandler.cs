using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TouchRotateDirction { 
    ClockWise = 1,
    CounterClockWise = -1,
    NotDetected = 0
}
public class InputHandler : MonoBehaviour
{
    bool mouseMoving;
    private Vector3 BeganTouchPosition;
    private Vector3 EndTouchPosition;
    public TouchEvent_SO touchEvent_SO;
    void Update()
    {
        if (Input.touches.Length > 0)
        {
            var finger = Input.GetTouch(0);
            switch (finger.phase)
            {
                case TouchPhase.Began:
                    BeginClick(Input.GetTouch(0).position);
                    break;
                case TouchPhase.Moved:
                    Moved(Input.GetTouch(0).position);
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    EndClick(Input.GetTouch(0).position);
                    break;
                case TouchPhase.Canceled:
                    EndClick(Input.GetTouch(0).position);
                    break;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            BeginClick(Input.mousePosition);
            mouseMoving = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            EndClick(Input.mousePosition);
            mouseMoving = false;
        }
        if (mouseMoving)
        {
            Moved(Input.mousePosition);
        }
    }
    public void BeginClick(Vector3 position) {
        BeganTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public void Moved(Vector3 position)
    {

    }
    public void EndClick(Vector3 position) {
        EndTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CheckDirction();
        touchEvent_SO.Rais(EndTouchPosition, CheckDirction());
    }
    public TouchRotateDirction CheckDirction() {
        if (Mathf.Abs(BeganTouchPosition.x - EndTouchPosition.x) < 3)
        {
            return TouchRotateDirction.NotDetected;
        }
        if (BeganTouchPosition.x> EndTouchPosition.x)
        {
            return TouchRotateDirction.ClockWise;
        }
        else if (BeganTouchPosition.x < EndTouchPosition.x) {
            return TouchRotateDirction.CounterClockWise;
        }
        return TouchRotateDirction.ClockWise;
    }
}
