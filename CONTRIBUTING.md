# Contributing

Thanks for helping! This project teaches coding problems to beginners, so the most valuable contributions are **clear explanations** and **simple, correct code**. Every contribution is welcome: a new question, a better explanation, a bug fix, or a typo.

## Ways to contribute

- **Add a question** to an existing topic, with approaches from worst to best.
- **Improve an explanation**: a topic introduction (`README.md` in a topic folder), a problem statement or an approach's idea.
- **Fix a bug**: a wrong answer, a broken page, a layout problem on phones.
- **Report a problem or suggest an idea** by opening an [issue](https://github.com/rghvgrv/CSharpCodingQuestions/issues). Please include the question number or a link, and what you expected to see.

## Set up your computer

You need the [.NET 10 SDK](https://dotnet.microsoft.com/download), [Node.js 20+](https://nodejs.org/) and Git.

```bash
# 1. Fork the repository on GitHub, then clone your fork
git clone https://github.com/<your-username>/CSharpCodingQuestions.git
cd CSharpCodingQuestions

# 2. Run it (the first run also builds the React UI)
dotnet run
```

Open http://localhost:5080. If Windows blocks the new `.exe`, run `dotnet bin/Debug/net10.0/CSharpCodingQuestions.dll` instead.

## Workflow

1. Create a branch from `master`: `git checkout -b add-rotate-string-question`
2. Make your change.
3. Run the check (see below). It must print `…/… passed`.
4. Commit, push to your fork, and open a **pull request into `master`**.
5. GitHub Actions builds the project and runs every question on your pull request. Please make sure it's green.
6. After review, the pull request is merged. **Merging into `master` deploys the site automatically** to https://csharpquestions.rghvgrv.live. There's nothing to deploy by hand.

```bash
dotnet run -- --check     # runs every approach of every question against its examples
```

## Adding a question

Create one file: `Questions/<Section>/<Topic>/<QuestionName>.cs`, for example `Questions/Dsa/Arrays/RotateString.cs`.

- The **file name must match the class name**, because the site shows the file's source.
- The **namespace must match the folder**: `CSharpCodingQuestions.Questions.Dsa.Arrays`.
- `Order` is the question's position inside its topic. Use the next free number.
- Write the approaches **from worst to best**. Everything between one `[Approach]` and the next is shown as that approach's code, so keep helper methods right after the approach that uses them.

### Template: a question with fixed inputs

```csharp
namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 22, Title = "Rotate a String", Level = Easy, Problem = """
    Return `true` if `goal` can be made by moving letters from the start of `text` to its end.
    `text = "abcde"`, `goal = "cdeab"` → `true`.
    """)]
public static class RotateString
{
    [Approach(Name = "Try Every Rotation", Time = "O(n²)", Space = "O(n)", Idea = """
        Build every rotation of `text` and compare it with `goal`.
        """)]
    public static bool CanRotateBruteForce(string text, string goal)
    {
        for (int shift = 0; shift < text.Length; shift++)
        {
            string rotated = text.Substring(shift) + text.Substring(0, shift);
            if (rotated == goal)
            {
                return true;
            }
        }
        return text.Length == 0 && goal.Length == 0;
    }

    [Approach(Name = "Search Inside text + text", Time = "O(n)", Space = "O(n)", Idea = """
        Every rotation of `text` appears inside `text + text`, so check the lengths and search there.
        """)]
    public static bool CanRotateWithDoubling(string text, string goal)
    {
        return text.Length == goal.Length && (text + text).Contains(goal);
    }

    public static Example[] Examples =>
    [
        new(["abcde", "cdeab"], true),
        new(["abcde", "abced"], false),
    ];
}
```

- `Examples`: each `new([inputs…], expectedAnswer)`. **Every** approach must return the expected answer. Add `AnyOrder: true` when the order of a list answer doesn't matter.
- The page shows the **last** (best) approach's output for each example.
- For linked lists and trees, use `ListNode.FromValues(1, 2, 3)` and `TreeNode.FromLevelOrder(1, 2, null, 3)` for inputs and expected answers.

### Template: a demo (no fixed inputs)

Parallelism questions and "build your own X" questions have no single answer to compare. Use `Demo()` instead of `Examples`, and `Print` to show results:

```csharp
public static void Demo()
{
    Print("With lock", CountWithLock(4, 100_000), expected: 400_000);   // fails the check if different
    Print("Took", $"{milliseconds} ms");                                // just shown, not checked
}
```

Keep a demo under about 2 seconds, and make it **deterministic**: every `expected:` value must come out right on every run, on any machine.

### Shared helpers

Code placed **above the first `[Approach]`** (like a fake web call used by every approach) is shown on the page as a "Shared code" block.

## Code style

The readers are beginners, so clarity beats cleverness:

- **Descriptive names**: `numbers`, `target`, `left`, `right`, `seenIndexByValue`. Single letters only for loop indexes (`i`, `j`, `k`) and well-known math (`n`).
- **Method names say what and how**: `TwoSumBruteForce`, `TwoSumHashMap`, `SortWithBuiltIn`.
- **Always use braces** `{ }`, even for one-line `if` and `for` bodies. One statement per line.
- **Prefer plain loops** over clever LINQ in approach code, unless the approach is *about* LINQ or PLINQ.
- **Keep each approach self-contained**, so its code can be copied and pasted into a project and work.
- **Explain the "why"** in `Idea`: what the approach does, and why it's faster or slower than the previous one. Use short Markdown: `code`, **bold**, numbered steps.
- **State the complexity honestly** in `Time` and `Space`, using the usual notation (`O(n)`, `O(n log n)`, `O(n²)`).
- **Threads**: never let an exception escape a `new Thread(...)`. It would crash the whole site. Use timeouts (`Monitor.TryEnter(gate, 300)`) instead of real deadlocks in demos.

## Adding a new topic

1. Create the folder `Questions/<Section>/<TopicName>/`.
2. Add a `README.md` there. The first line is `# Title` and the first paragraph is the one-line summary shown on the home page.
   Then explain the topic for beginners: what it is, when to use it, the costs, and a small C# example.
3. Add the folder to `Core/Curriculum.cs`, in study order.
4. Add at least one question and run the check.

## UI changes

The UI lives in `web/` (React + Vite).

```bash
dotnet run                  # data on http://localhost:5080
cd web && npm run dev       # hot-reload UI on http://localhost:5173
```

Please check your change at phone width (about 390 px) as well as on a desktop, and in both light and dark mode.

## Pull request checklist

- [ ] `dotnet run -- --check` passes
- [ ] New questions: approaches go from worst to best, each with `Time`, `Space` and an `Idea`
- [ ] File name = class name, namespace = folder
- [ ] Code follows the style above: clear names, braces, self-contained approaches
- [ ] UI changes checked on phone width and desktop

## Commit messages

Write short, clear messages in the imperative: `Add Rotate a String question`, `Fix wrong output in Coin Change`, `Explain Big-O table on phones`.

Thank you for contributing!
