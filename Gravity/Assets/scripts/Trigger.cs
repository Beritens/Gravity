using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    public UnityEvent methodEnter;
    public UnityEvent methodExit;
    public LayerMask layer;
    public bool count;
    int inside = 0;
    public int max;
    public float min;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (count)
        {
            inside++;
            if(inside >= max)
            {
                methodEnter.Invoke();
            }
            return;
        }
        if(methodEnter != null && layer == (layer | (1 << collision.gameObject.layer)))
        {
            methodEnter.Invoke();
        }
            
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (count)
        {
            inside--;
            if (inside <= min)
            {
                methodExit.Invoke();
            }
            return;
        }
        if (methodExit != null && layer == (layer | (1 << collision.gameObject.layer)))
        {
            methodExit.Invoke();
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (count)
        {
            inside++;
            if (inside >= max)
            {
                methodEnter.Invoke();
            }
            return;
        }
        if (methodEnter != null && layer == (layer | (1 << collision.gameObject.layer)))
        {
            methodEnter.Invoke();
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (count)
        {
            inside--;
            if (inside <= min)
            {
                methodExit.Invoke();
            }
            return;
        }
        if (methodExit != null && layer == (layer | (1 << collision.gameObject.layer)))
        {
            methodExit.Invoke();
        }

    }
}
