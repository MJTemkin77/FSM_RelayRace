using Assets.Scripts;
using UnityEngine;


public enum Direction { Forward, Reverse, Stop };
public enum RaceState { Wait, Start, Accelerate, Steady, Decelerate, ReverseDirection, Stop, Unintialized }

public class Racer : MonoBehaviour
{
    /// <summary>
    /// Let the designer pick the speed
    /// </summary>
    [SerializeField] float speed = 1.0f;

    RaceState nextRaceState;
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
        nextRaceState = GetNextRaceState(other, TriggerState.Enter);
    }
    private void OnTriggerExit(Collider other)
    {
        nextRaceState =
        GetNextRaceState(other, TriggerState.Exit);

        
        Debug.Log($"{this.name} has exited {other.name}");

    }


    private RaceState GetNextRaceState(Collider other, TriggerState triggerState)
    {
        RaceState nextRaceState = RaceState.Unintialized;
        if (other.CompareTag("Barrier"))
        {
            BarrierDetection barrierDetection = other.GetComponent<BarrierDetection>();
            if (barrierDetection != null)
            {
                nextRaceState = barrierDetection.GetNextRaceState(triggerState, direction);
                Debug.Log($"Next Race State is: {nextRaceState}");
            }
        }
        return nextRaceState;
    }


}

