module VPur.UI.Components.AdventurersLeagueLogEntry

open System
open Elmish
open Fulma
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.FontAwesome
open Fable.Import.React
open Fable.PowerPack

type Tier =
  | First
  | Second
  | Third
  | Fourth

type DnDCharacter =
  { Id:    int
    Name:  string
    Class: string
    Level: int }
  static member Empty =
    { Id    = -1
      Name  = ""
      Class = ""
      Level = 1 }

type LogEntry =
  { Id:                     int
    SessionNumber:          int
    Date:                   DateTime
    AdventureTitle:         string
    Tier:                   Tier
    AdvancementCheckpoints: int
    TreasureCheckpoints:    int
    Downtime:               int
    GoldPieces:             int
    Location:               string
    DungeonMaster:          string
    Notes:                  string
    Image:                  string
    Character:              DnDCharacter }
  static member Empty =
    { Id                     = -1
      SessionNumber          = -1
      Date                   = DateTime.UtcNow
      AdventureTitle         = ""
      Tier                   = Tier.First
      AdvancementCheckpoints = 0
      TreasureCheckpoints    = 0
      Downtime               = 0
      GoldPieces             = 0
      Location               = ""
      DungeonMaster          = ""
      Notes                  = ""
      Image                  = ""
      Character              = DnDCharacter.Empty }

type State =
  { LogEntry:          LogEntry
    FullDetailsActive: bool
    EditActive:        bool }
  static member Empty =
    { LogEntry          = LogEntry.Empty
      FullDetailsActive = false
      EditActive        = false }

type Msg =
  | ShowDetails of int
  | HideDetails
  | Edit of int
  | SaveEdit of LogEntry
  | CancelEdit

let update state = function
  | ShowDetails logEntryId ->
      if logEntryId = state.LogEntry.Id
      then { state with FullDetailsActive = true }, Cmd.none
      else { state with FullDetailsActive = false }, Cmd.none

  | HideDetails ->
      { state with FullDetailsActive = false }, Cmd.none

  | Edit logEntryId ->
      if logEntryId = state.LogEntry.Id
      then { state with EditActive = true }, Cmd.none
      else { state with EditActive = false }, Cmd.none

  | SaveEdit logEntry ->
      if logEntry.Id = state.LogEntry.Id
      then { state with LogEntry = logEntry; EditActive = false }, Cmd.none
      else { state with EditActive = false }, Cmd.none

  | CancelEdit ->
      { state with EditActive = false }, Cmd.none

let private deleteButton dispatch logEntryId =
  let clickAction (clickEvent:MouseEvent) =
    clickEvent.stopPropagation()
    Logger.debug <| sprintf "Requested to delete log entry %i" logEntryId
    // dispatch <| Msg.Delete logEntryId

  Button.a
    [ Button.Props [ OnClick clickAction ]
      Button.Color IsDanger ]
    [ Icon.icon [ ] [ Fa.i [ Fa.Regular.TrashAlt ] [ ] ] ]

let cardSection title content =
  Level.item [ Level.Item.HasTextCentered ]
    [ div [ ]
        [ Level.heading [ ] [ str title ]
          Level.item [ ] [ content ] ] ]

let fullView dispatch state =
  Card.card [ ]
    [ Card.content [ ]
        [ Content.content [ ]
            [ h1 [ ]
                [ str <| sprintf "#%i %s" state.LogEntry.SessionNumber state.LogEntry.AdventureTitle ]
              p [ ]
                [ str "Session was at "
                  b [ ] [ str <| state.LogEntry.Location ]
                  str " on "
                  b [ ] [ str <| Date.Format.localFormat Date.Local.englishUK "dddd, d MMMM yyyy" state.LogEntry.Date ] ]
              p [ ]
                [ str "Supervised by "
                  b [ ] [ str <| state.LogEntry.DungeonMaster ]
                  str " as a Dungeon Master" ]
              p [ ]
                [ str "Played as "
                  b [ ] [ str <| state.LogEntry.Character.Name ]
                  str ", level "
                  b [ ] [ str <| sprintf "%i" state.LogEntry.Character.Level ]
                  str " "
                  b [ ] [ str <| state.LogEntry.Character.Class ]
                  str " in a role of a "
                  b [ ] [ str <| "(tank/rogue/...) n/a" ] ]
              h3 [ ]
                [ str "Results" ]
              p [ ]
                [ Table.table [ Table.IsBordered
                                Table.IsFullWidth
                                Table.IsStriped ]
                    [ thead [ ]
                        [ tr [ ]
                            [ th [ ] [ div [ ] [ ] ]
                              th [ ] [ str "Starting amount" ]
                              th [ ] [ str "Earned amount" ]
                              th [ ] [ str "Total amount" ] ] ]
                      tbody [ ]
                        [ tr [ ]
                            [ th [ ] [ str "Adventure Checkpoints" ]
                              td [ ] [ str <| "0" ]
                              td [ ] [ str <| sprintf "%i" state.LogEntry.AdvancementCheckpoints ]
                              td [ ] [ str <| sprintf "%i" state.LogEntry.AdvancementCheckpoints ] ]
                          tr [ ]
                            [ th [ ] [ str "Treasure Checkpoints" ]
                              td [ ] [ str <| "0" ]
                              td [ ] [ str <| sprintf "%i" state.LogEntry.TreasureCheckpoints ]
                              td [ ] [ str <| sprintf "%i" state.LogEntry.TreasureCheckpoints ] ]
                          tr [ ]
                            [ th [ ] [ str "Gold" ]
                              td [ ] [ str <| "n/a" ]
                              td [ ] [ str <| "n/a" ]
                              td [ ] [ str <| "n/a" ] ]
                          tr [ ]
                            [ th [ ] [ str "Downtime" ]
                              td [ ] [ str <| "n/a" ]
                              td [ ] [ str <| "n/a" ]
                              td [ ] [ str <| "n/a" ] ]
                          tr [ ]
                            [ th [ ] [ str "Renown" ]
                              td [ ] [ str <| "n/a" ]
                              td [ ] [ str <| "n/a" ]
                              td [ ] [ str <| "n/a" ] ] ] ] ]
              h3 [ ]
                [ str "Notes" ]
              p [ ]
                [ str <| state.LogEntry.Notes ] ] ]
      Card.footer [ ]
        [ Card.Footer.a
            [ Props [ OnClick <| fun _ -> dispatch <| Msg.Edit state.LogEntry.Id ] ]
            [ Icon.icon [ ] [ Fa.i [ Fa.Regular.Edit ] [ ] ]
              str " Edit " ]
          Card.Footer.a
            [ Props [ OnClick <| fun _ -> Logger.debug <| sprintf "Requested to delete log entry %i" state.LogEntry.Id ] ]
            [ Icon.icon [ ] [ Fa.i [ Fa.Regular.TrashAlt ] [ ] ]
              str " Delete " ] ] ]

let shortView dispatch state =
  let character =
    sprintf "%s - Lvl %i %s" state.LogEntry.Character.Name
                             state.LogEntry.Character.Level
                             state.LogEntry.Character.Class
  Card.card [ Props [ OnClick <| fun _ -> dispatch <| Msg.ShowDetails state.LogEntry.Id ] ]
    [ Card.content [ ]
        [ Level.level [ ]
            [ cardSection "Session"  (str <| sprintf "#%i " state.LogEntry.SessionNumber)
              cardSection "Title"    (str <| state.LogEntry.AdventureTitle)
              cardSection "Caracter" (str <| character)
              cardSection "Date"     (str <| state.LogEntry.Date.ToString("yyyy/MM/dd")) ] ] ]

// TODO: when modal window is shown the background is still scrollable
let view dispatch state =
  Container.container [ Container.IsFluid ]
    [ Content.content [ ]
        [ shortView dispatch state
          Modal.modal [ Modal.IsActive state.FullDetailsActive ]
            [ Modal.background [ Props [ OnClick <| fun _ -> dispatch Msg.HideDetails ] ] [ ]
              Modal.content [ ] [ fullView dispatch state ] ] ] ]
