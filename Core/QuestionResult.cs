namespace CSharpCodingQuestions.Core;

/// <param name="Examples">Input → output rows, for questions with an Examples property.</param>
/// <param name="Text">What Demo() printed, for questions without fixed inputs.</param>
/// <param name="Errors">Wrong answers, crashes or timeouts. Empty when everything is correct.</param>
public sealed record QuestionResult(IReadOnlyList<ExampleResult> Examples, string Text, IReadOnlyList<string> Errors);
