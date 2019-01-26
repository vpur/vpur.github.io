module VPur.UI.Components.DnD.AdventurersLeague.Logsheet.View

open System
open Elmish
open Fulma
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.FontAwesome
open Fable.Import.React
open Fable.PowerPack
open Fable.Core.JsInterop

type Tier = VPur.UI.Components.DnD.AdventurersLeague.Logsheet.Data.Tier
type CharacterClass = VPur.UI.Components.DnD.AdventurersLeague.Logsheet.Data.Class
type CombatRole = VPur.UI.Components.DnD.AdventurersLeague.Logsheet.Data.CombatRole

let formatDate format value =
  Date.Format.localFormat Date.Local.englishUK format value

let cardSection title content =
  Level.item [ Level.Item.HasTextCentered ]
    [ div [ ]
        [ Level.heading [ ] [ str title ]
          Level.item [ ] [ content ] ] ]

let private shortEntryView dispatch (logEntry:Data.LogEntry) =
  let character =
    sprintf "%s (%A %i)" logEntry.CharacterName
                         logEntry.CharacterClass
                         logEntry.CharacterLevel
  Box.box'
    [ Props [ OnClick <| fun _ -> dispatch <| State.Msg.ChangeView (State.ChangeViewMsg.Full logEntry) ] ]
    [ div [ ]
        [ Media.media [ Media.Modifiers [ Modifier.IsHiddenOnly (Screen.Touch, true) ] ]
            [ Media.left [ ]
                [ Image.image
                    [ Image.Is64x64 ]
                    [ img [ Src logEntry.Image ] ] ]
              Media.content [ ]
                [ Level.level [ ]
                    [ cardSection "Session"  (str <| sprintf "#%i " logEntry.SessionNumber)
                      cardSection "Title"    (str <| logEntry.AdventureTitle)
                      cardSection "Caracter" (str <| character)
                      cardSection "Date"     (str <| Date.Format.localFormat Date.Local.englishUK "dddd, d MMMM yyyy" logEntry.Date) ] ] ]
          Media.media [ Media.Modifiers [ Modifier.IsHidden (Screen.Mobile, true); Modifier.IsHidden (Screen.Desktop, true) ] ]
            [ Media.left [ ]
                [ Image.image
                    [ Image.Is32x32 ]
                    [ img [ Src logEntry.Image ] ] ]
              Media.content [ ]
                [ Level.level [ ]
                    [ cardSection "Session"  (str <| sprintf "#%i %s" logEntry.SessionNumber logEntry.AdventureTitle)
                      cardSection "Caracter" (str <| character)
                      cardSection "Date"     (str <| Date.Format.localFormat Date.Local.englishUK "dd/MM/yy" logEntry.Date) ] ] ]
          Media.media [ Media.Modifiers [ Modifier.IsHidden (Screen.Tablet, true); Modifier.IsMarginless ] ]
            [ Media.left [ ]
                [ Image.image
                    [ Image.Is16x16 ]
                    [ img [ Src logEntry.Image ] ] ]
              Media.content [ ]
                [ str <| sprintf "#%i %s on %s" logEntry.SessionNumber
                           logEntry.AdventureTitle
                           (formatDate "dd/MM/yy" logEntry.Date) ] ] ] ]

let private listView dispatch (state:State.T) =
  let addNewEntry _ev =
    VPur.UI.Components.DnD.AdventurersLeague.Logsheet.Data.LogEntry.Empty
    |> State.ChangeViewMsg.Edit
    |> State.Msg.ChangeView
    |> dispatch
  let (Data.LogEntries logEntries) = state.LogSheet
  div [ ]
    [ yield! logEntries |> List.map (shortEntryView dispatch)
      yield Button.a [ Button.Props [ OnClick addNewEntry ]
                       Button.Color IsInfo ]
                     [ str "Add new" ] ]

let private fullView dispatch (logEntry:Data.LogEntry) =
  Card.card [ ]
    [ Card.content [ ]
        [ Content.content [ ]
            [ Media.media [ ]
                [ Media.left [ ]
                    [ Image.image [ Image.Is64x64 ] [ img [ Src logEntry.Image ] ] ]
                  Media.content [ ]
                    [ h1 [ ]
                        [ str <| sprintf "#%i %s" logEntry.SessionNumber logEntry.AdventureTitle ] ] ]
              p [ ]
                [ b [ ] [ str <| sprintf "%A" logEntry.Tier ]
                  str " tier session was at "
                  b [ ] [ str <| logEntry.Location ]
                  str " on "
                  b [ ] [ str <| Date.Format.localFormat Date.Local.englishUK "dddd, d MMMM yyyy" logEntry.Date ] ]
              p [ ]
                [ str "Supervised by "
                  b [ ] [ str <| sprintf "%s (%s)" logEntry.DungeonMaster.Name logEntry.DungeonMaster.DCINumber ]
                  str " as a Dungeon Master" ]
              p [ ]
                [ str "Played as "
                  b [ ] [ str <| logEntry.CharacterName ]
                  str ", level "
                  b [ ] [ str <| sprintf "%i" logEntry.CharacterLevel ]
                  str " "
                  b [ ] [ str <| sprintf "%A" logEntry.CharacterClass ]
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
                              th [ ] [ str "Spent amount" ]
                              th [ ] [ str "Total amount" ] ] ]
                      tbody [ ]
                        [ tr [ ]
                            [ th [ ] [ str "Adventure Checkpoints" ]
                              td [ ] [ str <| sprintf "%i" logEntry.AdventureCheckpointsStart ]
                              td [ ] [ str <| sprintf "%i" logEntry.AdventureCheckpointsEarned ]
                              td [ ] [ str "" ]
                              td [ ] [ str <| sprintf "%i" (logEntry.AdventureCheckpointsStart + logEntry.AdventureCheckpointsEarned) ] ]
                          tr [ ]
                            [ th [ ] [ str "Treasure Checkpoints" ]
                              td [ ] [ str <| sprintf "%i" logEntry.TreasureCheckpointsStart ]
                              td [ ] [ str <| sprintf "%i" logEntry.TreasureCheckpointsEarned ]
                              td [ ] [ str <| sprintf "%i" logEntry.TreasureCheckpointsSpent ]
                              td [ ] [ str <| sprintf "%i" (logEntry.TreasureCheckpointsStart + logEntry.TreasureCheckpointsEarned - logEntry.TreasureCheckpointsSpent) ] ]
                          tr [ ]
                            [ th [ ] [ str "Gold" ]
                              td [ ] [ str <| sprintf "%i" logEntry.GoldStart ]
                              td [ ] [ str <| sprintf "%i" logEntry.GoldEarned ]
                              td [ ] [ str <| sprintf "%i" logEntry.GoldSpent ]
                              td [ ] [ str <| sprintf "%i" (logEntry.GoldStart + logEntry.GoldEarned - logEntry.GoldSpent) ] ]
                          tr [ ]
                            [ th [ ] [ str "Downtime" ]
                              td [ ] [ str <| sprintf "%i" logEntry.DowntimeStart ]
                              td [ ] [ str <| sprintf "%i" logEntry.DowntimeEarned ]
                              td [ ] [ str <| sprintf "%i" logEntry.DowntimeSpent ]
                              td [ ] [ str <| sprintf "%i" (logEntry.DowntimeStart + logEntry.DowntimeEarned - logEntry.DowntimeSpent) ] ]
                          tr [ ]
                            [ th [ ] [ str "Renown" ]
                              td [ ] [ str <| sprintf "%i" logEntry.RenownStart ]
                              td [ ] [ str <| sprintf "%i" logEntry.RenownEarned ]
                              td [ ] [ str "" ]
                              td [ ] [ str <| sprintf "%i" (logEntry.RenownStart + logEntry.RenownEarned) ] ] ] ] ]
              h3 [ ]
                [ str "Notes" ]
              p [ ]
                [ str <| logEntry.Notes ] ] ]
      Card.footer [ ]
        [ Card.Footer.a
            [ Props [ OnClick <| fun _ -> dispatch <| State.Msg.ChangeView (State.ChangeViewMsg.Edit logEntry) ] ]
            [ Icon.icon [ ] [ Fa.i [ Fa.Regular.Edit ] [ ] ]
              str " Edit " ]
          Card.Footer.a
            [ Props [ OnClick <| fun _ -> dispatch <| State.Msg.DeleteLogEntry logEntry ] ]
            [ Icon.icon [ ] [ Fa.i [ Fa.Regular.TrashAlt ] [ ] ]
              str " Delete " ] ] ]

let private editView dispatch (state:State.T) (logEntry:Data.LogEntry) =
  let save _mouseEvent = dispatch <| State.Msg.EditMsg (logEntry.Id, State.EditViewMsg.Save)
  let cancel _mouseEvent = dispatch <| State.Msg.EditMsg (logEntry.Id, State.EditViewMsg.Cancel)

  let onInputChange onChange (ev:FormEvent) =
    State.Msg.EditMsg (logEntry.Id, !!ev.target?value |> onChange)
    |> dispatch

  let formTextField label value onChange =
    Field.div [ Field.HasAddons ]
      [ Control.p [ ] [ Button.a [ Button.IsStatic true ] [ str label ] ]
        Control.p [ Control.IsExpanded ]
          [ Input.text [ Input.Value value
                         Input.Disabled state.Busy
                         Input.Props [ OnChange <| onInputChange onChange ] ] ] ]

  let formIntField label value onChange =
    formTextField label (sprintf "%i" value) onChange

  let formLargeTextField label placeholder value onChange =
    Field.div [ ]
      [ Label.label [ ] [ str label ]
        Control.div [ ]
          [ Textarea.textarea [ Textarea.Value <| sprintf "%s" value
                                Textarea.Placeholder placeholder
                                Textarea.Disabled state.Busy
                                Textarea.Props [ OnChange <| onInputChange onChange ] ]
              [ ] ] ]

  let formDropDownField label values currentValue onChange =
    Field.div [ Field.HasAddons ]
      [ Control.p [ ] [ Button.a [ Button.IsStatic true ] [ str label ] ]
        Control.p [ Control.IsExpanded ]
          [ Select.select [ Select.Props [ OnChange <| onInputChange onChange ]; Select.IsFullWidth ]
              [ select [ DefaultValue currentValue ]
                  (values |> List.map (fun v -> option [ Value v ] [ str v ] )) ] ] ]

  let imageAttachment value onChange =
    Field.div []
      [ Media.media [ ]
          [ Media.left [ ]
              [ Image.image [ Image.Is64x64 ] [ img [ Src value ] ] ]
            Media.content [ ]
              [ formTextField "Url" value onChange ] ] ]

  let formSection label isHorizontal elements =
    if isHorizontal then
      Field.p [ Field.IsHorizontal ]
        [ Field.label [ ] [ Label.label [ ] [ str label ] ]
          Field.body [ ] elements ]
    else
      Field.div [ ]
        [ yield Label.label [ ] [ b [ ] [ str label ] ]
          yield! elements ]

  let errorMsg =
    match state.Error with
    | Some error ->
        Notification.notification [ Notification.Color IsDanger ]
          [ Notification.delete [ ] [ ]
            str error ]

    | None ->
        div [ ] [ ]
  Card.card [ ]
    [ Card.content [ ]
        [ Content.content [ ]
            [ form [ ]
                [ errorMsg
                  //   Player: WizardsOfTheCoastAccount
                  formSection "Session details" false
                    [ formIntField "Session number" logEntry.SessionNumber (int >> State.EditViewMsg.UpdateSessionNumber)
                      // Date: DateTime
                      formTextField "Location" logEntry.Location State.EditViewMsg.UpdateLocation
                      formTextField "Adventure title" logEntry.AdventureTitle State.EditViewMsg.UpdateAdventureTitle
                      formDropDownField "Tier" Tier.Values logEntry.Tier (Tier.Parse >> State.EditViewMsg.UpdateTier)
                    ]
                  formSection "Character" false
                    [ formTextField "Name" logEntry.CharacterName State.EditViewMsg.UpdateCharacterName
                      formDropDownField "Combat role" CombatRole.Values logEntry.CharacterCombatRole (CombatRole.Parse >> State.EditViewMsg.UpdateCharacterCombatRole)
                      formDropDownField "Character class" CharacterClass.Values logEntry.CharacterClass (CharacterClass.Parse >> State.EditViewMsg.UpdateCharacterClass)
                      formDropDownField "Character level" ([1..20] |> List.map string) logEntry.CharacterLevel (int >> State.EditViewMsg.UpdateCharacterLevel)
                   ]
                  formSection "Dungeon Master" true [ ]
                  formSection "Player" true [ ]
                  formSection "Adventure Checkpoints" true
                    [ formIntField "Starting" logEntry.AdventureCheckpointsStart (int >> State.EditViewMsg.UpdateAdventureCheckpointsStart)
                      formIntField "Earned" logEntry.AdventureCheckpointsEarned (int >> State.EditViewMsg.UpdateAdventureCheckpointsEarned) ]
                  formSection "Treasure Checkpoints" true
                    [ formIntField "Starting" logEntry.TreasureCheckpointsStart (int >> State.EditViewMsg.UpdateTreasureCheckpointsStart)
                      formIntField "Earned" logEntry.TreasureCheckpointsEarned (int >> State.EditViewMsg.UpdateTreasureCheckpointsEarned)
                      formIntField "Spent" logEntry.TreasureCheckpointsSpent (int >> State.EditViewMsg.UpdateTreasureCheckpointsSpent) ]
                  formSection "Gold" true
                    [ formIntField "Starting" logEntry.GoldStart (int >> State.EditViewMsg.UpdateGoldStart)
                      formIntField "Earned" logEntry.GoldEarned (int >> State.EditViewMsg.UpdateGoldEarned)
                      formIntField "Spent" logEntry.GoldSpent (int >> State.EditViewMsg.UpdateGoldSpent) ]
                  formSection "Downtime" true
                    [ formIntField "Starting" logEntry.DowntimeStart (int >> State.EditViewMsg.UpdateDowntimeStart)
                      formIntField "Earned" logEntry.DowntimeEarned (int >> State.EditViewMsg.UpdateDowntimeEarned)
                      formIntField "Spent" logEntry.DowntimeSpent (int >> State.EditViewMsg.UpdateDowntimeSpent) ]
                  formSection "Renown" true
                    [ formIntField "Starting" logEntry.RenownStart (int >> State.EditViewMsg.UpdateRenownStart)
                      formIntField "Earned" logEntry.RenownEarned (int >> State.EditViewMsg.UpdateRenownEarned) ]
                  imageAttachment logEntry.Image State.EditViewMsg.UpdateImage
                  formLargeTextField "Notes" "add session notes here..." logEntry.Notes State.EditViewMsg.UpdateNotes ] ] ]
      Card.footer [ ]
        [ Card.Footer.a [ Props [ OnClick save ] ]
            [ Icon.icon [ ] [ Fa.i [ Fa.Regular.Save ] [ ] ]
              str " Save " ]
          Card.Footer.a [ Props [ OnClick cancel ] ]
            [ Icon.icon [ ] [ Fa.i [ Fa.Regular.WindowClose ] [ ] ]
              str " Cancel " ] ] ]

let logsheet dispatch (state:State.T) =
  let modalView content =
    let closeModalView _ = dispatch <| State.Msg.ChangeView State.ChangeViewMsg.List
    Modal.modal [ Modal.IsActive true ]
      [ Modal.background [ Props [ OnClick closeModalView ] ] [ ]
        Modal.content [ ] [ content ] ]
  Container.container [ Container.IsFluid ]
    [ Content.content [ ]
        [ div [ ]
            [ yield match state.ViewState with
                    | State.ViewState.Full logEntry -> modalView <| fullView dispatch logEntry
                    | State.ViewState.Edit logEntry -> modalView <| editView dispatch state logEntry
                    | _ -> div [ ] [ ]
              yield listView dispatch state ] ] ]
