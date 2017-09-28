using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type1 : MonoBehaviour {

    private Rigidbody2D rb;
    public float thrust;

    public float limit_X_right;
    public float limit_X_left;
    public float limit_Y_low;
    public float scale_factor;
    public int life;

    float alpha_speed = .025f; //the speed that your alpha changes (between 0 and 1, I think)
    float scale_speed = .01f;
    private bool appearing;
    private bool disappearing;

    private SpriteRenderer spRend;
    
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        spRend= GetComponent<SpriteRenderer>();
        Color col = spRend.color;
        //  change col's alpha value (0 = invisible, 1 = fully opaque)
        col.a = -0.5f; // 0.5f = half transparent
                       // change the SpriteRenderer's color property to match the copy with the altered alpha value
        Vector3 scale = transform.localScale;
        scale.x = 0.1f;
        scale.y = 0.1f;
        transform.localScale = scale;
        spRend.color = col;
        appearing = true;
        //Debug.Log(scale_factor);
    }

    void ApplyDamage (int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Explode();
        }
    }

    private void Update()
    {
        if (appearing || disappearing)
        {
            // copy the SpriteRenderer's color property
            Color col = spRend.color;
            //  change col's alpha value (0 = invisible, 1 = fully opaque)
            if (appearing)
            {
                Vector3 scale = transform.localScale;
                scale.x += scale_speed;
                if (scale.x > scale_factor)
                { scale.x = scale_factor; }
                //Debug.Log(scale.x);
                scale.y += scale_speed;
                if (scale.y > scale_factor)
                { scale.y = scale_factor; }
                transform.localScale = scale;
                col.a += alpha_speed; // 0.5f = half transparent
                if (col.a > 1)
                {
                    col.a = 1;
                }
                if (scale.x == scale_factor && scale.y == scale_factor && col.a == 1)
                {
                    appearing = false;
                    //Debug.Log("THE END");
                }
            }
            else
            {
                col.a -= alpha_speed; // 0.5f = half transparent
                if (col.a < 0)
                {
                    col.a = 0;
                    disappearing = false;
                    Explode();
                }
            }

            // change the SpriteRenderer's color property to match the copy with the altered alpha value
            spRend.color = col;
        }
        if (transform.position.y < limit_Y_low)
        {
            disappearing = true;
        }
    }

    void FixedUpdate() {
        // Move the spaceship when an arrow key is pressed
        Vector3 right = transform.TransformDirection(Vector3.right);
        if (transform.position.x < limit_X_left)
            //rb.AddForce(transform.right * thrust);
            rb.AddForce(Vector3.right * thrust);
        else if (transform.position.x > limit_X_right)
            rb.AddForce(-Vector3.right * thrust);
        //Debug.Log(thrust);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log("COLLISION!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("TRIGGER!");
    }

    // Function called when the object goes out of the screen
    void OnBecameInvisible()
    {
        //enabled = false;
        Destroy(gameObject);
    }

    void Explode()
    {
        Destroy(gameObject, 0.2f);
        //Destroy(gameObject);
        //Debug.Log("TYPE1: THE END");
    }
}
