using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AddForceOnEnable : MonoBehaviour {
    public Vector2 Force;
    
	void OnEnable()
    {
        GetComponent<Rigidbody2D>().AddForce(Force, ForceMode2D.Impulse);
    }
}
