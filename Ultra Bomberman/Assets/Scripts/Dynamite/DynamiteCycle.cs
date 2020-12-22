using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteCycle : MonoBehaviour
{

    public GameObject Dynamite;
    public GameObject Explosion;
    public GameObject Trail;
    public GameObject Flame;
    public int explosionDelay;

    public int rotationZ;

    private GameObject temp_flame;

    private float timeTillExplosion;
    public float bonusRange = 0;
    public static float defaultTrailDistance = 0.6f;

    private float forwardTrailDistance = defaultTrailDistance;
    private float rightTrailDistance = defaultTrailDistance;
    private float bottomTrailDistance = defaultTrailDistance;
    private float leftTrailDistance = defaultTrailDistance;

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
            Instantiate(Explosion, new Vector3(transform.position.x, 0.65f, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 0)));

            spawnTrail();

            Destroy(temp_flame);
            Destroy(gameObject);
        }

        Debug.DrawRay(transform.position, Vector3.forward * 10, Color.red);
        RaycastHit forwardHitInfo;

        if (Physics.Raycast(transform.position, Vector3.forward, out forwardHitInfo)) 
        {
            if ((forwardHitInfo.distance / 1.5) < defaultTrailDistance + bonusRange)
            {
                forwardTrailDistance = forwardHitInfo.distance / 2;
                Debug.Log(forwardHitInfo.distance);
            }
        }

        Debug.DrawRay(transform.position, Vector3.right * 10, Color.red);
        RaycastHit rightHitInfo;

        if (Physics.Raycast(transform.position, Vector3.right, out rightHitInfo))
        {
            Debug.Log(rightHitInfo.distance);
        }

        Debug.DrawRay(transform.position, Vector3.back * 10, Color.red);
        RaycastHit backHitInfo;

        if (Physics.Raycast(transform.position, Vector3.back, out backHitInfo))
        {
            Debug.Log(backHitInfo.distance);
        }

        Debug.DrawRay(transform.position, Vector3.left * 10, Color.red);
        RaycastHit leftHitInfo;

        if (Physics.Raycast(transform.position, Vector3.back, out leftHitInfo))
        {
            Debug.Log(leftHitInfo.distance);
        }
    }

    void spawnTrail()
    {
        GameObject topTrail = Instantiate(Trail, new Vector3(transform.position.x, 0.65f, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        GameObject rightTrail = Instantiate(Trail, new Vector3(transform.position.x, 0.65f, transform.position.z), Quaternion.Euler(new Vector3(0, 90, 0)));
        GameObject bottomTrail = Instantiate(Trail, new Vector3(transform.position.x, 0.65f, transform.position.z), Quaternion.Euler(new Vector3(0, 180, 0)));
        GameObject leftTrail = Instantiate(Trail, new Vector3(transform.position.x, 0.65f, transform.position.z), Quaternion.Euler(new Vector3(0, 270, 0)));

        topTrail.transform.localScale = new Vector3(1, 1, forwardTrailDistance + bonusRange);
        rightTrail.transform.localScale = new Vector3(1, 1, rightTrailDistance + bonusRange);
        bottomTrail.transform.localScale = new Vector3(1, 1, bottomTrailDistance + bonusRange);
        leftTrail.transform.localScale = new Vector3(1, 1, leftTrailDistance + bonusRange);
    }
}
