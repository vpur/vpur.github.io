[<RequireQualifiedAccess>]
module VPur.UI.Components.Navigation

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.FontAwesome
open Elmish
open Fulma

open VPur.UI.Routing
open VPur.UI.Components

type State =
  { IsBurgerOpen: bool
    Auth: Auth.State }
  static member Empty =
    { IsBurgerOpen = false
      Auth         = Auth.State.Empty }

type Msg =
  | ToggleBurger
  | AuthMsg of Auth.Msg

let update msg state =
  match (msg, state) with
  | (ToggleBurger, _) ->
      { state with IsBurgerOpen = not state.IsBurgerOpen }, Cmd.none

  | (AuthMsg msg, _) ->
      let (authState, authCmd) = Auth.update msg state.Auth
      Logger.debugfn "updated auth state: %A" authState
      { state with Auth = authState }, Cmd.map AuthMsg authCmd

let view state dispatch =
  let navbarEnd =
      Navbar.End.div [ ]
          [ Navbar.Item.div [ ]
              [ Auth.view (AuthMsg >> dispatch) state.Auth ]
            Navbar.Item.div [ ]
              [ Field.div [ Field.IsGrouped ]
                  [ Control.p [ ]
                      [ Button.a [ Button.Props [ Href Constants.GithubPage ] ]
                          [ Icon.icon [ ] [ Fa.i [ Fa.Brand.Github ] [ ] ]
                            span [ ] [ str "Source" ] ] ] ] ] ]

  let navbarStart dispatch =
    let games =
      let dndOnClick _ = DnD.Index |> Page.DnD |> Router.modifyLocation

      Navbar.Item.div [ Navbar.Item.HasDropdown; Navbar.Item.IsHoverable ] [
        Navbar.Link.div [ ] [ str "Games" ]
        Navbar.Dropdown.div [ ] [
          Navbar.Item.a [ Navbar.Item.Props [ OnClick dndOnClick] ] [ str "DnD" ]
        ] ]

    Navbar.Start.div [ ] [ games ]

  let sourceCodeNavItem =
    Navbar.Item.a [ Navbar.Item.Props [ Href Constants.GithubPage ]
                    Navbar.Item.CustomClass "is-hidden-desktop" ]
                  [ Icon.icon [ ] [ Fa.i [ Fa.Brand.Github; Fa.Size Fa.FaLarge ] [ ] ] ]

  div [ ClassName "navbar-bg" ]
      [ Container.container [ ]
          [ Navbar.navbar [ Navbar.CustomClass "is-primary" ]
              [ Navbar.Brand.div [ ]
                  [ Navbar.Item.a [ Navbar.Item.Props [ Href "#" ] ]
                      [ Heading.p [ Heading.Is4 ]
                          [ str "VPur" ] ]
                    // Icon display only on mobile
                    sourceCodeNavItem
                    // Make sure to have the navbar burger as the last child of the brand
                    Navbar.burger [ Fulma.Common.CustomClass (if state.IsBurgerOpen then "is-active" else "")
                                    Fulma.Common.Props [ OnClick (fun _ -> dispatch ToggleBurger) ] ]
                      [ span [ ] [ ]
                        span [ ] [ ]
                        span [ ] [ ] ] ]
                Navbar.menu [ Navbar.Menu.IsActive state.IsBurgerOpen ]
                  [ navbarStart dispatch
                    navbarEnd ] ] ] ]
