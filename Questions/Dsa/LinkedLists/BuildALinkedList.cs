namespace CSharpCodingQuestions.Questions.Dsa.LinkedLists;

[Question(Order = 1, Title = "Build Your Own Linked List", Level = Easy, Problem = """
    Build a singly linked list with `AddFirst`, `AddLast`, `Remove(value)` and `Contains(value)`.
    Each node holds a value and a link to the next node.
    """)]
public static class BuildALinkedList
{
    [Approach(Name = "Head Only", Time = "AddLast O(n)", Space = "O(n)", Idea = """
        Keep only a reference to the first node (`head`).
        Adding at the front is instant, but adding at the end means walking the whole list to find the last node.
        """)]
    public class HeadOnlyList
    {
        private ListNode? head;

        public void AddFirst(int value)
        {
            head = new ListNode(value, head);
        }

        public void AddLast(int value)
        {
            var node = new ListNode(value);
            if (head == null)
            {
                head = node;
                return;
            }

            ListNode last = head;
            while (last.Next != null)
            {
                last = last.Next;
            }
            last.Next = node;
        }

        public override string ToString() => head?.ToString() ?? "(empty)";
    }

    [Approach(Name = "Head and Tail", Time = "AddFirst / AddLast O(1)", Space = "O(n)", Idea = """
        Also keep a reference to the **last** node (`tail`), so adding at the end is instant too.
        The price: every method that changes the ends must keep `tail` correct.

        - `Remove` uses a **dummy** node in front of the head, so removing the first node needs no special case.
        - `Contains` walks the list: `O(n)`.
        """)]
    public class LinkedListWithTail
    {
        private ListNode? head;
        private ListNode? tail;

        public int Count { get; private set; }

        public void AddFirst(int value)
        {
            head = new ListNode(value, head);
            if (tail == null)
            {
                tail = head;
            }
            Count++;
        }

        public void AddLast(int value)
        {
            var node = new ListNode(value);
            if (tail == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                tail.Next = node;
                tail = node;
            }
            Count++;
        }

        public bool Remove(int value)
        {
            var dummy = new ListNode(0, head);
            ListNode previous = dummy;
            while (previous.Next != null)
            {
                if (previous.Next.Value == value)
                {
                    if (previous.Next == tail)
                    {
                        tail = previous == dummy ? null : previous;
                    }
                    previous.Next = previous.Next.Next;
                    head = dummy.Next;
                    Count--;
                    return true;
                }
                previous = previous.Next;
            }
            return false;
        }

        public bool Contains(int value)
        {
            for (ListNode? node = head; node != null; node = node.Next)
            {
                if (node.Value == value)
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString() => head?.ToString() ?? "(empty)";
    }

    public static void Demo()
    {
        var list = new LinkedListWithTail();
        list.AddLast(2);
        list.AddLast(3);
        list.AddFirst(1);
        list.AddLast(4);
        Print("AddLast 2, AddLast 3, AddFirst 1, AddLast 4", list.ToString(), expected: "1 → 2 → 3 → 4");

        list.Remove(1);
        list.Remove(4);
        Print("Remove 1 (the head) and 4 (the tail)", list.ToString(), expected: "2 → 3");

        list.AddLast(5);
        Print("AddLast 5 (tail was updated correctly)", list.ToString(), expected: "2 → 3 → 5");
        Print("Contains(3)", list.Contains(3), expected: true);
        Print("Count", list.Count, expected: 3);

        var simple = new HeadOnlyList();
        simple.AddLast(1);
        simple.AddLast(2);
        Print("HeadOnlyList after AddLast 1, 2", simple.ToString(), expected: "1 → 2");
    }
}
