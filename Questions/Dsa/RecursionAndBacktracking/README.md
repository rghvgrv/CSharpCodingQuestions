# Recursion & Backtracking

Recursion solves a problem by solving smaller copies of it. Backtracking is recursion that explores choices, one at a time, and undoes a choice when it leads nowhere.

## Recursion recap

Every recursive method has a **base case** that stops it and a **recursive step** that shrinks the problem.

```csharp
int SumTo(int n)
{
    if (n == 0)
    {
        return 0;              // base case
    }
    return n + SumTo(n - 1);   // smaller problem
}
```

Each call waits on the **call stack** until the smaller call returns. Very deep recursion, like 100,000 levels, can crash with a stack overflow.

## Backtracking: choose → explore → un-choose

Picture a maze. At each fork you **choose** a path, **explore** it, and if it's a dead end you **go back** and try the next path.

```csharp
void Backtrack(List<int> current, ...)
{
    if (/* current is a complete answer */)
    {
        results.Add(new List<int>(current));   // save a COPY
        return;
    }
    foreach (var choice in /* choices still allowed */)
    {
        current.Add(choice);                    // choose
        Backtrack(current, ...);                // explore
        current.RemoveAt(current.Count - 1);    // un-choose
    }
}
```

## Why it's usually exponential

With 2 choices per item and `n` items there are `2ⁿ` combinations, and `n!` orderings for permutations. Backtracking can't beat that when you must list every answer. But **pruning** (stopping early when a partial answer is already invalid) skips huge parts of the search, which makes problems like N-Queens and Sudoku fast in practice.

## Typical problems

- All subsets, permutations and combinations.
- Puzzles: N-Queens, Sudoku, word search in a grid.
- "Generate all valid …" problems, like valid parentheses.
