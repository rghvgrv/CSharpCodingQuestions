# Arrays

An array stores items side by side in memory, so you can jump to any position instantly. It's the most used data structure, and many other structures are built on it.

## What is an array?

Think of a row of numbered lockers. Each locker has an **index** starting at `0`. If you know the index, you open that locker directly without checking the others.

```csharp
int[] numbers = { 10, 20, 30, 40 };
Console.WriteLine(numbers[0]);      // 10: first item
Console.WriteLine(numbers[^1]);     // 40: last item
Console.WriteLine(numbers.Length);  // 4
numbers[2] = 99;                    // change an item
```

## Cost of common operations

| Operation | Cost | Why |
|---|---|---|
| Read or write `numbers[i]` | `O(1)` | Jump straight to the position |
| Search for a value | `O(n)` | May need to check every item |
| Insert or delete in the middle | `O(n)` | Items after it must shift |
| Add at the end of a `List<int>` | `O(1)` on average | The list grows its hidden array when it's full |

## Patterns you will see again and again

- **Two pointers**: one index at the start and one at the end, moving toward each other. Great for reversing and pair searching.
- **Sliding window**: a range `[left, right]` that grows and shrinks as you scan.
- **Prefix sums**: `prefix[i]` = sum of the first `i` items, so any range sum takes one subtraction.
- **Hash map**: remember what you've already seen, to avoid a second loop.
- **Sort first**: sorted data often allows a faster scan.

## Tip

When the simple solution has two nested loops (`O(n²)`), ask: *what am I searching for in the inner loop, and can I remember it instead?* The answer is usually a dictionary, two pointers or prefix sums.
