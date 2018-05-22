using UnityEngine;
using System.Collections;
public class collectable : MonoBehaviour{

    public float Health;
    public float Energy;
    public GameObject particles;
    GameObject bob;
    public void destroy()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        bob = GameObject.Instantiate(particles, transform.position, Quaternion.Euler(0, 0, 0));
        bob.GetComponent< ParticleSystemRenderer>().material.mainTexture = GetComponent<SpriteRenderer>().sprite.texture;
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        Destroy(bob);
        Destroy(this.gameObject);
        if (GetComponent<attracted>())
        {
            GameObject.FindObjectOfType<controls>().attractedObj.Remove(GetComponent<attracted>());
        }
    }

}
