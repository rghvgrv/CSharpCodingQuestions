# C# Coding Questions

197 coding questions, beginner to advanced, all solved in C#:

| Section | Topics |
|---|---|
| **DSA** (123) | Basics · Arrays · Strings · Hashing · Searching & Sorting · Linked Lists · Stacks & Queues · Recursion & Backtracking · Bit Manipulation · Heaps · Greedy · Dynamic Programming · Graphs |
| **Tree** (32) | Binary Tree Basics · Binary Tree Problems · Binary Search Tree · Advanced Trees (Trie, AVL, Segment Tree, Fenwick) |
| **Parallelism** (42) | Thread Basics · Synchronization · Tasks & Async · Data Parallelism · Concurrent Collections · Classic Problems (Dining Philosophers, H2O, …) |

The web app has a React UI that works on phones and desktops. When you open a question, the server runs its C# code and shows the real output, with a ✓ or ✗ next to each expected answer. The code runs on the server, so the parallelism questions use real threads and cores.

## Run

Requires the .NET 10 SDK and Node 20+.

```bash
dotnet run
```

On the first run, this installs and builds the React UI into `wwwroot/`. Then open http://localhost:5080.

**On your phone:** connect it to the same Wi-Fi as your PC and open `http://<your-PC-IP>:5080` (find the IP with `ipconfig`). If Windows asks about the firewall, allow access on private networks.

## Verify every answer

```bash
dotnet run -- --check
```

This runs all questions and exits non-zero if any answer shows ✗, crashes, or times out.

## UI development

```bash
dotnet run                    # API on :5080
cd web && npm run dev         # hot-reload UI on :5173, proxies /api to :5080
cd web && npm run build       # rebuild wwwroot/ after UI changes
```

## Add a question

Create one file under `Questions/<Section>/<Topic>/`. The folder and namespace decide where it appears, and the number in `[Q]` decides its order (`section_topic_number`):

```csharp
namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_22, "My Question", Medium, "Problem statement shown in the UI.")]
public static class MyQuestion
{
    public static int Solve(int[] nums) => nums.Sum();

    public static void Run()
    {
        Check("Solve([1,2,3])", Solve([1, 2, 3]), 6);   // prints ✓ or ✗
    }
}
```

The file name must match the class name, because the UI shows the file's source. Shared helpers (`Check`, `Fmt`, `ListNode`, `TreeNode`) are in `Lib.cs`.

## Layout

```
Program.cs        API: /api/questions, /api/questions/{id}, POST /api/questions/{id}/run
Lib.cs            question discovery, runner (per-request Console capture), helpers
Questions/        one .cs file per question
web/              React (Vite) UI → builds into wwwroot/
```
