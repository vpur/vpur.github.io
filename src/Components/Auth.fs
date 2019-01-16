module VPur.UI.Components.Auth

open Elmish
open Fulma
open Fable.FontAwesome
open Fable.Helpers.React
open Fable.Helpers.React.Props

type User =
  { Name: string
    Id:   string }

type State =
  { User: User option
    LoginForm: LoginForm.State }
  static member Empty =
    { User = None
      LoginForm = LoginForm.State.Empty }

type Msg =
  | Logout
  | LoginFormMsg of LoginForm.Msg

let update (msg:Msg) state : State*Cmd<Msg> =
  match msg with
  | Logout ->
      State.Empty, Cmd.none

  | LoginFormMsg(LoginForm.Msg.LoginSuccess(userId, username)) ->
      { state with User = Some { Name = username; Id = userId } }, Cmd.none

  | LoginFormMsg msg ->
      let (subState, subCmd) = LoginForm.update state.LoginForm msg
      { state with LoginForm = subState }, Cmd.map LoginFormMsg subCmd

let view dispatch state =
  match state.User with
  | Some user ->
      Field.div [ Field.IsGrouped ]
                [ Control.div [ ] [ str <| sprintf "Welcome, %s" user.Name ]
                  Control.p [ ]
                            [ Button.a [ Button.Props [ OnClick <| fun _ -> dispatch Msg.Logout ] ]
                                       [ Icon.icon [ ] [ Fa.i [ Fa.Regular.Surprise ] [ ] ]
                                         span [ ] [ str "Logout" ] ] ] ]

  | None ->
      LoginForm.view (LoginFormMsg >> dispatch) state.LoginForm

let authRequired view = function
  | { User = Some _user } -> view
  | _                     -> Render.pageNotFound
