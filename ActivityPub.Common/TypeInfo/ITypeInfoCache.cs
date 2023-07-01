// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace ActivityPub.Common.TypeInfo;

public interface ITypeInfoCache
{
    /// <summary>
    /// Get or create type metadata for a given type.
    /// Type info is automatically cached.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public TypeInfo GetFor(Type type);
}