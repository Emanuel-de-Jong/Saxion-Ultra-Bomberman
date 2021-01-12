using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    [SerializeField] KeyCode forwardKey = KeyCode.W;
    [SerializeField] KeyCode backKey = KeyCode.S;
    [SerializeField] KeyCode leftKey = KeyCode.A;
    [SerializeField] KeyCode rightKey = KeyCode.D;
    [SerializeField] KeyCode bombKey = KeyCode.Space;

    protected override void UpdateInput()
    {
        input[Direction.Forward] = Input.GetKey(forwardKey);
        input[Direction.Back] = Input.GetKey(backKey);
        input[Direction.Left] = Input.GetKey(leftKey);
        input[Direction.Right] = Input.GetKey(rightKey);

        if (Input.GetKey(bombKey)) // maybe GetKeyDown
        {
            spawnBomb = true;
        }
        else
        {
            spawnBomb = false;
        }
    }
}
