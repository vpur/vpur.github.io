[<RequireQualifiedAccess>]
module VPur.UI.Pages.AdventurersLeagueLogPage

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Elmish
open Fulma
open VPur.UI.Routing
open VPur.UI.Components
open VPur.UI.Components.Tiles
open System

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

let fakeData : AdventurersLeagueLogEntry.LogEntry[] =
  [| { Id                     = 1
       SessionNumber          = 1
       Date                   = DateTime(2018, 12, 10)
       AdventureTitle         = "DDAL08-04 A Wrinkle in the Weave"
       Tier                   = AdventurersLeagueLogEntry.Tier.First
       AdvancementCheckpoints = 3
       TreasureCheckpoints    = 3
       Downtime               = 5
       GoldPieces             = 0
       Location               = "Gamers World (Dublin)"
       DungeonMaster          = "Mariana Gomes (8317105025)"
       Notes                  = "Mind controlling ring unlocked"
       Image                  = ""
       Character              = { Id = 0; Name = "Kirito"; Level = 1; Class = "Rogue" } }

     { Id                     = 2
       SessionNumber          = 2
       Date                   = DateTime(2019, 1, 7)
       AdventureTitle         = "DDAL08-05 Hero of the Troll Wars"
       Tier                   = AdventurersLeagueLogEntry.Tier.First
       AdvancementCheckpoints = 4
       TreasureCheckpoints    = 4
       Downtime               = 10
       GoldPieces             = 75
       Location               = "Gamers World (Dublin)"
       DungeonMaster          = "Gavin Hickey (2318793187)"
       Notes                  = "Javelin of Lightning unlocked, lvl up"
       Image                  = "assets/hero_of_the_trolls_war.jpeg"
       Character              = { Id = 0; Name = "Kirito"; Level = 1; Class = "Rogue" } } |]

type Msg =
  | LogEntryMsg of AdventurersLeagueLogEntry.Msg

type State =
  { LogEntries: AdventurersLeagueLogEntry.State[] }
  with static member Empty =
         let dataToEntry logEntry =
           { AdventurersLeagueLogEntry.State.Empty with LogEntry = logEntry }
         { LogEntries = fakeData |> Array.map dataToEntry }

let update state = function
  | LogEntryMsg msg ->
      let mutable cmd = Cmd.none
      let newLogEntries =
        state.LogEntries
        |> Array.map (fun logEntry ->
            let (newLogEntry, currCmd) = AdventurersLeagueLogEntry.update logEntry msg
            if not currCmd.IsEmpty then Logger.debugfn "updated state of log entry #%i" logEntry.LogEntry.Id; cmd <- currCmd
            newLogEntry )
      { state with LogEntries = newLogEntries }, Cmd.map LogEntryMsg cmd

let view dispatch state =
  div [ ]
      [ yield! state.LogEntries |> Array.map (AdventurersLeagueLogEntry.view (LogEntryMsg >> dispatch))
        yield Button.a [ Button.Props [ OnClick <| fun _ -> Logger.debug "wanna add new log entry" ]
                         Button.Color IsInfo ]
                       [ str "Add new" ] ]
