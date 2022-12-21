module CLI.Commands

type InitWorkspace =
    { Path : string 
      ConfigFile : string
      Continue: bool }

[<RequireQualifiedAccess>]
type MainCommand =
    | Usage
    | Init
    | Unknown

[<RequireQualifiedAccess>]
type Command =
    | Version
    | Usage
    | Init of InitWorkspace
    | Error of MainCommand
