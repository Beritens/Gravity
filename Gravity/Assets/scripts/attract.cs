using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attract : MonoBehaviour {

    public const float G = 1f;
    public float attractorMass = 10f;
    public bool player = false;
    public float rao = 1.5f;
    public LayerMask attractedStuff;
    // Use this for initialization
    void FixedUpdate () {
        if (!player)
        {
            attractStuff(attractorMass,transform.position,rao);
        }
	}
	
	// Update is called once per frame
	public void attractStuff (float mass, Vector2 posi, float radiusAtOne) {
        Collider2D[] inRadius = Physics2D.OverlapCircleAll(posi, radiusAtOne * Mathf.Abs(mass),attractedStuff);
        for (int i = 0; i < inRadius.Length; i++)
        {
            if (!inRadius[i].GetComponent<attracted>())
                continue;
            Collider2D col = inRadius[i].GetComponent<Collider2D>();
            if (col.OverlapPoint(posi))
            {
                continue;
            }

            Rigidbody2D rba = inRadius[i].GetComponent<Rigidbody2D>();
            //Vector2 pos = col.bounds.ClosestPoint(MousePos);
            Vector2 pos = ClosestPoint(col, posi);
            Vector2 direction = posi - pos;
            float Gravdistance = direction.magnitude;
            if (Gravdistance == 0)
            {
                continue;
            }
            float minDis = inRadius[i].GetComponent<attracted>().MinDistance;
            Gravdistance += minDis;
            if (rba.GetComponent<controls>() != null && Gravdistance < rba.GetComponent<controls>().jumpBreakDistance)
            {
                rba.GetComponent<controls>().jumping = false;
            }



            float forceMagnitude = G * (mass * rba.mass) / Mathf.Pow(Gravdistance, 2);
            Vector2 force = direction.normalized * forceMagnitude;

            rba.AddForceAtPosition(force, pos);
        }
    }
    public static Vector2 ClosestPoint(Collider2D col, Vector2 point)
    {
        GameObject go = new GameObject("tempCollider");
        go.transform.position = point;
        CircleCollider2D c = go.AddComponent<CircleCollider2D>();
        c.radius = 0.1f;
        ColliderDistance2D dist = col.Distance(c);
        Object.Destroy(go);
        return dist.pointA;
    }
}
