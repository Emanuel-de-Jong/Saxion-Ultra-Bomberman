using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [System.Serializable]
    public class CharacterEvent : UnityEvent<Character> { };
    public CharacterEvent takeDamager;
    public CharacterEvent die;

    public int characterNumber = 1;
    public bool isPlayer = false;
    public int startHealth = 3;
    public int bombRange = 2;
    public float cooldownDuration = 3f;
    public float movementSpeed = 7.5f;
    public string model = "MechanicalGolem";

    [HideInInspector]
    public int health;
    [HideInInspector]
    public float cooldown;

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

    private Animator animator;
    private AudioSource damageSound;
    private CustomAgent customAgent;
    private Renderer renderer;
    private Direction lookDir = Direction.Forward;
    private Direction lastMoveDir = Direction.None;
    private Direction moveDir = Direction.None;
    private Vector3 startPos;
    private Dictionary<Direction, bool> input;

    private void Start()
    {
        if (G.characterCount < characterNumber)
        {
            gameObject.SetActive(false);
            return;
        }

        if (G.train)
            G.gameController.reset.AddListener(Reset);

        startPos = transform.position;
        Reset();

        animator = GetComponent<Animator>();
        damageSound = GetComponent<AudioSource>();
        customAgent = GetComponent<CustomAgent>();
        renderer = transform.Find(model).GetComponent<Renderer>();

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

    public void Reset()
    {
        transform.position = startPos;
        health = startHealth;
        cooldown = cooldownDuration;
    }

    public void UpdateInput(Direction dir = Direction.None)
    {
        if (isPlayer)
        {
            input[Direction.Forward] = Input.GetKey(forwardKey);
            input[Direction.Back] = Input.GetKey(backKey);
            input[Direction.Left] = Input.GetKey(leftKey);
            input[Direction.Right] = Input.GetKey(rightKey);
            input[Direction.Bomb] = Input.GetKey(bombKey);
        }
        else
        {
            input[Direction.Forward] = false;
            input[Direction.Back] = false;
            input[Direction.Left] = false;
            input[Direction.Right] = false;
            input[Direction.Bomb] = false;

            input[dir] = true;
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
                if (entry.Key != Direction.Bomb && entry.Value)
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
        cooldown -= Time.fixedDeltaTime;
        if (input[Direction.Bomb] && cooldown <= 0)
        {
            PlaceBomb();
            cooldown = cooldownDuration;
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
        Bomb bombScript = bombInstance.GetComponent<Bomb>();
        bombScript.owner = this;
        bombScript.characterHit.AddListener(customAgent.CharacterHit);
    }

    public void TakeDamage()
    {
        health--;
        if (health < 1)
        {
            Die();
            return;
        }

        damageSound.Play();
        StartCoroutine(DamageColor());

        takeDamager.Invoke(this);
    }

    private void Die()
    {
        die.Invoke(this);

        Instantiate(deathExplosion, new Vector3(transform.position.x, deathExplosion.transform.position.y, transform.position.z), deathExplosion.transform.rotation);

        if (G.train)
        {
            Reset();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DamageColor()
    {
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        renderer.material.color = Color.white;
    }
}
