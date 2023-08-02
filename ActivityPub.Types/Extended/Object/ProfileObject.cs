// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;

namespace ActivityPub.Types.Extended.Object;

/// <summary>
/// A Profile is a content object that describes another Object, typically used to describe Actor Type objects.
/// The describes property is used to reference the object being described by the profile. 
/// </summary>
public class ProfileObject : ASObject
{
    private ProfileObjectEntity Entity { get; }
    
    public ProfileObject() => Entity = new ProfileObjectEntity(TypeMap);
    public ProfileObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ProfileObjectEntity>();

    /// <summary>
    /// On a Profile object, the describes property identifies the object described by the Profile.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-describes"/>
    public ASObject? Describes
    {
        get => Entity.Describes;
        set => Entity.Describes = value;
    }
}

/// <inheritdoc cref="ProfileObject"/>
[ASTypeKey(ProfileType)]
public sealed class ProfileObjectEntity : ASBase
{
    public const string ProfileType = "Profile";

        /// <inheritdoc cref="ASBase(string?, TypeMap)"/>
    public ProfileObjectEntity(TypeMap typeMap) : base(ProfileType, typeMap) {}
    
    /// <inheritdoc cref="ASBase(string?)"/>
    [JsonConstructor]
    public ProfileObjectEntity() : base(ProfileType) {}

    /// <inheritdoc cref="ProfileObject.Describes"/>
    [JsonPropertyName("describes")]
    public ASObject? Describes { get; set; }
}