# Searching & Sorting

Finding an item and putting items in order are the two most common jobs in programming. Binary search and the classic sorting algorithms teach ideas you will use everywhere.

## Searching

- **Linear search** checks items one by one: `O(n)`. It works on any array.
- **Binary search** works only on a **sorted** array. Look at the middle item: if the target is bigger, throw away the left half; if smaller, the right half. Each step halves the work, so 1,000,000 items need only about 20 steps: `O(log n)`.

```csharp
int left = 0;
int right = numbers.Length - 1;
while (left <= right)
{
    int middle = left + (right - left) / 2;   // avoids overflow of (left + right)
    if (numbers[middle] == target) return middle;
    if (numbers[middle] < target) left = middle + 1;
    else right = middle - 1;
}
```

**Binary search on the answer.** If you can ask "is `x` big enough?" and the answer switches from *no* to *yes* only once, you can binary search `x` itself. Square root is an example.

## Sorting algorithms at a glance

| Algorithm | Best | Average | Worst | Extra space | Stable |
|---|---|---|---|---|---|
| Bubble sort | `O(n)` | `O(n²)` | `O(n²)` | `O(1)` | Yes |
| Selection sort | `O(n²)` | `O(n²)` | `O(n²)` | `O(1)` | No |
| Insertion sort | `O(n)` | `O(n²)` | `O(n²)` | `O(1)` | Yes |
| Merge sort | `O(n log n)` | `O(n log n)` | `O(n log n)` | `O(n)` | Yes |
| Quick sort | `O(n log n)` | `O(n log n)` | `O(n²)` | `O(log n)` | No |
| Heap sort | `O(n log n)` | `O(n log n)` | `O(n log n)` | `O(1)` | No |
| Counting sort | `O(n + k)` | `O(n + k)` | `O(n + k)` | `O(k)` | Yes |

A **stable** sort keeps equal items in their original order.

## In real C# code

Use the built-in sort: `Array.Sort(numbers)` or `list.Sort()`. It's a fast hybrid of quick sort, heap sort and insertion sort. Learn the algorithms here to understand the ideas (divide and conquer, partitioning, heaps), which show up in many other problems.
