// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Nodes;
using ActivityPub.Types.Conversion.Overrides;

namespace ActivityPub.Types.Conversion.Pivots;

public interface ICustomConvertedEntityPivot
{
    public ASEntity? ReadEntity(Type entityType, JsonElement jsonElement, DeserializationMetadata meta);
    public void PostReadEntity(Type entityType, JsonElement jsonElement, DeserializationMetadata meta, ASEntity entity);

    public JsonElement? WriteEntity(Type entityType, ASEntity entity, SerializationMetadata meta);
    public void PostWriteEntity(Type entityType, ASEntity entity, SerializationMetadata meta, JsonElement entityJson, JsonObject outputJson);
}
