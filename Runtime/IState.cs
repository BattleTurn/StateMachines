namespace BattleTurn.StateMachines
{
    /// <summary>
    /// Interface for defining states in a state machine.
    /// </summary>
    public interface IState
    {
        void OnEnter();    // Called when entering the state
        void OnExit();     // Called when leaving the state
        void Tick();       // Called every Update frame
        void FixedTick();  // Called every FixedUpdate (physics)
    }
}