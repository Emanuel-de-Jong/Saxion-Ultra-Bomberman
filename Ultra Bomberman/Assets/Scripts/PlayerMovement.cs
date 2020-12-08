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
    Vector3 zAxisOriginA;
    Vector3 zAxisOriginB;
    Vector3 xAxisOriginA;
    Vector3 xAxisOriginB;

    Vector3 targetPosition;
    Vector3 startPosition;

    Animator animator;

    string rotation = "FORWARD";

    bool moving;

    void Start()
    {
        animator = GetComponent<Animator>();
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

        bool w = Input.GetKey(KeyCode.W);
        bool a = Input.GetKey(KeyCode.A);
        bool s = Input.GetKey(KeyCode.S);
        bool d = Input.GetKey(KeyCode.D);

        if (moving)
        {

            if (Vector3.Distance(startPosition, transform.position) > 1f)
            {
                transform.position = targetPosition;
                moving = false;
                return;
            }

            transform.position += (targetPosition - startPosition) * movementSpeed * Time.deltaTime;
            return;
        }

        if (w)
        {
            if (CanMove(Vector3.forward))
            {
                targetPosition = transform.position + Vector3.forward;
                startPosition = transform.position;

                if (!rotation.Equals("FORWARD"))
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    rotation = "FORWARD";
                }

                moving = true;
                animator.SetBool("isWalking", true);
            }
        } 
        else if (s)
        {
            if (CanMove(Vector3.back))
            {
                targetPosition = transform.position + Vector3.back;
                startPosition = transform.position;

                if (!rotation.Equals("BACK"))
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    rotation = "BACK";
                }

                moving = true;
                animator.SetBool("isWalking", true);
            }
        }
        else if (a)
        {
            if (CanMove(Vector3.left))
            {
                targetPosition = transform.position + Vector3.left;
                startPosition = transform.position;

                if (!rotation.Equals("LEFT"))
                {
                    transform.localRotation = Quaternion.Euler(0, 270, 0);
                    rotation = "LEFT";
                }

                moving = true;
                animator.SetBool("isWalking", true);
            }
        }
        else if (d)
        {
            if (CanMove(Vector3.right))
            {
                targetPosition = transform.position + Vector3.right;
                startPosition = transform.position;

                if (!rotation.Equals("RIGHT"))
                {
                    transform.localRotation = Quaternion.Euler(0, 90, 0);
                    rotation = "RIGHT";
                }

                moving = true;
                animator.SetBool("isWalking", true);
            }
        } 
        else
        {
            animator.SetBool("isWalking", false);
        }

    }

    bool CanMove(Vector3 direction)
    {
        if (direction.z != 0)
        {
            if (Physics.Raycast(zAxisOriginA, direction, rayLength)) return false;
            if (Physics.Raycast(zAxisOriginB, direction, rayLength)) return false;
        }
        else if (direction.x != 0)
        {
            if (Physics.Raycast(xAxisOriginA, direction, rayLength)) return false;
            if (Physics.Raycast(xAxisOriginB, direction, rayLength)) return false;
        }
        return true;
    }
}
