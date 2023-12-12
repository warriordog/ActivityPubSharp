// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Conversion.Overrides;

namespace ActivityPub.Types.Conversion.Pivots;

public interface INamelessEntityPivot
{
    bool ShouldConvert(Type entityType, DeserializationMetadata meta);
}
