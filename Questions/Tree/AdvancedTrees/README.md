# Advanced Trees

Special trees built for one job each: the Trie for words and prefixes, the AVL tree to stay balanced, and Segment and Fenwick trees for fast range queries.

## Trie (prefix tree)

Each node is a letter position. Words that share a beginning share a path: "car", "card" and "care" all go through c → a → r. Finding a word or a prefix takes `O(length of the word)`, no matter how many words are stored. Autocomplete and spell checkers use tries.

## AVL tree

A binary search tree that **rebalances itself** after every insert, using small **rotations**, so its height stays about `log₂(n)`. Search stays `O(log n)` even when the values arrive sorted, which would turn a normal BST into a slow chain.

## Range queries: Segment tree and Fenwick tree

Problem: an array changes over time, and you keep asking "what is the sum from index `l` to `r`?".

| Method | Update one value | Range sum |
|---|---|---|
| Loop over the range | `O(1)` | `O(n)` |
| Prefix sums | `O(n)` (rebuild) | `O(1)` |
| Fenwick tree | `O(log n)` | `O(log n)` |
| Segment tree | `O(log n)` | `O(log n)` |

- A **segment tree** stores the sum of each half, each quarter, and so on. Any range is covered by about `log n` stored pieces.
- A **Fenwick tree** (Binary Indexed Tree) does the same job in a small array, using a clever bit trick: `index & -index`.

Both are fast when you have **many updates and many queries** mixed together.
