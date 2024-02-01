using UnityEngine;

namespace Assets.Scripts
{
    public enum TriggerState { Enter, Exit, None};
    [CreateAssetMenu]
    public class ActionByDirection : ScriptableObject
    {
        public Direction Direction = Direction.Forward;
        public RaceState RaceState  = RaceState.Wait;
        public TriggerState TriggerState = TriggerState.Enter;
        private RaceState unintialized;
        private TriggerState state;


        public ActionByDirection SetValues(RaceState raceState, TriggerState triggerState, Direction direction)
        {
            this.RaceState = raceState;
            this.state = triggerState;
            Direction = direction;
            return this;
        }

        /// <summary>
        /// Match on TriggerState and Direction to find 
        /// the matching ActionByDirection.
        /// </summary>
        /// <param name="match"></param>
        /// <returns>True if there is a match on the RaceState and TriggerState values.</returns>
        public bool IsMatchForUnitialized(ActionByDirection match)
        {
            if (match.RaceState != RaceState.Unintialized)
                return false;

            return match.TriggerState == this.TriggerState && match.Direction == this.Direction;    
        }

        public override string ToString() => $"Direction {Direction}, RaceState {RaceState}, TriggerState {TriggerState}";
    }
}
