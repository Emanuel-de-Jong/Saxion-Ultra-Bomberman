using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    private GameObject collapse;

    public void Break()
    {
        Instantiate(collapse, new Vector3(transform.position.x, collapse.transform.position.y, transform.position.z), collapse.transform.rotation);

        Destroy(gameObject);
    }
}
