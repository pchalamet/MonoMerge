module CLI.Commands

type InitWorkspace =
    { Path : string 
      Url : string }

type ConvertWorkspace =
    { Path : string }

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
    | Convert of ConvertWorkspace
    | Error of MainCommand
