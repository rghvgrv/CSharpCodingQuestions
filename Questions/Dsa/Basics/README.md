# Basics & Big-O

Start here. Warm-up problems with loops, math and recursion, and how to tell whether code is fast or slow using Big-O notation.

## What is an algorithm?

An algorithm is a list of steps that solves a problem, like a recipe. The same problem can often be solved in several ways. Some ways finish instantly, and others take hours on the same input. That's why every question here shows several approaches, from the **worst** to the **best**.

## Big-O: how fast does it grow?

Big-O describes how the work grows as the input gets bigger. `n` is the size of the input, such as the number of items in an array.

| Big-O | Name | n = 1,000 means about… | Example |
|---|---|---|---|
| `O(1)` | Constant | 1 step | Read `numbers[5]` |
| `O(log n)` | Logarithmic | 10 steps | Binary search |
| `O(n)` | Linear | 1,000 steps | One loop over the array |
| `O(n log n)` | Linearithmic | 10,000 steps | Good sorting |
| `O(n²)` | Quadratic | 1,000,000 steps | A loop inside a loop |
| `O(2ⁿ)` | Exponential | more than atoms in the universe | Trying every subset |

**Rules of thumb**

- One loop over the input → `O(n)`.
- A loop inside a loop → `O(n²)`.
- Cutting the problem in half each step → `O(log n)`.
- Constants are ignored: `2n` and `n + 5` are both `O(n)`.

## Time vs space

- **Time complexity**: how many steps the code takes.
- **Space complexity**: how much *extra* memory it needs, such as a new array or a dictionary.

A common trade-off: use more memory, like a dictionary, to save time.

## C# you will use a lot

```csharp
int[] numbers = { 3, 1, 4 };           // array: fixed size
var list = new List<int> { 3, 1, 4 };  // list: can grow
list.Add(5);

for (int i = 0; i < numbers.Length; i++)
{
    Console.WriteLine(numbers[i]);
}

int Square(int x)
{
    return x * x;
}
```

## Recursion in one minute

A **recursive** method calls itself on a smaller version of the problem. Every recursive method needs:

1. A **base case**: the smallest input, answered directly (it stops the recursion).
2. A **recursive step**: solve a smaller piece, then build the answer from it.

```csharp
int Factorial(int n)
{
    if (n <= 1)
    {
        return 1;               // base case
    }
    return n * Factorial(n - 1); // recursive step
}
```
