namespace CodingQuestions.Dsa.LinkedLists;

[Q(1_06_01, "Implement a Singly Linked List", Easy,
"Build a linked list from scratch with AddFirst, AddLast, InsertAt, Remove(value), Find and Print.")]
public static class SinglyLinkedList
{
    public class MyLinkedList
    {
        class Node(int value) { public int Value = value; public Node? Next; }

        Node? head, tail;
        public int Count { get; private set; }

        public void AddFirst(int value) // O(1)
        {
            var node = new Node(value) { Next = head };
            head = node;
            tail ??= node;
            Count++;
        }

        public void AddLast(int value) // O(1) thanks to the tail pointer
        {
            var node = new Node(value);
            if (tail == null) head = tail = node;
            else tail = tail.Next = node;
            Count++;
        }

        public void InsertAt(int index, int value) // O(n)
        {
            if (index < 0 || index > Count) throw new ArgumentOutOfRangeException(nameof(index));
            if (index == 0) { AddFirst(value); return; }
            if (index == Count) { AddLast(value); return; }
            var prev = head!;
            for (int i = 0; i < index - 1; i++) prev = prev.Next!;
            prev.Next = new Node(value) { Next = prev.Next };
            Count++;
        }

        public bool Remove(int value) // O(n); a dummy node avoids a special case for the head
        {
            var dummy = new Node(0) { Next = head };
            for (var prev = dummy; prev.Next != null; prev = prev.Next)
            {
                if (prev.Next.Value != value) continue;
                if (prev.Next == tail) tail = prev == dummy ? null : prev;
                prev.Next = prev.Next.Next;
                head = dummy.Next;
                Count--;
                return true;
            }
            return false;
        }

        public int Find(int value)
        {
            int i = 0;
            for (var n = head; n != null; n = n.Next, i++)
                if (n.Value == value) return i;
            return -1;
        }

        public override string ToString()
        {
            var parts = new List<int>();
            for (var n = head; n != null; n = n.Next) parts.Add(n.Value);
            return parts.Count == 0 ? "(empty)" : string.Join(" → ", parts) + " → null";
        }
    }

    public static void Run()
    {
        var list = new MyLinkedList();
        list.AddLast(2); list.AddLast(3); list.AddFirst(1); list.AddLast(5);
        Check("AddLast 2,3 · AddFirst 1 · AddLast 5", list.ToString(), "1 → 2 → 3 → 5 → null");
        list.InsertAt(3, 4);
        Check("InsertAt(3, 4)", list.ToString(), "1 → 2 → 3 → 4 → 5 → null");
        Check("Find(4)", list.Find(4), 3);
        list.Remove(1); list.Remove(5);
        Check("Remove(1), Remove(5)", list.ToString(), "2 → 3 → 4 → null");
        list.AddLast(6);
        Check("AddLast(6) after removing the tail", list.ToString(), "2 → 3 → 4 → 6 → null");
        Check("Count", list.Count, 4);
    }
}
