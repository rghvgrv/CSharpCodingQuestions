namespace CodingQuestions.Dsa.LinkedLists;

[Q(1_06_07, "Palindrome Linked List", Easy,
"Check whether a linked list reads the same forwards and backwards, in O(n) time and O(1) space.")]
public static class PalindromeLinkedList
{
    // Find the middle, reverse the second half, compare both halves node by node.
    public static bool Solve(ListNode? head)
    {
        ListNode? slow = head, fast = head;
        while (fast?.Next != null) { slow = slow!.Next; fast = fast.Next.Next; }

        ListNode? prev = null;
        while (slow != null) { var next = slow.Next; slow.Next = prev; prev = slow; slow = next; }

        for (ListNode? l = head, r = prev; r != null; l = l!.Next, r = r.Next)
            if (l!.Val != r.Val) return false;
        return true;
    }

    public static void Run()
    {
        Check("1→2→2→1", Solve(ListNode.From(1, 2, 2, 1)), true);
        Check("1→2→3→2→1", Solve(ListNode.From(1, 2, 3, 2, 1)), true);
        Check("1→2", Solve(ListNode.From(1, 2)), false);
    }
}
