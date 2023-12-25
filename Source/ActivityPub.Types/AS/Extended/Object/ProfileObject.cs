// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Object;

/// <summary>
///     A Profile is a content object that describes another Object, typically used to describe Actor Type objects.
///     The describes property is used to reference the object being described by the profile.
/// </summary>
public class ProfileObject : ASObject, IASModel<ProfileObject, ProfileObjectEntity, ASObject>
{
    /// <summary>
    ///     ActivityStreams type name for "Profile" types.
    /// </summary>
    [PublicAPI]
    public const string ProfileType = "Profile";
    static string IASModel<ProfileObject>.ASTypeName => ProfileType;

    /// <inheritdoc />
    public ProfileObject() => Entity = TypeMap.Extend<ProfileObject, ProfileObjectEntity>();

    /// <inheritdoc />
    public ProfileObject(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<ProfileObject, ProfileObjectEntity>(isExtending);

    /// <inheritdoc />
    public ProfileObject(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public ProfileObject(TypeMap typeMap, ProfileObjectEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<ProfileObject, ProfileObjectEntity>();

    static ProfileObject IASModel<ProfileObject>.FromGraph(TypeMap typeMap) => new(typeMap, null);


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
public sealed class ProfileObjectEntity : ASEntity<ProfileObject, ProfileObjectEntity>
{
    /// <inheritdoc cref="ProfileObject.Describes" />
    [JsonPropertyName("describes")]
    public ASObject? Describes { get; set; }
}