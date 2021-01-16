using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] List<GameObject> powerups = new List<GameObject>();

    public void Break()
    {
        if (Random.Range(0, 2) == 1)
            SpawnPowerup();

        Destroy(gameObject);
    }

    private void SpawnPowerup()
    {
        GameObject powerup = powerups[Random.Range(0, powerups.Count)];

        Vector3 pos = new Vector3(transform.position.x, powerup.transform.position.y, transform.position.z);
        Instantiate(powerup, pos, powerup.transform.rotation);
    }
}
