using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [HideInInspector]
    public Character owner;

    [SerializeField]
    private int trailLength = 9;
    [SerializeField]
    private float explosionDelay = 3f;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private GameObject trail;
    [SerializeField]
    private int[] raycastPoints = new int[] { 0, 90, 180, 270 };

    private int range = 2;
    private float[] trailDistances;
    private GameObject[] trails;

    private void Start()
    {
        range = owner.bombRange;

        trails = new GameObject[raycastPoints.Length];
        trailDistances = new float[raycastPoints.Length + 1]; // not hitting character on top quickfix
        Invoke(nameof(Explode), explosionDelay);
    }

    private void Explode()
    {
        Instantiate(explosion, new Vector3(transform.position.x, explosion.transform.position.y, transform.position.z), explosion.transform.rotation);
        CastRays();
        SpawnTrail();
        Destroy(gameObject);
    }

    private void CastRays()
    {
        Vector3 pos = new Vector3(transform.position.x, 0.5f, transform.position.z);

        Vector3 dir = Vector3.forward;
        RaycastHit[] hits;
        for (int i = 0; i < raycastPoints.Length + 1; i++)
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
                // not hitting character on top quickfix
                case 4:
                    dir = Vector3.up;
                    break;
            }

            trailDistances[i] = (range * 2);

            hits = Physics.RaycastAll(pos, dir, (range * 2));
            if (hits.Length != 0)
            {
                System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.CompareTag("Character"))
                    {
                        hit.transform.GetComponent<Character>().TakeDamage();
                    }
                    else if (hit.transform.CompareTag("Destructible"))
                    {
                        hit.transform.GetComponent<Destructible>().Break();
                        trailDistances[i] = hit.distance + 0.7f;

                        break;
                    }
                    else
                    {
                        trailDistances[i] = hit.distance >= 1.2 ? hit.distance : 0;

                        break;
                    }
                }
            }
        }
    }

    private void SpawnTrail()
    {
        Vector3 trailPos = new Vector3(transform.position.x, trail.transform.position.y, transform.position.z);
        float xRotation = trail.transform.rotation.x, zRotation = trail.transform.rotation.z;
        float xScale = trail.transform.localScale.x, yScale = trail.transform.localScale.y;
        for (int i = 0; i < raycastPoints.Length; i++)
        {
            if (trailDistances[i] == 0)
                continue;

            GameObject tempTrail = Instantiate(trail, trailPos, Quaternion.Euler(xRotation, raycastPoints[i], zRotation));
            tempTrail.transform.localScale = new Vector3(xScale, yScale, trailDistances[i] / trailLength);

            trails[i] = tempTrail;
        }
    }
}
