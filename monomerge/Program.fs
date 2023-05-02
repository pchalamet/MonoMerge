
module Main
open CLI.Commands

let processMain argv =
    let cmd = CLI.CommandLine.Parse (argv |> Seq.toList)
    match cmd with
    | Command.Init info -> Commands.Workspace.Init info; 0
    | Command.Convert info -> Commands.Workspace.Convert info; 0
    | Command.Usage -> Commands.Help.Usage MainCommand.Unknown; 0
    | Command.Error info -> Commands.Help.Usage info; 5
    | Command.Version -> Commands.Help.Version (); 0


let tryMain verbose argv =
    try
        processMain argv
    with
        x -> let err = if verbose |> not then sprintf "Error:\n%s" x.Message
                       else let sep = "---------------------------------------------------"
                            sprintf "%s\n%A\n%s" sep x sep
               
             Helpers.Console.PrintError err
             5

[<EntryPoint>]
let main argv =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let verbose, args = CLI.CommandLine.IsVerbose (argv |> List.ofArray)
    let retCode = tryMain verbose args
    stopWatch.Stop()
    let elapsed = stopWatch.Elapsed
    if verbose then printfn "Completed in %d seconds." ((int)elapsed.TotalSeconds)
    retCode
