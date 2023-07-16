// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Reflection;
using ActivityPub.Client;
using ActivityPub.Types;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;
using InternalUtils;
using Microsoft.Extensions.Hosting;

namespace SimpleClient;

public class ConsoleService : BackgroundService
{
    private readonly Stack<object?> _focus = new();

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
                case "resolve":
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
            await Console.Out.WriteLineAsync("Please specify a URI or property to push.");
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
            if (current == null)
            {
                await Console.Out.WriteLineAsync("Can't select from null.");
                return;
            }
            
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

    private async Task PushSelector(object current, string parameter, CancellationToken stoppingToken)
    {
        var newObject = await SelectObject(current, parameter, stoppingToken);
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
            if (current == null)
            {
                await Console.Out.WriteLineAsync("Can't select from null.");
                return;
            }
            
            toPrint = await SelectObject(current, parameter, stoppingToken);
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

        if (current == null)
        {
            await Console.Out.WriteLineAsync("Can't populate a null object.");
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
            var value = await SelectObject(asObj, prop, stoppingToken);
            
            // Verify and set
            if (value != null && !value.GetType().IsAssignableTo(prop.PropertyType))
                throw new ApplicationException($"Selected property is not compatible: {value.GetType()} is not assignable to {prop.PropertyType}");
            
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

    private async Task<object?> SelectObject(object obj, string propertyName, CancellationToken stoppingToken)
    {
        var property = FindProperty(obj, propertyName);
        return await SelectObject(obj, property, stoppingToken);
    }
    
    private async Task<object?> SelectObject(object obj, PropertyInfo property, CancellationToken stoppingToken)
    {
        // Get and populate property value
        var value = property.GetValue(obj);
        return await ResolveValue(value, stoppingToken);
    }

    private async Task<object?> ResolveValue(object? value, CancellationToken stoppingToken)
    {
        if (value == null)
            return null;
        
        // If its ASLink, then call Get<ASType>
        if (value is ASLink link)
            return await _apClient.Get<ASType>(link, cancellationToken: stoppingToken);
        
        var valueType = value.GetType();
        
        // If its Linkable<T>, then extract T and call ResolveValueOfLinkable(object) which pivots to ResolveValueOfLinkableOf<T>)(Linkable<T>)
        if (valueType.TryGetGenericArgumentsFor(typeof(Linkable<>), out var linkableSlots))
            return await ResolveValueOfLinkable(value, linkableSlots[0], stoppingToken);
        
        // If its LinkableList<T>, then extract T and call ResolveValueOfLinkableList(object) which pivots to ResolveValueOfLinkableListOf<T>(LinkableList<T>)
        if (valueType.TryGetGenericArgumentsFor(typeof(Linkable<>), out var linkableListSlots))
            return await ResolveValueOfLinkableList(value, linkableListSlots[0], stoppingToken);

        // Otherwise, we can't resolve so return as-is
        return value;
    }

    private async Task<object?> ResolveValueOfLinkable(object value, Type valueType, CancellationToken stoppingToken)
    {
        var method = typeof(ConsoleService)
                         .GetMethod(nameof(ResolveValueOfLinkableOf), BindingFlags.NonPublic | BindingFlags.Instance)
                         ?.MakeGenericMethod(valueType)
                     ?? throw new MissingMethodException($"Missing method ResolveValueOfLinkableOf<{valueType}>(Linkable<{valueType}>, CancellationToken)");
        return await (Task<object?>)method.Invoke(this, new[] {value, stoppingToken})!;
    }
    
    private async Task<object?> ResolveValueOfLinkableOf<T>(Linkable<T> value, CancellationToken stoppingToken)
        where T : ASObject
    {
        var result = await _apClient.Resolve(value, cancellationToken: stoppingToken);
        return new Linkable<T>(result);
    }

    private async Task<object?> ResolveValueOfLinkableList(object value, Type valueType, CancellationToken stoppingToken)
    {
        var method = typeof(ConsoleService)
                         .GetMethod(nameof(ResolveValueOfLinkableListOf), BindingFlags.NonPublic | BindingFlags.Instance)
                         ?.MakeGenericMethod(valueType)
                     ?? throw new MissingMethodException($"Missing method ResolveValueOfLinkableListOf<{valueType}>(LinkableList<{valueType}>, CancellationToken)");
        return await (Task<object?>)method.Invoke(this, new[] {value, stoppingToken})!;
    }
    private async Task<object?> ResolveValueOfLinkableListOf<T>(LinkableList<T> value, CancellationToken stoppingToken)
        where T : ASObject
    {
        var results = await _apClient.Resolve(value, cancellationToken: stoppingToken);
        return new LinkableList<T>(results);
    }

    private static PropertyInfo FindProperty(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName);
        if (property == null)
            throw new MissingMemberException("Can't find that property within the object in focus.");
        return property;
    }
}