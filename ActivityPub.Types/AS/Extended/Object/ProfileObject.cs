// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Attributes;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     A Profile is a content object that describes another Object, typically used to describe Actor Type objects.
///     The describes property is used to reference the object being described by the profile.
/// </summary>
public class ProfileObject : ASObject
{
    public ProfileObject() => Entity = new ProfileObjectEntity { TypeMap = TypeMap };
    public ProfileObject(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<ProfileObjectEntity>();
    private ProfileObjectEntity Entity { get; }

    /// <summary>
    ///     On a Profile object, the describes property identifies the object described by the Profile.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-describes" />
    public ASObject? Describes
    {
        get => Entity.Describes;
        set => Entity.Describes = value;
    }
}

/// <inheritdoc cref="ProfileObject" />
[APType(ProfileType)]
[ImpliesOtherEntity(typeof(ASObjectEntity))]
public sealed class ProfileObjectEntity : ASEntity<ProfileObject>
{
    public const string ProfileType = "Profile";
    public override string ASTypeName => ProfileType;

    public override IReadOnlySet<string> ReplacesASTypes { get; } = new HashSet<string>
    {
        ASObjectEntity.ObjectType
    };

    /// <inheritdoc cref="ProfileObject.Describes" />
    [JsonPropertyName("describes")]
    public ASObject? Describes { get; set; }
}