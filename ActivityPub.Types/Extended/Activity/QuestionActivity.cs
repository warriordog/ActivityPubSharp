/* This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Text.Json.Serialization;
using ActivityPub.Types.Json;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Extended.Activity;

/// <summary>
/// Represents a question being asked.
/// Question objects are an extension of IntransitiveActivity.
/// That is, the Question object is an Activity, but the direct object is the question itself and therefore it would not contain an object property.
/// Either of the anyOf and oneOf properties MAY be used to express possible answers, but a Question object MUST NOT have both properties. 
/// </summary>
[ASTypeKey(QuestionType)]
public class QuestionActivity : ASIntransitiveActivity
{
    public const string QuestionType = "Question";

    [JsonConstructor]
    public QuestionActivity() : this(QuestionType) {}

    protected QuestionActivity(string type) : base(type) {}

    /// <summary>
    /// Identifies an exclusive option for a Question.
    /// Use of oneOf implies that the Question can have only a single answer.
    /// To indicate that a Question can have multiple answers, use anyOf. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-oneOf"/>
    public Linkable<ASObject>? OneOf { get; set; }

    /// <summary>
    /// Identifies an inclusive option for a Question.
    /// Use of anyOf implies that the Question can have multiple answers.
    /// To indicate that a Question can have only one answer, use oneOf. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-anyOf"/>
    public Linkable<ASObject>? AnyOf { get; set; }

    /// <summary>
    /// Contains the time at which a question was closed.
    /// </summary>
    /// <remarks>
    /// * Won't always be set - can be null even if <see cref="Closed"/> is true.
    /// * We don't support the Object or Link forms, because what would that even mean??
    /// * May possibly be in the future? Spec does not specify. 
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-closed"/>
    public DateTime? ClosedAt
    {
        get => _closedAt;
        set
        {
            _closedAt = value;
            if (_closedAt == null)
            {
                _closed = null;
            }
        }
    }

    /// <summary>
    /// Indicates that a question has been closed, and answers are no longer accepted. 
    /// </summary>
    /// <remarks>
    /// We don't support the Object or Link forms, because what would that even mean??
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-closed"/>
    public bool? Closed
    {
        get => _closed ?? _closedAt != null;
        set
        {
            _closed = value;
            if (value != true)
            {
                _closedAt = null;
            }
        }
    }

    private bool? _closed;
    private DateTime? _closedAt;
}