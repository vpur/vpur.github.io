module App

open Elmish
open Elmish.React
open Elmish.Debug
open Elmish.Browser.Navigation
open Elmish.Browser.UrlParser
open Elmish.HMR

open VPur.UI
open VPur.UI.Routing

Program.mkProgram Layout.init Layout.update Layout.view
|> Program.toNavigable (parseHash Router.routes) Layout.urlUpdate
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
