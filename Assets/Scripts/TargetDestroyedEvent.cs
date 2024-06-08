using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetDestroyedEvent : MonoBehaviour
{
    public static event Action OnTargetDestroyed;

    public static void TargetDestroyed()
    {
        Debug.Log("TargetDestroyed event invoked.");
        OnTargetDestroyed?.Invoke();
    }
}
