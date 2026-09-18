# Hashing

A hash map remembers values by key and finds them again in constant time. It's the number one tool for turning slow `O(n²)` solutions into fast `O(n)` ones.

## How it works

A hash table turns each key into a number (its **hash**) and uses that number to pick a bucket. To find the key later, it computes the hash again and looks in that one bucket, so there's no need to check every item.

It's like a library that shelves each book by the first letter of its title: you go straight to the right shelf.

## In C#

```csharp
// Dictionary: key → value
var ages = new Dictionary<string, int>();
ages["Alice"] = 30;                          // add or update
if (ages.TryGetValue("Alice", out int age))  // safe lookup
{
    Console.WriteLine(age);
}

// HashSet: just keys, no duplicates
var seen = new HashSet<int>();
bool isNew = seen.Add(5);   // true the first time, false after that
bool has = seen.Contains(5);
```

## Cost

| Operation | Average | Worst (many collisions) |
|---|---|---|
| Add | `O(1)` | `O(n)` |
| Find / Contains | `O(1)` | `O(n)` |
| Remove | `O(1)` | `O(n)` |

## When to use it

- "Have I seen this before?" → `HashSet`.
- "How many times does each item appear?" → `Dictionary<item, count>`.
- "Where did I see this value?" → `Dictionary<value, index>`.
- Grouping items by a key, like anagrams by their sorted letters.

The price is memory: a hash map uses `O(n)` extra space.
