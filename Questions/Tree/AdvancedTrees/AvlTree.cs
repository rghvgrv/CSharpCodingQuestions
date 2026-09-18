namespace CSharpCodingQuestions.Questions.Tree.AdvancedTrees;

[Question(Order = 3, Title = "AVL Tree (Self-Balancing BST)", Level = Hard, Problem = """
    Inserting sorted values (1, 2, 3, …) into a plain binary search tree creates a long chain, so searching becomes `O(n)`.
    Build a BST that **rebalances itself** after every insert so its height stays about `log n`.
    """)]
public static class AvlTree
{
    [Approach(Name = "Plain BST", Time = "Insert O(h), h up to n", Space = "O(n)", Idea = """
        Normal BST insert, with no balancing. Great on random input, terrible on sorted input:
        every new value becomes the right child of the last one.
        """)]
    public class PlainBst
    {
        private TreeNode? root;

        public void Insert(int value)
        {
            root = Insert(root, value);
        }

        private static TreeNode Insert(TreeNode? node, int value)
        {
            if (node == null)
            {
                return new TreeNode(value);
            }
            if (value < node.Value)
            {
                node.Left = Insert(node.Left, value);
            }
            else if (value > node.Value)
            {
                node.Right = Insert(node.Right, value);
            }
            return node;
        }

        public int Height() => Height(root);

        private static int Height(TreeNode? node) => node == null ? 0 : 1 + Math.Max(Height(node.Left), Height(node.Right));
    }

    [Approach(Name = "AVL Tree", Time = "Insert O(log n)", Space = "O(n)", Idea = """
        Every node stores its height. After inserting, walk back up. If a node's two subtrees differ in height by more than 1, fix it with a **rotation**:

        ```text
            y                 x
           / \    rotate     / \
          x   C   right →   A   y
         / \                   / \
        A   B                 B   C
        ```

        A rotation keeps the BST order (A < x < B < y < C) and shortens the heavy side. There are four unbalanced shapes:

        - left-left → rotate right
        - right-right → rotate left
        - left-right → rotate the child left, then the node right
        - right-left → rotate the child right, then the node left
        """)]
    public class AvlSearchTree
    {
        private class Node(int value)
        {
            public int Value = value;
            public int Height = 1;
            public Node? Left;
            public Node? Right;
        }

        private Node? root;

        public void Insert(int value)
        {
            root = Insert(root, value);
        }

        public int Height() => HeightOf(root);

        private static int HeightOf(Node? node) => node?.Height ?? 0;

        private static void UpdateHeight(Node node) => node.Height = 1 + Math.Max(HeightOf(node.Left), HeightOf(node.Right));

        private static Node RotateRight(Node y)
        {
            Node x = y.Left!;
            y.Left = x.Right;
            x.Right = y;
            UpdateHeight(y);
            UpdateHeight(x);
            return x;
        }

        private static Node RotateLeft(Node x)
        {
            Node y = x.Right!;
            x.Right = y.Left;
            y.Left = x;
            UpdateHeight(x);
            UpdateHeight(y);
            return y;
        }

        private static Node Insert(Node? node, int value)
        {
            if (node == null)
            {
                return new Node(value);
            }
            if (value < node.Value)
            {
                node.Left = Insert(node.Left, value);
            }
            else if (value > node.Value)
            {
                node.Right = Insert(node.Right, value);
            }
            else
            {
                return node;
            }

            UpdateHeight(node);
            int balance = HeightOf(node.Left) - HeightOf(node.Right);

            if (balance > 1 && value < node.Left!.Value)
            {
                return RotateRight(node);                    // left-left
            }
            if (balance < -1 && value > node.Right!.Value)
            {
                return RotateLeft(node);                     // right-right
            }
            if (balance > 1)
            {
                node.Left = RotateLeft(node.Left!);          // left-right
                return RotateRight(node);
            }
            if (balance < -1)
            {
                node.Right = RotateRight(node.Right!);       // right-left
                return RotateLeft(node);
            }
            return node;
        }

        public List<int> LevelOrder()
        {
            var values = new List<int>();
            var queue = new Queue<Node>();
            if (root != null)
            {
                queue.Enqueue(root);
            }
            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();
                values.Add(node.Value);
                if (node.Left != null)
                {
                    queue.Enqueue(node.Left);
                }
                if (node.Right != null)
                {
                    queue.Enqueue(node.Right);
                }
            }
            return values;
        }
    }

    public static void Demo()
    {
        var avl = new AvlSearchTree();
        foreach (int value in new[] { 10, 20, 30, 40, 50, 25 })
        {
            avl.Insert(value);
        }
        Print("AVL after inserting 10, 20, 30, 40, 50, 25 (level order)", avl.LevelOrder(), expected: new[] { 30, 20, 40, 10, 25, 50 });

        var plain = new PlainBst();
        var balanced = new AvlSearchTree();
        for (int value = 1; value <= 1000; value++)
        {
            plain.Insert(value);
            balanced.Insert(value);
        }
        Console.WriteLine("Inserted 1, 2, 3, …, 1000 in sorted order:");
        Print("  Plain BST height", plain.Height(), expected: 1000);
        Print("  AVL tree height", balanced.Height(), expected: 10);
    }
}
