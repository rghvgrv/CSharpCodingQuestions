namespace CSharpCodingQuestions.Questions.Dsa.LinkedLists;

[Question(Order = 10, Title = "Build a Doubly Linked List", Level = Medium, Problem = """
    In a doubly linked list each node links to the **next** and the **previous** node.
    Build one with `AddFirst`, `AddLast`, `RemoveFirst`, `RemoveLast` and `Remove(node)`, all in O(1), and print it in both directions.
    """)]
public static class DoublyLinkedList
{
    [Approach(Name = "Sentinel Head and Tail", Time = "O(1) per operation", Space = "O(n)", Idea = """
        Two fake **sentinel** nodes sit at the start and end and are never removed.
        Every real node is always between two nodes, so inserting and removing never need `null` checks.

        - Insert after a node: link the new node between it and its old next node.
        - Remove a node: link its previous and next nodes to each other.
        """)]
    public class MyDeque
    {
        public class Node(int value)
        {
            public int Value = value;
            public Node? Previous;
            public Node? Next;
        }

        private readonly Node head = new(0);
        private readonly Node tail = new(0);

        public MyDeque()
        {
            head.Next = tail;
            tail.Previous = head;
        }

        public Node AddFirst(int value) => InsertAfter(head, value);

        public Node AddLast(int value) => InsertAfter(tail.Previous!, value);

        public int RemoveFirst() => Remove(head.Next!);

        public int RemoveLast() => Remove(tail.Previous!);

        public int Remove(Node node)
        {
            node.Previous!.Next = node.Next;
            node.Next!.Previous = node.Previous;
            return node.Value;
        }

        private Node InsertAfter(Node before, int value)
        {
            var node = new Node(value) { Previous = before, Next = before.Next };
            before.Next!.Previous = node;
            before.Next = node;
            return node;
        }

        public List<int> Forward()
        {
            var values = new List<int>();
            for (Node node = head.Next!; node != tail; node = node.Next!)
            {
                values.Add(node.Value);
            }
            return values;
        }

        public List<int> Backward()
        {
            var values = new List<int>();
            for (Node node = tail.Previous!; node != head; node = node.Previous!)
            {
                values.Add(node.Value);
            }
            return values;
        }
    }

    public static void Demo()
    {
        var list = new MyDeque();
        list.AddLast(2);
        MyDeque.Node three = list.AddLast(3);
        list.AddLast(4);
        list.AddFirst(1);
        Print("Forward", list.Forward(), expected: new[] { 1, 2, 3, 4 });
        Print("Backward", list.Backward(), expected: new[] { 4, 3, 2, 1 });

        list.Remove(three);
        Print("After Remove(node 3)", list.Forward(), expected: new[] { 1, 2, 4 });
        Print("RemoveFirst", list.RemoveFirst(), expected: 1);
        Print("RemoveLast", list.RemoveLast(), expected: 4);
        Print("Left over", list.Forward(), expected: new[] { 2 });
    }
}
