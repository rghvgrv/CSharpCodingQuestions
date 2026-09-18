namespace CSharpCodingQuestions.Questions.Dsa.LinkedLists;

[Question(Order = 3, Title = "Middle of a Linked List", Level = Easy, Problem = """
    Return the middle node's value. With two middle nodes (an even count), return the second one.
    `1 → 2 → 3 → 4 → 5` → `3`. `1 → 2 → 3 → 4 → 5 → 6` → `4`.
    """)]
public static class MiddleOfLinkedList
{
    [Approach(Name = "Count, Then Walk", Time = "O(n)", Space = "O(1)", Idea = """
        1. Walk the whole list to count the nodes.
        2. Walk again, `count / 2` steps from the head.

        Two passes over the list.
        """)]
    public static int MiddleByCounting(ListNode head)
    {
        int count = 0;
        for (ListNode? node = head; node != null; node = node.Next)
        {
            count++;
        }

        ListNode middle = head;
        for (int i = 0; i < count / 2; i++)
        {
            middle = middle.Next!;
        }
        return middle.Value;
    }

    [Approach(Name = "Slow and Fast Pointers", Time = "O(n)", Space = "O(1)", Idea = """
        Move two pointers together: `slow` takes 1 step and `fast` takes 2 steps.
        When `fast` reaches the end, `slow` has gone exactly half as far, so it's in the middle. One pass.
        """)]
    public static int MiddleWithTwoPointers(ListNode head)
    {
        ListNode slow = head;
        ListNode? fast = head;
        while (fast != null && fast.Next != null)
        {
            slow = slow.Next!;
            fast = fast.Next.Next;
        }
        return slow.Value;
    }

    public static Example[] Examples =>
    [
        new([ListNode.FromValues(1, 2, 3, 4, 5)], 3),
        new([ListNode.FromValues(1, 2, 3, 4, 5, 6)], 4),
        new([ListNode.FromValues(7)], 7),
    ];
}
