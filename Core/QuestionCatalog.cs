using System.Reflection;

namespace CSharpCodingQuestions.Core;

/// <summary>Finds every question class and topic README, and puts them in learning order.</summary>
public static class QuestionCatalog
{
    const string QuestionsNamespace = "CSharpCodingQuestions.Questions.";

    public static readonly IReadOnlyList<TopicInfo> Topics = Curriculum.Topics.Select(LoadTopic).ToList();

    public static readonly IReadOnlyList<QuestionInfo> Questions = LoadQuestions();

    public static readonly IReadOnlyDictionary<string, QuestionInfo> ById =
        Questions.ToDictionary(question => question.Id, StringComparer.OrdinalIgnoreCase);

    static TopicInfo LoadTopic(string topicId)
    {
        string[] lines = ResourceFiles.Read($"{topicId}/README.md").Split('\n');
        string title = lines[0].TrimStart('#', ' ');
        string body = string.Join('\n', lines.Skip(1)).Trim();
        string summary = body.Split("\n\n")[0].Replace('\n', ' ');
        string sectionTitle = Curriculum.SectionTitles[topicId.Split('/')[0]];
        return new TopicInfo(topicId, sectionTitle, title, summary, body);
    }

    static List<QuestionInfo> LoadQuestions()
    {
        var found = typeof(QuestionCatalog).Assembly.GetTypes()
            .Select(type => (Type: type, Attribute: type.GetCustomAttribute<QuestionAttribute>()))
            .Where(item => item.Attribute != null)
            .Select(item => (item.Type, Attribute: item.Attribute!, Topic: FindTopic(item.Type)))
            .OrderBy(item => Topics.ToList().IndexOf(item.Topic))
            .ThenBy(item => item.Attribute.Order)
            .ToList();

        return found
            .Select((item, index) => new QuestionInfo(
                Number: index + 1,
                Id: item.Type.Name,
                Title: item.Attribute.Title,
                Level: item.Attribute.Level,
                Problem: item.Attribute.Problem,
                Topic: item.Topic,
                SharedCode: ApproachSource.SharedCode(ReadSource(item.Type, item.Topic)),
                Approaches: LoadApproaches(item.Type, item.Topic),
                Type: item.Type))
            .ToList();
    }

    static TopicInfo FindTopic(Type type)
    {
        string topicId = type.Namespace!.Replace(QuestionsNamespace, "").Replace('.', '/');
        return Topics.FirstOrDefault(topic => topic.Id == topicId)
            ?? throw new InvalidOperationException($"{type.Name}: topic '{topicId}' is not listed in Curriculum.Topics.");
    }

    static string ReadSource(Type type, TopicInfo topic) => ResourceFiles.Read($"{topic.Id}/{type.Name}.cs");

    static List<ApproachInfo> LoadApproaches(Type type, TopicInfo topic)
    {
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Select(method => (Attribute: method.GetCustomAttribute<ApproachAttribute>(), Method: (MethodInfo?)method));
        var classes = type.GetNestedTypes()
            .Select(nested => (Attribute: nested.GetCustomAttribute<ApproachAttribute>(), Method: (MethodInfo?)null));
        var byName = methods.Concat(classes)
            .Where(item => item.Attribute != null)
            .ToDictionary(item => item.Attribute!.Name);

        var snippets = ApproachSource.Split(ReadSource(type, topic));
        if (snippets.Count == 0 || snippets.Count != byName.Count)
        {
            throw new InvalidOperationException($"{type.Name}: found {snippets.Count} approach snippets but {byName.Count} [Approach] attributes.");
        }

        return snippets
            .Select(snippet =>
            {
                var (attribute, method) = byName[snippet.Name];
                return new ApproachInfo(attribute!.Name, attribute.Time, attribute.Space, attribute.Idea, snippet.Code, method);
            })
            .ToList();
    }
}
