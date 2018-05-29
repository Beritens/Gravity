using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class controls : MonoBehaviour {

    Camera cam;
    [Header("detecting stuff")]
    public Rigidbody2D movingPlatform;
    public Vector2 colliderSize;
    public Vector2 BoxCastPos;
    public float wallDistance;
    bool grounded;
    public Vector2 groundCirclePos;
    public float groundCircleRadius = 0.5f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    [Space(10)]
    [Header("actual controls")]
    public float MaxSpeedGround;
    public float MaxSpeedAir;
    public float speedGround;
    public float speedAir;
    public float jumpForce;
    public float friction;
    public float shortJump;
    Rigidbody2D rb;
    bool jumping = false;
    bool probablyJumping = false;
    public Animator anim;
    public float MaxHealth;
    public float health;
    public bar healthBar;
    
    
    
    
    
    [Space(10)]
    [Header("gravity stuff")]
    public List<attracted> attractedObj;
    const float G = 1f;
    public float attractorMass = 10f;
    public float MaxAttractorMass;
    public float RWMEO = 2f;
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
    string number = "";
    TextMeshProUGUI numberText;

    
    public bar attractorMassBar;

    Vector2 MousePos;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        energyBar = GameObject.Find("energyBar").GetComponent<bar>();
        attractorMassBar = GameObject.Find("energyUsageBar").GetComponent<bar>();
        healthBar = GameObject.Find("healthBar").GetComponent<bar>();
        energyBar.changeValue(energy , MaxEnergy);
        attractorMassBar.changeValue(attractorMass , MaxAttractorMass);
        healthBar.changeValue(health , MaxHealth);
        numberText = GameObject.Find("attractorMass").GetComponent<TextMeshProUGUI>();
    }
	
	// Update is called once per frame
	void Update () {
        bool previouslyGrounded = grounded;
        grounded = checkGrounded();
        if(!previouslyGrounded && grounded)
        {
            jumping = false;
        }
        if(previouslyGrounded && !grounded && probablyJumping){
            jumping = true;
            probablyJumping = false;
        }
        anim.SetBool("grounded", grounded);
        if (Input.GetButtonDown("Jump") && grounded && !jumping)
        {
            probablyJumping = true;
            rb.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);
        }
        
        
        
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            attractorMass = Mathf.Clamp(attractorMass + Input.GetAxis("Mouse ScrollWheel")*MaxAttractorMass, 0, MaxAttractorMass);
            attractorMassBar.changeValue(attractorMass , MaxAttractorMass);
        }
        NumberStuff();
        
    }
    void LateUpdate()
    {
        MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if(cursorAnim != null)
        {
            cursorAnim.transform.position = MousePos;
        }
        ArmStuff();
    }
    void FixedUpdate()
    {
        
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
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "collectable")
        {
            collectable col = collision.gameObject.GetComponent<collectable>();
           
            if (col.MaxEnergy != 0)
            {
                changeMaxEnergy(col.MaxEnergy);
            }
            if (col.Energy != 0)
            {
                changeEnergy(col.Energy);

            }
            if (col.MaxAttractorMass != 0)
            {
                changeMaxAttractorMass(col.MaxAttractorMass);
            }
            
            col.destroy();

        }
        else if(collision.gameObject.tag == "death")
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        //if (checkGrounded())
        //   jumping = false;
    }
    #region character
    bool checkGrounded()
    {
        Collider2D ground = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y) + groundCirclePos,groundCircleRadius,groundLayer);
        if (ground != null && ground.tag == "movingPlatform")
            movingPlatform = ground.GetComponent<Rigidbody2D>();
        else if(ground != null)
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
                if(movingPlatform == null)
                {
                    lerp = (input * MaxSpeed - rb.velocity.x) / (input * MaxSpeed);
                }
                else
                {
                    lerp = (input * MaxSpeed - (rb.velocity.x - movingPlatform.velocity.x)) / (input * MaxSpeed);
                }
                
                float movement = input * lerp * speed;

                if((movement>0) == (input > 0))
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
            hit = Physics2D.BoxCast((Vector2)transform.position+BoxCastPos, colliderSize, 0,new Vector2(1,0),wallDistance,wallLayer);
            if (hit.collider != null)
            {
                hitWall = true;
                
            }
        }
        else if (input < 0)
        {
            hit = Physics2D.BoxCast((Vector2)transform.position + BoxCastPos, colliderSize, 0, new Vector2(-1, 0), wallDistance, wallLayer);
            if (hit.collider != null)
            {
                hitWall = true;
                
            }
        }

        return hitWall;
    }
    #endregion
    #region gravity
    void attract(int lol)
    {
        MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        float attractMass = attractorMass;
        if(attractMass > energy)
        {
            attractMass = energy;
        }
        Collider2D[] inRadius = Physics2D.OverlapCircleAll(MousePos, RWMEO * attractorMass);
        for (int i = 0; i < inRadius.Length; i++)
        {
            if (!inRadius[i].GetComponent<attracted>())
                continue;
            Collider2D col = inRadius[i].GetComponent<Collider2D>();
            if (col.OverlapPoint(MousePos))
            {
                continue;
            }

            Rigidbody2D rba = inRadius[i].GetComponent<Rigidbody2D>();
            //Vector2 pos = col.bounds.ClosestPoint(MousePos);
            Vector2 pos = ClosestPoint(col, MousePos);
            Vector2 direction = MousePos - pos;
            float Gravdistance = direction.magnitude;
            if (Gravdistance == 0)
            {
                continue;
            }
            float minDis = inRadius[i].GetComponent<attracted>().MinDistance;
            Gravdistance += minDis;
            if (Gravdistance < jumpBreakDistance && rba.GetComponent<controls>() != null)
            {
                rba.GetComponent<controls>().jumping = false;
            }



            float forceMagnitude = G * (attractMass * rba.mass) / Mathf.Pow(Gravdistance, 2);
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
        
        animSpeed = Mathf.Clamp(attractMass* 0.15f, 0.5f, 3)*lol;
        changeEnergy(-attractMass);
        cursorAnim.SetFloat("strength", animSpeed);
        cursorAnim.GetComponent<WindZone>().windMain = lol*20;
        cursorAnim.GetComponent<WindZone>().radius = attractMass *1.5f;
        if(attractMass == 0)
        {
            cursorAnim.gameObject.SetActive(false);
        }
        else
        {
            cursorAnim.gameObject.SetActive(true);
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
    void NumberStuff()
    {
        if (Input.anyKeyDown)
        {
            
            
            if (Input.GetKeyDown("."))
            {
                if (!number.Contains("."))
                {
                    number += ".";
                    numberText.text = number;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Backspace) && number.Length >0)
            {
                print("hi");
                number = number.Remove(number.Length - 1);

                numberText.text = number;
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Input.GetKeyDown(i.ToString()))
                    {
                        number += i.ToString();
                        numberText.text = number;
                    }
                }
            }
            
            if (Input.GetButtonDown("confirm") && number != "")
            {
                attractorMass = Mathf.Clamp(float.Parse(number), 0, MaxAttractorMass);
                attractorMassBar.changeValue(attractorMass , MaxAttractorMass);
                number = "";
                numberText.text = number;
            }
        }
        
        
        
    }
    void changeEnergy(float newEnergy)
    {
        energy = Mathf.Clamp(energy + newEnergy, 0, MaxEnergy);
        energyBar.changeValue(energy, MaxEnergy);
    }
    void changeMaxEnergy(float newMaxEnergy)
    {
        MaxEnergy += newMaxEnergy;
        energyBar.changeValue(energy, MaxEnergy);
    }
    void changeMaxAttractorMass(float newMaxAttractorMass)
    {
        MaxAttractorMass += newMaxAttractorMass;
        attractorMassBar.changeValue(attractorMass, MaxAttractorMass);
    }
    #endregion
}
