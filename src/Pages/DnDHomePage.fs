[<RequireQualifiedAccess>]
module VPur.UI.Pages.DnDHomePage

open Fable.Helpers.React
open Fulma
open VPur.UI.Components
open VPur.UI.Components.Tiles
open VPur.UI.Routing

let tile =
  let tileContent =
    div [ ]
      [ str "I like playing dungeons and dragons, so why not build some tools for it?"
        br [ ]
        str "Even though there are plenty of resources for this already ¯\\_(ツ)_/¯"
        br [ ]
        str "So come check out what I've created so far"
        br [ ]
        Button.a [ Button.Props [ Router.href (DnD.Index |> Page.DnD) ]
                   Button.Color IsInfo ]
                 [ str "Check it out!" ] ]
  { Title = "D&D related things"
    Content = tileContent }

let helloMsgTile =
  { Title = "D&D"
    Content = str "I like to play DnD and there are loads of tools to help with it, but I want to create my own also :D" }

let view =
  Tiles.view
  <| { Tiles = [ helloMsgTile
                 AdventurersLeagueLogPage.tile ] }
