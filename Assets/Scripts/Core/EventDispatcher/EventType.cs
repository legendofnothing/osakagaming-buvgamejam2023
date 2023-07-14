namespace Scripts.Core.EventDispatcher {
    /// <summary>
    /// Store EventTypes here, foreach new events create an enum here
    /// </summary>
    public enum EventType {
        None = 0,
    
        //Game State Events
        SwitchToShooting,
        SwitchToEnemy,
        SwitchToShop,
        SwitchToPlayer,
        SwitchToEnd,
        
        //Enemy Manager Events
        EnemyTurn,
        OnEnemyDying,
        EnemyKilled,

        //Bullet Events
        BulletDestroyed,
    
        //Target System Events
        TargetSystemOnTargetHit,
    
        //Pickup Type
        PickupDestroyed,
    
        //Shop Event
        OpenShop,
        CloseShop,
        OnItemBought,

        //UI Events
        OnTextUIChange,
        OnBarUIChange,
        OnDimUI,
        OnPlayerHPChange,
    }
}