using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAsset : MonoBehaviour
{
    public int destroyDelay;
    private float timeTillDestroy;

    // Start is called before the first frame update
    void Start()
    {
        timeTillDestroy = Time.time + destroyDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeTillDestroy <= Time.time)
        {
            Destroy(gameObject);
        }
    }
}
