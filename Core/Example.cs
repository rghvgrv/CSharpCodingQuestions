namespace CSharpCodingQuestions.Core;

/// <summary>
/// One input for a question and the answer every approach must return.
/// <paramref name="AnyOrder"/> accepts the answer's items in any order.
/// </summary>
public sealed record Example(object?[] Input, object? Expected, bool AnyOrder = false);
