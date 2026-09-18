namespace CSharpCodingQuestions.Questions.Dsa.LinkedLists;

[Question(Order = 9, Title = "Add Two Numbers Stored in Lists", Level = Medium, Problem = """
    Each list holds a number with its digits **in reverse order**: `2 → 4 → 3` means 342.
    Return the sum as a list in the same form. `342 + 465 = 807` → `7 → 0 → 8`.
    """)]
public static class AddTwoNumbers
{
    [Approach(Name = "Add Digit by Digit With a Carry", Time = "O(max(m, n))", Space = "O(max(m, n)) for the result", Idea = """
        This is exactly how you add numbers on paper. The digits are already in the order you add them (ones first).

        At each step, add the two digits plus the `carry`. Write `sum % 10` and carry `sum / 10` to the next step.
        Keep going while either list has digits **or** a carry is left.

        Converting both lists to `int` first would break for long lists, since they would overflow.
        """)]
    public static ListNode? AddLists(ListNode? first, ListNode? second)
    {
        var dummy = new ListNode(0);
        ListNode tail = dummy;
        int carry = 0;

        while (first != null || second != null || carry > 0)
        {
            int sum = carry;
            if (first != null)
            {
                sum += first.Value;
                first = first.Next;
            }
            if (second != null)
            {
                sum += second.Value;
                second = second.Next;
            }

            tail.Next = new ListNode(sum % 10);
            tail = tail.Next;
            carry = sum / 10;
        }
        return dummy.Next;
    }

    public static Example[] Examples =>
    [
        new([ListNode.FromValues(2, 4, 3), ListNode.FromValues(5, 6, 4)], ListNode.FromValues(7, 0, 8)),
        new([ListNode.FromValues(9, 9, 9), ListNode.FromValues(1)], ListNode.FromValues(0, 0, 0, 1)),
    ];
}
