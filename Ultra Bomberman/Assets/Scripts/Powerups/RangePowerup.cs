using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangePowerup : Powerup
{
    [SerializeField] int rangeToAdd = 1;
    [SerializeField] int maxRange = 4;

    protected override void ApplyPowerup(PlayerController player)
    {
        if ((player.bombRange + rangeToAdd) >= maxRange)
        {
            player.bombRange = maxRange;
        }
        else
        {
            player.bombRange += rangeToAdd;
        }
    }
}
