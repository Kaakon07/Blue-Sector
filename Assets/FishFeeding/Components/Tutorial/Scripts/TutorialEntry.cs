using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class TutorialEntry : MonoBehaviour
{
    public Tutorial Tutorial;
    public void SetCompleted()
    {
        if (gameObject.activeSelf
            && Tutorial != null
            && Tutorial.Current == this.gameObject)
        {
            Tutorial.MoveNext();
        }
    }
}
