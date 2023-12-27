// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ActivityPub.Types.Util;
using JetBrains.Annotations;

namespace ActivityPub.Types.AS.Extended.Activity;

/// <summary>
///     Represents a question being asked.
///     Question objects are an extension of IntransitiveActivity.
///     That is, the Question object is an Activity, but the direct object is the question itself and therefore it would not contain an object property.
///     Either of the anyOf and oneOf properties MAY be used to express possible answers, but a Question object MUST NOT have both properties.
/// </summary>
public class QuestionActivity : ASIntransitiveActivity, IASModel<QuestionActivity, QuestionActivityEntity, ASIntransitiveActivity>
{
    /// <summary>
    ///     ActivityStreams type name for "Question" types.
    /// </summary>
    [PublicAPI]
    public const string QuestionType = "Question";
    static string IASModel<QuestionActivity>.ASTypeName => QuestionType;

    /// <inheritdoc />
    public QuestionActivity() => Entity = TypeMap.Extend<QuestionActivity, QuestionActivityEntity>();

    /// <inheritdoc />
    public QuestionActivity(TypeMap typeMap, bool isExtending = true) : base(typeMap, false)
        => Entity = TypeMap.ProjectTo<QuestionActivity, QuestionActivityEntity>(isExtending);

    /// <inheritdoc />
    public QuestionActivity(ASType existingGraph) : this(existingGraph.TypeMap) {}

    /// <inheritdoc />
    [SetsRequiredMembers]
    public QuestionActivity(TypeMap typeMap, QuestionActivityEntity? entity) : base(typeMap, null)
        => Entity = entity ?? typeMap.AsEntity<QuestionActivity, QuestionActivityEntity>();

    static QuestionActivity IASModel<QuestionActivity>.FromGraph(TypeMap typeMap) => new(typeMap, null);

    private QuestionActivityEntity Entity { get; }

    /// <summary>
    ///     Indicates whether this question accepts multiple responses.
    ///     If <see langword="true"/>, then multiple options can be selected.
    ///     If <see langword="false"/> (default), then only one option may be selected.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This is a synthetic field implemented to simplify parts of the Question schema.
    ///         It does not exist in the ActivityStreams specification.
    ///     </para>
    ///     <para>
    ///         This field controls how the question is serialized.
    ///         If <see langword="true"/>, then <see cref="Options"/> will map to "anyOf".
    ///         Otherwise, it maps to "oneOf".
    ///     </para>
    /// </remarks>
    public bool AllowMultiple
    {
        get => Entity.AllowMultiple;
        set => Entity.AllowMultiple = value;
    }
    
    /// <summary>
    ///     The list of options for this Question.
    /// </summary>
    /// <remarks>
    ///     This is a semi-synthetic property.
    ///     It does not exist in the ActivityStreams specification, but encapsulates the usage of both "oneOf" and "anyOf".
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-oneOf" />
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-anyof" />
    public LinkableList<ASObject>? Options
    {
        get => Entity.Options;
        set => Entity.Options = value;
    }

    /// <summary>
    ///     Indicates that a question has been closed, and answers are no longer accepted.
    /// </summary>
    /// <remarks>
    ///     We don't support the Object or Link forms, because what would that even mean??
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-closed" />
    public FlagWithTimestamp? Closed
    {
        get => Entity.Closed;
        set => Entity.Closed = value;
    }
}

/// <inheritdoc cref="QuestionActivity" />
public sealed class QuestionActivityEntity : ASEntity<QuestionActivity, QuestionActivityEntity>, IJsonOnDeserialized, IJsonOnSerializing
{
    /// <summary>
    ///     Use <see cref="Options"/> instead.
    ///     This internal property exists only for serialization purposes.
    /// </summary>
    [JsonPropertyName("oneOf")]
    public LinkableList<ASObject>? OneOf { get; set; }

    /// <summary>
    ///     Use <see cref="Options"/> instead.
    ///     This internal property exists only for serialization purposes.
    /// </summary>
    [JsonPropertyName("anyOf")]
    public LinkableList<ASObject>? AnyOf { get; set; }

    /// <inheritdoc cref="QuestionActivity.Options" />
    [JsonIgnore]
    public LinkableList<ASObject>? Options { get; set; }
    
    /// <inheritdoc cref="QuestionActivity.AllowMultiple" />
    [JsonIgnore]
    public bool AllowMultiple { get; set; }

    /// <inheritdoc cref="QuestionActivity.Closed" />
    [JsonPropertyName("closed")]
    public FlagWithTimestamp? Closed { get; set; }
    
    void IJsonOnDeserialized.OnDeserialized()
    {
        AllowMultiple = false;
        
        if (AnyOf != null)
        {
            AllowMultiple = true;
            
            Options = AnyOf;
            AnyOf = null;
        }

        if (OneOf != null)
        {
            if (Options == null)
                Options = OneOf;
            else
                Options.AddRange(OneOf);

            OneOf = null;
        }
    }
    void IJsonOnSerializing.OnSerializing()
    {
        OneOf = null;
        AnyOf = null;
        
        if (AllowMultiple)
            AnyOf = Options;
        else
            OneOf = Options;
    }
}