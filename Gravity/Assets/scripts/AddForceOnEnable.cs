using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AddForceOnEnable : MonoBehaviour {
    public Vector2 Force;
    Rigidbody2D rb;
    public Vector2 COM;
	void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = COM;
        rb.AddForce(Force, ForceMode2D.Impulse);
    }
}
