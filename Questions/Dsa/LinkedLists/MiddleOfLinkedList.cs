namespace CodingQuestions.Dsa.LinkedLists;

[Q(1_06_03, "Middle of the Linked List", Easy,
"Return the middle node. If there are two middle nodes, return the second one.")]
public static class MiddleOfLinkedList
{
    // Slow/fast pointers: fast moves 2 steps for every 1 of slow, so slow is halfway when fast finishes.
    public static ListNode? Solve(ListNode? head)
    {
        ListNode? slow = head, fast = head;
        while (fast?.Next != null)
        {
            slow = slow!.Next;
            fast = fast.Next.Next;
        }
        return slow;
    }

    public static void Run()
    {
        Check("1→2→3→4→5", Solve(ListNode.From(1, 2, 3, 4, 5))!.Val, 3);
        Check("1→2→3→4→5→6", Solve(ListNode.From(1, 2, 3, 4, 5, 6))!.Val, 4);
    }
}
