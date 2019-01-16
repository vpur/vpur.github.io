module VPur.UI.Routing

open Fable.Import
open Fable.Helpers.React.Props
open Elmish.Browser.Navigation
open Elmish.Browser.UrlParser

[<RequireQualifiedAccess>]
type DnD =
  | Index
  | AdventurersLeagueLog

[<RequireQualifiedAccess>]
type Page =
  | Home
  | DnD of DnD

type UrlParser = Parser<Page->Page,Page>

module DnD =
  let private indexUrl = ""
  let private adventurersLeagueLogUrl = "adventurersleaguelog"

  let toPath = function
    | DnD.Index                -> indexUrl
    | DnD.AdventurersLeagueLog -> adventurersLeagueLogUrl

  let urlParser =
    oneOf
      [ map DnD.AdventurersLeagueLog (s adventurersLeagueLogUrl)
        map DnD.Index                 top ]

module Page =
  let private homeUrl = ""
  let private dndUrl = "dnd"

  let toPath = function
    | Page.Home     -> homeUrl
    | Page.DnD page -> sprintf "%s/%s" dndUrl (DnD.toPath page)

  let urlParser : UrlParser =
    oneOf
      [ map Page.DnD   (s dndUrl </> DnD.urlParser)
        map Page.Home   top ]

[<RequireQualifiedAccess>]
module Router =
  let routes: Parser<Page->Page,Page> = Page.urlParser

  let private toPath = Page.toPath >> sprintf "#%s"

  let href = toPath >> Href

  let modifyUrl route = route |> toPath |> Navigation.modifyUrl

  let newUrl route = route |> toPath |> Navigation.newUrl

  let modifyLocation route =
    Browser.window.location.href <- toPath route
