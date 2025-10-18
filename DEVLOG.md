# TODO
- Fully implement basic combat
- Save to file
    - each character has a save file
- Make a lot of weapons and flesh out item system with different types and crit variable etc.



# Save Slot System — Design Summary
 ## Overview
    A practical guide to implementing a save slot system for a console-based RPG. This summary outlines
    the components, structure, workflows, and best practices for reliable and extensible save handling.
 
 ## Core Components
    • SaveSystem — Manages slots, creates/overwrites/deletes saves, and serializes data to disk.
    • SaveData (or GameState) — Structured object capturing player, world, entities, and metadata.
    • Menu Integration — Provides options for Start, Load, Save, and Delete/Overwrite slots.
 
 ## What to Save (SaveData fields)
    • Player: name, level, HP, MaxHP, position, inventory, equipped items, stats.
    • World: map layout, unlocked/cleared rooms, story/quest flags.
    • Entities: persistent entities with HP, alive status, position, and state.
    • Metadata: save timestamp, playtime, slot name, and version number for compatibility.

 ## File Organization
    Use a dedicated saves folder, one file per slot (e.g., JSON):
    /saves/
        slot1.json
        slot2.json
        slot3.json
    This structure is simple, human-readable, and easy to manage. For larger saves, consider per-slot
    folders or compression.
    Save / Load Workflows
    Saving (SaveGame):
    1 Gather world state into a SaveData object (serialize IDs, not object references).
    2 Add metadata such as timestamp and playtime.
    3 Serialize to JSON or binary and write to /saves/slot{N}.json.
    4 Use atomic saves — write to a temp file and then rename it.
    Loading (LoadGame):
    1 Read and deserialize the file into a SaveData object.
    2 Reconstruct the world — player, entities, and map.
    3 Validate version compatibility and handle missing data safely.

 ## Example SaveData JSON Skeleton
    {
    "metadata": {
        "timestamp": "2025-10-16T12:00:00Z",
        "playtimeSeconds": 732,
        "version": "1.0.0",
        "slotName": "Auto Save 1"
    },
    "player": {
        "name": "Hero",
        "level": 5,
        "hp": 42,
        "maxHp": 50,
        "position": { "roomId": "tavern", "x": 4, "y": 2 },
        "inventory": [
        { "instanceId": "b1f3...", "baseId": "HealthPotion", "count": 2 },
        { "instanceId": "c8a9...", "baseId": "IronSword" }
        ]
    },
    "world": {
        "roomsCleared": ["dungeon_entrance"],
        "entities": [
        { "instanceId": "e1...", "baseType": "Goblin", "hp": 0, "alive": false, 
            "position": { "roomId": "dungeon_entrance", "x": 2, "y": 3 } }
        ]
    }
    }

 ## Tips & Best Practices
    • Use deterministic base IDs for item types and GUIDs for instances.
    • Serialize IDs and properties, not in-memory pointers.
    • Always use atomic saves to prevent corruption.
    • Include a version number for backward compatibility.
    • Use human-readable JSON for development; compress or encrypt for release