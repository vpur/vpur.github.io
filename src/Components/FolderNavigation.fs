[<RequireQualifiedAccess>]
module VPur.UI.Components.FolderNavigation

open Fulma
open Fable.FontAwesome
open Fable.Helpers.React

open VPur.UI.Routing

let view parentPage pathParts =
  pathParts
  |> List.mapi (fun index (page, path) ->
      Breadcrumb.item [ Breadcrumb.Item.IsActive (index + 1 = List.length pathParts) ]
                      [ a [ Router.href page ] [ str path ] ])
  |> fun path ->
      Section.section [ ]
        [ Container.container [ Container.IsFluid ]
            [ Breadcrumb.breadcrumb [ Breadcrumb.Size IsLarge ] path ] ]
