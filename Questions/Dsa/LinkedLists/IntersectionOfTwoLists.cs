namespace CSharpCodingQuestions.Questions.Dsa.LinkedLists;

[Question(Order = 8, Title = "Where Two Lists Meet", Level = Easy, Problem = """
    Two lists may join at some node and share everything after it (a Y shape).
    Return the first shared **node** (shown with the rest of the list after it), or `null` if they never meet.
    """)]
public static class IntersectionOfTwoLists
{
    [Approach(Name = "Remember List A's Nodes", Time = "O(m + n)", Space = "O(m)", Idea = """
        Put every node of the first list in a `HashSet`. Then walk the second list; the first node that's in the set is where they meet.
        We compare **nodes**, not values: two different nodes can hold the same value.
        """)]
    public static ListNode? FindMeetingWithHashSet(ListNode? headA, ListNode? headB)
    {
        var nodesInA = new HashSet<ListNode>();
        for (ListNode? node = headA; node != null; node = node.Next)
        {
            nodesInA.Add(node);
        }
        for (ListNode? node = headB; node != null; node = node.Next)
        {
            if (nodesInA.Contains(node))
            {
                return node;
            }
        }
        return null;
    }

    [Approach(Name = "Two Pointers Swap Lists", Time = "O(m + n)", Space = "O(1)", Idea = """
        Walk pointer `a` through list A and then list B, and pointer `b` through list B and then list A.
        Both travel the same total distance (`lengthA + lengthB`), so they arrive at the meeting node **at the same moment**,
        or both reach `null` together if there is none.
        """)]
    public static ListNode? FindMeetingWithTwoPointers(ListNode? headA, ListNode? headB)
    {
        ListNode? a = headA;
        ListNode? b = headB;
        while (a != b)
        {
            a = a == null ? headB : a.Next;
            b = b == null ? headA : b.Next;
        }
        return a;
    }

    public static Example[] Examples
    {
        get
        {
            ListNode shared = ListNode.FromValues(8, 4, 5)!;
            ListNode listA = new(4, new(1, shared));
            ListNode listB = new(5, new(6, new(1, shared)));
            return
            [
                new([listA, listB], shared),
                new([ListNode.FromValues(1, 2), ListNode.FromValues(3)], null),
            ];
        }
    }
}
