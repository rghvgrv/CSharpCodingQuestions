namespace CodingQuestions.Dsa.LinkedLists;

[Q(1_06_04, "Linked List Cycle (Floyd's Algorithm)", Medium,
"Detect whether a linked list has a cycle, and if so return the node where the cycle begins, using O(1) memory.")]
public static class LinkedListCycle
{
    // Tortoise & hare: if there's a loop, the fast pointer eventually laps the slow one.
    // Then restart one pointer at head; moving both 1 step at a time, they meet at the cycle start.
    public static ListNode? CycleStart(ListNode? head)
    {
        ListNode? slow = head, fast = head;
        while (fast?.Next != null)
        {
            slow = slow!.Next;
            fast = fast.Next.Next;
            if (slow == fast)
            {
                for (slow = head; slow != fast; slow = slow!.Next, fast = fast!.Next) { }
                return slow;
            }
        }
        return null;
    }

    public static void Run()
    {
        // 3 → 2 → 0 → -4 → back to 2
        var head = ListNode.From(3, 2, 0, -4)!;
        head.Next!.Next!.Next!.Next = head.Next;
        Check("3→2→0→-4→(2) has cycle", CycleStart(head) != null, true);
        Check("cycle starts at", CycleStart(head)!.Val, 2);
        Check("1→2→3 has cycle", CycleStart(ListNode.From(1, 2, 3)) != null, false);
    }
}
