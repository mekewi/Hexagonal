using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    public int LevelScore;

    internal void ResetDefault()
    {
        LevelScore = 0;
    }
}
