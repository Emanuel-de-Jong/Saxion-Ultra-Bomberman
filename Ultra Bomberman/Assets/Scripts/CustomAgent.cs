using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

public class CustomAgent : Agent
{
    public bool isPlayer = false;

    public const int NONE = 0;

    public const int FORWARD = 1;
    public const int BACK = 2;
    public const int LEFT = 3;
    public const int RIGHT = 4;

    public const int BOMB = 1;

    private Character character;
    private int characterNumber;

    private void Start()
    {
        character = GetComponent<Character>();
        if (isPlayer)
            GetComponent<BehaviorParameters>().BehaviorType = BehaviorType.HeuristicOnly;

        if (G.train)
            G.gameController.reset.AddListener(Reset);

        character.takeDamager.AddListener(TakeDamage);
        character.die.AddListener(Die);

        characterNumber = character.characterNumber;
    }

    private void Reset()
    {
        EndEpisode();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (character)
        {
            sensor.AddObservation(character.health);
            sensor.AddObservation(character.cooldown);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = (int)character.GetNextDirection();
        discreteActionsOut[1] = (int)character.GetNextAction();
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int direction = actionBuffers.DiscreteActions[0];
        if (!isPlayer)
        {
            direction = SyncDirectionWithCamera(direction);
        }

        character.currentDirection = (Direction)direction;
        character.currentAction = (Action)actionBuffers.DiscreteActions[1];

        AddReward(-0.001f);
    }

    public void CharacterHit()
    {
        AddReward(5);
    }

    public void BombPlaced()
    {
        //AddReward(0.5f);
    }

    private void TakeDamage(Character character)
    {
        AddReward(-2);
    }

    private void Die(Character character)
    {
        AddReward(-10);
    }

    private int SyncDirectionWithCamera(int direction)
    {
        if (direction == 0 || characterNumber == 1)
            return direction;

        if (characterNumber == 2)
        {
            switch (direction)
            {
                case 1:
                    direction = 2;
                    break;
                case 2:
                    direction = 1;
                    break;
                case 3:
                    direction = 4;
                    break;
                case 4:
                    direction = 3;
                    break;
            }
        }
        else if (characterNumber == 3)
        {
            switch (direction)
            {
                case 3:
                    direction = 4;
                    break;
                case 4:
                    direction = 3;
                    break;
            }
        }
        else if (characterNumber == 4)
        {
            switch (direction)
            {
                case 1:
                    direction = 2;
                    break;
                case 2:
                    direction = 1;
                    break;
            }
        }

        return direction;
    }
}
