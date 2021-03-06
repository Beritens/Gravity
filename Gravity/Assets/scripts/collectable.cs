﻿using UnityEngine;
using System.Collections;
public class collectable : MonoBehaviour{

    public float Health;
    public float Energy;
    public float MaxEnergy;
    public float MaxAttractorMass;
    public GameObject particles;
    GameObject bob;
    public void destroy()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        bob = GameObject.Instantiate(particles, transform.position, Quaternion.Euler(0, 0, 0));
        bob.GetComponent< ParticleSystemRenderer>().material.mainTexture = GetComponent<SpriteRenderer>().sprite.texture;
        if (GetComponent<WindZone>())
        {
            GetComponent<WindZone>().windMain = 0;
        }
        if (GetComponent<attract>())
        {
            GetComponent<attract>().enabled = false;
        }
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        Destroy(bob);
        Destroy(this.gameObject);
    }

}
