// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Net.Http.Headers;
using System.Text.Json;
using ActivityPub.Common.Util;
using ActivityPub.Types.AS;
using ActivityPub.Types.Conversion;
using ActivityPub.Types.Util;

namespace ActivityPub.Client;

/// <summary>
///     Default implementation of <see cref="IActivityPubClient" />.
/// </summary>
public class ActivityPubClient : IActivityPubClient
{
    private readonly ActivityPubOptions _apOptions;
    private readonly HttpClient _httpClient = new();
    private readonly IJsonLdSerializer _jsonLdSerializer;

    /// <summary>
    ///     Constructs a new ActivityPub client
    /// </summary>
    /// <param name="apOptions">Options for the ActivityPub protocol</param>
    /// <param name="jsonLdSerializer">JSON-LD serializer</param>
    public ActivityPubClient(ActivityPubOptions apOptions, IJsonLdSerializer jsonLdSerializer)
    {
        _apOptions = apOptions;
        _jsonLdSerializer = jsonLdSerializer;

        // The fuck?
        // https://stackoverflow.com/questions/47176104/c-sharp-add-accept-header-to-httpclient
        foreach (var mimeType in apOptions.RequestContentTypes)
        {
            var mediaType = new MediaTypeWithQualityHeaderValue(mimeType.MediaType);
            if (mimeType.Profile != "") {
                mediaType.Parameters.Add(new NameValueHeaderValue("profile", mimeType.Profile));
            }
            _httpClient.DefaultRequestHeaders.Accept.Add(mediaType);
        }
    }

    /// <inheritdoc />
    public int DefaultGetRecursion { get; set; }

    /// <inheritdoc />
    public async Task<T> Get<T>(Uri uri, int? maxRecursion = null, CancellationToken cancellationToken = default)
        where T : ASType
        => (T)await Get(uri, typeof(T), maxRecursion, cancellationToken);

    /// <inheritdoc />
    public int DefaultResolveRecursion { get; set; } = 1;

    /// <inheritdoc />
    public async Task<T> Resolve<T>(Linkable<T> linkable, int? maxRecursion = null, CancellationToken cancellationToken = default)
        where T : ASObject
    {
        // short-circuit the easy case
        if (linkable.HasValue)
            return linkable.Value;

        // Get and recursively populate
        maxRecursion ??= DefaultResolveRecursion;
        return await Get<T>(linkable.Link, maxRecursion - 1, cancellationToken);
    }

    /// <inheritdoc />
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

    private async Task<ASType> Get(Uri uri, Type targetType, int? maxRecursion, CancellationToken cancellationToken = default)
    {
        var resp = await _httpClient.GetAsync(uri, cancellationToken);
        if (!resp.IsSuccessStatusCode)
            throw new ApplicationException($"Request failed: got status {resp.StatusCode}");

        var contentType = resp.Content.Headers.ContentType;
        if (contentType == null || !_apOptions.ResponseContentTypes.Any(expectedContentType => IsContentTypeMatch(contentType, expectedContentType)))
            throw new ApplicationException($"Request failed: unsupported content type {contentType?.MediaType}");

        var json = await resp.Content.ReadAsStringAsync(cancellationToken);
        var jsonObj = _jsonLdSerializer.Deserialize(json, targetType);
        if (jsonObj is not ASType obj)
            throw new JsonException($"Failed to deserialize object - parser returned unsupported object {jsonObj?.GetType()}");

        // If its a link, then recursively follow it.
        maxRecursion ??= DefaultGetRecursion;
        if (maxRecursion > 0 && obj is ASLink link)
            obj = await Get(link.HRef.Uri, targetType, maxRecursion - 1, cancellationToken);

        return obj;
    }

    private static bool IsContentTypeMatch(MediaTypeHeaderValue actual, ActivityPubOptions.ContentType expected) {
        if (!expected.MediaType.Equals(actual.MediaType)) {
            return false;
        }

        var actualProfile = actual.Parameters.FirstOrDefault(p => p.Name.Equals("profile"))?.Value ?? "";

        return string.Equals(actualProfile, expected.Profile,
            StringComparison.OrdinalIgnoreCase);
    }


    #region Dispose

    /// <inheritdoc />
    public void Dispose()
    {
        // Dispose of unmanaged resources.
        Dispose(true);

        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Dispose the object's resources
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            _httpClient.Dispose();

        _disposed = true;
    }

    private bool _disposed;

    #endregion
}
