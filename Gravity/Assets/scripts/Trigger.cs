using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    public UnityEvent methodEnter;
    public UnityEvent methodExit;
    public LayerMask layer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(methodEnter != null && layer == (layer | (1 << collision.gameObject.layer)))
        {
            methodEnter.Invoke();
        }
            
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (methodExit != null && layer == (layer | (1 << collision.gameObject.layer)))
        {
            methodExit.Invoke();
        }

    }
}
