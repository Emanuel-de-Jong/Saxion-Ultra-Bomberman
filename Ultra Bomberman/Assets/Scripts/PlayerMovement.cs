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

    enum Direction
    {
        Forward,
        Back,
        Left,
        Right,
        None
    }

    Vector3 xOffset;
    Vector3 yOffset;
    Vector3 zOffset;
    Vector3 zAxisOriginA;
    Vector3 zAxisOriginB;
    Vector3 xAxisOriginA;
    Vector3 xAxisOriginB;

    Vector3 offset;

    Animator animator;
    private Rigidbody rigidbody;

    string rotation = "FORWARD";
    Direction lastDir = Direction.Forward;
    Dictionary<Direction, bool> input = new Dictionary<Direction, bool>();
    bool moved;

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

        zAxisOriginA = yOffset + xOffset;
        zAxisOriginB = yOffset - xOffset;

        xAxisOriginA = yOffset + zOffset;
        xAxisOriginB = yOffset - zOffset;

        rigidbody.angularVelocity = Vector3.zero;

        input[Direction.Forward] = Input.GetKey(KeyCode.W);
        input[Direction.Back] = Input.GetKey(KeyCode.S);
        input[Direction.Left] = Input.GetKey(KeyCode.A);
        input[Direction.Right] = Input.GetKey(KeyCode.D);

        moved = false;
        if (lastDir != Direction.None && input[lastDir])
        {
            move(lastDir);
        }
        else
        {
            foreach (KeyValuePair<Direction, bool> entry in input)
            {
                if (entry.Value)
                {
                    move(entry.Key);
                    lastDir = entry.Key;
                    break;
                }
            }
        }

        if (moved)
        {
            transform.position += offset * movementSpeed * Time.deltaTime;
            animator.SetBool("isWalking", true);
        }
        else
        {
            lastDir = Direction.None;
            animator.SetBool("isWalking", false);
        }
    }

    void move(Direction dir)
    {
        moved = true;

        if (dir == Direction.Forward)
        {
            offset = Vector3.forward;
            if (!rotation.Equals("FORWARD"))
            {
                rotation = "FORWARD";
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (dir == Direction.Back)
        {
            offset = Vector3.back;
            if (!rotation.Equals("BACK"))
            {
                rotation = "BACK";
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else if (dir == Direction.Left)
        {
            offset = Vector3.left;
            if (!rotation.Equals("LEFT"))
            {
                rotation = "LEFT";
                transform.localRotation = Quaternion.Euler(0, 270, 0);
            }
        }
        else if (dir == Direction.Right)
        {
            offset = Vector3.right;
            if (!rotation.Equals("RIGHT"))
            {
                rotation = "RIGHT";
                transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
        }
    }
}
