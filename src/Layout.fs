module VPur.UI.Layout

open Elmish
open Fable.Helpers.React
open Fulma
open Fable.Import
open Fable.Helpers.React.Props

open VPur.UI.Components
open VPur.UI.Routing
open VPur.UI.Pages

type State =
  { Navigation:  Navigation.State
    CurrentPage: Page
    DnDPageState: DnDPage.State }
  static member Empty =
    { CurrentPage = Page.Home
      Navigation = Navigation.State.Empty
      DnDPageState = DnDPage.State.Empty }

type Msg =
  | Navigation of Navigation.Msg
  | DnDMessage of DnDPage.Msg

let urlUpdate (result: Option<Page>) state : State * Cmd<Msg> =
  match result with
  | None ->
      Browser.console.error("Error parsing url: " + Browser.window.location.href)
      state, Router.modifyUrl state.CurrentPage
  | Some page -> { state with CurrentPage = page }, Cmd.none

let init result : State * Cmd<Msg> =
  urlUpdate result State.Empty

let update msg state =
    match (msg, state) with
    | (Navigation navMsg, _) ->
        let navState, navCmd = Navigation.update navMsg state.Navigation
        Logger.debugfn "updated nav state: %A" navState
        { state with Navigation = navState }, Cmd.map Msg.Navigation navCmd
    | (DnDMessage dndMsg, _) ->
        let dndState, dndCmd = DnDPage.update state.DnDPageState dndMsg
        { state with DnDPageState = dndState }, Cmd.map DnDMessage dndCmd

let private renderPage model dispatch =
    match model with
    | { CurrentPage = Page.Home } -> HomePage.view
    | { CurrentPage = Page.DnD page } -> DnDPage.view page model.DnDPageState (DnDMessage >> dispatch)

let view model dispatch =
    div [ ]
        [ Navigation.view model.Navigation (Navigation >> dispatch)
          Section.section [ ]
            [ Container.container [ Container.IsFluid ]
                [ renderPage model dispatch ] ]
          Footer.footer [ ]
            [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                [ Columns.columns [ Columns.IsCentered ]
                    [
                      Column.column [ Column.Width (Screen.All, Column.Is2) ]
                        [ str "Twitter: "
                          a [ Href "https://twitter.com/VasylPurchel" ] [ str "@VasylPurchel" ]
                          br [ ]
                          str "Github: "
                          a [ Href "https://github.com/vasyl-purchel" ] [ str "vasyl-purchel" ] ]
                      Column.column [ Column.Width (Screen.All, Column.Is2) ]
                        [ str "Webpage built for fun with a help of"
                          a [ Href "https://fable.io/" ] [ str " Fable " ]
                          str "and"
                          a [ Href "https://mangelmaxime.github.io/Fulma/"] [ str " Fulma " ] ]
                      Column.column [ Column.Width (Screen.All, Column.Is2) ]
                        [ Image.image [ ] [ img [ Src Constants.BuildStatusBadge ] ] ]
                    ] ] ] ]
