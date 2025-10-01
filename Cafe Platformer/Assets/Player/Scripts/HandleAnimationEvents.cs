using System;
using UnityEngine;

public class HandleAnimationEvents : MonoBehaviour
{
    public Action<string> OnAnimationEventTriggered;

    public void HandleAnimationEvent(string eventName)
    {
        OnAnimationEventTriggered?.Invoke(eventName);
    }
}