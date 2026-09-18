namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 11, Title = "Serialize and Deserialize a Tree", Level = Hard, Problem = """
    Turn a tree into a string (to save it or send it over a network) and turn that string back into the **exact same** tree.
    """)]
public static class SerializeTree
{
    [Approach(Name = "Preorder With Null Markers", Time = "O(n)", Space = "O(n)", Idea = """
        **Serialize**: write the tree in preorder (node, left, right), and write `#` for every missing child.
        Those markers make the shape unambiguous.

        **Deserialize**: read the values back in the same preorder. Each value becomes a node, then build its left subtree,
        then its right subtree. A `#` means "no node here".
        """)]
    public class TreeCodec
    {
        public string Serialize(TreeNode? root)
        {
            var parts = new List<string>();
            Write(root, parts);
            return string.Join(",", parts);
        }

        private void Write(TreeNode? node, List<string> parts)
        {
            if (node == null)
            {
                parts.Add("#");
                return;
            }
            parts.Add(node.Value.ToString());
            Write(node.Left, parts);
            Write(node.Right, parts);
        }

        public TreeNode? Deserialize(string text)
        {
            var parts = new Queue<string>(text.Split(','));
            return Read(parts);
        }

        private TreeNode? Read(Queue<string> parts)
        {
            string part = parts.Dequeue();
            if (part == "#")
            {
                return null;
            }
            var node = new TreeNode(int.Parse(part));
            node.Left = Read(parts);
            node.Right = Read(parts);
            return node;
        }
    }

    public static void Demo()
    {
        var codec = new TreeCodec();
        TreeNode? tree = TreeNode.FromLevelOrder(1, 2, 3, null, null, 4, 5);
        string text = codec.Serialize(tree);
        Print("Tree", tree!.ToString());
        Print("Serialize", text, expected: "1,2,#,#,3,4,#,#,5,#,#");
        Print("Deserialize gives back", codec.Deserialize(text)!.ToString(), expected: "[1, 2, 3, null, null, 4, 5]");
        Print("Empty tree serializes to", codec.Serialize(null), expected: "#");
    }
}
