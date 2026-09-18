namespace CodingQuestions.Dsa.LinkedLists;

[Q(1_06_09, "Add Two Numbers", Medium,
"Two non-negative numbers are stored as linked lists with digits in reverse order (342 → 2→4→3). Return their sum in the same form.")]
public static class AddTwoNumbers
{
    // Grade-school addition from the least significant digit, carrying as we go.
    public static ListNode? Solve(ListNode? a, ListNode? b)
    {
        var dummy = new ListNode(0);
        var tail = dummy;
        int carry = 0;
        while (a != null || b != null || carry > 0)
        {
            int sum = (a?.Val ?? 0) + (b?.Val ?? 0) + carry;
            carry = sum / 10;
            tail = tail.Next = new ListNode(sum % 10);
            a = a?.Next;
            b = b?.Next;
        }
        return dummy.Next;
    }

    public static void Run()
    {
        Check("342 + 465 (2→4→3 + 5→6→4)", Solve(ListNode.From(2, 4, 3), ListNode.From(5, 6, 4))?.ToString(), "7 → 0 → 8");
        Check("9999999 + 9999", Solve(ListNode.From(9, 9, 9, 9, 9, 9, 9), ListNode.From(9, 9, 9, 9))?.ToString(), "8 → 9 → 9 → 9 → 0 → 0 → 0 → 1");
    }
}
