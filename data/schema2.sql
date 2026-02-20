PRAGMA foreign_keys = ON;

---------------------------------------------------
-- ENTITIES (Players and Enemies)
---------------------------------------------------
CREATE TABLE Entities (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    entity_type TEXT NOT NULL,    -- 'player' or 'enemy'
    name TEXT NOT NULL,
    alive BOOLEAN NOT NULL,
    hp REAL NOT NULL,
    max_hp REAL NOT NULL,
    dmg REAL NOT NULL,
    xp INTEGER NOT NULL,
    xp_drop INTEGER NOT NULL,
    lvl INTEGER NOT NULL,
    enemy_type INTEGER NULL,      -- Only used if entity_type = 'enemy'
    inventory_size INTEGER NOT NULL
);
---------------------------------------------------
-- ITEMS (Weapons, consumables, etc.)
---------------------------------------------------
CREATE TABLE Items (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    item_type TEXT NOT NULL,      -- 'weapon', 'consumable', etc.
    name TEXT NOT NULL,
    effect_amount REAL NOT NULL,
    weapon_type TEXT NULL,        -- Sword / Axe / etc
    crit_chance REAL NULL,
    crit_damage REAL NULL,
    UNIQUE(name)
);

---------------------------------------------------
-- INVENTORY (Many-to-Many: entity → items)
---------------------------------------------------
CREATE TABLE Inventories (
    entity_id INTEGER NOT NULL REFERENCES Entities(id),
    item_id INTEGER NOT NULL REFERENCES Items(id),
    quantity INTEGER NOT NULL DEFAULT 1,
    PRIMARY KEY (entity_id, item_id)
);

---------------------------------------------------
-- EQUIPPED ITEMS (Equipment slots)
---------------------------------------------------
CREATE TABLE EquippedInventories (
    entity_id INTEGER NOT NULL REFERENCES Entities(id),
    slot INTEGER NOT NULL,           -- 'weapon', 'armor', etc.
    item_id INTEGER NOT NULL REFERENCES Items(id),
    PRIMARY KEY (entity_id, slot)
);

---------------------------------------------------
-- SAVE SLOTS
---------------------------------------------------
CREATE TABLE SaveSlots (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    created_at TEXT DEFAULT (datetime('now'))
);

CREATE TABLE PlayerClasses (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    alive BOOLEAN NOT NULL,
    hp REAL NOT NULL,
    mp REAL NOT NULL,
    max_hp REAL NOT NULL,
    dmg REAL NOT NULL,
    xp INTEGER NOT NULL,
    xp_drop INTEGER NOT NULL,
    lvl INTEGER NOT NULL,
    inventory_size INTEGER NOT NULL,
    UNIQUE (name)
);

CREATE TABLE EntitiesPerSave (
    entity INTEGER NOT NULL REFERENCES Entities(id),
    slot INTEGER NOT NULL REFERENCES SaveSlots(id),
    UNIQUE (entity, slot)
);