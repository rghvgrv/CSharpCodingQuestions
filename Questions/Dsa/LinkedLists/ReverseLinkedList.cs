namespace CodingQuestions.Dsa.LinkedLists;

[Q(1_06_02, "Reverse a Linked List", Easy,
"Reverse a singly linked list iteratively and recursively.")]
public static class ReverseLinkedList
{
    // Time O(n), Space O(1): point each node back at the previous one.
    public static ListNode? Iterative(ListNode? head)
    {
        ListNode? prev = null;
        while (head != null)
        {
            var next = head.Next;
            head.Next = prev;
            prev = head;
            head = next;
        }
        return prev;
    }

    // Time O(n), Space O(n) stack: reverse the rest, then hook this node on the end.
    public static ListNode? Recursive(ListNode? head)
    {
        if (head?.Next == null) return head;
        var newHead = Recursive(head.Next);
        head.Next.Next = head;
        head.Next = null;
        return newHead;
    }

    public static void Run()
    {
        Check("Iterative(1→2→3→4→5)", Iterative(ListNode.From(1, 2, 3, 4, 5))?.ToString(), "5 → 4 → 3 → 2 → 1");
        Check("Recursive(1→2)", Recursive(ListNode.From(1, 2))?.ToString(), "2 → 1");
        Check("Iterative(empty)", Iterative(null), null);
    }
}
