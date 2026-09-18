namespace CodingQuestions.Dsa.LinkedLists;

[Q(1_06_06, "Remove Nth Node From End", Medium,
"Remove the nth node from the end of the list in one pass.")]
public static class RemoveNthFromEnd
{
    // Move `fast` n steps ahead, then move both until fast hits the end: slow is right before the target.
    public static ListNode? Solve(ListNode? head, int n)
    {
        var dummy = new ListNode(0, head);
        ListNode fast = dummy, slow = dummy;
        for (int i = 0; i < n; i++) fast = fast.Next!;
        while (fast.Next != null)
        {
            fast = fast.Next;
            slow = slow.Next!;
        }
        slow.Next = slow.Next!.Next;
        return dummy.Next;
    }

    public static void Run()
    {
        Check("1→2→3→4→5, n=2", Solve(ListNode.From(1, 2, 3, 4, 5), 2)?.ToString(), "1 → 2 → 3 → 5");
        Check("1, n=1", Solve(ListNode.From(1), 1)?.ToString(), null);
        Check("1→2, n=2", Solve(ListNode.From(1, 2), 2)?.ToString(), "2");
    }
}
