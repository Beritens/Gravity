using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    public UnityEvent method;
    public LayerMask layer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( layer == (layer | (1 << collision.gameObject.layer)))
        {
            method.Invoke();
        }
            
    }
}
