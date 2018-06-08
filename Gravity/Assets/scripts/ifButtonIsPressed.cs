using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ifButtonIsPressed : MonoBehaviour {

    public string button;
    public UnityEvent method;
    private void Update()
    {
        if (Input.GetButtonDown(button))
        {
            method.Invoke();
        }
    }
}
