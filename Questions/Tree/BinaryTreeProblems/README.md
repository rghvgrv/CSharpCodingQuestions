# Binary Tree Problems

Interview-style tree problems: views, paths, ancestors and building trees. Most use one recursive pass that returns something useful from each subtree.

## Pattern 1: return a value, update a best answer

Many problems need information from **both** subtrees, like heights or path sums. Write a helper that returns that information, and update a best-so-far variable while it runs. Diameter and maximum path sum work this way, in a single `O(n)` pass instead of `O(n²)`.

```csharp
int best = 0;
int Height(TreeNode? node)
{
    if (node == null) return 0;
    int left = Height(node.Left);
    int right = Height(node.Right);
    best = Math.Max(best, left + right);   // use both sides here
    return 1 + Math.Max(left, right);      // return one side upward
}
```

## Pattern 2: level by level (BFS)

Views and zigzag orders are about **levels**. Use a queue, and read `queue.Count` at the start of each level so you know how many nodes belong to it.

## Pattern 3: carry a path downward

For root-to-leaf questions, pass the current path or remaining sum **down** as a parameter. Add the node before recursing and remove it afterwards (backtracking).

## Pattern 4: columns

Give the root column `0`, a left child `column - 1` and a right child `column + 1`. Top, bottom and vertical views all group nodes by this column number.

## Cost

Visiting each node once is `O(n)`. Storing a queue or a recursion path costs `O(width)` or `O(height)` extra memory.
