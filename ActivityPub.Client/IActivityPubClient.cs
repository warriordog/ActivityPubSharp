// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ActivityPub.Types.AS;
using ActivityPub.Types.Util;

namespace ActivityPub.Client;

/// <summary>
///     Client for accessing resources over ActivityPub
/// </summary>
public interface IActivityPubClient : IDisposable
{
    /// <summary>
    ///     Default recursion depth for <see cref="Get{T}" />.
    /// </summary>
    /// <seealso cref="DefaultResolveRecursion" />
    [Range(0, int.MaxValue)]
    [DefaultValue(0)]
    public int DefaultGetRecursion { get; set; }

    /// <summary>
    ///     Default recursion depth for <see cref="Resolve{T}(Linkable{T}, int?, CancellationToken)" /> and <see cref="Resolve{T}(LinkableList{T}, int?, CancellationToken)" />
    /// </summary>
    /// <seealso cref="DefaultGetRecursion" />
    [Range(0, int.MaxValue)]
    [DefaultValue(1)]
    public int DefaultResolveRecursion { get; set; }

    /// <summary>
    ///     Retrieves and validates an ActivityPub object.
    ///     Links are automatically followed, up to <see cref="maxRecursion" /> layers of recursion.
    /// </summary>
    /// <param name="uri">URI to the object</param>
    /// <param name="maxRecursion">Maximum depth to recurse while populating the returned object. Defaults to <see cref="DefaultGetRecursion" />.</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <typeparam name="T">Type of object to return. The actual returned object may be a subclass of T.</typeparam>
    /// <returns>Returns the object</returns>
    public Task<T> Get<T>(Uri uri, int? maxRecursion = null, CancellationToken cancellationToken = default)
        where T : ASType;

    /// <summary>
    ///     Resolves a <see cref="Linkable{T}" /> to an object.
    ///     If the linkable has an object, then its returned.
    ///     If it has a link, then the link is retrieved using <see cref="Get{T}" />.
    /// </summary>
    /// <param name="linkable">Linkable to resolve</param>
    /// <param name="maxRecursion">Maximum depth to recurse while populating the returned object. <see cref="DefaultResolveRecursion" />.</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <typeparam name="T">Type of object to return. The actual returned object may be a subclass of T.</typeparam>
    /// <returns>Returns the object</returns>
    public Task<T> Resolve<T>(Linkable<T> linkable, int? maxRecursion = null, CancellationToken cancellationToken = default)
        where T : ASObject;

    /// <summary>
    ///     Resolves a <see cref="LinkableList{T}" /> to objects.
    ///     Each linkable is resolved using <see cref="Resolve{T}(Linkable{T}, int?, CancellationToken)" />.
    /// </summary>
    /// <param name="linkables">List of linkables to resolve</param>
    /// <param name="maxRecursion">Maximum depth to recurse while populating the returned objects. <see cref="DefaultResolveRecursion" />.</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <typeparam name="T">Type of objects to return. The actual returned objects may be subclasses of T.</typeparam>
    /// <returns>Returns a list of resolved objects.</returns>
    public Task<List<T>> Resolve<T>(LinkableList<T> linkables, int? maxRecursion = null, CancellationToken cancellationToken = default)
        where T : ASObject;
}