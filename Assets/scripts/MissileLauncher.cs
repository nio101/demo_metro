using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour {

    public int ship_class;  // 0, 1, 2 (type de vaisseau / fréquence de tir)
    public int max_buffer;      // buffer de tir max (1-5)
    public float cadence_tir_init;
    public float cadence_tir;
    public GameObject prefabMissile;

    private int buffer;
    private GameObject ship1, ship2, ship3;
    private GameObject launch_point;
    private GameObject[] icon = new GameObject[5];

	// Use this for initialization
	void Start () {
        //Debug.Log(name);
        //ship1 = GameObject.Find("./ship1");
        foreach (Transform t in transform)
        {
            //Debug.Log(t.name);
            switch(t.name)
            {
                case "ship1":
                    ship1 = t.gameObject;
                    break;
                case "ship2":
                    ship2 = t.gameObject;
                    break;
                case "ship3":
                    ship3 = t.gameObject;
                    break;
                case "launch_point":
                    launch_point = t.gameObject;
                    break;
                case "icon1":
                    icon[0] = t.gameObject;
                    break;
                case "icon2":
                    icon[1] = t.gameObject;
                    break;
                case "icon3":
                    icon[2] = t.gameObject;
                    break;
                case "icon4":
                    icon[3] = t.gameObject;
                    break;
                case "icon5":
                    icon[4] = t.gameObject;
                    break;
            }
        }
        update_ship_class();
        buffer = max_buffer;
        update_max_icons();
        update_icons();
        //Debug.Log("** mL start ***");
        Debug.Log("invoke du start");
        Invoke("ItsLaunchTime", Random.Range(0.5f, 1.0f));
    }

    void ItsLaunchTime()
    {
        if (buffer > 0)
        {
            Debug.Log(buffer);
            buffer -= 1;
            update_icons();
            GameObject MissileInstance;
            MissileInstance = Instantiate(prefabMissile, launch_point.transform.position, Quaternion.identity) as GameObject;
            MissileInstance.transform.parent = transform;
            MissileInstance.transform.name = "missile";
            MissileInstance.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 2.0f, ForceMode2D.Impulse);
            //MissileInstance.GetComponent<Rigidbody2D>().AddForce(Vector3.up * Random.Range(10.0f, 50.0f));
        }
        Invoke("ItsLaunchTime", 60.0f / cadence_tir);
    }

    /*void stop_launcher()
    {
        CancelInvoke();
        // kill all missiles & reset buffer
        foreach (Transform t in transform)
        {
            if (t.name == "missile")
            {
                t.gameObject.GetComponent<Missile>().SendMessage("Explode");
                
            }
        }
    }*/

    /*void start_launcher()
    {
        Debug.Log("starting_launcher)");
        buffer = max_buffer;
        update_max_icons();
        update_icons();
        Invoke("ItsLaunchTime", periode_tir_init);
    }*/

    void update_max_icons()
    {
        for (int i=0; i<=(max_buffer-1); i++)
            icon[i].SetActive(true);
        for (int i=max_buffer; i<=4; i++)
            icon[i].SetActive(false);
    }

    void update_icons()
    {
        for (int i = 0; i <= (buffer - 1); i++)
        {
            Color col = icon[i].GetComponent<SpriteRenderer>().color;
            col.a = 1f;
            icon[i].GetComponent<SpriteRenderer>().color = col;
        }
        for (int i = buffer; i <= 4; i++)
        {
            Color col = icon[i].GetComponent<SpriteRenderer>().color;
            col.a = 0.5f;
            icon[i].GetComponent<SpriteRenderer>().color = col;
        }
    }

    void update_ship_class()
    {
        switch (ship_class)
        {
            case 0:
                ship1.SetActive(true);
                ship2.SetActive(false);
                ship3.SetActive(false);
                break;
            case 1:
                ship1.SetActive(false);
                ship2.SetActive(true);
                ship3.SetActive(false);
                break;
            case 2:
                ship1.SetActive(false);
                ship2.SetActive(false);
                ship3.SetActive(true);
                break;
        }
    }

    void kill_all_missiles()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "missile")
                GameObject.Destroy(child.gameObject);
        }
    }

    void SetCadence(int cadence)
    {
        cadence_tir = cadence;
        Debug.Log(cadence_tir);
    }

    void SetMunitions(int max)
    {
        max_buffer = max;
        buffer = max_buffer;
        kill_all_missiles();
        update_icons();
        update_max_icons();
    }

    void missile_destroyed()
    {
        buffer += 1;
        if (buffer > max_buffer)
            buffer = max_buffer;
        update_icons();
    }

	// Update is called once per frame
	void Update () {

    }
}
