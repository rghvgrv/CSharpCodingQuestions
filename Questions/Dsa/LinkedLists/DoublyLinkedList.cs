namespace CodingQuestions.Dsa.LinkedLists;

[Q(1_06_10, "Implement a Doubly Linked List", Medium,
"Build a doubly linked list with O(1) add/remove at both ends and O(1) removal of a known node. Print it forwards and backwards.")]
public static class DoublyLinkedList
{
    public class Node(int value) { public int Value = value; public Node? Prev, Next; }

    public class MyDeque
    {
        // Sentinel head and tail: no null checks when linking or unlinking.
        readonly Node head = new(0), tail = new(0);
        public MyDeque() { head.Next = tail; tail.Prev = head; }

        public Node AddFirst(int v) => InsertAfter(head, v);
        public Node AddLast(int v) => InsertAfter(tail.Prev!, v);

        Node InsertAfter(Node prev, int v)
        {
            var node = new Node(v) { Prev = prev, Next = prev.Next };
            prev.Next!.Prev = node;
            prev.Next = node;
            return node;
        }

        public void Remove(Node node)
        {
            node.Prev!.Next = node.Next;
            node.Next!.Prev = node.Prev;
        }

        public int RemoveFirst() { var n = head.Next!; Remove(n); return n.Value; }
        public int RemoveLast() { var n = tail.Prev!; Remove(n); return n.Value; }

        public string Forward() { var s = new List<int>(); for (var n = head.Next; n != tail; n = n!.Next) s.Add(n!.Value); return Fmt(s); }
        public string Backward() { var s = new List<int>(); for (var n = tail.Prev; n != head; n = n!.Prev) s.Add(n!.Value); return Fmt(s); }
    }

    public static void Run()
    {
        var list = new MyDeque();
        list.AddLast(2);
        var three = list.AddLast(3);
        list.AddLast(4);
        list.AddFirst(1);
        Check("Forward", list.Forward(), "[1, 2, 3, 4]");
        Check("Backward", list.Backward(), "[4, 3, 2, 1]");
        list.Remove(three);
        Check("Remove(node 3) in O(1)", list.Forward(), "[1, 2, 4]");
        Check("RemoveFirst", list.RemoveFirst(), 1);
        Check("RemoveLast", list.RemoveLast(), 4);
        Check("Left", list.Forward(), "[2]");
    }
}
