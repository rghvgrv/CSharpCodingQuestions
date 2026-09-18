namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_11, "Serialize & Deserialize a Binary Tree", Hard,
"Convert a tree to a string and back, so the rebuilt tree has exactly the same shape and values.")]
public static class SerializeDeserialize
{
    // Preorder with "#" for null children. The null markers make the shape unambiguous.
    public static string Serialize(TreeNode? root)
    {
        var parts = new List<string>();
        void Walk(TreeNode? n)
        {
            if (n == null) { parts.Add("#"); return; }
            parts.Add(n.Val.ToString());
            Walk(n.Left);
            Walk(n.Right);
        }
        Walk(root);
        return string.Join(",", parts);
    }

    // Read tokens in the same preorder: value → build left → build right.
    public static TreeNode? Deserialize(string data)
    {
        var tokens = new Queue<string>(data.Split(','));
        TreeNode? Build()
        {
            var t = tokens.Dequeue();
            if (t == "#") return null;
            var n = new TreeNode(int.Parse(t));
            n.Left = Build();
            n.Right = Build();
            return n;
        }
        return Build();
    }

    public static void Run()
    {
        var root = TreeNode.From(1, 2, 3, null, null, 4, 5);
        string s = Serialize(root);
        Check("Serialize([1,2,3,null,null,4,5])", s, "1,2,#,#,3,4,#,#,5,#,#");
        Check("Deserialize → same tree", Deserialize(s)?.ToString(), root!.ToString());
        Check("Round trip empty", Deserialize(Serialize(null)), null);
    }
}
