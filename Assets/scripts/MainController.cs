using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;

public class MainController : MonoBehaviour
{
    public GameObject prefabMissileLauncher;
    public GameObject spawn11;
    public GameObject spawn21;
    public GameObject spawn22;
    public GameObject spawn31;
    public GameObject spawn32;
    public GameObject spawn33;
    public GameObject spawn41;
    public GameObject spawn42;
    public GameObject spawn43;
    public GameObject spawn44;
    public GameObject prefabType1;
    public GameObject prefabType2;
    public GameObject prefabType3;
    public float cadence_initiale_tir;
    private float cadence_tir;
    public int munitions_init;
    private int munitions;
    private GameObject missile_launcher1;
    private GameObject missile_launcher2;
    private GameObject missile_launcher3;
    private GameObject missile_launcher4;
    public int nombre_bases_initial;
    public float y_spawn_value;
    public float minType1force;
    public float maxType1force;
    public float type1SpawnParMinute;
    public Text type1RateText;
    public float minType2force;
    public float maxType2force;
    public float type2SpawnParMinute;
    public Text type2RateText;
    public float minType3force;
    public float maxType3force;
    public float type3SpawnParMinute;
    public Text type3RateText;
    private int nombre_bases;   // de 1 à 4
    public Text cadenceText;

    // Use this for initialization
    void Start()
    {
        nombre_bases = nombre_bases_initial;
        cadence_tir = cadence_initiale_tir;
        munitions = munitions_init;
        /*missile_launcher1 = GameObject.Find("missile_launcher1");
        missile_launcher2 = GameObject.Find("missile_launcher2");
        missile_launcher3 = GameObject.Find("missile_launcher3");
        missile_launcher4 = GameObject.Find("missile_launcher4");*/
        update_bases();
        //spawnType1 = GameObject.Find("Spawn_Type1");
        //InvokeRepeating("SpawnMissile1", 1f, 1.0f);
        //InvokeRepeating("SpawnMissile2", 0.5f, 2.0f);
        Invoke("SpawnType1", 60.0f / type1SpawnParMinute);
        Invoke("SpawnType2", 60.0f / type2SpawnParMinute);
        Invoke("SpawnType3", 60.0f / type3SpawnParMinute);
        //Invoke("SpawnType1", 60.0f / type3SpawnParMinute);
        type1RateText.text = type1SpawnParMinute.ToString() + "/mn";
        type2RateText.text = type2SpawnParMinute.ToString() + "/mn";
        type3RateText.text = type3SpawnParMinute.ToString() + "/mn";
        cadenceText.text = cadence_tir.ToString() + "/mn";
    }

    void update_bases()
    {
        // recreate / spawn missile_launchers
        // peut-être 0 si plus de connection avec le module "earth"...
        switch (nombre_bases)
        {
            case 0:
                if (missile_launcher1 != null)
                    GameObject.Destroy(missile_launcher1.gameObject);
                if (missile_launcher2 != null)
                    GameObject.Destroy(missile_launcher2.gameObject);
                if (missile_launcher3 != null)
                    GameObject.Destroy(missile_launcher3.gameObject);
                if (missile_launcher4 != null)
                    GameObject.Destroy(missile_launcher4.gameObject);
                break;
            case 1:
                if (missile_launcher1 == null)
                {
                    missile_launcher1 = Instantiate(prefabMissileLauncher, spawn11.transform.position, Quaternion.identity) as GameObject;
                }
                else
                    missile_launcher1.transform.position = spawn11.transform.position;
                if (missile_launcher2 != null)
                    GameObject.Destroy(missile_launcher2.gameObject);
                if (missile_launcher3 != null)
                    GameObject.Destroy(missile_launcher3.gameObject);
                if (missile_launcher4 != null)
                    GameObject.Destroy(missile_launcher4.gameObject);
                break;
            case 2:
                if (missile_launcher1 == null)
                    missile_launcher1 = Instantiate(prefabMissileLauncher, spawn21.transform.position, Quaternion.identity) as GameObject;
                else
                    missile_launcher1.transform.position = spawn21.transform.position;
                if (missile_launcher2 == null)
                    missile_launcher2 = Instantiate(prefabMissileLauncher, spawn22.transform.position, Quaternion.identity) as GameObject;
                else
                    missile_launcher2.transform.position = spawn22.transform.position;
                if (missile_launcher3 != null)
                    GameObject.Destroy(missile_launcher3.gameObject);
                if (missile_launcher4 != null)
                    GameObject.Destroy(missile_launcher4.gameObject);
                break;
            case 3:
                if (missile_launcher1 == null)
                    missile_launcher1 = Instantiate(prefabMissileLauncher, spawn31.transform.position, Quaternion.identity) as GameObject;
                else
                    missile_launcher1.transform.position = spawn31.transform.position;
                if (missile_launcher2 == null)
                    missile_launcher2 = Instantiate(prefabMissileLauncher, spawn32.transform.position, Quaternion.identity) as GameObject;
                else
                    missile_launcher2.transform.position = spawn32.transform.position;
                if (missile_launcher3 == null)
                    missile_launcher3 = Instantiate(prefabMissileLauncher, spawn33.transform.position, Quaternion.identity) as GameObject;
                else
                    missile_launcher3.transform.position = spawn33.transform.position;
                if (missile_launcher4 != null)
                    GameObject.Destroy(missile_launcher4.gameObject);
                break;
            case 4:
                if (missile_launcher1 == null)
                    missile_launcher1 = Instantiate(prefabMissileLauncher, spawn41.transform.position, Quaternion.identity) as GameObject;
                else
                    missile_launcher1.transform.position = spawn41.transform.position;
                if (missile_launcher2 == null)
                    missile_launcher2 = Instantiate(prefabMissileLauncher, spawn42.transform.position, Quaternion.identity) as GameObject;
                else
                    missile_launcher2.transform.position = spawn42.transform.position;
                if (missile_launcher3 == null)
                    missile_launcher3 = Instantiate(prefabMissileLauncher, spawn43.transform.position, Quaternion.identity) as GameObject;
                else
                    missile_launcher3.transform.position = spawn43.transform.position;
                if (missile_launcher4 == null)
                    missile_launcher4 = Instantiate(prefabMissileLauncher, spawn44.transform.position, Quaternion.identity) as GameObject;
                else
                    missile_launcher4.transform.position = spawn44.transform.position;
                break;
        }
        SendCadence2Launchers();
        SendMunitions2Launchers();
    }

    void SpawnType1()
    {
        GameObject type1Instance;
        Vector3 pos;
        pos.y = y_spawn_value;
        pos.z = 0;
        pos.x = -6.0f;
        type1Instance = Instantiate(prefabType1, pos, Quaternion.identity) as GameObject;
        type1Instance.GetComponent<Type1>().thrust = Random.Range(minType1force, maxType1force);
        //type1Instance.GetComponent<Type1>().thrust = 10.0f;
        type1Instance.GetComponent<Type1>().limit_X_right = 4.0f;
        type1Instance.GetComponent<Type1>().limit_X_left = -4.0f;
        //type1Instance.GetComponent<Rigidbody2D>().AddForce(Vector3.right * Random.Range(0.0f, 15.0f));
        type1Instance.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-0.25f,0.25f));
        Invoke("SpawnType1", 60.0f / type1SpawnParMinute);
    }

    void SpawnType2()
    { 
        GameObject type2Instance;
        Vector3 pos;
        pos.y = y_spawn_value;
        pos.z = 0;
        pos.x = 0;
        type2Instance = Instantiate(prefabType2, pos, Quaternion.identity) as GameObject;
        type2Instance.GetComponent<Type2>().thrust = Random.Range(minType2force, maxType2force);
        //type1Instance.transform.localScale = new Vector3(.1f, .1f, 1f);
        type2Instance.GetComponent<Rigidbody2D>().AddForce(Vector3.right * Random.Range(500.0f, 600.0f));
        type2Instance.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-0.15f, 0.15f));
        Invoke("SpawnType2", 60.0f / type2SpawnParMinute);
    }

    void SpawnType3()
    {
        GameObject type3Instance;
        Vector3 pos;
        pos.y = y_spawn_value;
        pos.z = 0;
        pos.x = 0;
        type3Instance = Instantiate(prefabType3, pos, Quaternion.identity) as GameObject;
        type3Instance.GetComponent<Type3>().thrust = Random.Range(minType3force, maxType3force);
        //type1Instance.transform.localScale = new Vector3(.1f, .1f, 1f);
        if (Random.Range(-1f, 1f)>0)
            type3Instance.GetComponent<Rigidbody2D>().AddForce(Vector3.right * Random.Range(20.0f, 100.0f));
        else
            type3Instance.GetComponent<Rigidbody2D>().AddForce(Vector3.right * Random.Range(-20.0f, -100.0f));
        type3Instance.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-0.15f, 0.15f));
        Invoke("SpawnType3", 60.0f / type3SpawnParMinute);
    }

    void kill_all_enemies()
    {
        // kill all enemies
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("enemies");
        var distance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
            GameObject.Destroy(enemy.gameObject);
    }

    void SendCadence2Launchers()
    {
        if (missile_launcher1 != null)
            missile_launcher1.GetComponent<MissileLauncher>().SendMessage("SetCadence", cadence_tir);
        if (missile_launcher2 != null)
            missile_launcher2.GetComponent<MissileLauncher>().SendMessage("SetCadence", cadence_tir);
        if (missile_launcher3 != null)
            missile_launcher3.GetComponent<MissileLauncher>().SendMessage("SetCadence", cadence_tir);
        if (missile_launcher4 != null)
            missile_launcher4.GetComponent<MissileLauncher>().SendMessage("SetCadence", cadence_tir);
    }

    void SendMunitions2Launchers()
    {
        if (missile_launcher1 != null)
            missile_launcher1.GetComponent<MissileLauncher>().SendMessage("SetMunitions", munitions);
        if (missile_launcher2 != null)
            missile_launcher2.GetComponent<MissileLauncher>().SendMessage("SetMunitions", munitions);
        if (missile_launcher3 != null)
            missile_launcher3.GetComponent<MissileLauncher>().SendMessage("SetMunitions", munitions);
        if (missile_launcher4 != null)
            missile_launcher4.GetComponent<MissileLauncher>().SendMessage("SetMunitions", munitions);
    }

    void reset_scene()
    {
        kill_all_enemies();
        if (missile_launcher1 != null)
            GameObject.Destroy(missile_launcher1.gameObject);
        if (missile_launcher2 != null)
            GameObject.Destroy(missile_launcher2.gameObject);
        if (missile_launcher3 != null)
            GameObject.Destroy(missile_launcher3.gameObject);
        if (missile_launcher4 != null)
            GameObject.Destroy(missile_launcher4.gameObject);
        missile_launcher1 = null;
        missile_launcher2 = null;
        missile_launcher3 = null;
        missile_launcher4 = null;
        update_bases();
    }

    void update_earth_bases(int b)
    {
        if (b != nombre_bases)
        {
            nombre_bases = b;
            update_bases();
        }
    }

    void update_earth_munitions(int m)
    {
        if (m != munitions)
        {
            munitions = m;
            SendMunitions2Launchers();
        }
    }

    void update_earth_cadence(int c)
    {
        if (c != cadence_tir)
        {
            cadence_tir = c;
            SendCadence2Launchers();
            cadenceText.text = cadence_tir.ToString() + "/mn";
        }
    }

    void update_alien1(int c)
    {
        if (c != type1SpawnParMinute)
        {
            type1SpawnParMinute = c;
            type1RateText.text = type1SpawnParMinute.ToString() + "/mn";
            CancelInvoke("SpawnType1");
            Invoke("SpawnType1", 60.0f / type1SpawnParMinute);
        }
    }

    void update_alien2(int c)
    {
        if (c != type2SpawnParMinute)
        {
            type2SpawnParMinute = c;
            type2RateText.text = type2SpawnParMinute.ToString() + "/mn";
            CancelInvoke("SpawnType2");
            Invoke("SpawnType2", 60.0f / type2SpawnParMinute);
        }
    }

    void update_alien3(int c)
    {
        if (c != type3SpawnParMinute)
        {
            type3SpawnParMinute = c;
            type3RateText.text = type3SpawnParMinute.ToString() + "/mn";
            CancelInvoke("SpawnType3");
            Invoke("SpawnType3", 60.0f / type3SpawnParMinute);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            type1SpawnParMinute += 10;
            type1RateText.text = type1SpawnParMinute.ToString() + "/mn";
            CancelInvoke("SpawnType1");
            Invoke("SpawnType1", 60.0f / type1SpawnParMinute);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            type1SpawnParMinute -= 10;
            type1RateText.text = type1SpawnParMinute.ToString() + "/mn";
            CancelInvoke("SpawnType1");
            Invoke("SpawnType1", 60.0f / type1SpawnParMinute);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            type2SpawnParMinute += 10;
            type2RateText.text = type2SpawnParMinute.ToString() + "/mn";
            CancelInvoke("SpawnType2");
            Invoke("SpawnType2", 60.0f / type2SpawnParMinute);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            type2SpawnParMinute -= 10;
            type2RateText.text = type2SpawnParMinute.ToString() + "/mn";
            CancelInvoke("SpawnType2");
            Invoke("SpawnType2", 60.0f / type2SpawnParMinute);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            type3SpawnParMinute += 10;
            type3RateText.text = type3SpawnParMinute.ToString() + "/mn";
            CancelInvoke("SpawnType3");
            Invoke("SpawnType3", 60.0f / type3SpawnParMinute);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            type3SpawnParMinute -= 10;
            type3RateText.text = type3SpawnParMinute.ToString() + "/mn";
            CancelInvoke("SpawnType3");
            Invoke("SpawnType3", 60.0f / type3SpawnParMinute);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            nombre_bases++;
            if (nombre_bases > 4)
                nombre_bases = 4;
            //kill_all_enemies();
            update_bases();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            nombre_bases--;
            if (nombre_bases < 0)
                nombre_bases = 0;
            //kill_all_enemies();
            update_bases();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            cadence_tir+=10;
            SendCadence2Launchers();
            cadenceText.text = cadence_tir.ToString() + "/mn";
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            cadence_tir -= 10;
            SendCadence2Launchers();
            cadenceText.text = cadence_tir.ToString() + "/mn";
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            munitions += 1;
            if (munitions > 5)
                munitions = 5;
            SendMunitions2Launchers();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            munitions -= 1;
            if (munitions < 1)
                munitions = 1;
            SendMunitions2Launchers();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            reset_scene();
        }
    }
}
