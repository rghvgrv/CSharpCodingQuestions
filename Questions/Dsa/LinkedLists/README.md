# Linked Lists

A linked list is a chain of nodes where each node points to the next one. Adding or removing a node is cheap, but reaching the 100th node means walking past the first 99.

## The node

Every question in this topic uses this class:

```csharp
public class ListNode(int value, ListNode? next = null)
{
    public int Value { get; set; } = value;
    public ListNode? Next { get; set; } = next;
}

// 1 → 2 → 3
var head = new ListNode(1, new ListNode(2, new ListNode(3)));
```

The first node is the **head**. The last node's `Next` is `null`.

## Walking a list

```csharp
ListNode? current = head;
while (current != null)
{
    Console.WriteLine(current.Value);
    current = current.Next;   // move to the next node
}
```

## Array vs linked list

| Operation | Array | Linked list |
|---|---|---|
| Get item `i` | `O(1)` | `O(n)` |
| Insert or delete at the front | `O(n)` | `O(1)` |
| Insert or delete after a known node | `O(n)` | `O(1)` |
| Extra memory per item | none | one pointer |

## Tricks worth knowing

- **Dummy head**: a fake node before the real head removes special cases when the head changes.
- **Slow and fast pointers**: one moves 1 step and the other 2 steps. They find the middle and detect cycles.
- **Reverse in place**: turn each `Next` arrow around, one node at a time.
- **Draw it!** Sketching boxes and arrows on paper is the fastest way to get pointer code right.
