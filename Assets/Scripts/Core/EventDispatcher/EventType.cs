namespace Core.EventDispatcher {
    /// <summary>
    /// Store EventTypes here, foreach new events create an enum here
    /// </summary>
    public enum EventType {
        None = 0,
        
        OnTurnBegin,
        OnTurnEnd,
        
        OnSurvivorAdded,
        OnSurvivorDecreased,
        OnSurvivorEnteredBase,
        
        OnPlayerTakeDamage,
        OnPlayerDeath,
        
        OnTransferDefendersToResearchers,
        OnTransferResearchersToDefenders, 
        
        OnModifierActivated,
        OnModifierDeactivated,
        
        OnEnemyDie,
        OnEnemyConvert,
        OnBarUIChange,
        OnTextUIChange,
        OnWeaponChange,
        OnMolotovAdded,
        OnCureReset,
    }
}