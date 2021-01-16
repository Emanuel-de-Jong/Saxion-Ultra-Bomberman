using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float range = 4f;

    [SerializeField] int trailLength = 9;
    [SerializeField] float explosionDelay = 3f;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject trail;
    [SerializeField] int[] raycastPoints = new int[] { 0, 90, 180, 270 };

    float[] trailDistances;
    GameObject[] trails;

    void Start()
    {
        trails = new GameObject[raycastPoints.Length];
        trailDistances = new float[raycastPoints.Length];
        Invoke(nameof(Explode), explosionDelay);
    }

    void Explode()
    {
        Instantiate(explosion, new Vector3(transform.position.x, explosion.transform.position.y, transform.position.z), explosion.transform.rotation);
        CastRays();
        SpawnTrail();
        Destroy(gameObject);
    }

    void CastRays()
    {
        Vector3 pos = new Vector3(transform.position.x, 0.5f, transform.position.z);

        Vector3 dir = Vector3.forward;
        RaycastHit hit;
        for (int i = 0; i < raycastPoints.Length; i++)
        {
            switch (i)
            {
                case 0:
                    dir = Vector3.forward;
                    break;
                case 1:
                    dir = Vector3.right;
                    break;
                case 2:
                    dir = Vector3.back;
                    break;
                case 3:
                    dir = Vector3.left;
                    break;
            }

            trailDistances[i] = range;
            if (Physics.Raycast(pos, dir, out hit, range))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    hit.transform.GetComponent<PlayerController>().TakeDamage();
                }
                else if (hit.transform.CompareTag("AI"))
                {
                    hit.transform.GetComponent<PlayerController>().TakeDamage();
                }
                else if (hit.transform.CompareTag("Destructible"))
                {
                    hit.transform.GetComponent<Destructible>().Break();
                }
                else
                {
                    if (hit.distance >= 1.2)
                    {
                        trailDistances[i] = hit.distance;
                    }
                    else
                    {
                        trailDistances[i] = 0;
                    }
                }
            }
        }
    }

    void SpawnTrail()
    {
        Vector3 trailPos = new Vector3(transform.position.x, trail.transform.position.y, transform.position.z);
        float xRotation = trail.transform.rotation.x, zRotation = trail.transform.rotation.z;
        float xScale = trail.transform.localScale.x, yScale = trail.transform.localScale.y;
        for (int i=0; i<raycastPoints.Length; i++)
        {
            if (trailDistances[i] == 0)
                continue;

            GameObject tempTrail = Instantiate(trail, trailPos, Quaternion.Euler(xRotation, raycastPoints[i], zRotation));
            tempTrail.transform.localScale = new Vector3(xScale, yScale, trailDistances[i] / trailLength);

            trails[i] = tempTrail;
        }
    }
}
