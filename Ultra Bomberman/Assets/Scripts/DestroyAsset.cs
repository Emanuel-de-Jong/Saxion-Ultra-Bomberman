using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAsset : MonoBehaviour
{
    [SerializeField]
    private float destroyDelay = 3f;

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
}
