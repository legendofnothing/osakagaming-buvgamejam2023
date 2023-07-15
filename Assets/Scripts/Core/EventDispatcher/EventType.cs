namespace Core.EventDispatcher {
    /// <summary>
    /// Store EventTypes here, foreach new events create an enum here
    /// </summary>
    public enum EventType {
        None = 0,
        
        OnSurvivorAdded,
        OnSurvivorDecreased,
        OnSurvivorEnteredBase,
        
        OnPlayerTakeDamage,
    }
}