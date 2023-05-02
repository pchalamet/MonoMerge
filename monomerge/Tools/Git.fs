module Tools.Git
open System.IO
open Helpers.Fs
open Helpers.Exec
open Helpers.Collections
open Helpers.IO


[<RequireQualifiedAccess>]
type private Branch =
    | None
    | Default
    | Named of string


let Clone (uri : string) (targetDir : DirectoryInfo) (shallow : bool) (branch : string option) =
    let gitBranch = match branch with
                    | Some name -> let chkBranchArgs = sprintf "ls-remote --exit-code --heads %s %s" uri name
                                   match ExecGetOutput "git" chkBranchArgs targetDir Map.empty |> IsError with
                                   | true -> Branch.Default
                                   | false -> Branch.Named name
                    | None -> Branch.None

    let br = match gitBranch with
             | Branch.None -> "--no-single-branch"
             | Branch.Default -> "--single-branch"
             | Branch.Named name -> sprintf "--branch %s --single-branch" name
    let depth = shallow ? ("--depth=1", "")
    let args = sprintf @"clone --recurse-submodules %s %s %s %A" uri br depth targetDir.FullName
    Exec "git" args targetDir Map.empty

let Init (wsDir: DirectoryInfo) =
    let args = sprintf "init %A" wsDir.FullName
    Exec "git" args wsDir Map.empty

let Checkout (branch: string) (wsDir: DirectoryInfo) =
    let chkVersionArgs = $"checkout {branch}" 
    Exec "git" chkVersionArgs wsDir Map.empty

let Branch (branch: string) (from: string) (wsDir: DirectoryInfo) =
    let chkVersionArgs = $"checkout -b {branch} {from}" 
    Exec "git" chkVersionArgs wsDir Map.empty

let AddSubmodule (name: string) (uri: string) (wsDir: DirectoryInfo) =
    let args = $"submodule add {uri} {name}"
    Exec "git" args wsDir Map.empty

let AddRemote (name: string) (uri: string) (wsDir: DirectoryInfo) =
    let args = $"remote add --fetch {name} {uri}"
    Exec "git" args wsDir Map.empty

let RemoveRemote (name: string) (wsDir: DirectoryInfo) =
    let args = $"git remote rm {name}"
    Exec "git" args wsDir Map.empty

let StartMerge (branch: string) (wsDir: DirectoryInfo) =
    let args = $"git merge -s ours --no-commit {branch}"
    Exec "git" args wsDir Map.empty

let Move (source: string) (target: string) (wsDir: DirectoryInfo) =
    let args = $"mv -k {source} {target}"
    Exec "git" args wsDir Map.empty

let Add (file: string) (wsDir: DirectoryInfo) =
    let args = $"add {file}"
    Exec "git" args wsDir Map.empty

let Commit (comment: string) (wsDir: DirectoryInfo) =
    let args = $"commit -am '{comment}'"
    Exec "git" args wsDir Map.empty

let Merge (branch: string) (comment: string) (wsDir: DirectoryInfo) =
    let args = $"merge {branch} -m '{comment}' --allow-unrelated-histories"
    Exec "git" args wsDir Map.empty

let ImportWithHistory (name: string) (uri: string) (branch: string) (wsDir: DirectoryInfo) =
    let exec cmd args = Exec cmd args wsDir Map.empty |> CheckResponseCode
    let ignoreExec cmd args = Exec cmd args wsDir Map.empty |> ignore

    let monomergesh = Helpers.Env.SbsDir() |> GetFile "monomerge.sh"

    // ignoreExec "git" $"remote rm {name}"
    // ignoreExec "git" $"branch -D monomerge/{name}"
    // exec "git" $"remote add --fetch {name} {uri}"
    // exec "git" $"checkout -b monomerge/{name} {name}/{branch}"
    exec monomergesh.FullName $"{name} {uri} {branch}"
    // exec "git" $"checkout main"
    // exec "git" $"merge --allow-unrelated-histories -m \"monomerge {name}\" monomerge/{name}"
    // exec "git" $"remote rm {name}"
    // exec "git" $"branch -D monomerge/{name}"
