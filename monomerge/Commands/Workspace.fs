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

        // first clone master repository
        let masterUri = cmd.Uri
        let masterRepo = { Configuration.Master.Repository.Name = ""
                           Configuration.Master.Repository.Uri = masterUri
                           Configuration.Master.Repository.Branch = "main" }
        Tools.Git.Clone masterRepo wsDir false None |> CheckResponseCode

    wsDir |> Tools.Git.Checkout "main" |> CheckResponseCode

    let currentDir = System.Environment.CurrentDirectory
    try
        System.Environment.CurrentDirectory <- wsDir.FullName

        let master = Configuration.Master.Load wsDir
        for repository in master.Repositories do
            let targetFolder = wsDir |> GetDirectory repository.Name
            if targetFolder.Exists |> not then
                wsDir |> Tools.Git.ImportWithHistory repository.Name repository.Uri repository.Branch
    finally
        System.Environment.CurrentDirectory <- currentDir
