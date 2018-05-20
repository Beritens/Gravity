using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class attracted : MonoBehaviour {

    public Rigidbody2D rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject.FindObjectOfType<controls>().attractedObj.Add(this);
    }
}
