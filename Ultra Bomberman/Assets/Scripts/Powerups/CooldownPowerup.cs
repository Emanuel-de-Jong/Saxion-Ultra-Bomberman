using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownPowerup : Powerup
{
    [SerializeField] float durationToRemove = 0.5f;
    [SerializeField] float minCooldown = 1f;

    protected override void ApplyPowerup(PlayerController player)
    {
        if ((player.cooldownDuration - durationToRemove) <= minCooldown)
        {
            player.cooldownDuration = minCooldown;
        }
        else
        {
            player.cooldownDuration -= durationToRemove;
        }
    }
}
