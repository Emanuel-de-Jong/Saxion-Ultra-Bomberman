using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    float movementSpeed = 0.25f;
    [SerializeField]
    float rayLength = 1.4f;
    [SerializeField]
    float rayOffsetX = 1f;
    [SerializeField]
    float rayOffsetY = 1f;
    [SerializeField]
    float rayOffsetZ = 1f;

    Vector3 xOffset;
    Vector3 yOffset;
    Vector3 zOffset;

    Vector3 targetPosition;
    Vector3 startPosition;

    Animator animator;
    private Rigidbody rigidbody;

    public GameObject Dynamite;
    private float cooldown;

    string rotation = "FORWARD";

    bool moving;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        yOffset = transform.position + Vector3.up * rayOffsetY;
        zOffset = Vector3.forward * rayOffsetZ;
        xOffset = Vector3.right * rayOffsetX;

        bool w = Input.GetKey(KeyCode.W);
        bool a = Input.GetKey(KeyCode.A);
        bool s = Input.GetKey(KeyCode.S);
        bool d = Input.GetKey(KeyCode.D);
        bool space = Input.GetKey(KeyCode.Space);

        rigidbody.angularVelocity = Vector3.zero;

        if (space && cooldown <= Time.time)
        {
            spawnDynamite();
            cooldown = Time.time + 3;
        }

        if (w)
        {
            targetPosition = transform.position + Vector3.forward;
            startPosition = transform.position;

            if (!rotation.Equals("FORWARD"))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                rotation = "FORWARD";
            }

            transform.position += (targetPosition - startPosition) * movementSpeed * Time.deltaTime;

            moving = true;
            animator.SetBool("isWalking", true);
        } 
        else if (s)
        {
            targetPosition = transform.position + Vector3.back;
            startPosition = transform.position;

            if (!rotation.Equals("BACK"))
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                rotation = "BACK";
            }

            transform.position += (targetPosition - startPosition) * movementSpeed * Time.deltaTime;

            moving = true;
            animator.SetBool("isWalking", true);
        }
        else if (a)
        {
            targetPosition = transform.position + Vector3.left;
            startPosition = transform.position;

            if (!rotation.Equals("LEFT"))
            {
                transform.localRotation = Quaternion.Euler(0, 270, 0);
                rotation = "LEFT";
            }

            transform.position += (targetPosition - startPosition) * movementSpeed * Time.deltaTime;

            moving = true;
            animator.SetBool("isWalking", true);
        }
        else if (d)
        {
            targetPosition = transform.position + Vector3.right;
            startPosition = transform.position;

            if (!rotation.Equals("RIGHT"))
            {
                transform.localRotation = Quaternion.Euler(0, 90, 0);
                rotation = "RIGHT";
            }

            transform.position += (targetPosition - startPosition) * movementSpeed * Time.deltaTime;

            moving = true;
            animator.SetBool("isWalking", true);
        } 
        else
        {
            moving = false;
            animator.SetBool("isWalking", false);
        }

    }

    void spawnDynamite()
    {
        System.Random random = new System.Random();

        int rotation = random.Next(0, 361);

        GameObject dynamite = Instantiate(Dynamite, new Vector3(transform.position.x, 0.65f, transform.position.z), Quaternion.Euler(new Vector3(90f, rotation, 0)));
        dynamite.GetComponent<DynamiteCycle>().rotationZ = rotation;
    }
}
