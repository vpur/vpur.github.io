[<RequireQualifiedAccess>]
module VPur.UI.Pages.DnDPage

open Elmish
open Fable.Helpers.React

open VPur.UI.Routing
open VPur.UI.Components

type State =
  { AdventurersLeagueLogPageState: AdventurersLeagueLogPage.State }
  with static member Empty =
         { AdventurersLeagueLogPageState = AdventurersLeagueLogPage.init () }

type Msg =
  | AdventurersLeagueLogPageMsg of DnD.AdventurersLeague.Logsheet.State.Msg

let update state = function
  | AdventurersLeagueLogPageMsg msg ->
      let (subState, subCmd) = AdventurersLeagueLogPage.update state.AdventurersLeagueLogPageState msg
      { state with AdventurersLeagueLogPageState = subState }, Cmd.map AdventurersLeagueLogPageMsg subCmd

let view page state dispatch =
  let parentPage =
    match page with
    | DnD.Index -> Page.Home
    | _         -> DnD.Index |> Page.DnD
  let pageView =
    match page with
    | DnD.Index                -> DnDHomePage.view
    | DnD.AdventurersLeagueLog -> AdventurersLeagueLogPage.view (AdventurersLeagueLogPageMsg >> dispatch) state.AdventurersLeagueLogPageState
  div [ ]
    [ FolderNavigation.view parentPage
        [ Page.Home, "home"
          DnD.Index |> Page.DnD, "dnd"
          page |> Page.DnD, page |> DnD.toPath ]
      section [ ] [ pageView ] ]
