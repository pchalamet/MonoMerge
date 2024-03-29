﻿module Configuration.Master
open Helpers.Collections
open Helpers.Fs
open System.IO
open YamlDotNet.Serialization


[<CLIMutable>]
[<RequireQualifiedAccess>]
type Repository =
    { [<YamlMember(Alias="name")>] Name : string
      [<YamlMember(Alias="uri")>] Uri : string
      [<YamlMember(Alias="branch")>] Branch : string }

[<CLIMutable>]
[<RequireQualifiedAccess>]
type Configuration =
    {
        [<YamlMember(Alias="repositories")>] Repositories : Repository array 
    }
with
    member this.GetRepository name =
        this.Repositories |> Seq.tryFind (fun x -> x.Name = name)


let Load (masterConfigFile : FileInfo) =
    let yaml = System.IO.File.ReadAllText(masterConfigFile.FullName)
    let deserializer = DeserializerBuilder().Build()
    let config = deserializer.Deserialize<Configuration>(yaml)
    config
