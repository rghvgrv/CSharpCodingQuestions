namespace CSharpCodingQuestions.Questions.Dsa.LinkedLists;

[Question(Order = 6, Title = "Remove the N-th Node From the End", Level = Medium, Problem = """
    Remove the `n`-th node counting from the end, and return the head.
    `1 → 2 → 3 → 4 → 5`, `n = 2` → `1 → 2 → 3 → 5`.
    """)]
public static class RemoveNthFromEnd
{
    [Approach(Name = "Count the Length First", Time = "O(n)", Space = "O(1)", Idea = """
        1. Count the nodes: `length`.
        2. The node to remove is number `length - n` from the start (counting from 0). Walk to the node before it and skip over it.

        A dummy node in front handles removing the head.
        """)]
    public static ListNode? RemoveByCounting(ListNode? head, int n)
    {
        int length = 0;
        for (ListNode? node = head; node != null; node = node.Next)
        {
            length++;
        }

        var dummy = new ListNode(0, head);
        ListNode before = dummy;
        for (int i = 0; i < length - n; i++)
        {
            before = before.Next!;
        }
        before.Next = before.Next!.Next;
        return dummy.Next;
    }

    [Approach(Name = "Two Pointers, One Pass", Time = "O(n)", Space = "O(1)", Idea = """
        Move a `fast` pointer `n` steps ahead. Then move `fast` and `slow` together.
        When `fast` is on the last node, `slow` is right before the node to remove, because the gap between them is `n`.
        """)]
    public static ListNode? RemoveWithTwoPointers(ListNode? head, int n)
    {
        var dummy = new ListNode(0, head);
        ListNode fast = dummy;
        ListNode slow = dummy;

        for (int i = 0; i < n; i++)
        {
            fast = fast.Next!;
        }
        while (fast.Next != null)
        {
            fast = fast.Next;
            slow = slow.Next!;
        }

        slow.Next = slow.Next!.Next;
        return dummy.Next;
    }

    public static Example[] Examples =>
    [
        new([ListNode.FromValues(1, 2, 3, 4, 5), 2], ListNode.FromValues(1, 2, 3, 5)),
        new([ListNode.FromValues(1, 2), 2], ListNode.FromValues(2)),
        new([ListNode.FromValues(1), 1], null),
    ];
}
