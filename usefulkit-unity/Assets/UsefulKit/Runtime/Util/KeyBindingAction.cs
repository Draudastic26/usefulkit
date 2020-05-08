using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyBindingAction : MonoBehaviour
{
    public KeyCode TriggerKeyCode;
    public UnityEvent Action = new UnityEvent();

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(TriggerKeyCode))
        {
            Action.Invoke();
        }
    }
}
