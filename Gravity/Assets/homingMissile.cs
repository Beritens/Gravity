using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingMissile : MonoBehaviour {

    public Transform target;
    Rigidbody2D rb;
    public float speed = 10f;
    public float rotateSpeed = 0.1f;
    public float MaxVelocity = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction,transform.up).z;
        rb.AddTorque(-rotateAmount * rotateSpeed);
        float velocityToTarget = Vector3.Dot(rb.velocity, direction);
        float lerp = 1-(velocityToTarget / MaxVelocity);

        //rb.velocity = transform.up * speed;
        rb.AddForce(transform.up * speed*lerp);
    }
}
