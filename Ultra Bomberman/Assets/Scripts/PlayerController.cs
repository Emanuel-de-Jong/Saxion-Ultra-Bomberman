using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode bombKey = KeyCode.F;

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
