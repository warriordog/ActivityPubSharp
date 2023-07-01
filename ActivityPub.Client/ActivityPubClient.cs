// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using ActivityPub.Common.TypeInfo;
using ActivityPub.Common.Util;
using ActivityPub.Types;
using ActivityPub.Types.Util;

namespace ActivityPub.Client;

/// <summary>
/// Default implementation of <see cref="IActivityPubClient"/>.
/// </summary>
public class ActivityPubClient : IActivityPubClient
{
    private readonly HttpClient _httpClient = new();
    private readonly ITypeInfoCache _typeInfoCache;
    private readonly ActivityPubOptions _apOptions;

    public ActivityPubClient(ITypeInfoCache typeInfoCache, ActivityPubOptions apOptions)
    {
        _typeInfoCache = typeInfoCache;
        _apOptions = apOptions;
    }

    public int DefaultGetRecursion { get; set; }

    public async Task<T> Get<T>(Uri uri, int? maxRecursion = null, CancellationToken cancellationToken = default)
        where T : ASType
        => (T)await Get(uri, typeof(T), maxRecursion, cancellationToken);

    private async Task<ASType> Get(Uri uri, Type targetType, int? maxRecursion, CancellationToken cancellationToken = default)
    {
        var resp = await _httpClient.GetAsync(uri, cancellationToken);
        if (!resp.IsSuccessStatusCode)
            throw new ApplicationException($"Request failed: got status {resp.StatusCode}");

        var mediaType = resp.Content.Headers.ContentType?.MediaType;
        if (mediaType == null || !_apOptions.ContentTypes.Contains(mediaType))
            throw new ApplicationException($"Request failed: unsupported content type {mediaType}");

        var json = await resp.Content.ReadAsStringAsync(cancellationToken);
        var jsonObj = JsonSerializer.Deserialize(json, targetType, _apOptions.SerializerOptions);
        if (jsonObj is not ASType obj)
            throw new JsonException($"Failed to deserialize object - parser returned unsupported object {jsonObj?.GetType()}");

        // Recursively populate the object
        maxRecursion ??= DefaultGetRecursion;
        if (maxRecursion > 0)
        {
            await Populate(targetType, obj, maxRecursion - 1, cancellationToken);
        }

        return obj;
    }

    public int DefaultResolveRecursion { get; set; } = 1;

    public async Task<T> Resolve<T>(Linkable<T> linkable, int? maxRecursion = null, CancellationToken cancellationToken = default) where T : ASObject
    {
        // short-circuit the easy case
        if (linkable.HasValue)
            return linkable.Value;

        // Get and recursively populate
        maxRecursion ??= DefaultResolveRecursion;
        return await Get<T>(linkable.Link, maxRecursion - 1, cancellationToken);
    }

    public async Task<List<T>> Resolve<T>(LinkableList<T> linkables, int? maxRecursion = null, CancellationToken cancellationToken = default)
        where T : ASObject
    {
        var list = new List<T>();

        foreach (var linkable in linkables)
        {
            var value = await Resolve(linkable, maxRecursion, cancellationToken);
            list.Add(value);
        }

        return list;
    }

    public int DefaultPopulateRecursion { get; set; } = 1;

    public async Task<T> Populate<T>(T obj, int? maxRecursion = null, CancellationToken cancellationToken = default)
        where T : ASObject
    {
        await Populate(typeof(T), obj, maxRecursion, cancellationToken);
        return obj;
    }

    private async Task Populate(Type type, ASType obj, int? maxRecursion, CancellationToken cancellationToken = default)
    {
        maxRecursion ??= DefaultPopulateRecursion;
        var typeInfo = _typeInfoCache.GetFor(type);

        // Replace Linkables entirely
        foreach (var (prop, genericType) in typeInfo.LinkableProperties)
        {
            if (prop.GetValue(obj) is ILinkable { HasLink: true } linkable)
            {
                var propType = prop.GetType();

                var value = await Get(linkable.Link, genericType, maxRecursion - 1, cancellationToken);
                var linkableWithValue = Activator.CreateInstance(propType, value);

                prop.SetValue(obj, linkableWithValue);
            }
        }

        // Modify LinkableLists in-place
        foreach (var (prop, genericType) in typeInfo.LinkableListProperties)
        {
            if (prop.GetValue(obj) is ILinkableList list)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    var linkable = list.Get(i);
                    if (linkable.HasLink)
                    {
                        var value = await Get(linkable.Link, genericType, maxRecursion - 1, cancellationToken);
                        list.Set(i, value);
                    }
                }
            }
        }
    }

    #region Dispose

    public void Dispose()
    {
        // Dispose of unmanaged resources.
        Dispose(true);

        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _httpClient.Dispose();
        }

        _disposed = true;
    }

    private bool _disposed;

    #endregion
}