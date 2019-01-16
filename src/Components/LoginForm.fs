module VPur.UI.Components.LoginForm

open System
open Elmish
open Fable.PowerPack
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.FontAwesome
open Fulma
open Fable.Core.JsInterop

type State =
  { FormVisible: bool
    Username:    string option
    Password:    string option
    Busy:        bool
    ErrorMsg:    string option }
  static member Empty =
    { FormVisible = false
      Username    = None
      Password    = None
      Busy        = false
      ErrorMsg    = None }

type Msg =
  | UsernameChanged of string
  | PasswordChanged of string
  | LoginRequested
  | LoginSuccess of string * string
  | LoginFailed of Exception
  | FormOpen
  | FormClose

let authUser state = promise {
    if Option.isNone state.Username then return! failwithf "You need to fill in a username." else
    if Option.isNone state.Password then return! failwithf "You need to fill in a password." else

    if state.Username = Some "test" && state.Password = Some "test"
    then
      do! Promise.sleep 500
      return "1", "test"
    else
      do! Promise.sleep 1000
      return! failwithf "Could not authenticate user."
}

let update state = function
  | UsernameChanged username ->
      let newUsername = if String.IsNullOrWhiteSpace username then None else Some username
      { state with Username = newUsername }, Cmd.none

  | PasswordChanged password ->
      let newPassword = if String.IsNullOrWhiteSpace password then None else Some password
      { state with Password = newPassword }, Cmd.none

  | LoginRequested ->
      { state with Busy = true }, Cmd.ofPromise authUser state LoginSuccess LoginFailed

  | LoginSuccess _ ->
      state, Cmd.none // do nothing as it is needed for the higher lvl component

  | LoginFailed err ->
      { state with ErrorMsg = Some err.Message }, Cmd.none

  | FormOpen ->
      Logger.debug "requesting to open login form"
      { state with FormVisible = true }, Cmd.none

  | FormClose ->
      State.Empty, Cmd.none

let view dispatch state =
  let loginForm =
    let errorMsg = function
      | Some msg -> Help.help [ Help.Color IsDanger ] [ str msg ]
      | None     -> div [ ] [ ]

    form [ ]
      [ // Username field
        Field.div [ ]
          [ Label.label [ ] [ str "Username" ]
            Control.div [ Control.HasIconLeft; Control.HasIconRight ]
              [ Input.text [ Input.Color IsSuccess
                             Input.Placeholder "test"
                             Input.Value (match state.Username with Some name -> name | None -> "")
                             Input.OnChange <| fun ev -> !!ev.target?value |> UsernameChanged |> dispatch ]
                Icon.icon [ Icon.Size IsSmall; Icon.IsLeft ] [ Fa.i [ Fa.Regular.User ] [ ] ]
                Icon.icon [ Icon.Size IsSmall; Icon.IsRight ]
                  [ Fa.i [ Fa.Regular.CheckCircle ] [ ] ] ] ]
        // Password field
        Field.div [ ]
          [ Label.label [ ] [ str "Password" ]
            Control.div [ Control.HasIconLeft; Control.HasIconRight ]
              [ Input.text [ Input.Color IsSuccess
                             Input.Placeholder "test"
                             Input.Type Input.Password
                             Input.Value (match state.Password with Some pwd -> pwd | None -> "")
                             Input.OnChange <| fun ev -> !!ev.target?value |> PasswordChanged |> dispatch ]
                Icon.icon [ Icon.Size IsSmall; Icon.IsLeft ] [ Fa.i [ Fa.Regular.KissBeam ] [ ] ]
                Icon.icon [ Icon.Size IsSmall; Icon.IsRight ]
                  [ Fa.i [ Fa.Regular.CheckCircle ] [ ] ] ] ]
        // Error message if error exists
        errorMsg state.ErrorMsg ]

  div [ ]
      [ Modal.modal [ Modal.IsActive state.FormVisible ]
          [ Modal.background [ Props [ OnClick <| fun _ -> dispatch Msg.FormClose ] ] [ ]
            Modal.Card.card [ ]
              [ Modal.Card.head [ ]
                  [ Modal.Card.title [ ] [ str "Login" ] ]
                Modal.Card.body [ ] [ loginForm ]
                Modal.Card.foot [ ]
                  [ Button.button [ Button.Props [ OnClick <| fun _ -> dispatch Msg.LoginRequested ] ]
                      [ str "Login" ] ] ] ]
        Control.p [ ]
          [ Button.a [ Button.Props [ OnClick <| fun _ -> dispatch Msg.FormOpen
                                      Disabled state.Busy ] ]
              [ Icon.icon [ ] [ Fa.i [ Fa.Regular.Surprise ] [ ] ]
                span [ ] [ str "Login" ] ] ] ]
