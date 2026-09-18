namespace CSharpCodingQuestions.Core;

/// <param name="Id">Folder path under Questions/, like "Dsa/Arrays".</param>
/// <param name="Summary">The README's first paragraph.</param>
/// <param name="Body">The README without its title line.</param>
public sealed record TopicInfo(string Id, string SectionTitle, string Title, string Summary, string Body);
