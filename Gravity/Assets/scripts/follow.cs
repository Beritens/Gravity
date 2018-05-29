using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public float smoothSpeed;
    Camera cam;
    public SpriteRenderer[] bg;
    List<Vector2> SpriteStuff = new List<Vector2>();
    public float[] parralax;
    public Vector2[] bgOffset;
    public Color[] tint;
   /* public float minZoom = 60f;
    public float maxZoom = 10f;
    Bounds bounds;
    public float zoomLimiter = 50f;
    Vector2 MousePos;
    Vector3 velocity;
    Bounds boundy;*/
    private void Start()
    {
        cam = GetComponent<Camera>();
        for(int i = 0; i < bg.Length; i++)
        {
            SpriteStuff.Add(new Vector2(bg[i].sprite.rect.width / bg[i].sprite.pixelsPerUnit, bg[i].sprite.rect.height / bg[i].sprite.pixelsPerUnit));
            bg[i].material.color = tint[i];
        }
        
    }

    private void Update()
    {
        
        
        
        transform.position = target.position+offset;
        for (int i = 0; i < bg.Length; i++)
        {
            bg[i].material.SetVector("_Offset", new Vector4((transform.position.x+bgOffset[i].x) / SpriteStuff[i].x * parralax[i], (transform.position.y + bgOffset[i].y) / SpriteStuff[i].y * parralax[i], 0, 0));
        }
            
        
    }

    
}
