using UnityEngine;

namespace Assets.Scripts
{
    public enum TriggerState { Enter, Exit, None};
    [CreateAssetMenu]
    public class ActionByDirection : ScriptableObject
    {
        public Direction Direction = Direction.Forward;
        public RaceState State  = RaceState.Wait;
        public TriggerState TriggerState = TriggerState.Enter;

        public override string ToString() => $"Direction {Direction}, RaceState {State}, TriggerState {TriggerState}";
    }
}
