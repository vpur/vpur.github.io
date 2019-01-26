module VPur.UI.Components.DnD.AdventurersLeague.Logsheet.State

open Elmish
open Fable.PowerPack
open System

type ViewState =
  | List
  | Full of Data.LogEntry
  | Edit of Data.LogEntry

type T =
  { LogSheet: Data.LogSheet
    ViewState: ViewState
    Busy: bool
    Error: string option }

let init (logsheet: Data.LogSheet option) =
  { LogSheet = match logsheet with | Some data -> data | None -> Data.LogEntries []
    ViewState = ViewState.List
    Busy = false
    Error = None }

type ChangeViewMsg =
  | Full of Data.LogEntry
  | Edit of Data.LogEntry
  | List

type EditViewMsg =
  | UpdatePlayer of Data.WizardsOfTheCoastAccount
  | UpdateDungeonMaster of Data.WizardsOfTheCoastAccount
  | UpdateSessionNumber of int
  | UpdateDate of DateTime
  | UpdateLocation of string
  | UpdateAdventureTitle of string
  | UpdateTier of Data.Tier
  | UpdateCharacterName of string
  | UpdateCharacterCombatRole of Data.CombatRole
  | UpdateCharacterClass of Data.Class
  | UpdateCharacterLevel of int
  | UpdateAdventureCheckpointsStart of int
  | UpdateAdventureCheckpointsEarned of int
  | UpdateTreasureCheckpointsStart of int
  | UpdateTreasureCheckpointsEarned of int
  | UpdateTreasureCheckpointsSpent of int
  | UpdateGoldStart of int
  | UpdateGoldEarned of int
  | UpdateGoldSpent of int
  | UpdateDowntimeStart of int
  | UpdateDowntimeEarned of int
  | UpdateDowntimeSpent of int
  | UpdateRenownStart of int
  | UpdateRenownEarned of int
  | UpdateNotes of string
  | UpdateImage of string
  | Save
  | Cancel

type Msg =
  | ChangeView of ChangeViewMsg
  | AddLogEntry of Data.LogEntry
  | DeleteLogEntry of Data.LogEntry
  | ReplaceLogEntry of Data.LogEntry
  | EditMsg of LogEntryId:int * EditViewMsg
  | SaveFailed of Exception

let addNewLogEntryPromise (logEntry:Data.LogEntry) =
  promise {
    do! Promise.sleep 500
    return logEntry
  }

let replaceLogEntryPromise (logEntry:Data.LogEntry) =
  promise {
    do! Promise.sleep 1000
    failwithf "oh no! db was not accessible :P"
    return logEntry
  }


let openListViewCmd = Cmd.ofMsg (Msg.ChangeView ChangeViewMsg.List)

let apply state : Msg -> T * Cmd<Msg> = function
  | ChangeView (ChangeViewMsg.Full logEntry) ->
      { state with ViewState = ViewState.Full logEntry }, Cmd.none
  | ChangeView (ChangeViewMsg.Edit logEntry) ->
      { state with ViewState = ViewState.Edit logEntry }, Cmd.none
  | ChangeView ChangeViewMsg.List ->
      { state with ViewState = ViewState.List }, Cmd.none

  | SaveFailed err ->
      { state with Error = Some err.Message; Busy = false }, Cmd.none

  | AddLogEntry logEntry ->
      let (Data.LogEntries logEntries) = state.LogSheet
      let newLogEntries =
        logEntry :: logEntries
        |> Data.LogEntries
      { state with LogSheet = newLogEntries; Busy = false; Error = None }, openListViewCmd

  | ReplaceLogEntry logEntry ->
      let (Data.LogEntries logEntries) = state.LogSheet
      let newLogEntries =
        logEntries
        |> List.map(fun entry -> if entry.Id = logEntry.Id then logEntry else entry)
        |> Data.LogEntries
      { state with LogSheet = newLogEntries; Busy = false; Error = None }, openListViewCmd

  | DeleteLogEntry logEntry ->
      let (Data.LogEntries logEntries) = state.LogSheet
      let newLogSheet = Data.LogEntries(logEntries |> List.filter((=) logEntry >> not))
      { state with LogSheet = newLogSheet }, Cmd.ofMsg <| Msg.ChangeView ChangeViewMsg.List

  | EditMsg (logEntryId, msg) ->
      match state.ViewState with
      | ViewState.Edit logEntry ->
          if not (logEntry.Id = logEntryId) then
            Logger.error <| sprintf "Received msg %A for logEntry %i, while editing %i. Ignoring" msg logEntryId logEntry.Id
            state, Cmd.none
          else
            match msg with
            | Save ->
                let (Data.LogEntries logEntries) = state.LogSheet
                logEntries
                |> List.tryFind (fun entry -> entry.Id = logEntryId)
                |> function
                   | Some entry ->
                      { state with Busy = true }, Cmd.ofPromise replaceLogEntryPromise logEntry Msg.ReplaceLogEntry Msg.SaveFailed

                   | None       ->
                      { state with Busy = true }, Cmd.ofPromise addNewLogEntryPromise logEntry Msg.AddLogEntry Msg.SaveFailed


            | Cancel ->
                state, Cmd.ofMsg <| (ChangeViewMsg.List |> Msg.ChangeView)

            | UpdatePlayer value ->
                { state with ViewState = ViewState.Edit { logEntry with Player = value } }, Cmd.none

            | UpdateDungeonMaster value ->
                { state with ViewState = ViewState.Edit { logEntry with DungeonMaster = value } }, Cmd.none

            | UpdateSessionNumber value ->
                { state with ViewState = ViewState.Edit { logEntry with SessionNumber = value } }, Cmd.none

            | UpdateDate value ->
                { state with ViewState = ViewState.Edit { logEntry with Date = value } }, Cmd.none

            | UpdateLocation value ->
                { state with ViewState = ViewState.Edit { logEntry with Location = value } }, Cmd.none

            | UpdateAdventureTitle value ->
                { state with ViewState = ViewState.Edit { logEntry with AdventureTitle = value } }, Cmd.none

            | UpdateTier value ->
                { state with ViewState = ViewState.Edit { logEntry with Tier = value } }, Cmd.none

            | UpdateCharacterName value ->
                { state with ViewState = ViewState.Edit { logEntry with CharacterName = value } }, Cmd.none

            | UpdateCharacterCombatRole value ->
                { state with ViewState = ViewState.Edit { logEntry with CharacterCombatRole = value } }, Cmd.none

            | UpdateCharacterClass value ->
                { state with ViewState = ViewState.Edit { logEntry with CharacterClass = value } }, Cmd.none

            | UpdateCharacterLevel value ->
                { state with ViewState = ViewState.Edit { logEntry with CharacterLevel = value } }, Cmd.none

            | UpdateAdventureCheckpointsStart value ->
                { state with ViewState = ViewState.Edit { logEntry with AdventureCheckpointsStart = value } }, Cmd.none

            | UpdateAdventureCheckpointsEarned value ->
                { state with ViewState = ViewState.Edit { logEntry with AdventureCheckpointsEarned = value } }, Cmd.none

            | UpdateTreasureCheckpointsStart value ->
                { state with ViewState = ViewState.Edit { logEntry with TreasureCheckpointsStart = value } }, Cmd.none

            | UpdateTreasureCheckpointsEarned value ->
                { state with ViewState = ViewState.Edit { logEntry with TreasureCheckpointsEarned = value } }, Cmd.none

            | UpdateTreasureCheckpointsSpent value ->
                { state with ViewState = ViewState.Edit { logEntry with TreasureCheckpointsSpent = value } }, Cmd.none

            | UpdateGoldStart value ->
                { state with ViewState = ViewState.Edit { logEntry with GoldStart = value } }, Cmd.none

            | UpdateGoldEarned value ->
                { state with ViewState = ViewState.Edit { logEntry with GoldEarned = value } }, Cmd.none

            | UpdateGoldSpent value ->
                { state with ViewState = ViewState.Edit { logEntry with GoldSpent = value } }, Cmd.none

            | UpdateDowntimeStart value ->
                { state with ViewState = ViewState.Edit { logEntry with DowntimeStart = value } }, Cmd.none

            | UpdateDowntimeEarned value ->
                { state with ViewState = ViewState.Edit { logEntry with DowntimeEarned = value } }, Cmd.none

            | UpdateDowntimeSpent value ->
                { state with ViewState = ViewState.Edit { logEntry with DowntimeSpent = value } }, Cmd.none

            | UpdateRenownStart value ->
                { state with ViewState = ViewState.Edit { logEntry with RenownStart = value } }, Cmd.none

            | UpdateRenownEarned value ->
                { state with ViewState = ViewState.Edit { logEntry with RenownEarned = value } }, Cmd.none

            | UpdateNotes value ->
                { state with ViewState = ViewState.Edit { logEntry with Notes = value } }, Cmd.none

            | UpdateImage value ->
                { state with ViewState = ViewState.Edit { logEntry with Image = value } }, Cmd.none

      | _ ->
          Logger.errorfn "Requested edit log entry %i, outside edit mode" logEntryId
          state, Cmd.none
