using Assets.Scripts;
using UnityEngine;


public enum Direction { Forward, Reverse, Stop };
public enum RaceState { Wait, Start, Accelerate, Steady, Decelerate, ReverseDirection, Stop }

public class Racer : MonoBehaviour
{
    /// <summary>
    /// Let the designer pick the speed
    /// </summary>
    [SerializeField] float speed = 1.0f;

    ActionByDirection currentActionState;
    /// <summary>
    /// 
    /// </summary>
    Direction direction = Direction.Forward;

    void Start()
    {
        direction = Direction.Forward;
    }

    // Update is called once per frame
    void Update()
    {
        //if (direction != Direction.Stop)
        {
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{this.name} has entered {other.name}");
        currentActionState =
        GetActionState(other, TriggerState.Enter);
    }
    private void OnTriggerExit(Collider other)
    {
        currentActionState =
        GetActionState(other, TriggerState.Exit);

        
        Debug.Log($"{this.name} has exited {other.name}");

    }


    private ActionByDirection GetActionState(Collider other, TriggerState triggerState)
    {
        ActionByDirection action = null;
        if (other.CompareTag("Barrier"))
        {
            BarrierDetection barrierDetection = other.GetComponent<BarrierDetection>();
            if (barrierDetection != null)
            {
                action = barrierDetection.GetCurrentAction(triggerState);
                if (action != null)
                    Debug.Log(action.ToString());
            }
        }
        return action;
    }


}

