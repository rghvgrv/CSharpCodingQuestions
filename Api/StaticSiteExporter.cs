using System.Text.Json;
using CSharpCodingQuestions.Core;

namespace CSharpCodingQuestions.Api;

/// <summary>
/// `dotnet run -- --export`: runs every question once and writes the whole site as static files
/// (the React build plus one JSON file per page) so it can be hosted on Cloudflare Pages or any static host.
/// </summary>
public static class StaticSiteExporter
{
    static readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web);

    public static int Export(string outputFolder)
    {
        if (!File.Exists("wwwroot/index.html"))
        {
            Console.Error.WriteLine("wwwroot/index.html is missing. Run `dotnet build` first, which builds the React UI.");
            return 1;
        }

        if (Directory.Exists(outputFolder))
        {
            Directory.Delete(outputFolder, recursive: true);
        }
        CopyFolder("wwwroot", outputFolder);

        WriteJson(outputFolder, "data/catalog.json", ApiResponses.Catalog());
        foreach (TopicInfo topic in QuestionCatalog.Topics)
        {
            WriteJson(outputFolder, $"data/topics/{topic.Id}.json", ApiResponses.Topic(topic));
        }

        int failed = 0;
        foreach (QuestionInfo question in QuestionCatalog.Questions)
        {
            WriteJson(outputFolder, $"data/questions/{question.Id}.json", ApiResponses.Question(question));
            if (QuestionRunner.GetResult(question).Errors.Count > 0)
            {
                failed++;
                Console.Error.WriteLine($"FAIL #{question.Number} {question.Id}");
            }
        }

        Console.Error.WriteLine($"Exported {QuestionCatalog.Questions.Count} questions to {Path.GetFullPath(outputFolder)} ({failed} failed)");
        return failed == 0 ? 0 : 1;
    }

    static void WriteJson(string outputFolder, string relativePath, object data)
    {
        string path = Path.Combine(outputFolder, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, JsonSerializer.Serialize(data, jsonOptions));
    }

    static void CopyFolder(string source, string destination)
    {
        foreach (string file in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
        {
            string target = Path.Combine(destination, Path.GetRelativePath(source, file));
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.Copy(file, target);
        }
    }
}
