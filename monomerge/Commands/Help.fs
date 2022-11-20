module Commands.Help
open CLI.Commands


let private usageContent() =
    let content = [
        [MainCommand.Usage], "usage : display help on command or area"
        [MainCommand.Init], "init <folder> <master-repository-uri>: initialize workspace"
    ]
    content


let Usage (what : MainCommand) =
    let lines = usageContent () |> List.filter (fun (cmd, _) -> cmd |> Seq.contains what || what = MainCommand.Unknown)
                                |> List.map (fun (_, desc) -> desc)
    printfn "Usage:"
    lines |> Seq.iter (printfn "  %s")


let private versionContent() =
    let version = Helpers.Env.Version()
    let versionContent = sprintf "sbs %s" (version.ToString())
    [ versionContent ]


let Version () =
    versionContent() |> List.iter (fun x -> printfn "%s" x)
