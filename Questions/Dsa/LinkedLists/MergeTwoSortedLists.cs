namespace CodingQuestions.Dsa.LinkedLists;

[Q(1_06_05, "Merge Two Sorted Lists", Easy,
"Merge two sorted linked lists into one sorted list by splicing the nodes together.")]
public static class MergeTwoSortedLists
{
    // Dummy head removes the special case for the first node. Time O(m + n), Space O(1)
    public static ListNode? Solve(ListNode? a, ListNode? b)
    {
        var dummy = new ListNode(0);
        var tail = dummy;
        while (a != null && b != null)
        {
            if (a.Val <= b.Val) { tail.Next = a; a = a.Next; }
            else { tail.Next = b; b = b.Next; }
            tail = tail.Next;
        }
        tail.Next = a ?? b;
        return dummy.Next;
    }

    public static void Run()
    {
        Check("1→2→4 + 1→3→4", Solve(ListNode.From(1, 2, 4), ListNode.From(1, 3, 4))?.ToString(), "1 → 1 → 2 → 3 → 4 → 4");
        Check("empty + 0", Solve(null, ListNode.From(0))?.ToString(), "0");
    }
}
