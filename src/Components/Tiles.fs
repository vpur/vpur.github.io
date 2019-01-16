module VPur.UI.Components.Tiles

open Fable.Helpers.React
open Fulma
open Fable.Import.React

type Tile =
  { Title: string
    Content: ReactElement }

type Model =
  { Tiles: Tile list }

let view (model:Model) =
  let tiles =
    model.Tiles
    |> List.map(fun tile ->
        Tile.child [ ]
          [ Box.box' [ ]
              [ Heading.p [ ] [ str tile.Title ]
                p [ ] [ tile.Content ] ] ])
    |> List.mapi(fun id tile -> id, tile)
    |> List.groupBy(fun (id,_) -> id % 2 = 1)
    |> List.map snd
    |> List.map(fun chunck ->
        let tiles = chunck |> List.map snd
        Tile.parent [ Tile.Size Tile.Is6; Tile.IsVertical ] tiles )

  Section.section [ ]
    [ Container.container [ Container.IsFluid ] [ Tile.ancestor [ ] tiles ] ]
