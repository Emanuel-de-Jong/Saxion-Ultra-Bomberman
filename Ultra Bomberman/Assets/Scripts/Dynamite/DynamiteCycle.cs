using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteCycle : MonoBehaviour
{

    public GameObject Dynamite;
    public GameObject Explosion;
    public GameObject Flame;
    public int explosionDelay;

    public int rotationZ;

    private GameObject temp_flame;
    private GameObject temp_explosion;

    private float timeTillExplosion;

    void Start()
    {
        timeTillExplosion = Time.time + explosionDelay;
        spawnFlame();
    }

    void spawnFlame()
    {
        temp_flame = Instantiate(Flame, new Vector3(transform.position.x, 0.65f, transform.position.z), Quaternion.Euler(new Vector3(0f, rotationZ, 0)));
        temp_flame.transform.position += temp_flame.transform.forward * 0.7f;
    }

    void Update()
    {
        if (timeTillExplosion <= Time.time)
        {
            Instantiate(Explosion, new Vector3(transform.position.x, 0.65f, transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)));

            Destroy(temp_flame);
            Destroy(gameObject);
        }
    }
}
