namespace CodingQuestions.Dsa.Heaps;

[Q(1_10_04, "Merge K Sorted Lists", Hard,
"Merge k sorted linked lists into one sorted list.")]
public static class MergeKSortedLists
{
    // Heap holds the current head of each list. Pop the smallest, append it, push its successor.
    // Time O(N log k) for N total nodes.
    public static ListNode? Solve(ListNode?[] lists)
    {
        var heap = new PriorityQueue<ListNode, int>();
        foreach (var head in lists)
            if (head != null) heap.Enqueue(head, head.Val);

        var dummy = new ListNode(0);
        var tail = dummy;
        while (heap.TryDequeue(out var node, out _))
        {
            tail = tail.Next = node;
            if (node.Next != null) heap.Enqueue(node.Next, node.Next.Val);
        }
        return dummy.Next;
    }

    public static void Run()
    {
        Check("[1→4→5, 1→3→4, 2→6]",
            Solve([ListNode.From(1, 4, 5), ListNode.From(1, 3, 4), ListNode.From(2, 6)])?.ToString(),
            "1 → 1 → 2 → 3 → 4 → 4 → 5 → 6");
        Check("[]", Solve([]), null);
    }
}
