#r "paket: groupref netcorebuild //"
#load ".fake/build.fsx/intellisense.fsx"
#if !FAKE
#r "Facades/netstandard"
#r "netstandard"
#endif

#nowarn "52"

open System
open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.IO.FileSystemOperators
open Fake.Tools.Git
open Fake.JavaScript

Target.create "Clean" (fun _ ->
    !! "src/bin"
    ++ "src/obj"
    ++ "output"
    |> Seq.iter Shell.cleanDir
)

Target.create "DotnetRestore" (fun _ ->
    DotNet.restore
        (DotNet.Options.withWorkingDirectory __SOURCE_DIRECTORY__)
        "vpur.sln"
)

Target.create "YarnInstall" (fun _ -> Yarn.install id)

Target.create "Build" (fun _ -> Yarn.exec "webpack" id)

Target.create "Watch" (fun _ -> Yarn.exec "webpack-dev-server" id)

// Output locations
let rootDir   = __SOURCE_DIRECTORY__
let temp      = rootDir </> "temp"
let docsOuput = rootDir </> "output"

// --------------------------------------------------------------------------------------
// Release Scripts
Target.create "Publish" (fun _ ->
    Shell.cleanDir temp
    Repository.cloneSingleBranch "" "git@github.com:vpur/vpur.github.io.git" "master" temp

    Shell.copyRecursive docsOuput temp true |> Trace.logfn "%A"
    Staging.stageAll temp
    Commit.exec temp (sprintf "Update site (%s)" (DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
    Branches.push temp
)

// Build order
"Clean"
    ==> "DotnetRestore"
    ==> "YarnInstall"
    ==> "Build"

"YarnInstall"
    ==> "Watch"

"Build"
    ==> "Publish"

// start build
Target.runOrDefault "Build"
