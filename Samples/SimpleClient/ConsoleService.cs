// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;
using ActivityPub.Client;
using ActivityPub.Types;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;
using Microsoft.Extensions.Hosting;

namespace SimpleClient;

public class ConsoleService : BackgroundService
{
    private readonly Stack<ASType> _focus = new();

    private readonly IActivityPubClient _apClient;
    private readonly IHostApplicationLifetime _hostLifetime;
    private readonly IJsonLdSerializer _jsonLdSerializer;

    private bool _isRunning = true;

    public ConsoleService(IActivityPubClient apClient, IHostApplicationLifetime hostLifetime, IJsonLdSerializer jsonLdSerializer)
    {
        _apClient = apClient;
        _hostLifetime = hostLifetime;
        _jsonLdSerializer = jsonLdSerializer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // This has to be here
        // https://github.com/dotnet/runtime/issues/36063
        await Task.Yield();

        try
        {
            // Process line by line
            while (_isRunning && !stoppingToken.IsCancellationRequested)
            {
                await Console.Out.WriteAsync("> ");

                // Read line
                var line = (await Console.In.ReadLineAsync(stoppingToken))?.Trim();
                if (line == null)
                    break;

                // Respond to next statement
                await ProcessLine(line, stoppingToken);

                await Console.Out.WriteLineAsync();
            }
        }
        catch (OperationCanceledException)
        {
            // Ignored
        }
        finally
        {
            _isRunning = true;
        }
    }

    private async Task ProcessLine(string line, CancellationToken stoppingToken)
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
                    await Console.Out.WriteLineAsync("Bye 👋");
                    _hostLifetime.StopApplication();
                    _isRunning = false;
                    break;

                case "help":
                    await Console.Out.WriteLineAsync("Not implemented.");
                    break;

                case "back":
                case "pop":
                case "out":
                    await HandlePop(stoppingToken);
                    break;

                case "open":
                case "push":
                case "in":
                    await HandlePush(parameter, stoppingToken);
                    break;

                case "show":
                case "dump":
                case "print":
                    await HandlePrint(parameter, stoppingToken);
                    break;

                case "expand":
                case "populate":
                case "fill":
                    await HandlePopulate(parameter, stoppingToken);
                    break;

                default:
                    await Console.Out.WriteLineAsync($"Unknown directive '{directive}'");
                    break;
            }
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync(e.ToString());
        }
    }

    private async Task HandlePop(CancellationToken stoppingToken)
    {
        if (!_focus.Any())
        {
            await Console.Out.WriteLineAsync("Can't go back - no objects are in focus");
            return;
        }

        _focus.Pop();
        await HandlePrint(null, stoppingToken);
    }

    private async Task HandlePush(string? parameter, CancellationToken stoppingToken)
    {
        if (string.IsNullOrEmpty(parameter))
        {
            await Console.Out.WriteLineAsync("Please specify a URI or property to push");
            return;
        }

        // If param is a URI, then its a new object
        if (Uri.TryCreate(parameter, UriKind.Absolute, out var uri))
        {
            await PushUri(uri, stoppingToken);
            await HandlePrint(null, stoppingToken);
        }

        // Otherwise, its a selector
        else if (_focus.TryPeek(out var current))
        {
            await PushSelector(current, parameter, stoppingToken);
            await HandlePrint(null, stoppingToken);
        }

        // Fallback
        else
        {
            await Console.Out.WriteLineAsync("Can't open - no object is in focus and input is not a URI");
        }
    }

    private async Task PushUri(Uri uri, CancellationToken stoppingToken)
    {
        var newObj = await _apClient.Get<ASType>(uri, cancellationToken: stoppingToken);
        _focus.Push(newObj);
    }

    private async Task PushSelector(ASType current, string parameter, CancellationToken stoppingToken)
    {
        var newObject = await SelectObject<ASType>(current, parameter, stoppingToken);
        _focus.Push(newObject);
    }

    private async Task HandlePrint(string? parameter, CancellationToken stoppingToken)
    {
        if (!_focus.TryPeek(out var current))
        {
            await Console.Out.WriteLineAsync("Can't print - no object in focus.");
            return;
        }

        object? toPrint = current;

        if (parameter != null)
        {
            toPrint = await SelectObject<object?>(current, parameter, stoppingToken);
        }

        var json = _jsonLdSerializer.Serialize(toPrint);
        await Console.Out.WriteLineAsync(json);
    }

    private async Task HandlePopulate(string? parameter, CancellationToken stoppingToken)
    {
        if (!_focus.TryPeek(out var current))
        {
            await Console.Out.WriteLineAsync("Can't populate - no object in focus.");
            return;
        }

        if (current is ASObject asObj)
        {
            if (parameter == null)
            {
                await Console.Out.WriteLineAsync("Please specify a property to populate.");
                return;
            }

            var prop = FindProperty(asObj, parameter);
            var value = await SelectObject<ASObject>(asObj, prop, stoppingToken);
            prop.SetValue(asObj, value);
        }
        else if (current is ASLink asLink)
        {
            current = await _apClient.Get<ASObject>(asLink, cancellationToken: stoppingToken);
            _focus.Push(current);
        }
        else
        {
            await Console.Out.WriteLineAsync($"Unsupported type: {current.GetType()}");
        }

        await HandlePrint(null, stoppingToken);
    }

    private async Task<T> SelectObject<T>(ASType obj, string propertyName, CancellationToken stoppingToken)
    {
        var property = FindProperty(obj, propertyName);
        return await SelectObject<T>(obj, property, stoppingToken);
    }
    
    private async Task<T> SelectObject<T>(ASType obj, PropertyInfo property, CancellationToken stoppingToken)
    {
        // Get and populate property value
        var value = property.GetValue(obj) switch
        {
            ASLink asLink => await _apClient.Get<ASType>(asLink, cancellationToken: stoppingToken),
            Linkable<ASObject> linkable => await _apClient.Resolve(linkable, cancellationToken: stoppingToken),
            LinkableList<ASObject> linkables => await _apClient.Resolve(linkables, cancellationToken: stoppingToken),
            var v => v
        };

        // Verify and return
        if (value is not T typedValue)
            throw new ApplicationException("Selected property is not compatible");
        return typedValue;
    }

    private static PropertyInfo FindProperty(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName);
        if (property == null)
            throw new MissingMemberException("Can't find that property within the object in focus.");
        return property;
    }
}