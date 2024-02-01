using Assets.Scripts;
using UnityEngine;
using TMPro;
using UnityEditor.Search;
using System.Runtime.Serialization.Json;

/// <summary>
/// The various direction states that the Race Team Member can move on the track 
/// which are Forward, Reverse, or no movement (Stop)
/// </summary>
public enum Direction { Forward, Reverse, Stop };

/// <summary>
/// Each RaceState indicates a stage in the race.
/// Wait - waiting for the start of the race
/// Start - start the race
/// Accelerate - continuously increase the running speed
/// Steady - hold the running speed 
/// Decelerate - slow down as quickly as possible
/// ReverseDirection - go back to the other end of the track. This may involve changing runners.
/// Uninitialized - Essentially no state.
/// </summary>
public enum RaceState { Wait, Start, Accelerate, Steady, Decelerate, ReverseDirection, Stop, Unintialized }


/// <summary>
/// Class manages a N-Member relay race team, which is typically four members.
/// There will be one runner at a time for each team. 
/// When the player reaches the other end a new runner will take their place.
/// The race ends when the last player reaches the StartFinish line.
/// </summary>
public class RaceTeam : MonoBehaviour
{
    /// <summary>
    /// Let the designer pick the base speed.
    /// </summary>  
    [Range(2f, 100f)]
    [SerializeField] float initialSpeed = 2.0f;

    /// <summary>
    /// Let the designer pick the amount to increment (boost) 
    /// the runners speed by when accelerating.
    /// </summary>
    [SerializeField] float accelarationIncrement = .01f;

    /// <summary>
    /// Maximum speed that the runner can reach
    /// </summary>
    [SerializeField] float maxSpeed = 10;

    /// <summary>
    /// Direct reference to the child TextMeshPro Text item used to set the 
    /// number of the runner.
    /// </summary>
    [SerializeField] TMPro.TMP_Text runnerLabel;

    [SerializeField]
    [Range(1, 20)]
    int numberOfRunners;

    /// <summary>
    /// The current speed.
    /// </summary>
    float speed = 1.0f;

    int currentRunner = 0;

    /// <summary>
    /// The stage of the race. At the beginning of the race and at each end the runners are waiting.
    /// </summary>
    RaceState nextRaceState = RaceState.Wait;

    /// <summary>
    /// Which direction is the running moving: forward, reverse, or stop (no movement)
    /// This variable along with speed and accelerationIncrement is used to control the amount and
    /// direction of movement. The race starts in the forward direction.
    /// </summary>
    Direction direction = Direction.Forward;


    private void Start()
    {
        speed = initialSpeed;
    }
    /// <summary>
    /// The state machine is used to largely control the movement values based on the RaceState and Direction.
    /// </summary>
    void Update()
    {
        Vector3 movement = Vector3.zero;
        switch (nextRaceState)
        {
            case RaceState.Wait:
                movement = Vector3.zero;
                break;
            case RaceState.Start:
                movement = speed * (direction == Direction.Forward ? Vector3.forward : Vector3.back);
                break;
            case RaceState.Accelerate:
                speed += accelarationIncrement;
                speed = Mathf.Max(speed, maxSpeed);
                if (speed < initialSpeed)
                    speed = initialSpeed;
                Debug.Log($"Acceleration: Speed is now {speed}");
                movement = speed * (direction == Direction.Forward ? Vector3.forward : Vector3.back);
                break;
            case RaceState.Steady:
                movement = speed * (direction == Direction.Forward ? Vector3.forward : Vector3.back);
                break;
            case RaceState.Decelerate:
                speed -= accelarationIncrement;
                speed = Mathf.Min(speed, maxSpeed);
                Debug.Log($"Deceleration: Speed is now {speed}");
                movement = speed * (direction == Direction.Forward ? Vector3.forward : Vector3.back);
                break;
            case RaceState.ReverseDirection:
                speed = initialSpeed;
                direction = direction == Direction.Forward ? Direction.Reverse : Direction.Forward;
                
                if (currentRunner + 1 < numberOfRunners)
                {
                    currentRunner++;
                    runnerLabel.text = (currentRunner + 1).ToString();
                    //this.runnerLabel.transform.Rotate(0, 180, 0);
                    nextRaceState = RaceState.Accelerate;
                }
                else
                {
                    nextRaceState =RaceState.Stop;
                }

                break;
            case RaceState.Stop:
                movement = Vector3.zero;
                break;
            default:
                break;

        }
        Debug.Log($"Current State:{nextRaceState}");
        if (movement != Vector3.zero) 
        {
            this.transform.Translate(movement * Time.deltaTime);
        }
    }

    /// <summary>
    /// Receives the Start Race button click to start the race.
    /// </summary>
    public void StartRace()
    {
        nextRaceState = RaceState.Start;
    }


    /// <summary>
    /// The runner will encounter game objects used as triggers. 
    /// The triggers provide the transition to the next Race State.
    /// The next Race State is used by Update.
    /// Both OnTriggerEnter and OnTriggerExit call a common method.
    /// </summary>
    /// <param name="other">The object that the runner has triggered.</param>
    private void OnTriggerEnter(Collider other)
    {
            bool changedState =
            GetNextRaceState(other, TriggerState.Enter);
            if (changedState)
            {
                Debug.Log($"{this.name} has entered {other.name} and the race state has changed to {nextRaceState}");
            }
     

    }

    /// <summary>
    /// See the above explanation.
    /// </summary>
    /// <param name="other">The object that the runner has triggered.</param>
    private void OnTriggerExit(Collider other)
    {

        bool changedState =
        GetNextRaceState(other, TriggerState.Exit);
        if (changedState)
        {
            Debug.Log($"{this.name} has exited {other.name} and the race state has changed to {nextRaceState}");
        }
    }


    /// <summary>
    /// If a valid race state has been found then change the nextRaceState, class variable,
    /// to the value of the local variable if the found race state is different than 
    /// the current race state.
    /// 
    /// If a race state was not found then return false indicating that there is no change. 
    /// True otherwise.
    /// </summary>
    /// <param name="other">The trigger object</param>
    /// <param name="triggerState">What kind of trigger - Enter, Exit</param>
    /// <returns>True if there is a change in the state.</returns>
    private bool GetNextRaceState(Collider other, TriggerState triggerState)
    {
        RaceState raceState = RaceState.Unintialized;
        if (other.CompareTag("Barrier"))
        {
            BarrierDetection barrierDetection = other.GetComponent<BarrierDetection>();
            if (barrierDetection != null)
            {
                raceState = barrierDetection.GetNextRaceState(triggerState, direction);
                Debug.Log($"Next Race State is: {raceState}");
            }
        }

        // Only change next race state if it is valid and different from the previous state.
        if (raceState != RaceState.Unintialized && raceState != nextRaceState)
        {
            nextRaceState = raceState;
            return true;
        }
        else
        {
            return false;
        }
    }


}

