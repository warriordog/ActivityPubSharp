// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Types.Conversion.Overrides;


/// <summary>
///     Common options to configure JSON / JSON-LD conversion.
/// </summary>
public interface IConversionOptions
{
    /// <summary>
    ///     Additional <see cref="IAnonymousEntitySelector"/> instances to execute when converting any incoming JSON.
    /// </summary>
    public IEnumerable<IAnonymousEntitySelector> AnonymousEntitySelectors { get; }
}

/// <inheritdoc cref="IConversionOptions"/>
public class ConversionOptions : IConversionOptions
{
    /// <inheritdoc cref="IConversionOptions.AnonymousEntitySelectors"/>
    public HashSet<IAnonymousEntitySelector> AnonymousEntitySelectors { get; set; } = new();
    IEnumerable<IAnonymousEntitySelector> IConversionOptions.AnonymousEntitySelectors => AnonymousEntitySelectors;
}