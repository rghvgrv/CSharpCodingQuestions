using System.Reflection;

namespace CSharpCodingQuestions.Core;

/// <param name="Method">The method to run for examples, or null when the approach is a class.</param>
public sealed record ApproachInfo(string Name, string Time, string Space, string Idea, string Code, MethodInfo? Method);
