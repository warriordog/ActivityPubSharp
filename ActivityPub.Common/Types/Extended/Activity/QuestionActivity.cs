using ActivityPub.Common.Util;

namespace ActivityPub.Common.Types.Extended.Activity;

/// <summary>
/// Represents a question being asked.
/// Question objects are an extension of IntransitiveActivity.
/// That is, the Question object is an Activity, but the direct object is the question itself and therefore it would not contain an object property.
/// Either of the anyOf and oneOf properties MAY be used to express possible answers, but a Question object MUST NOT have both properties. 
/// </summary>
public class QuestionActivity : ASIntransitiveActivity
{
    public const string QuestionType = "Question";
    public QuestionActivity(string type = QuestionType) : base(type) {}
    
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
    /// Indicates that a question has been closed, and answers are no longer accepted. 
    /// </summary>
    /// <seealso href="https://www.w3.org/TR/activitystreams-vocabulary/#dfn-closed"/>
    public Linkable<Range<ASObject, DateTime, bool>>? Closed { get; set; }
}