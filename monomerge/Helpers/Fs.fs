module Helpers.Fs
open System
open System.IO


let GetDirectory (subDir : string) (dir : DirectoryInfo) : DirectoryInfo =
    let newPath = Path.Combine(dir.FullName, subDir)
    DirectoryInfo (newPath)

let GetFile (fileName : string) (dir : DirectoryInfo) : FileInfo =
    let fullFileName = Path.Combine(dir.FullName, fileName)
    FileInfo (fullFileName)

let GetFiles (pattern: string) (dir : DirectoryInfo) =
    dir.EnumerateFiles(pattern) |> List.ofSeq

let GetDirs (pattern: string) (dir : DirectoryInfo) =
    dir.EnumerateDirectories(pattern) |> List.ofSeq

let EnsureExists (dir : DirectoryInfo) =
    if dir.Exists |> not then dir.Create()
    dir

let Delete (dir : DirectoryInfo) =
    if dir.Exists then dir.Delete(true)

let Exists (dir : DirectoryInfo) =
    dir.Exists

let CurrentDir () =
    Environment.CurrentDirectory |> DirectoryInfo

let ReadAllText (fileName : FileInfo) =
    fileName.FullName |> File.ReadAllText

let WriteAllLines (lines: string seq) (fileName : FileInfo) =
    File.WriteAllLines(fileName.FullName, lines) 
