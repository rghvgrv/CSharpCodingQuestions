namespace CSharpCodingQuestions.Core;

/// <param name="Number">Position in the whole learning path, starting at 1.</param>
/// <param name="SharedCode">Helper code the approaches use (may be empty).</param>
/// <param name="Approaches">From worst to best.</param>
public sealed record QuestionInfo(
    int Number,
    string Id,
    string Title,
    Difficulty Level,
    string Problem,
    TopicInfo Topic,
    string SharedCode,
    IReadOnlyList<ApproachInfo> Approaches,
    Type Type)
{
    /// <summary>True when the question has fixed inputs (an Examples property); false when it has a Demo() method.</summary>
    public bool HasExamples => Type.GetProperty("Examples") != null;
}
