using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TouchEvent : UnityEvent<Vector3,TouchRotateDirction>
{
}
