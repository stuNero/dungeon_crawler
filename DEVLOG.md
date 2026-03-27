# TODO


# Ideas
- DOTs
- Weapon damage types
- Item effects
- Hit Accuracy

# Bugs
- Check why you cant start a fight



    /// <summary>
    /// The currently equipped items. Index mapping:
    /// [0] Primary weapon, [1] Off-hand, [2] Consumable.
    /// Elements may be <c>null</c> when the slot is empty.
    /// </summary>

# Data flow
Game Loads -> (GlobalItems, GlobalClasses) Load -> Start Menu -> StartButton -> 
-> Load save slots
- if exists -> load all data connected to save slot id
- - Entities
- - Inventories
- if not exists ->  
- - Generate new saveSlot -> On player creation generate a new player -> 