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
    ///     Identifies an exclusive option for a Question.
    ///     Use of oneOf implies that the Question can have only a single answer.
    ///     To indicate that a Question can have multiple answers, use anyOf.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-oneOf" />
    public LinkableList<ASObject>? OneOf
    {
        get => Entity.OneOf;
        set => Entity.OneOf = value;
    }

    /// <summary>
    ///     Identifies an inclusive option for a Question.
    ///     Use of anyOf implies that the Question can have multiple answers.
    ///     To indicate that a Question can have only one answer, use oneOf.
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-anyof" />
    public LinkableList<ASObject>? AnyOf
    {
        get => Entity.AnyOf;
        set => Entity.AnyOf = value;
    }

    /// <summary>
    ///     Contains the time at which a question was closed.
    /// </summary>
    /// <remarks>
    ///     * Won't always be set - can be null even if <see cref="Closed" /> is true.
    ///     * We don't support the Object or Link forms, because what would that even mean??
    ///     * May possibly be in the future? Spec does not specify.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-closed" />
    public DateTime? ClosedAt
    {
        get => Entity.ClosedAt;
        set => Entity.ClosedAt = value;
    }

    /// <summary>
    ///     Indicates that a question has been closed, and answers are no longer accepted.
    /// </summary>
    /// <remarks>
    ///     We don't support the Object or Link forms, because what would that even mean??
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-closed" />
    public bool? Closed
    {
        get => Entity.Closed;
        set => Entity.Closed = value;
    }
}

/// <inheritdoc cref="QuestionActivity" />
public sealed class QuestionActivityEntity : ASEntity<QuestionActivity, QuestionActivityEntity>
{
    private bool? _closed;
    private DateTime? _closedAt;

    /// <inheritdoc cref="QuestionActivity.OneOf" />
    [JsonPropertyName("oneOf")]
    public LinkableList<ASObject>? OneOf { get; set; }

    /// <inheritdoc cref="QuestionActivity.AnyOf" />
    [JsonPropertyName("anyOf")]
    public LinkableList<ASObject>? AnyOf { get; set; }

    /// <inheritdoc cref="QuestionActivity.ClosedAt" />
    [JsonPropertyName("closed")]
    public DateTime? ClosedAt
    {
        get => _closedAt;
        set
        {
            _closedAt = value;
            if (_closedAt == null)
                _closed = null;
        }
    }

    /// <inheritdoc cref="QuestionActivity.Closed" />
    [JsonPropertyName("closed")]
    public bool? Closed
    {
        get => _closed ?? _closedAt != null;
        set
        {
            _closed = value;
            if (value != true)
                _closedAt = null;
        }
    }
}