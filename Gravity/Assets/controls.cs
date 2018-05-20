using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour {

    Camera cam;
    [Header("detecting stuff")]
    public Rigidbody2D movingPlatform;
    public Vector2 HalfExtends;
    public Vector2 BoxCastPos;
    public float distance;
    bool grounded;
    public Vector2 groundCirclePos;
    public float groundCircleRadius = 0.5f;
    public LayerMask groundLayer;
    [Space(10)]
    [Header("actual controls")]
    public float MaxSpeedGround;
    public float MaxSpeedAir;
    public float speedGround;
    public float speedAir;
    public float jumpForce;
    public float friction;
    public float fall;
    public float shortJump;
    Rigidbody2D rb;
    public bool jumping = false;
    public Animator anim;
    
    [Space(10)]
    [Header("gravity stuff")]
    public List<attracted> attractedObj;
    const float G = 1f;
    public float attractorMass = 10f;
    public float MinDistance;
    public float jumpBreakDistance;
    public Transform arm;
    public Sprite[] arms;
    public SpriteRenderer army;
    bool mouseDown = false;
    public GameObject cursorThing;
    Animator cursorAnim;
    public float MaxEnergy;
    public float energy;
    public bar energyBar;
    Vector2 MousePos;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        energyBar.changeValue(energy / MaxEnergy);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Jump") && grounded && !jumping)
        {
            jumping = true;
            rb.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);
        }
        MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        ArmStuff();
        if(cursorAnim != null)
        {
            cursorAnim.transform.position = MousePos;
        }
    }
    void FixedUpdate()
    {
        grounded = checkGrounded();
        Move();
        betterJump();
        
        
        
        if (Input.GetButton("Fire1"))
        {
            attract(1);
        }
        else if (Input.GetButton("Fire2"))
        {
            attract(-1);
        }
        else if(cursorAnim != null)
        {
            Destroy(cursorAnim.gameObject);
            mouseDown = false;
        }
    }
    #region character
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "collectable")
        {
            print("col");
            collectable col = collision.gameObject.GetComponent<collectable>();
            energy = Mathf.Clamp(energy+col.Energy,0,MaxEnergy);
            energyBar.changeValue(energy / MaxEnergy);
            col.destroy();

        }
        if(collision.gameObject.tag == "death")
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        if (checkGrounded())
            jumping = false;
    }
    bool checkGrounded()
    {
        Collider2D ground = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y) + groundCirclePos,groundCircleRadius,groundLayer);
        if (ground != null && ground.tag == "movingPlatform")
            movingPlatform = ground.GetComponent<Rigidbody2D>();
        else
            movingPlatform = null;
        return ground != null;


    }
    void betterJump()
    {
        if (grounded || !jumping)
            return;
        if(!Input.GetButton("Jump"))
        {
            rb.AddForce(Vector2.up * shortJump);
        }
    }
    void Move()
    {
        float input = Input.GetAxis("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(input));
        float lerp = 1f;
        if(input != 0)
        {
            if(input > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector2(transform.localScale.x*-1, transform.localScale.y);
            }
            else if (input < 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
            if (!WillHitWall(input))
            {
                float MaxSpeed;
                float speed;
                if (grounded)
                {
                    MaxSpeed = MaxSpeedGround;
                    speed = speedGround;
                }
                else
                {
                    MaxSpeed = MaxSpeedAir;
                    speed = speedAir;
                }
                lerp = (input * MaxSpeed - rb.velocity.x) / (input * MaxSpeed);
                float movement = input * lerp * speed;
                rb.AddForce(new Vector3(movement, 0f, 0f)/rb.mass, ForceMode2D.Force);
            }
        }
        if (grounded)
        {
            if(movingPlatform == null)
                rb.velocity = new Vector2(rb.velocity.x * friction, rb.velocity.y);
            else
            {
                rb.velocity = new Vector2((rb.velocity.x-movingPlatform.velocity.x) * friction+movingPlatform.velocity.x, rb.velocity.y);
            }
        }
            
    }
    bool WillHitWall(float input)
    {
        RaycastHit2D hit;
        bool hitWall = false;
        if (input > 0)
        {
            
            hit = Physics2D.BoxCast((Vector2)transform.position + BoxCastPos, HalfExtends, 0, transform.right, distance, groundLayer);
            if (hit.collider != null)
            {
                
                hitWall = true;
                
            }
        }
        else if (input < 0)
        {
            hit = Physics2D.BoxCast((Vector2)transform.position + BoxCastPos, HalfExtends, 0, -transform.right, distance, groundLayer);
            if (hit.collider != null)
            {
                
                hitWall = true;
                
            }
        }
        return hitWall;
    }
    #endregion
    void attract(int lol)
    {
        float attractMass = attractorMass;
        if(attractMass > energy)
        {
            attractMass = energy;
        }
        for (int i = 0; i < attractedObj.Count; i++)
        {
            Collider2D col = attractedObj[i].GetComponent<Collider2D>();
            if (col.OverlapPoint(MousePos))
            {
                continue;
            }

            Rigidbody2D rba = attractedObj[i].rb;
            //Vector2 pos = col.bounds.ClosestPoint(MousePos);
            Vector2 pos = ClosestPoint(col, MousePos);
            Vector2 direction = MousePos - pos;
            float distance = direction.magnitude;
            if (distance == 0)
            {
                continue;
            }
            if (distance < MinDistance)
                distance = MinDistance;
            if (distance < jumpBreakDistance && rba.GetComponent<controls>() != null)
            {
                rba.GetComponent<controls>().jumping = false;
            }



            float forceMagnitude = G * (attractMass * rba.mass) / Mathf.Pow(distance, 2);
            Vector2 force = direction.normalized * forceMagnitude * lol;
            
            rba.AddForceAtPosition(force, pos);
        }
        if (!mouseDown)
        {
            GameObject bob = GameObject.Instantiate(cursorThing, MousePos, Quaternion.identity);
            cursorAnim = bob.GetComponent<Animator>();
            mouseDown = true;
        }
        float animSpeed;
        animSpeed = Mathf.Clamp(attractMass * lol * 0.15f, -4, 4);
        energy -= Mathf.Clamp(attractMass,0,MaxEnergy);
        energyBar.changeValue(energy / MaxEnergy);

        if(animSpeed < 0.5f && animSpeed >= 0)
        {
            animSpeed = 0.5f;
        }
        else if (animSpeed > -0.5f && animSpeed < 0)
        {
            animSpeed = -0.5f;
        }
        cursorAnim.SetFloat("strength", animSpeed);
        
        
        
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
    void ArmStuff()
    {
       /* Vector2 direction = (MousePos - (Vector2)arm.position).normalized;
        arm.right = direction * transform.localScale.x;*/
        float AngleRad = Mathf.Atan2(MousePos.y - arm.position.y, MousePos.x - arm.position.x);
        
        if(transform.localScale.x < 0)
        {
            if(AngleRad < 0)
            {
                AngleRad += Mathf.PI;
            }
            else
            {
                AngleRad -= Mathf.PI;
            }
        }
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        float z = 0;

        if(AngleDeg > -45 && AngleDeg < 45)
        {
            z = 0; ;
        }
        else if (AngleDeg > 45 && AngleDeg < 135)
        {
            z = 90;
        }
        else if (AngleDeg > -135 && AngleDeg < -45)
        {
            z = -90;
        }
        else
        {
            z = 180;
        }
        arm.rotation = Quaternion.Euler(0,0, z);
        float newAngle = AngleDeg*transform.localScale.x;
        if (newAngle > 90)
            newAngle -= 90;
        else if(newAngle < -90)
        {
            newAngle += 90;
        }
        if(newAngle < 0)
        {
            newAngle += 90;
        }
        if(newAngle < 15)
        {
            army.sprite = arms[0];
        }
        else if (newAngle < 45)
        {
            army.sprite = arms[1];
        }
        else if (newAngle < 75)
        {
            army.sprite = arms[2];
        }
        else
        {
            army.sprite = arms[0];
        }
    }
}
