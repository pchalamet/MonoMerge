﻿
module CLI.CommandLine
open Commands
open System.IO

type private TokenOption =
    | Continue

let private (|TokenOption|_|) token =
    match token with
    | "--continue" -> Some TokenOption.Continue
    | _ -> None

type private Token =
    | Version
    | Usage
    | Init
    | Convert

let private (|Token|_|) token =
    match token with
    | "version" -> Some Token.Version
    | "usage" -> Some Token.Usage
    | "init" -> Some Token.Init
    | "convert" -> Some Token.Convert
    | _ -> None

let private (|Param|_|) (prm : string) =
    if prm.StartsWith("--") then None
    else Some prm

let private (|Params|_|) prms =
    let hasNotParam = prms |> List.exists (fun x -> match x with
                                                    | Param _ -> false
                                                    | _ -> true)
    if hasNotParam || prms = List.empty then None
    else Some prms

let private commandUsage args =
    match args with
    | _ -> Command.Usage

let rec private commandInit args =
    match args with
    | [Param path; Param url]
        -> Command.Init { Path = path; Url = url }
    | _ -> Command.Error MainCommand.Init

let rec private commandConvert args =
    match args with
    | [Param path]
        -> Command.Convert { Path = path }
    | _ -> Command.Error MainCommand.Init


let Parse (args : string list) : Command =
    match args with
    | [Token Version] -> Command.Version
    | Token Usage :: cmdArgs -> cmdArgs |> commandUsage
    | Token Init :: cmdArgs -> cmdArgs |> commandInit
    | Token Convert :: cmdArgs -> cmdArgs |> commandConvert
    | _ -> Command.Error MainCommand.Usage

let IsVerbose (args : string list) : (bool * string list) =
    if (args <> List.empty && args |> List.head = "--verbose") then
        let newArgs = args.Tail
        (true, newArgs)
    else
        (false, args)
