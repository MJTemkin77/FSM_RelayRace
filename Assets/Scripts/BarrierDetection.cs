using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierDetection : MonoBehaviour
{
    [SerializeField] ActionByDirection[] actionsByDirection = new ActionByDirection[2];

#nullable enable
    public ActionByDirection? GetCurrentAction(TriggerState state)
    {
        ActionByDirection? actionByDirection = null;

        // Find the action based on the trigger
        bool fnd = false;
        for (int i = 0; i < actionsByDirection.Length && !fnd; i++)
        {
            if (actionsByDirection[i].TriggerState == state)
            {
                fnd = true;
                actionByDirection = actionsByDirection[i];
            }

        }
        return actionByDirection;
    }
#nullable disable
}
