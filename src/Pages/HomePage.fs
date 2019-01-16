[<RequireQualifiedAccess>]
module VPur.UI.Pages.HomePage

open Fable.Helpers.React
open VPur.UI.Components
open VPur.UI.Components.Tiles

let private helloMsgTile =
  { Title = "Hello"
    Content = str "Hi, I'm playing with some UI development and this is is a result of such \"coding for fun\"" }

let view =
  Tiles.view
  <| { Tiles = [ helloMsgTile
                 DnDHomePage.tile ] }
