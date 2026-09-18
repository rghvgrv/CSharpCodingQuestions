namespace CodingQuestions.Tree.AdvancedTrees;

[Q(2_04_02, "AVL Tree (Self-Balancing BST)", Hard,
"Implement AVL insertion: after each insert, rebalance with rotations so the heights of left and right subtrees differ by at most 1, keeping O(log n) height.")]
public static class AvlTree
{
    public class Node(int key)
    {
        public int Key = key, Height = 1;
        public Node? Left, Right;
    }

    static int H(Node? n) => n?.Height ?? 0;
    static int BalanceOf(Node n) => H(n.Left) - H(n.Right);
    static void Update(Node n) => n.Height = 1 + Math.Max(H(n.Left), H(n.Right));

    //      y                x
    //     / \              / \
    //    x   C   ──→      A   y
    //   / \                  / \
    //  A   B                B   C
    static Node RotateRight(Node y)
    {
        var x = y.Left!;
        y.Left = x.Right;
        x.Right = y;
        Update(y); Update(x);
        return x;
    }

    static Node RotateLeft(Node x)
    {
        var y = x.Right!;
        x.Right = y.Left;
        y.Left = x;
        Update(x); Update(y);
        return y;
    }

    public static Node Insert(Node? n, int key)
    {
        if (n == null) return new Node(key);
        if (key < n.Key) n.Left = Insert(n.Left, key);
        else if (key > n.Key) n.Right = Insert(n.Right, key);
        else return n;

        Update(n);
        int balance = BalanceOf(n);
        if (balance > 1 && key < n.Left!.Key) return RotateRight(n);                                // Left-Left
        if (balance < -1 && key > n.Right!.Key) return RotateLeft(n);                               // Right-Right
        if (balance > 1) { n.Left = RotateLeft(n.Left!); return RotateRight(n); }                   // Left-Right
        if (balance < -1) { n.Right = RotateRight(n.Right!); return RotateLeft(n); }                // Right-Left
        return n;
    }

    static string Level(Node? root)
    {
        var parts = new List<int>();
        var q = new Queue<Node>();
        if (root != null) q.Enqueue(root);
        while (q.Count > 0)
        {
            var n = q.Dequeue();
            parts.Add(n.Key);
            if (n.Left != null) q.Enqueue(n.Left);
            if (n.Right != null) q.Enqueue(n.Right);
        }
        return Fmt(parts);
    }

    public static void Run()
    {
        Node? root = null;
        foreach (int k in new[] { 10, 20, 30, 40, 50, 25 }) root = Insert(root, k);
        Check("Insert 10,20,30,40,50,25 → level order", Level(root), "[30, 20, 40, 10, 25, 50]");

        // Sorted inserts would make a plain BST a 1000-deep chain. AVL stays ~log₂(n).
        Node? big = null;
        for (int k = 1; k <= 1000; k++) big = Insert(big, k);
        Check("1000 sorted inserts → height", big!.Height, 10);
    }
}
