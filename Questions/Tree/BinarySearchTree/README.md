# Binary Search Tree

A Binary Search Tree (BST) keeps its values ordered: everything in a node's left subtree is smaller, and everything in its right subtree is bigger. That lets search, insert and delete skip half of the tree at each step.

## The rule

```text
        8
       / \
      3   10
     / \    \
    1   6    14
```

For every node: **left < node < right**. That's true for the whole subtree, not just the direct children: `6` is in 8's left subtree, so it must be smaller than 8.

## Why it's fast

Searching for `6`: start at 8 → 6 is smaller, go left → at 3, 6 is bigger, go right → found. Each step discards a whole subtree, just like binary search.

| Operation | Balanced tree | Unbalanced (worst) |
|---|---|---|
| Search | `O(log n)` | `O(n)` |
| Insert | `O(log n)` | `O(n)` |
| Delete | `O(log n)` | `O(n)` |

**Worst case**: inserting already-sorted values (1, 2, 3, 4…) makes every node a right child, so the tree becomes a linked list. **Self-balancing** trees (AVL, red-black) fix this. See *Advanced Trees*.

## Key fact: inorder is sorted

An inorder traversal (left, node, right) of a BST visits the values **in sorted order**. Many BST questions are solved just by using that fact.

## In C#

`SortedSet<T>` and `SortedDictionary<TKey, TValue>` are balanced binary search trees (red-black trees) that are ready to use.
