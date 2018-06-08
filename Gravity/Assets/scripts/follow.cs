using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public float smoothSpeed;
    Camera cam;
    //public SpriteRenderer[] bg;
    [System.Serializable]
    public struct bgstruct
    {
        public SpriteRenderer bg;
        public float parralax;
        public Vector2 bgOffset;
        public Color color;
    }
    List<Vector2> SpriteStuff = new List<Vector2>();
    public bgstruct[] bgs;
    /*public float[] parralax;
    public Vector2[] bgOffset;
    public Color[] tint;*/
    
   /* public float minZoom = 60f;
    public float maxZoom = 10f;
    Bounds bounds;
    public float zoomLimiter = 50f;
    Vector2 MousePos;
    Vector3 velocity;
    Bounds boundy;*/
    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        cam = GetComponent<Camera>();
        for(int i = 0; i < bgs.Length; i++)
        {
            SpriteStuff.Add(new Vector2(bgs[i].bg.sprite.rect.width / bgs[i].bg.sprite.pixelsPerUnit, bgs[i].bg.sprite.rect.height / bgs[i].bg.sprite.pixelsPerUnit));
            bgs[i].bg.material.color = bgs[i].color;
        }
        
    }

    private void Update()
    {
        
        
        
        transform.position = target.position+offset;
        for (int i = 0; i < bgs.Length; i++)
        {
            bgs[i].bg.material.SetVector("_Offset", new Vector4((transform.position.x+ bgs[i].bgOffset.x) / SpriteStuff[i].x * bgs[i].parralax, (transform.position.y + bgs[i].bgOffset.y) / SpriteStuff[i].y * bgs[i].parralax, 0, 0));
        }
            
        
    }

    
}
