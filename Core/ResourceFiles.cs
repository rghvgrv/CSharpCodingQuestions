namespace CSharpCodingQuestions.Core;

/// <summary>Reads files embedded from the Questions folder, by path like "Dsa/Arrays/TwoSum.cs".</summary>
public static class ResourceFiles
{
    static readonly Dictionary<string, string> resourceNamesByPath = typeof(ResourceFiles).Assembly
        .GetManifestResourceNames()
        .ToDictionary(name => name.Replace('\\', '/'), name => name, StringComparer.OrdinalIgnoreCase);

    public static string Read(string path)
    {
        if (!resourceNamesByPath.TryGetValue(path, out string? resourceName))
        {
            throw new FileNotFoundException($"Questions/{path} is missing.");
        }

        using Stream stream = typeof(ResourceFiles).Assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd().Replace("\r\n", "\n");
    }
}
