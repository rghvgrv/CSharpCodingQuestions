namespace CSharpCodingQuestions.Core;

/// <summary>
/// Marks one way of solving a question. Write approaches in the file from worst to best:
/// the UI shows them in that order, and the last one produces the result shown on the page.
/// Everything from this attribute up to the next approach (or to Examples / Demo) is shown as the code.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class ApproachAttribute : Attribute
{
    public string Name { get; set; } = "";
    public string Time { get; set; } = "";
    public string Space { get; set; } = "";

    /// <summary>How the approach works, in simple Markdown.</summary>
    public string Idea { get; set; } = "";
}
