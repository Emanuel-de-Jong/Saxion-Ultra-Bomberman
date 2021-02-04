using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CustomAgent : Agent
{
    [SerializeField]
    private float decisionTime = 0.1f;

    public const int NONE = 0;
    public const int FORWARD = 1;
    public const int BACK = 2;
    public const int LEFT = 3;
    public const int RIGHT = 4;
    public const int BOMB = 5;

    private float timeSinceDecision = 0;
    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    public void FixedUpdate()
    {
        WaitTimeInference();
    }

    private void WaitTimeInference()
    {
        if (Academy.Instance.IsCommunicatorOn)
        {
            RequestDecision();
        }
        else
        {
            if (timeSinceDecision >= decisionTime)
            {
                timeSinceDecision = 0f;
                RequestDecision();
            }
            else
            {
                timeSinceDecision += Time.fixedDeltaTime;
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(character.health);
        sensor.AddObservation(character.cooldown);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        character.UpdateInput((Direction)actionBuffers.DiscreteActions[0]);
    }
}
