namespace CodingQuestions.Dsa.LinkedLists;

[Q(1_06_08, "Intersection of Two Linked Lists", Easy,
"Two lists may merge at some node and share the rest. Return the first shared node, or null.")]
public static class IntersectionOfTwoLists
{
    // Pointer a walks A then B; pointer b walks B then A. Both travel lenA + lenB,
    // so they line up at the intersection (or both reach null together). Time O(m + n), Space O(1)
    public static ListNode? Solve(ListNode? headA, ListNode? headB)
    {
        ListNode? a = headA, b = headB;
        while (a != b)
        {
            a = a == null ? headB : a.Next;
            b = b == null ? headA : b.Next;
        }
        return a;
    }

    public static void Run()
    {
        var shared = ListNode.From(8, 4, 5);
        var listA = new ListNode(4, new ListNode(1, shared));
        var listB = new ListNode(5, new ListNode(6, new ListNode(1, shared)));
        Check("A = 4→1→[8→4→5], B = 5→6→1→[8→4→5]", Solve(listA, listB)?.Val, 8);
        Check("no intersection", Solve(ListNode.From(1, 2), ListNode.From(3)), null);
    }
}
