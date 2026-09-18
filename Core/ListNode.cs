namespace CSharpCodingQuestions.Core;

/// <summary>A node of a singly linked list: a value and a link to the next node.</summary>
public class ListNode(int value, ListNode? next = null)
{
    public int Value { get; set; } = value;
    public ListNode? Next { get; set; } = next;

    /// <summary>Builds 1 → 2 → 3 from (1, 2, 3). Returns null for no values.</summary>
    public static ListNode? FromValues(params int[] values)
    {
        ListNode? head = null;
        for (int i = values.Length - 1; i >= 0; i--)
        {
            head = new ListNode(values[i], head);
        }
        return head;
    }

    /// <summary>Builds a list whose last node links back to the node at <paramref name="cycleStart"/>.</summary>
    public static ListNode WithCycle(int[] values, int cycleStart)
    {
        ListNode head = FromValues(values)!;
        ListNode last = head;
        while (last.Next != null)
        {
            last = last.Next;
        }

        ListNode target = head;
        for (int i = 0; i < cycleStart; i++)
        {
            target = target.Next!;
        }
        last.Next = target;
        return head;
    }

    /// <summary>"1 → 2 → 3". A cycle is shown as "… → (back to 2)".</summary>
    public override string ToString()
    {
        var seen = new HashSet<ListNode>();
        var parts = new List<string>();
        for (ListNode? node = this; node != null; node = node.Next)
        {
            if (!seen.Add(node))
            {
                parts.Add($"(back to {node.Value})");
                break;
            }
            parts.Add(node.Value.ToString());
        }
        return string.Join(" → ", parts);
    }
}
