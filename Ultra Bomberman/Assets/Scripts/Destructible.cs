using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    private GameObject collapse;

    private new Renderer renderer;
    private new BoxCollider collider;
    private Renderer mlViewRenderer;

    private void Start()
    {
        if (G.train)
            G.gameController.reset.AddListener(Reset);

        renderer = GetComponent<Renderer>();
        collider = GetComponent<BoxCollider>();
        mlViewRenderer = transform.GetChild(0).GetComponent<Renderer>();
    }

    private void Reset()
    {
        SetShow(true);
    }

    public void Break()
    {
        if (!G.train || G.record)
            Instantiate(collapse, new Vector3(transform.position.x, collapse.transform.position.y, transform.position.z), collapse.transform.rotation);

        SetShow(false);
    }

    private void SetShow(bool show)
    {
        renderer.enabled = show;
        collider.enabled = show;
        mlViewRenderer.enabled = show;
    }
}
