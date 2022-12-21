module Commands.Workspace
open System.IO
open Helpers.Fs
open System.Linq
open Helpers.IO

let Init (cmd : CLI.Commands.InitWorkspace) =
    let wsDir = cmd.Path |> DirectoryInfo
    if cmd.Continue |> not then
        let isWorkspaceNotEmpty = wsDir.Exists 
                                && (wsDir.EnumerateFiles().Count() > 0 || wsDir.EnumerateDirectories().Count() > 0)
        if isWorkspaceNotEmpty then failwithf "Workspace already exists"
        wsDir |> EnsureExists |> ignore

        // initialize repository
        Tools.Git.Init wsDir |> CheckResponseCode
        wsDir |> GetFile "README.md" |> WriteAllLines [ "# MonoMerge migration" ]
        Tools.Git.Add "." wsDir |> CheckResponseCode
        Tools.Git.Commit "migration" wsDir |> CheckResponseCode

    let currentDir = System.Environment.CurrentDirectory
    try
        System.Environment.CurrentDirectory <- wsDir.FullName

        let master = Configuration.Master.Load (cmd.ConfigFile |> FileInfo)
        for repository in master.Repositories do
            let targetFolder = wsDir |> GetDirectory repository.Name
            if targetFolder.Exists |> not then
                wsDir |> Tools.Git.ImportWithHistory repository.Name repository.Uri repository.Branch
    finally
        System.Environment.CurrentDirectory <- currentDir
