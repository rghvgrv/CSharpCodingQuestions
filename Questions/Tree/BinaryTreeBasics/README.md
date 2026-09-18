# Binary Tree Basics

A binary tree is a structure where each node has at most two children, left and right. Trees model hierarchies, and almost every tree problem is solved with a short recursive function.

## Words to know

```text
        1        ← root (the top node)
       / \
      2   3      ← 2 and 3 are children of 1
     / \
    4   5        ← 4 and 5 are leaves (no children)
```

- **Root**: the top node. **Leaf**: a node with no children.
- **Height / depth**: the number of levels. The tree above has height 3.
- **Subtree**: a node together with everything below it.

## The node

Every tree question uses this class:

```csharp
public class TreeNode(int value, TreeNode? left = null, TreeNode? right = null)
{
    public int Value { get; set; } = value;
    public TreeNode? Left { get; set; } = left;
    public TreeNode? Right { get; set; } = right;
}

var root = new TreeNode(1,
    new TreeNode(2, new TreeNode(4), new TreeNode(5)),
    new TreeNode(3));
```

Inputs and outputs are written in **level order**, top to bottom and left to right, with `null` for a missing child. The tree above is `[1, 2, 3, 4, 5]`.

## Traversals: the four ways to visit every node

| Traversal | Order | Result for the tree above |
|---|---|---|
| Preorder | node, left, right | `1 2 4 5 3` |
| Inorder | left, node, right | `4 2 5 1 3` |
| Postorder | left, right, node | `4 5 2 3 1` |
| Level order | level by level | `1 2 3 4 5` |

The first three are **depth-first** (recursion or a stack). Level order is **breadth-first** (a queue).

## The recursive pattern

Most tree answers follow the same shape: answer for the left subtree, answer for the right subtree, combine.

```csharp
int CountNodes(TreeNode? node)
{
    if (node == null)
    {
        return 0;   // an empty tree has 0 nodes
    }
    return 1 + CountNodes(node.Left) + CountNodes(node.Right);
}
```

Visiting every node once costs `O(n)` time. Recursion uses `O(h)` stack space, where `h` is the height of the tree.
