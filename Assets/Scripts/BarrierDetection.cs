using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierDetection : MonoBehaviour
{
    [SerializeField] ActionByDirection[] actionsByDirection = new ActionByDirection[2];

#nullable enable
    public RaceState GetNextRaceState(TriggerState triggerState, Direction direction)
    {
        RaceState nextRaceState = RaceState.Unintialized;
        ActionByDirection match = new(RaceState.Unintialized, triggerState, direction);

        // Find the action based on the trigger
        bool fnd = false;
        for (int i = 0; i < actionsByDirection.Length && !fnd; i++)
        {
            if (actionsByDirection[i].IsMatchForUnitialized(match))
            {
                fnd = true;
                nextRaceState = actionsByDirection[i].RaceState;
            }

        }
        return nextRaceState;
    }
#nullable disable
}
