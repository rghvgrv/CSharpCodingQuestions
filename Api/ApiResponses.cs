using CSharpCodingQuestions.Core;

namespace CSharpCodingQuestions.Api;

/// <summary>The JSON the UI reads. Used by the live server and by the static export, so both produce the same data.</summary>
public static class ApiResponses
{
    public static object Catalog() => new
    {
        Sections = QuestionCatalog.Topics
            .GroupBy(topic => topic.SectionTitle)
            .Select(section => new
            {
                Title = section.Key,
                Topics = section.Select(topic => new
                {
                    topic.Id,
                    topic.Title,
                    topic.Summary,
                    Questions = QuestionCatalog.Questions
                        .Where(question => question.Topic == topic)
                        .Select(question => new { question.Id, question.Number, question.Title, Level = question.Level.ToString() }),
                }),
            }),
    };

    public static object Topic(TopicInfo topic) => new { topic.Id, topic.SectionTitle, topic.Title, topic.Body };

    public static object Question(QuestionInfo question)
    {
        QuestionResult result = QuestionRunner.GetResult(question);
        return new
        {
            question.Id,
            question.Number,
            question.Title,
            Level = question.Level.ToString(),
            question.Problem,
            Topic = new { question.Topic.Id, question.Topic.Title, question.Topic.SectionTitle },
            question.SharedCode,
            Approaches = question.Approaches.Select(approach => new { approach.Name, approach.Time, approach.Space, approach.Idea, approach.Code }),
            Result = new { result.Examples, result.Text, result.Errors },
        };
    }
}
