using CodingQuestions;

Console.SetOut(new RoutedConsole(Console.Out));

// `dotnet run -- --check` runs every question and fails if any output has a ✗, a crash or a timeout.
if (args.Contains("--check"))
{
    var bad = 0;
    foreach (var q in Questions.All)
    {
        var (output, ms) = Questions.Run(q);
        var ok = !output.Contains('✗') && !output.Contains("💥") && !output.Contains("⏱");
        if (!ok) { bad++; Console.Error.WriteLine($"FAIL #{q.No} {q.Id}\n{output}"); }
        else Console.Error.WriteLine($"ok   #{q.No} {q.Id} ({ms:0} ms)");
    }
    Console.Error.WriteLine($"{Questions.All.Length - bad}/{Questions.All.Length} passed");
    return bad == 0 ? 0 : 1;
}

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/questions", () => Questions.All.Select(q => new
{
    q.No, q.Id, q.Section, q.Topic, q.Title, Level = q.Level.ToString()
}));

app.MapGet("/api/questions/{id}", (string id) =>
    Questions.ById.TryGetValue(id, out var q)
        ? Results.Ok(new { q.No, q.Id, q.Section, q.Topic, q.Title, Level = q.Level.ToString(), q.Text, Source = Questions.ReadSource(q.Id) })
        : Results.NotFound());

app.MapPost("/api/questions/{id}/run", (string id) =>
{
    if (!Questions.ById.TryGetValue(id, out var q)) return Results.NotFound();
    var (output, ms) = Questions.Run(q);
    return Results.Ok(new { output, ms, cores = Environment.ProcessorCount });
});

app.MapGet("/api/helpers", () => Questions.ReadSource("Lib"));

app.MapFallbackToFile("index.html");
app.Run();
return 0;
