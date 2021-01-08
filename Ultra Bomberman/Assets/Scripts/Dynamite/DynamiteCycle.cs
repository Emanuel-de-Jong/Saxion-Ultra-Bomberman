using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteCycle : MonoBehaviour
{
    public float range = 2f;

    [SerializeField] int trailLength = 9;
    [SerializeField] float gridSize = 2f;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject trail;
    [SerializeField] float explosionDelay = 3f;
    [SerializeField] int[] raycastPoints = new int[] { 0, 90, 180, 270 };

    public static float defaultTrailDistance = 0.6f;

    float forwardTrailDistance = defaultTrailDistance;
    float rightTrailDistance = defaultTrailDistance;
    float bottomTrailDistance = defaultTrailDistance;
    float leftTrailDistance = defaultTrailDistance;

    GameObject[] trails;
    float[] trailDistances;

    void Start()
    {
        range *= gridSize;
        trails = new GameObject[raycastPoints.Length];
        trailDistances = new float[raycastPoints.Length];
        Invoke(nameof(Explode), explosionDelay);

        // Debug Raycast
        Vector3 pos = new Vector3(transform.position.x, 0.5f, transform.position.z);
        for (int i = 0; i < raycastPoints.Length; i++)
        {
            Vector3 dir;
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
                default:
                    dir = Vector3.forward;
                    break;
            }

            Debug.DrawRay(pos, dir * range, Color.red, explosionDelay);
        }
    }

    void Explode()
    {
        Instantiate(explosion, new Vector3(transform.position.x, explosion.transform.position.y, transform.position.z), explosion.transform.rotation);

        CalculateTrailDinstances();

        SpawnTrail();

        Destroy(gameObject);
    }

    void CalculateTrailDinstances()
    {
        Vector3 pos = new Vector3(transform.position.x, 0.5f, transform.position.z);

        for (int i = 0; i < raycastPoints.Length; i++)
        {
            Vector3 dir;
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
                default:
                    dir = Vector3.forward;
                    break;
            }

            if (Physics.Raycast(pos, dir, out RaycastHit hit, range))
            {
                trailDistances[i] = hit.distance;
            }
        }
    }

    void SpawnTrail()
    {
        Vector3 trailPos = new Vector3(transform.position.x, trail.transform.position.y, transform.position.z);
        float xRotation = trail.transform.rotation.x, zRotation = trail.transform.rotation.z;
        for (int i=0; i<raycastPoints.Length; i++)
        {
            trails[i] = Instantiate(trail, trailPos, Quaternion.Euler(xRotation, raycastPoints[i], zRotation));
        }

        float xScale = trail.transform.localScale.x, yScale = trail.transform.localScale.y;
        for (int i = 0; i < raycastPoints.Length; i++)
        {
            trails[i].transform.localScale = new Vector3(xScale, yScale, trailDistances[i] / trailLength);
        }
    }
}
