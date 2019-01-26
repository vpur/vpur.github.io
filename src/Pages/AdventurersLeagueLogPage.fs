[<RequireQualifiedAccess>]
module VPur.UI.Pages.AdventurersLeagueLogPage

open Fable.Helpers.React
open Fulma
open VPur.UI.Routing
open VPur.UI.Components
open VPur.UI.Components.Tiles
open System
open VPur.UI.Components.DnD.AdventurersLeague

let tile =
  let tileContent =
    div [ ]
      [ str "Store your adventurers league logs online :)"
        br [ ]
        Button.a [ Button.Props [ Router.href (DnD.AdventurersLeagueLog |> Page.DnD) ]
                   Button.Color IsInfo ]
                 [ str "Check it out!" ] ]
  { Title = "D&D Adventurers League Log"
    Content = tileContent }

type State = Logsheet.State.T

let init () =
  let fakeLogSheet =
    [ { Logsheet.Data.LogEntry.Id = 0
        Logsheet.Data.LogEntry.SessionNumber = 1
        Logsheet.Data.LogEntry.Date = DateTime(2018, 12, 10)
        Logsheet.Data.LogEntry.Location = "Gamers World (Dublin)"
        Logsheet.Data.LogEntry.AdventureTitle = "DDAL08-04 A Wrinkle in the Weave"
        Logsheet.Data.LogEntry.Tier = Logsheet.Data.Tier.First
        Logsheet.Data.LogEntry.Player =
          { Logsheet.Data.WizardsOfTheCoastAccount.Name = "Vasyl Purchel"
            Logsheet.Data.WizardsOfTheCoastAccount.DCINumber = "pst" }
        Logsheet.Data.LogEntry.CharacterName = "Kirito"
        Logsheet.Data.LogEntry.CharacterClass = Logsheet.Data.Class.Rogue
        Logsheet.Data.LogEntry.CharacterLevel = 1
        Logsheet.Data.LogEntry.CharacterCombatRole = Logsheet.Data.CombatRole.Rogue
        Logsheet.Data.LogEntry.DungeonMaster =
          { Logsheet.Data.WizardsOfTheCoastAccount.Name = "Mariana Gomes"
            Logsheet.Data.WizardsOfTheCoastAccount.DCINumber = "8317105025" }
        Logsheet.Data.LogEntry.AdventureCheckpointsStart = 0
        Logsheet.Data.LogEntry.AdventureCheckpointsEarned = 3
        Logsheet.Data.LogEntry.TreasureCheckpointsStart  = 0
        Logsheet.Data.LogEntry.TreasureCheckpointsEarned = 3
        Logsheet.Data.LogEntry.TreasureCheckpointsSpent  = 0
        Logsheet.Data.LogEntry.GoldStart  = 0
        Logsheet.Data.LogEntry.GoldEarned = 0
        Logsheet.Data.LogEntry.GoldSpent  = 0
        Logsheet.Data.LogEntry.DowntimeStart  = 0
        Logsheet.Data.LogEntry.DowntimeEarned = 5
        Logsheet.Data.LogEntry.DowntimeSpent  = 0
        Logsheet.Data.LogEntry.RenownStart  = 0
        Logsheet.Data.LogEntry.RenownEarned = 0
        Logsheet.Data.LogEntry.Notes = "Mind controlling ring unlocked"
        Logsheet.Data.LogEntry.Image = "assets/al_default.png" }
      { Logsheet.Data.LogEntry.Id = 1
        Logsheet.Data.LogEntry.SessionNumber = 2
        Logsheet.Data.LogEntry.Date = DateTime(2019, 1, 7)
        Logsheet.Data.LogEntry.Location = "Gamers World (Dublin)"
        Logsheet.Data.LogEntry.AdventureTitle = "DDAL08-05 Hero of the Troll Wars"
        Logsheet.Data.LogEntry.Tier = Logsheet.Data.Tier.First
        Logsheet.Data.LogEntry.Player =
          { Logsheet.Data.WizardsOfTheCoastAccount.Name = "Vasyl Purchel"
            Logsheet.Data.WizardsOfTheCoastAccount.DCINumber = "pst" }
        Logsheet.Data.LogEntry.CharacterName = "Kirito"
        Logsheet.Data.LogEntry.CharacterClass = Logsheet.Data.Class.Rogue
        Logsheet.Data.LogEntry.CharacterLevel = 1
        Logsheet.Data.LogEntry.CharacterCombatRole = Logsheet.Data.CombatRole.Rogue
        Logsheet.Data.LogEntry.DungeonMaster =
          { Logsheet.Data.WizardsOfTheCoastAccount.Name = "Gavin Hickey"
            Logsheet.Data.WizardsOfTheCoastAccount.DCINumber = "2318793187" }
        Logsheet.Data.LogEntry.AdventureCheckpointsStart = 3
        Logsheet.Data.LogEntry.AdventureCheckpointsEarned = 4
        Logsheet.Data.LogEntry.TreasureCheckpointsStart  = 3
        Logsheet.Data.LogEntry.TreasureCheckpointsEarned = 4
        Logsheet.Data.LogEntry.TreasureCheckpointsSpent  = 0
        Logsheet.Data.LogEntry.GoldStart  = 0
        Logsheet.Data.LogEntry.GoldEarned = 75
        Logsheet.Data.LogEntry.GoldSpent  = 0
        Logsheet.Data.LogEntry.DowntimeStart  = 5
        Logsheet.Data.LogEntry.DowntimeEarned = 10
        Logsheet.Data.LogEntry.DowntimeSpent  = 0
        Logsheet.Data.LogEntry.RenownStart  = 0
        Logsheet.Data.LogEntry.RenownEarned = 0
        Logsheet.Data.LogEntry.Notes = "Javelin of Lightning unlocked, lvl up"
        Logsheet.Data.LogEntry.Image = "assets/al_default.png" } ]
    |> Logsheet.Data.LogEntries

  Logsheet.State.init (Some fakeLogSheet)

let update = Logsheet.State.apply

let view = Logsheet.View.logsheet
