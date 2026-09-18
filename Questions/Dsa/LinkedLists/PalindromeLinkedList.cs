namespace CSharpCodingQuestions.Questions.Dsa.LinkedLists;

[Question(Order = 7, Title = "Palindrome Linked List", Level = Easy, Problem = """
    Return `true` if the list reads the same forwards and backwards. `1 → 2 → 2 → 1` → `true`.
    """)]
public static class PalindromeLinkedList
{
    [Approach(Name = "Copy Into a List", Time = "O(n)", Space = "O(n)", Idea = """
        Copy the values into a `List<int>`, where you can jump to any position,
        then compare from both ends with two pointers.
        """)]
    public static bool IsPalindromeWithCopy(ListNode? head)
    {
        var values = new List<int>();
        for (ListNode? node = head; node != null; node = node.Next)
        {
            values.Add(node.Value);
        }

        int left = 0;
        int right = values.Count - 1;
        while (left < right)
        {
            if (values[left] != values[right])
            {
                return false;
            }
            left++;
            right--;
        }
        return true;
    }

    [Approach(Name = "Reverse the Second Half", Time = "O(n)", Space = "O(1)", Idea = """
        1. Find the middle with slow and fast pointers.
        2. Reverse the second half in place.
        3. Walk from the head and from the reversed half at the same time, comparing values.
        """)]
    public static bool IsPalindromeInPlace(ListNode? head)
    {
        ListNode? slow = head;
        ListNode? fast = head;
        while (fast != null && fast.Next != null)
        {
            slow = slow!.Next;
            fast = fast.Next.Next;
        }

        ListNode? reversed = null;
        while (slow != null)
        {
            ListNode? next = slow.Next;
            slow.Next = reversed;
            reversed = slow;
            slow = next;
        }

        ListNode? front = head;
        while (reversed != null)
        {
            if (front!.Value != reversed.Value)
            {
                return false;
            }
            front = front.Next;
            reversed = reversed.Next;
        }
        return true;
    }

    public static Example[] Examples =>
    [
        new([ListNode.FromValues(1, 2, 2, 1)], true),
        new([ListNode.FromValues(1, 2, 3, 2, 1)], true),
        new([ListNode.FromValues(1, 2)], false),
    ];
}
