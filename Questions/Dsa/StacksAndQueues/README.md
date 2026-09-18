# Stacks & Queues

A stack is last-in, first-out, like a pile of plates. A queue is first-in, first-out, like a line at a shop. Both are simple, and they solve a surprising number of problems.

## Stack (LIFO)

You add and remove only at the **top**.

```csharp
var stack = new Stack<int>();
stack.Push(1);
stack.Push(2);
int top = stack.Peek();   // 2 (look, don't remove)
int removed = stack.Pop(); // 2
```

Use a stack for **undo**, matching brackets, "the most recent unmatched thing", and turning recursion into a loop.

## Queue (FIFO)

You add at the **back** and remove from the **front**.

```csharp
var queue = new Queue<string>();
queue.Enqueue("Ann");
queue.Enqueue("Bob");
string next = queue.Dequeue(); // "Ann"
```

Use a queue for **processing in arrival order**, breadth-first search, and buffers between a producer and a consumer.

## Cost

| Operation | Stack | Queue |
|---|---|---|
| Add | `Push`: `O(1)` | `Enqueue`: `O(1)` |
| Remove | `Pop`: `O(1)` | `Dequeue`: `O(1)` |
| Look at next | `Peek`: `O(1)` | `Peek`: `O(1)` |

## Monotonic stack

A **monotonic stack** keeps its items always increasing, or always decreasing. When a new item breaks the order, you pop items, and each pop answers a question like "what is the next greater element?". Every item is pushed and popped at most once, so a problem that looks like `O(n²)` becomes `O(n)`.

## Deque

A **deque** (double-ended queue) can add and remove at both ends. In C# you can use `LinkedList<T>` for that (`AddFirst`, `AddLast`, `RemoveFirst`, `RemoveLast`).
