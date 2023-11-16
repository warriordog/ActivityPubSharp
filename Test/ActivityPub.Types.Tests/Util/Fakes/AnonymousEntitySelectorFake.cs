// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion.Overrides;
using ActivityPub.Types.Internal;

namespace ActivityPub.Types.Tests.Util.Fakes;

public class AnonymousEntitySelectorFake : IAnonymousEntitySelector
{
    public Dictionary<string, Type> PropertyNameMapping { get; set; } = new();

    public IEnumerable<Type> SelectAnonymousEntities(JsonElement inputJson)
    {
        if (inputJson.ValueKind != JsonValueKind.Object)
            yield break;
        
        foreach (var (propName, entityType) in PropertyNameMapping)
            if (inputJson.HasProperty(propName))
                yield return entityType;
    }
}