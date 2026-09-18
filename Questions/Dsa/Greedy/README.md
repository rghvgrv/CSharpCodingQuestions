# Greedy Algorithms

A greedy algorithm makes the choice that looks best right now and never goes back. When it works, it's simple and fast. The hard part is knowing when it works.

## The idea

At each step, take the locally best option: the meeting that ends first, the item with the best value per kilo, the jump that reaches farthest.

## When greedy is correct

Greedy works when a locally best choice can always be part of a globally best answer. You usually prove it with an argument like: "if the best answer didn't make my choice, I could swap my choice in and it would be no worse."

## When greedy fails

Coins `{1, 3, 4}`, amount `6`:

- Greedy takes the biggest coin first: `4 + 1 + 1` → **3 coins**
- Best answer: `3 + 3` → **2 coins**

For problems like this, use **Dynamic Programming** instead.

## Typical pattern

1. **Sort** by the right key (end time, ratio, deadline…).
2. Walk through once, **taking** or **skipping** each item.

Sorting costs `O(n log n)` and the walk costs `O(n)`, so most greedy solutions are `O(n log n)`.
