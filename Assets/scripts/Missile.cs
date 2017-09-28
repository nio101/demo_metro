using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    public float thrust;
    public float rotationRate;
    public float limit_Y_low;
    private GameObject closest_enemy = null;
    private Rigidbody2D rb;
    private bool locked;
    private bool exploding;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        Invoke("FindClosestEnemy", 0.1f);
        locked = false;
        exploding = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // find closest enemy
    void FindClosestEnemy()
    {
        GameObject[] enemies;
        GameObject closest = null;
        enemies = GameObject.FindGameObjectsWithTag("enemies");
        var distance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = enemy.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && enemy.transform.position.y >= limit_Y_low)
            {
                closest = enemy;
                distance = curDistance;
            }
        }
        if (closest != null)
        {
            closest_enemy = closest;
            locked = true;
            //Debug.Log("ennemi trouvé");
            //Debug.Log(closest_enemy);
        }
        else
        {
            Invoke("FindClosestEnemy", 0.5f);
        }
    }

    void FixedUpdate()
    {
        if (locked)
        {
            if (closest_enemy == null)
            {
                // ennemi a été détruit
                locked = false;
                Invoke("FindClosestEnemy", 0.5f);
            }
            else
            {
                //transform.LookAt(closest_enemy.transform);
                Vector3 diff = closest_enemy.transform.position - transform.position;
                diff.Normalize();
                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                rb.AddForce(transform.up * thrust);
                //Debug.Log("direction ennemi");

                Vector3 toTarget = closest_enemy.transform.position - transform.position;
                Vector3 newVelocity = Vector3.RotateTowards(rb.velocity, toTarget, rotationRate * Mathf.Deg2Rad * Time.fixedDeltaTime, 0);
                newVelocity.z = 0;
                rb.velocity = newVelocity;
            }
        }
        else
        {
            //Debug.Log("pas d'ennemi détecté");
            rb.AddForce(transform.up * thrust);
            //transform.GetComponent<Rigidbody>().AddForce(transform.forward * thrust);
        }
    }

    // Function called when the object goes out of the screen
    void OnBecameInvisible()
    {
        Explode();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "enemies")
        {
            other.gameObject.SendMessage("ApplyDamage", 10);
            Explode();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        Debug.Log("TRIGGER!");
        if (other.gameObject.tag == "enemies")
        {
            other.gameObject.SendMessage("ApplyDamage", 10);
            Explode();
        }
        //else if (other.gameObject.tag == "rockets")
        //other.gameObject.SendMessage("Explode");
        //Explode();
        */
    }

    void Explode()
    {
        if (exploding == false)
        {
            exploding = true;
            //var exp = GetComponent<ParticleSystem>();
            //exp.Play();
            //Destroy(gameObject, exp.duration);
            transform.parent.gameObject.GetComponent<MissileLauncher>().SendMessage("missile_destroyed");
            Destroy(gameObject, 0.2f);
        }
    }
}
