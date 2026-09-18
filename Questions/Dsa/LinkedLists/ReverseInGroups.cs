namespace CSharpCodingQuestions.Questions.Dsa.LinkedLists;

[Question(Order = 12, Title = "Reverse Nodes in Groups of K", Level = Hard, Problem = """
    Reverse the list `k` nodes at a time. A final group with fewer than `k` nodes stays as it is.
    `1 → 2 → 3 → 4 → 5`, `k = 2` → `2 → 1 → 4 → 3 → 5`.
    """)]
public static class ReverseInGroups
{
    [Approach(Name = "Reverse the Values in an Array", Time = "O(n)", Space = "O(n)", Idea = """
        Copy the values into an array, reverse every complete block of `k` values, and write them back into the nodes.
        Easy to get right, but it uses extra memory and moves values instead of nodes.
        """)]
    public static ListNode? ReverseGroupsWithArray(ListNode? head, int k)
    {
        var values = new List<int>();
        for (ListNode? node = head; node != null; node = node.Next)
        {
            values.Add(node.Value);
        }

        for (int start = 0; start + k <= values.Count; start += k)
        {
            values.Reverse(start, k);
        }

        int index = 0;
        for (ListNode? node = head; node != null; node = node.Next)
        {
            node.Value = values[index];
            index++;
        }
        return head;
    }

    [Approach(Name = "Reverse Links Group by Group", Time = "O(n)", Space = "O(1)", Idea = """
        Work on the real links. For each group:

        1. Check that `k` nodes are left. If not, stop.
        2. Reverse the links inside the group, the same way as "Reverse a Linked List".
        3. Reconnect: the node before the group now points to the group's new first node,
           and the group's old first node (now last) points to what came after the group.
        """)]
    public static ListNode? ReverseGroupsInPlace(ListNode? head, int k)
    {
        var dummy = new ListNode(0, head);
        ListNode beforeGroup = dummy;

        while (true)
        {
            ListNode? groupEnd = beforeGroup;
            for (int i = 0; i < k && groupEnd != null; i++)
            {
                groupEnd = groupEnd.Next;
            }
            if (groupEnd == null)
            {
                break;
            }

            ListNode groupStart = beforeGroup.Next!;
            ListNode? afterGroup = groupEnd.Next;

            ListNode? previous = afterGroup;
            ListNode? current = groupStart;
            while (current != afterGroup)
            {
                ListNode? next = current!.Next;
                current.Next = previous;
                previous = current;
                current = next;
            }

            beforeGroup.Next = groupEnd;
            beforeGroup = groupStart;
        }
        return dummy.Next;
    }

    public static Example[] Examples =>
    [
        new([ListNode.FromValues(1, 2, 3, 4, 5), 2], ListNode.FromValues(2, 1, 4, 3, 5)),
        new([ListNode.FromValues(1, 2, 3, 4, 5), 3], ListNode.FromValues(3, 2, 1, 4, 5)),
    ];
}
