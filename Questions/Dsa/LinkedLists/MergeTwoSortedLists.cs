namespace CSharpCodingQuestions.Questions.Dsa.LinkedLists;

[Question(Order = 5, Title = "Merge Two Sorted Lists", Level = Easy, Problem = """
    Both lists are sorted. Combine them into one sorted list.
    `1 → 2 → 4` and `1 → 3 → 4` → `1 → 1 → 2 → 3 → 4 → 4`.
    """)]
public static class MergeTwoSortedLists
{
    [Approach(Name = "Collect Values and Sort", Time = "O((m + n) log(m + n))", Space = "O(m + n)", Idea = """
        Copy all values into a `List<int>`, sort it, and build a new linked list from it.
        It ignores that both lists are already sorted.
        """)]
    public static ListNode? MergeBySorting(ListNode? first, ListNode? second)
    {
        var values = new List<int>();
        for (ListNode? node = first; node != null; node = node.Next)
        {
            values.Add(node.Value);
        }
        for (ListNode? node = second; node != null; node = node.Next)
        {
            values.Add(node.Value);
        }
        values.Sort();
        return ListNode.FromValues(values.ToArray());
    }

    [Approach(Name = "Splice the Nodes", Time = "O(m + n)", Space = "O(1)", Idea = """
        Compare the fronts of both lists and link the smaller node onto the result, then move forward in that list.
        When one list runs out, attach the rest of the other.

        A **dummy** starting node means we never need a special case for "the result is still empty".
        """)]
    public static ListNode? MergeBySplicing(ListNode? first, ListNode? second)
    {
        var dummy = new ListNode(0);
        ListNode tail = dummy;

        while (first != null && second != null)
        {
            if (first.Value <= second.Value)
            {
                tail.Next = first;
                first = first.Next;
            }
            else
            {
                tail.Next = second;
                second = second.Next;
            }
            tail = tail.Next;
        }

        tail.Next = first ?? second;
        return dummy.Next;
    }

    public static Example[] Examples =>
    [
        new([ListNode.FromValues(1, 2, 4), ListNode.FromValues(1, 3, 4)], ListNode.FromValues(1, 1, 2, 3, 4, 4)),
        new([null, ListNode.FromValues(0)], ListNode.FromValues(0)),
    ];
}
