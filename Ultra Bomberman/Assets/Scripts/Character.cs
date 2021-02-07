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
    public int startHealth = 3;
    public int bombRange = 2;
    public float cooldownDuration = 1.5f;
    public float movementSpeed = 7.5f;
    public string model = "MechanicalGolem";

    [HideInInspector]
    public int health;
    [HideInInspector]
    public float cooldown;
    [HideInInspector]
    public Direction currentDirection = Direction.None;
    [HideInInspector]
    public Action currentAction = Action.None;

    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode bombKey = KeyCode.F;
    public GameObject bomb;
    public GameObject deathExplosion;

    private Animator animator;
    private AudioSource damageSound;
    private CustomAgent customAgent;
    private new Renderer renderer;
    private Direction lookDirection = Direction.Forward;
    private Vector3 startPos;
    private Dictionary<Direction, bool> directionInput;

    private void Awake()
    {
        if (G.characterCount < characterNumber)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    private void Start()
    {
        if (G.train)
            G.gameController.reset.AddListener(Reset);

        startPos = transform.position;
        Reset();

        animator = GetComponent<Animator>();
        damageSound = GetComponent<AudioSource>();
        customAgent = GetComponent<CustomAgent>();
        renderer = transform.Find(model).GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateAnimation();
        UpdateBomb();
    }

    public void Reset()
    {
        transform.position = startPos;
        health = startHealth;
        cooldown = cooldownDuration;
    }

    public Direction GetNextDirection()
    {
        directionInput = new Dictionary<Direction, bool>()
        {
            [Direction.Forward] = Input.GetKey(forwardKey),
            [Direction.Back] = Input.GetKey(backKey),
            [Direction.Left] = Input.GetKey(leftKey),
            [Direction.Right] = Input.GetKey(rightKey)
        };

        Direction nextDirection = Direction.None;
        if (currentDirection != Direction.None && directionInput[currentDirection])
        {
            nextDirection = currentDirection;
        }
        else
        {
            foreach (KeyValuePair<Direction, bool> entry in directionInput)
            {
                if (entry.Value)
                {
                    nextDirection = entry.Key;
                    break;
                }
            }
        }

        return nextDirection;
    }

    public Action GetNextAction()
    {
        Action nextAction = Action.None;
        if (Input.GetKey(bombKey))
            nextAction = Action.Bomb;

        return nextAction;
    }

    private void UpdateMovement()
    {
        Vector3 offset = Vector3.zero;
        switch (currentDirection)
        {
            case Direction.Forward:
                offset = Vector3.forward;
                break;
            case Direction.Back:
                offset = Vector3.back;
                break;
            case Direction.Left:
                offset = Vector3.left;
                break;
            case Direction.Right:
                offset = Vector3.right;
                break;
        }

        if (offset != Vector3.zero)
        {
            transform.position += offset * movementSpeed * Time.deltaTime;
        }
    }

    private void UpdateAnimation()
    {
        if (currentDirection != Direction.None && lookDirection != currentDirection)
        {
            switch (currentDirection)
            {
                case Direction.Forward:
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Direction.Back:
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    break;
                case Direction.Left:
                    transform.localRotation = Quaternion.Euler(0, 270, 0);
                    break;
                case Direction.Right:
                    transform.localRotation = Quaternion.Euler(0, 90, 0);
                    break;
            }

            lookDirection = currentDirection;
        }

        bool isWalking = animator.GetBool("isWalking");
        if (currentDirection == Direction.None && isWalking)
        {
            animator.SetBool("isWalking", false);
        }
        else if (currentDirection != Direction.None && !isWalking)
        {
            animator.SetBool("isWalking", true);
        }
    }

    private void UpdateBomb()
    {
        if (cooldown != 0)
        {
            float tempCooldown = cooldown - Time.fixedDeltaTime;
            if (tempCooldown > 0)
            {
                cooldown = tempCooldown;
            }
            else
            {
                cooldown = 0;
            }
        }

        if (currentAction == Action.Bomb && cooldown == 0)
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

        customAgent.BombPlaced();
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
