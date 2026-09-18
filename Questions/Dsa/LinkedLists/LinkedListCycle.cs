namespace CSharpCodingQuestions.Questions.Dsa.LinkedLists;

[Question(Order = 4, Title = "Detect a Cycle in a Linked List", Level = Easy, Problem = """
    In a broken list, the last node may link back to an earlier node, forming a loop, so walking it never ends.
    Return `true` if the list has a cycle.
    """)]
public static class LinkedListCycle
{
    [Approach(Name = "Remember Visited Nodes", Time = "O(n)", Space = "O(n)", Idea = """
        Walk the list and put every node in a `HashSet`. If you reach a node that's already in the set, you've gone around a loop.
        If you reach the end (`null`), there's no cycle.
        """)]
    public static bool HasCycleWithHashSet(ListNode? head)
    {
        var visited = new HashSet<ListNode>();
        for (ListNode? node = head; node != null; node = node.Next)
        {
            if (!visited.Add(node))
            {
                return true;
            }
        }
        return false;
    }

    [Approach(Name = "Tortoise and Hare (Floyd)", Time = "O(n)", Space = "O(1)", Idea = """
        Send two runners: `slow` moves 1 step at a time and `fast` moves 2.

        - No cycle: `fast` reaches the end.
        - Cycle: both end up going around the loop, and `fast` gains one step on `slow` every move, so it eventually lands on it.

        No memory needed apart from two pointers.
        """)]
    public static bool HasCycleWithTwoPointers(ListNode? head)
    {
        ListNode? slow = head;
        ListNode? fast = head;
        while (fast != null && fast.Next != null)
        {
            slow = slow!.Next;
            fast = fast.Next.Next;
            if (slow == fast)
            {
                return true;
            }
        }
        return false;
    }

    public static Example[] Examples =>
    [
        new([ListNode.WithCycle([3, 2, 0, -4], cycleStart: 1)], true),
        new([ListNode.WithCycle([1, 2], cycleStart: 0)], true),
        new([ListNode.FromValues(1, 2, 3)], false),
    ];
}
