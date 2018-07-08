//
// Goal: Run a Powershell script as embedded script,
// capture and interogate resulting objects written
// to host.
// 

#nowarn

open System
open System.IO
open System.Management.Automation
open System.Collections.ObjectModel
open CommandLine


type Options = {
    [<Value(0, MetaName="file name", HelpText="Script file name", Required=true)>]
    FileName : string
}

let runScript (script : string) =
    use instance = PowerShell.Create()
    instance.AddScript script |> ignore
    instance.Invoke()


let run (opts : Options) = 
    let script = File.ReadAllText opts.FileName
    let coll = runScript script
    printfn "%A" coll
    ()


[<EntryPoint>]
let main argv =
    let parseResult = 
        CommandLine.Parser.Default.ParseArguments<Options>(argv)

    match parseResult with
    | :? Parsed<Options> as parsed -> run parsed.Value
    | :? NotParsed<Options> as notParsed -> 
        Console.Error.WriteLine("Failed:" + notParsed.ToString())

    Console.WriteLine("Press any key")
    Console.ReadKey() |> ignore
    0 // return an integer exit code
