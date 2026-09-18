namespace CodingQuestions.Dsa.LinkedLists;

[Q(1_06_12, "Reverse Nodes in k-Group", Hard,
"Reverse the list k nodes at a time. A leftover group with fewer than k nodes stays as is.")]
public static class ReverseKGroup
{
    // For each full group: reverse it in place, then connect the previous group's tail to the new group head.
    public static ListNode? Solve(ListNode? head, int k)
    {
        var dummy = new ListNode(0, head);
        var groupPrev = dummy;
        while (true)
        {
            var kth = groupPrev;
            for (int i = 0; i < k && kth != null; i++) kth = kth.Next;
            if (kth == null) break; // fewer than k left

            var groupNext = kth.Next;
            ListNode? prev = groupNext, cur = groupPrev.Next;
            while (cur != groupNext)
            {
                var next = cur!.Next;
                cur.Next = prev;
                prev = cur;
                cur = next;
            }
            var oldFirst = groupPrev.Next!; // now the group's last node
            groupPrev.Next = kth;
            groupPrev = oldFirst;
        }
        return dummy.Next;
    }

    public static void Run()
    {
        Check("1→2→3→4→5, k=2", Solve(ListNode.From(1, 2, 3, 4, 5), 2)?.ToString(), "2 → 1 → 4 → 3 → 5");
        Check("1→2→3→4→5, k=3", Solve(ListNode.From(1, 2, 3, 4, 5), 3)?.ToString(), "3 → 2 → 1 → 4 → 5");
        Check("1→2→3→4, k=4", Solve(ListNode.From(1, 2, 3, 4), 4)?.ToString(), "4 → 3 → 2 → 1");
    }
}
