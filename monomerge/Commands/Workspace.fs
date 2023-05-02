module Commands.Workspace
open System.IO
open Helpers.Fs
open System.Linq
open Helpers.IO

let Init (cmd : CLI.Commands.InitWorkspace) =
    let wsDir = cmd.Path |> DirectoryInfo
    let isWorkspaceNotEmpty = wsDir.Exists 
                            && (wsDir.EnumerateFiles().Count() > 0 || wsDir.EnumerateDirectories().Count() > 0)
    if isWorkspaceNotEmpty then failwithf "Workspace already exists"
    wsDir |> EnsureExists |> ignore

    Tools.Git.Clone cmd.Url wsDir false None |> CheckResponseCode

let Convert (cmd : CLI.Commands.ConvertWorkspace) =
    let wsDir = cmd.Path |> DirectoryInfo
    let currentDir = System.Environment.CurrentDirectory
    try
        System.Environment.CurrentDirectory <- wsDir.FullName

        let master = Configuration.Master.Load ("sbs.yaml" |> FileInfo)
        for repository in master.Repositories do
            let targetFolder = wsDir |> GetDirectory repository.Name
            if targetFolder.Exists |> not then
                wsDir |> Tools.Git.ImportWithHistory repository.Name repository.Uri repository.Branch
    finally
        System.Environment.CurrentDirectory <- currentDir
