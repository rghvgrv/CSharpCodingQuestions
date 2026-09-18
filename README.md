# C# Coding Questions

198 coding questions, from beginner to advanced, all solved in C# and explained for beginners.

| Section | Topics |
|---|---|
| **DSA** (124) | Basics & Big-O · Arrays · Strings · Hashing · Searching & Sorting · Linked Lists · Stacks & Queues · Recursion & Backtracking · Bit Manipulation · Heaps · Greedy · Dynamic Programming · Graphs |
| **Tree** (32) | Binary Tree Basics · Binary Tree Problems · Binary Search Tree · Advanced Trees (Trie, AVL, Segment & Fenwick trees) |
| **Parallelism** (42) | Thread Basics · Synchronization · Tasks & async/await · Data Parallelism · Concurrent Collections · Classic Problems |

It's a web app (React UI, ASP.NET Core backend) that works on phones and desktops. For every topic:

1. **Introduction**: what the topic is, when to use it, and its costs.
2. For every question: the **problem** in plain words, then **approaches from worst to best**,
   each with its time and space complexity, an explanation and the code (with a **Copy** button).
3. **Result**: the real output of the best approach, run on the server. The parallelism questions use real threads.

## Run

Requires the .NET 10 SDK and Node 20+.

```bash
dotnet run
```

On the first run, this installs and builds the React UI into `wwwroot/`. Then open http://localhost:5080.

**On your phone:** connect it to the same Wi-Fi as your PC and open `http://<your-PC-IP>:5080` (find the IP with `ipconfig`). If Windows asks about the firewall, allow access on private networks.

## Check every answer

```bash
dotnet run -- --check
```

Every approach of every question runs on every example and must return the expected answer. Demos must print their expected values.
The command exits non-zero on any wrong answer, crash or timeout.

## Project layout

```
Program.cs                    Web API + `--check`
Core/                         One type per file
  QuestionAttribute.cs        [Question(Order, Title, Level, Problem)]
  ApproachAttribute.cs        [Approach(Name, Time, Space, Idea)]
  QuestionCatalog.cs          Finds questions and topic READMEs, in learning order
  Curriculum.cs               Topic order
  ApproachSource.cs           Cuts each question file into per-approach code snippets
  QuestionRunner.cs           Runs examples/demos and captures Console output per request
  ListNode.cs, TreeNode.cs    Shared data structures used by the questions
Questions/<Section>/<Topic>/
  README.md                   The topic introduction shown before its questions
  <QuestionName>.cs           One question: its approaches, then Examples or Demo
web/                          React (Vite) UI → builds into wwwroot/
```

## Add a question

Create `Questions/<Section>/<Topic>/<ClassName>.cs`. Write the approaches **from worst to best**:
everything from one `[Approach]` to the next is shown as that approach's code.

```csharp
namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 22, Title = "Sum of an Array", Level = Easy, Problem = """
    Return the sum of all numbers in the array.
    """)]
public static class SumOfArray
{
    [Approach(Name = "Loop", Time = "O(n)", Space = "O(1)", Idea = """
        Add the numbers one by one.
        """)]
    public static int SumWithLoop(int[] numbers)
    {
        int total = 0;
        foreach (int number in numbers)
        {
            total += number;
        }
        return total;
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 2, 3 }], 6),
    ];
}
```

Questions without fixed inputs (like the parallelism ones) use `public static void Demo()` instead of `Examples`,
and `Print(label, value, expected)` to show their result.

## UI development

```bash
dotnet run                    # API on :5080
cd web && npm run dev         # hot-reload UI on :5173, proxies /api to :5080
cd web && npm run build       # rebuild wwwroot/ after UI changes
```
