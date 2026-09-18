namespace CSharpCodingQuestions.Questions.Dsa.LinkedLists;

[Question(Order = 2, Title = "Reverse a Linked List", Level = Easy, Problem = """
    Reverse the list and return the new head. `1 → 2 → 3 → 4 → 5` becomes `5 → 4 → 3 → 2 → 1`.
    """)]
public static class ReverseLinkedList
{
    [Approach(Name = "Stack of Values", Time = "O(n)", Space = "O(n)", Idea = """
        Push every value onto a stack, then walk the list again and pop the values back in.
        A stack gives items back in reverse order. It needs extra memory for all the values.
        """)]
    public static ListNode? ReverseWithStack(ListNode? head)
    {
        var values = new Stack<int>();
        for (ListNode? node = head; node != null; node = node.Next)
        {
            values.Push(node.Value);
        }
        for (ListNode? node = head; node != null; node = node.Next)
        {
            node.Value = values.Pop();
        }
        return head;
    }

    [Approach(Name = "Recursion", Time = "O(n)", Space = "O(n)", Idea = """
        Reverse everything **after** the head first (recursion). The old second node is now the last node of that reversed part,
        so attach the head behind it: `head.Next.Next = head`, then cut the head's old link.
        Each call waits on the call stack, so it uses `O(n)` memory.
        """)]
    public static ListNode? ReverseRecursive(ListNode? head)
    {
        if (head == null || head.Next == null)
        {
            return head;
        }

        ListNode? newHead = ReverseRecursive(head.Next);
        head.Next.Next = head;
        head.Next = null;
        return newHead;
    }

    [Approach(Name = "Flip the Links in One Pass", Time = "O(n)", Space = "O(1)", Idea = """
        Walk the list once and turn each arrow around. Keep three references:

        - `previous`: the part already reversed (starts empty)
        - `current`: the node being flipped
        - `next`: saved first, so we don't lose the rest of the list

        At the end, `previous` is the new head.
        """)]
    public static ListNode? ReverseIterative(ListNode? head)
    {
        ListNode? previous = null;
        ListNode? current = head;
        while (current != null)
        {
            ListNode? next = current.Next;
            current.Next = previous;
            previous = current;
            current = next;
        }
        return previous;
    }

    public static Example[] Examples =>
    [
        new([ListNode.FromValues(1, 2, 3, 4, 5)], ListNode.FromValues(5, 4, 3, 2, 1)),
        new([ListNode.FromValues(1, 2)], ListNode.FromValues(2, 1)),
        new([null], null),
    ];
}
