module VPur.UI.Components.DnD.AdventurersLeague.Logsheet.Data

open System

type Tier =
  | First
  | Second
  | Third
  | Fourth
  static member Parse = function
    | "First"  -> Tier.First
    | "Second" -> Tier.Second
    | "Third"  -> Tier.Third
    | "Fourth" -> Tier.Fourth
    | other -> failwithf "Unknown tier received: %s" other
  static member Values =
    [ "First"; "Second"; "Third"; "Fourth" ]

type Class =
  | Assassin
  | Barbarian
  | Bard
  | Cleric
  | Druid
  | Fighter
  | Monk
  | Paladin
  | Psion
  | Ranger
  | Rogue
  | Sorcerer
  | Warlock
  | Warlord
  | Wizard
  static member Parse = function
    | "Assassin"  -> Class.Assassin
    | "Barbarian" -> Class.Barbarian
    | "Bard"      -> Class.Bard
    | "Cleric"    -> Class.Cleric
    | "Druid"     -> Class.Druid
    | "Fighter"   -> Class.Fighter
    | "Monk"      -> Class.Monk
    | "Paladin"   -> Class.Paladin
    | "Psion"     -> Class.Psion
    | "Ranger"    -> Class.Ranger
    | "Rogue"     -> Class.Rogue
    | "Sorcerer"  -> Class.Sorcerer
    | "Warlock"   -> Class.Warlock
    | "Warlord"   -> Class.Warlord
    | "Wizard"    -> Class.Wizard
    | str -> failwithf "Unknown class received: %s" str
  static member Values =
    [ "Assassin"; "Barbarian"; "Bard"; "Cleric"; "Druid"; "Fighter"; "Monk"; "Paladin"; "Psion";
      "Ranger"; "Rogue"; "Sorcerer"; "Warlock"; "Warlord"; "Wizard" ]

type CombatRole =
  | ``Arcane Caster``
  | Archer
  | Artillery
  | Cavalry
  | Controller
  | Defender
  | ``Divine Caster``
  | Generalist
  | ``Leader - Healing``
  | ``Leader - Tactical``
  | ``Light Infantry``
  | ``Medic - Support``
  | ``Psionic Manifester``
  | Rogue
  | ``Striker - Melee``
  | ``Striker - Ranger``
  | Summoner
  | Tank
  static member Parse = function
    | "Arcane Caster"      -> CombatRole.``Arcane Caster``
    | "Archer"             -> CombatRole.Archer
    | "Artillery"          -> CombatRole.Artillery
    | "Cavalry"            -> CombatRole.Cavalry
    | "Controller"         -> CombatRole.Controller
    | "Defender"           -> CombatRole.Defender
    | "Divine Caster"      -> CombatRole.``Divine Caster``
    | "Generalist"         -> CombatRole.Generalist
    | "Leader - Healing"   -> CombatRole.``Leader - Healing``
    | "Leader - Tactical"  -> CombatRole.``Leader - Tactical``
    | "Light Infantry"     -> CombatRole.``Light Infantry``
    | "Medic - Support"    -> CombatRole.``Medic - Support``
    | "Psionic Manifester" -> CombatRole.``Psionic Manifester``
    | "Rogue"              -> CombatRole.Rogue
    | "Striker - Melee"    -> CombatRole.``Striker - Melee``
    | "Striker - Ranger"   -> CombatRole.``Striker - Ranger``
    | "Summoner"           -> CombatRole.Summoner
    | "Tank"               -> CombatRole.Tank
    | other -> failwithf "Unknown combat role received: %s" other
  static member Values =
    [ "Arcane Caster"; "Archer"; "Artillery"; "Cavalry"; "Controller"; "Defender"; "Divine Caster";
      "Generalist"; "Leader - Healing"; "Leader - Tactical"; "Light Infantry"; "Medic - Support";
      "Psionic Manifester"; "Rogue"; "Striker - Melee"; "Striker - Ranger"; "Summoner"; "Tank" ]

type WizardsOfTheCoastAccount =
  { Name:      string
    DCINumber: string }
  static member Empty =
    { Name = ""
      DCINumber = "" }

type LogEntry =
  { Id: int
    Player: WizardsOfTheCoastAccount
    DungeonMaster: WizardsOfTheCoastAccount
    SessionNumber: int
    Date: DateTime
    Location: string
    AdventureTitle: string
    Tier: Tier
    CharacterName: string
    CharacterCombatRole: CombatRole
    CharacterClass: Class
    CharacterLevel: int
    AdventureCheckpointsStart: int
    AdventureCheckpointsEarned: int
    TreasureCheckpointsStart: int
    TreasureCheckpointsEarned: int
    TreasureCheckpointsSpent: int
    GoldStart: int
    GoldEarned: int
    GoldSpent: int
    DowntimeStart: int
    DowntimeEarned: int
    DowntimeSpent: int
    RenownStart: int
    RenownEarned: int
    Notes: string
    Image: string }
  static member Empty =
    { Id = -1
      Player = WizardsOfTheCoastAccount.Empty
      DungeonMaster = WizardsOfTheCoastAccount.Empty
      SessionNumber = 0
      Date = DateTime.Today
      Location = ""
      AdventureTitle = ""
      Tier = Tier.First
      CharacterName = ""
      CharacterCombatRole = CombatRole.``Arcane Caster``
      CharacterClass = Class.Assassin
      CharacterLevel = 1
      AdventureCheckpointsStart = 0
      AdventureCheckpointsEarned = 0
      TreasureCheckpointsStart = 0
      TreasureCheckpointsEarned = 0
      TreasureCheckpointsSpent = 0
      GoldStart = 0
      GoldEarned = 0
      GoldSpent = 0
      DowntimeStart = 0
      DowntimeEarned = 0
      DowntimeSpent = 0
      RenownStart = 0
      RenownEarned = 0
      Notes = ""
      Image = "assets/al_default.png" }

type LogSheet = LogEntries of LogEntry list
