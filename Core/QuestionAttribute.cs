namespace CSharpCodingQuestions.Core;

/// <summary>
/// Marks a class as a question. The folder (namespace) decides its section and topic;
/// <see cref="Order"/> decides its position inside the topic.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class QuestionAttribute : Attribute
{
    public int Order { get; set; }
    public string Title { get; set; } = "";
    public Difficulty Level { get; set; }

    /// <summary>The problem statement, in simple Markdown.</summary>
    public string Problem { get; set; } = "";
}
