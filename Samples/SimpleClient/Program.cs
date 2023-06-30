// See https://aka.ms/new-console-template for more information

using System.Reflection;
using System.Text.Json;
using ActivityPub.Types;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;
using Microsoft.Extensions.DependencyInjection;
using SimpleClient;

var apContentTypes = new HashSet<string>()
{
    "application/ld+json",
    "application/activity+json",
    "application/json"
};

var serviceCollection = new ServiceCollection();
// TODO register services

var serviceProvider = serviceCollection.BuildServiceProvider();
// TODO create services

var httpClient = new HttpClient();
var jsonOptions = new JsonSerializerOptions { WriteIndented = true }.AddJsonLd();
var objects = new Stack<ASType>();


// Process line by line
Console.Write("> ");
while (Console.ReadLine()?.Trim() is {} line)
{
    await ProcessLine(line);

    Console.WriteLine();
    Console.Write("> ");
}

async Task ProcessLine(string line)
{
    try
    {
        // Parse line
        var parts = line.Split(' ', 2);
        var directive = parts[0].ToLower();
        var parameter = parts.Length >= 2 ? parts[1] : null;

        switch (directive)
        {
            case "quit":
            case "exit":
            case "close":
            case "stop":
                Console.WriteLine("Bye 👋");
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
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}

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

async Task HandlePush(string? parameter)
{
    if (string.IsNullOrEmpty(parameter))
    {
        Console.WriteLine("Please specify a URI or property to push");
        return;
    }

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
    var newObj = await GetObject<ASType>(uri);
    objects.Push(newObj);
}

async Task PushSelector(ASType current, string parameter)
{
    var newObject = await SelectObject<ASType>(current, parameter);
    objects.Push(newObject);
}

async Task HandlePrint(string? parameter = null)
{
    if (!objects.TryPeek(out var current))
    {
        Console.WriteLine("Can't print - no object in focus");
        return;
    }

    object? toPrint = current;

    if (parameter != null)
    {
        toPrint = await SelectObject<object?>(current, parameter);
    }

    var json = JsonSerializer.Serialize(toPrint, jsonOptions);
    Console.WriteLine(json);
}

async Task HandlePopulate(string? parameter)
{
    if (!objects.TryPeek(out var current))
    {
        Console.WriteLine("Can't populate - no object in focus.");
        return;
    }

    if (string.IsNullOrEmpty(parameter))
    {
        await PopulateAll(current);
    }
    else
    {
        await PopulateOne(current, parameter);
    }
}

async Task PopulateAll(ASType current)
{
    foreach (var property in current.GetType().GetProperties())
    {
        await PopulateProperty(current, property);
    }
}

async Task PopulateOne(ASType current, string propertyName)
{
    var property = current.GetType().GetProperty(propertyName);
    if (property == null)
    {
        Console.WriteLine("Can't find that property within the object in focus.");
        return;
    }

    await PopulateProperty(current, property);
}

async Task PopulateProperty(ASType obj, PropertyInfo property)
{
    var currentValue = property.GetValue(obj);
    var newValue = await GetPopulatedValue(currentValue);
    property.SetValue(obj, newValue);
}

async Task<T> GetObject<T>(Uri uri)
    where T : ASType
{
    var resp = await httpClient.GetAsync(uri);
    if (!resp.IsSuccessStatusCode)
        throw new ApplicationException($"Request failed: got status {resp.StatusCode}");

    var mediaType = resp.Content.Headers.ContentType?.MediaType;
    if (mediaType == null || !apContentTypes.Contains(mediaType))
        throw new ApplicationException($"Request failed: unsupported content type {mediaType}");

    var json = await resp.Content.ReadAsStringAsync();
    var obj = JsonSerializer.Deserialize<T>(json, jsonOptions);
    return obj ?? throw new ApplicationException("Failed to download object");
}

async Task<T> SelectObject<T>(ASType obj, string propertyName)
{
    var property = obj.GetType().GetProperty(propertyName);
    if (property == null)
        throw new MissingMemberException("Can't find that property within the object in focus.");

    // Get and populate property value
    var value = property.GetValue(obj);
    value = await GetPopulatedValue(value);

    // Verify and return
    if (value is not T typedValue)
        throw new ApplicationException("Selected property is not compatible");
    return typedValue;
}

async Task<object?> GetPopulatedValue(object? value) =>
    value switch
    {
        Linkable<ASObject> { HasLink: true } linkable => await GetObject<ASObject>(linkable.Link),
        LinkableList<ASObject> linkableList => await linkableList
            .ToAsyncEnumerable()
            .SelectAwait(async item =>
            {
                if (item.TryGetLink(out var link))
                    return await GetObject<ASObject>(link);
                return item;
            })
            .ToList(new LinkableList<ASObject>()),
        var _ => value
    };