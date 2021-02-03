using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    [System.Serializable]
    public class CharacterControllerEvent : UnityEvent<CharacterController> { };
    public CharacterControllerEvent takeDamager;
    public CharacterControllerEvent die;

    public int characterNumber;
    public bool isPlayer = true;
    public int startHealth = 3;
    public int health;
    public int bombRange = 2;
    public float cooldownDuration = 3f;
    public float movementSpeed = 7.5f;
    public string model = "MechanicalGolem";

    [SerializeField]
    private KeyCode forwardKey = KeyCode.W;
    [SerializeField]
    private KeyCode backKey = KeyCode.S;
    [SerializeField]
    private KeyCode leftKey = KeyCode.A;
    [SerializeField]
    private KeyCode rightKey = KeyCode.D;
    [SerializeField]
    private KeyCode bombKey = KeyCode.F;
    [SerializeField]
    private GameObject bomb;
    [SerializeField]
    private GameObject deathExplosion;

    private AudioSource damageSound;
    private float cooldown = 0f;
    private bool spawnBomb = false;
    private Animator animator;
    private new Renderer renderer;
    private Direction lookDir = Direction.Forward;
    private Direction lastMoveDir = Direction.None;
    private Direction moveDir = Direction.None;
    private Vector3 startPos;
    private Dictionary<Direction, bool> input;

    private enum Direction
    {
        Forward,
        Back,
        Left,
        Right,
        None
    }

    private void Start()
    {
        if (G.characterCount < characterNumber)
        {
            gameObject.SetActive(false);
            return;
        }

        health = startHealth;
        damageSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        renderer = gameObject.transform.Find(model).GetComponent<Renderer>();
        startPos = transform.position;
        input = new Dictionary<Direction, bool>() { [Direction.None] = false };
    }

    private void FixedUpdate()
    {
        UpdateInput();
        UpdateMovement();
        UpdateAnimation();
        UpdateBomb();
    }

    private void LateUpdate()
    {
        lastMoveDir = moveDir;
    }

    private void UpdateInput()
    {
        if (isPlayer)
        {
            input[Direction.Forward] = Input.GetKey(forwardKey);
            input[Direction.Back] = Input.GetKey(backKey);
            input[Direction.Left] = Input.GetKey(leftKey);
            input[Direction.Right] = Input.GetKey(rightKey);

            if (Input.GetKey(bombKey))
            {
                spawnBomb = true;
            }
            else
            {
                spawnBomb = false;
            }
        }
        else
        {

        }
    }

    private void UpdateMovement()
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

    private void Move()
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

    private void UpdateAnimation()
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

    private void UpdateBomb()
    {
        if (spawnBomb && cooldown <= Time.time)
        {
            PlaceBomb();
            cooldown = Time.time + cooldownDuration;
        }
    }

    private void PlaceBomb()
    {
        float x, z;
        if ((x = Mathf.Ceil(transform.position.x)) % 2 == 0)
            x = Mathf.Floor(transform.position.x);

        if ((z = Mathf.Ceil(transform.position.z)) % 2 == 0)
            z = Mathf.Floor(transform.position.z);

        Quaternion rotation = Quaternion.Euler(bomb.transform.rotation.x, 0, bomb.transform.rotation.z);
        GameObject bombInstance = Instantiate(bomb, new Vector3(x, bomb.transform.position.y, z), rotation);
        bombInstance.GetComponent<BombController>().range = bombRange;
    }

    public void TakeDamage()
    {
        damageSound.Play();

        health--;
        if (health < 1)
        {
            if (G.train)
            {
                Respawn();
            }
            else
            {
                Die();
            }
        }

        StartCoroutine(DamageColor());

        takeDamager.Invoke(this);
    }

    private void Respawn()
    {
        health = startHealth;
        transform.position = startPos;
    }

    private void Die()
    {
        die.Invoke(this);

        Instantiate(deathExplosion, new Vector3(transform.position.x, deathExplosion.transform.position.y, transform.position.z), deathExplosion.transform.rotation);

        GetComponent<BoxCollider>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        renderer.enabled = false;
        Destroy(gameObject, damageSound.clip.length);
    }

    private IEnumerator DamageColor()
    {
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        renderer.material.color = Color.white;
    }
}
