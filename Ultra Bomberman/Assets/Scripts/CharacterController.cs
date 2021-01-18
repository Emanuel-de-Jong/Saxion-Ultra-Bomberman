using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    public int playerNumber;
    public int health = 3;
    public int bombRange = 2;
    public float cooldownDuration = 3f;
    public float movementSpeed = 7.5f;
    public string model = "MechanicalGolem";

    [SerializeField] protected GameObject bomb;
    [SerializeField] protected GameUI ui;

    protected float cooldown = 0f;
    protected bool spawnBomb = false;
    protected Animator animator;
    protected new Renderer renderer;
    protected Dictionary<Direction, bool> input;
    protected Direction lookDir = Direction.Forward;
    protected Direction lastMoveDir = Direction.None;
    protected Direction moveDir = Direction.None;

    protected enum Direction
    {
        Forward,
        Back,
        Left,
        Right,
        None
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        renderer = gameObject.transform.Find(model).GetComponent<Renderer>();
        input = new Dictionary<Direction, bool>() { [Direction.None] = false };
    }

    void FixedUpdate()
    {
        UpdateInput();
        UpdateMovement();
        UpdateAnimation();
        UpdateBomb();
    }

    void LateUpdate()
    {
        lastMoveDir = moveDir;
    }

    protected abstract void UpdateInput();

    void UpdateMovement()
    {
        moveDir = Direction.None;
        if (input[lastMoveDir])
        {
            moveDir = lastMoveDir;
            Move();
        }
        else
        {
            foreach (KeyValuePair<Direction, bool> entry in input)
            {
                if (entry.Value)
                {
                    moveDir = entry.Key;
                    Move();
                    break;
                }
            }
        }
    }

    void Move()
    {
        Vector3 offset = Vector3.zero;
        if (moveDir == Direction.Forward)
        {
            offset = Vector3.forward;
        }
        else if (moveDir == Direction.Back)
        {
            offset = Vector3.back;
        }
        else if (moveDir == Direction.Left)
        {
            offset = Vector3.left;
        }
        else if (moveDir == Direction.Right)
        {
            offset = Vector3.right;
        }

        transform.position += offset * movementSpeed * Time.deltaTime;
    }

    void UpdateAnimation()
    {
        if (lookDir != moveDir)
        {
            if (moveDir == Direction.Forward)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (moveDir == Direction.Back)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (moveDir == Direction.Left)
            {
                transform.localRotation = Quaternion.Euler(0, 270, 0);
            }
            else if (moveDir == Direction.Right)
            {
                transform.localRotation = Quaternion.Euler(0, 90, 0);
            }

            lookDir = moveDir;
        }

        if (moveDir != Direction.None && lastMoveDir == Direction.None)
        {
            animator.SetBool("isWalking", true);
        }
        else if (moveDir == Direction.None && lastMoveDir != Direction.None)
        {
            animator.SetBool("isWalking", false);
        }
    }

    void UpdateBomb()
    {
        if (spawnBomb && cooldown <= Time.time)
        {
            PlaceBomb();
            cooldown = Time.time + cooldownDuration;
        }
    }

    void PlaceBomb()
    {
        float x, z;
        if ((x = Mathf.Ceil(transform.position.x)) % 2 == 0)
            x = Mathf.Floor(transform.position.x);

        if ((z = Mathf.Ceil(transform.position.z)) % 2 == 0)
            z = Mathf.Floor(transform.position.z);

        Quaternion rotation = Quaternion.Euler(bomb.transform.rotation.x, Random.Range(0f, 361f), bomb.transform.rotation.z);
        GameObject bombInstance = Instantiate(bomb, new Vector3(x, bomb.transform.position.y, z), rotation);
        bombInstance.GetComponent<BombController>().range = bombRange;
    }

    public void TakeDamage()
    {
        health--;

        ui.SetHealth(playerNumber, health);

        if (health < 1)
        {
            Die();
            return;
        }

        StartCoroutine(DamageColor());
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator DamageColor()
    {
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        renderer.material.color = Color.white;
    }
}
