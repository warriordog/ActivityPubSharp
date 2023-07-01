// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using ActivityPub.Types;
using ActivityPub.Types.Json;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
// TODO register services

var serviceProvider = serviceCollection.BuildServiceProvider();
// TODO create services

var jsonOptions = new JsonSerializerOptions { WriteIndented = true }.AddJsonLd();
var objects = new Stack<ASType>();

async Task HandlePop()
{
    if (!objects.Any())
    {
        Console.WriteLine("Can't go back - no objects are in focus");
        return;
    }

    objects.Pop();
    await HandlePrint();
}

async Task HandlePush(string parameter)
{
    // If param is a URI, then its a new object
    if (Uri.TryCreate(parameter, UriKind.RelativeOrAbsolute, out var uri))
    {
        await PushUri(uri);
        await HandlePrint();
    }

    // Otherwise, its a selector
    else if (objects.TryPeek(out var current))
    {
        await PushSelector(current, parameter);
        await HandlePrint();
    }

    // Fallback
    else
    {
        Console.WriteLine("Can't open - no object is in focus and input is not a URI");
    }
}

async Task PushUri(Uri uri)
{
    Console.WriteLine("Not implemented.");
}

async Task PushSelector(ASType current, string parameter)
{
    Console.WriteLine("Not implemented.");
}

async Task HandlePrint(string? parameter = null)
{
    if (objects.TryPeek(out var current))
    {
        Console.WriteLine("Can't print - no object in focus");
        return;
    }

    // if (parameter != null && !TrySelect(ref current, parameter))
    // {
    //     Console.WriteLine($"Bad selector - can't find '{parameter}' in current object.");
    //     return;
    // }

    var json = JsonSerializer.Serialize(current, JsonLdSerializerOptions.Default);
    Console.WriteLine(json);
}

async Task HandlePopulate(string parameter)
{
    Console.WriteLine("Not implemented.");
}


// Process line by line
while (Console.ReadLine()?.Trim() is {} line)
{
    Console.Write("> ");

    // Parse line
    var parts = line.Split(' ', 2);
    var directive = parts[0].ToLower();
    var parameter = parts[1];

    switch (directive)
    {
        case "quit":
        case "exit":
        case "close":
        case "stop":
            Environment.Exit(0);
            break;

        case "help":
            Console.WriteLine("Not implemented.");
            break;

        case "back":
        case "pop":
        case "out":
            await HandlePop();
            break;

        case "open":
        case "push":
        case "in":
            await HandlePush(parameter);
            break;

        case "show":
        case "dump":
        case "print":
            await HandlePrint(parameter);
            break;

        case "expand":
        case "populate":
        case "fill":
            await HandlePopulate(parameter);
            break;

        default:
            Console.WriteLine($"Unknown directive '{directive}'");
            break;
    }

    Console.WriteLine();
}