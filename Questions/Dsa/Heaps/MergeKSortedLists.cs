namespace CSharpCodingQuestions.Questions.Dsa.Heaps;

[Question(Order = 4, Title = "Merge K Sorted Lists", Level = Hard, Problem = """
    You get `k` sorted linked lists. Merge them into one sorted list.
    `[1 → 4 → 5, 1 → 3 → 4, 2 → 6]` → `1 → 1 → 2 → 3 → 4 → 4 → 5 → 6`.
    """)]
public static class MergeKSortedLists
{
    [Approach(Name = "Collect All Values and Sort", Time = "O(N log N)", Space = "O(N)", Idea = """
        Put every value from every list into one `List<int>`, sort it, and build a new list. `N` = total number of nodes.
        """)]
    public static ListNode? MergeBySorting(ListNode?[] lists)
    {
        var values = new List<int>();
        foreach (ListNode? head in lists)
        {
            for (ListNode? node = head; node != null; node = node.Next)
            {
                values.Add(node.Value);
            }
        }
        values.Sort();
        return ListNode.FromValues(values.ToArray());
    }

    [Approach(Name = "Min-Heap of List Fronts", Time = "O(N log k)", Space = "O(k)", Idea = """
        The next node of the answer is always one of the `k` current fronts. Keep those fronts in a min-heap:

        1. Put the head of every list into the heap.
        2. Take the smallest node out, attach it to the result, and push its `Next` node (if any) into the heap.
        3. Repeat until the heap is empty.

        The heap never holds more than `k` nodes, so each step costs `O(log k)`.
        """)]
    public static ListNode? MergeWithHeap(ListNode?[] lists)
    {
        var heap = new PriorityQueue<ListNode, int>();
        foreach (ListNode? head in lists)
        {
            if (head != null)
            {
                heap.Enqueue(head, head.Value);
            }
        }

        var dummy = new ListNode(0);
        ListNode tail = dummy;
        while (heap.Count > 0)
        {
            ListNode smallest = heap.Dequeue();
            tail.Next = smallest;
            tail = smallest;
            if (smallest.Next != null)
            {
                heap.Enqueue(smallest.Next, smallest.Next.Value);
            }
        }
        return dummy.Next;
    }

    public static Example[] Examples =>
    [
        new([new[] { ListNode.FromValues(1, 4, 5), ListNode.FromValues(1, 3, 4), ListNode.FromValues(2, 6) }], ListNode.FromValues(1, 1, 2, 3, 4, 4, 5, 6)),
        new([new ListNode?[] { null, ListNode.FromValues(3) }], ListNode.FromValues(3)),
    ];
}
