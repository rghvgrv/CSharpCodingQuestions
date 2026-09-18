namespace CSharpCodingQuestions.Questions.Dsa.LinkedLists;

[Question(Order = 11, Title = "LRU Cache", Level = Medium, Problem = """
    Build a cache with a fixed `capacity`. `Get(key)` returns the value or `-1`. `Put(key, value)` adds or updates a key.
    When the cache is full, adding a new key removes the **Least Recently Used** key: the one not read or written for the longest time.
    """)]
public static class LruCache
{
    [Approach(Name = "Dictionary + List of Keys", Time = "O(n) per operation", Space = "O(capacity)", Idea = """
        A dictionary holds the values. A `List<int>` holds the keys in usage order: the oldest at the front, the newest at the end.
        Each use removes the key from the list and appends it again. `List.Remove` has to search and shift items, so it's `O(n)`.
        """)]
    public class SimpleLru(int capacity)
    {
        private readonly Dictionary<int, int> values = new();
        private readonly List<int> usageOrder = new();

        public int Get(int key)
        {
            if (!values.ContainsKey(key))
            {
                return -1;
            }
            usageOrder.Remove(key);
            usageOrder.Add(key);
            return values[key];
        }

        public void Put(int key, int value)
        {
            if (values.ContainsKey(key))
            {
                usageOrder.Remove(key);
            }
            else if (values.Count == capacity)
            {
                int oldest = usageOrder[0];
                usageOrder.RemoveAt(0);
                values.Remove(oldest);
            }
            values[key] = value;
            usageOrder.Add(key);
        }
    }

    [Approach(Name = "Dictionary + Linked List", Time = "O(1) per operation", Space = "O(capacity)", Idea = """
        Keep the usage order in a **doubly linked list** (`LinkedList<T>` in C#), newest at the front.
        The dictionary maps each key to its **node** in that list, so we can jump straight to a node and move or remove it in `O(1)`.

        - `Get`: find the node, move it to the front.
        - `Put` when full: the node at the back is the least recently used; remove it from both structures.
        """)]
    public class FastLru(int capacity)
    {
        private readonly Dictionary<int, LinkedListNode<(int Key, int Value)>> nodes = new();
        private readonly LinkedList<(int Key, int Value)> usageOrder = new();

        public int Get(int key)
        {
            if (!nodes.TryGetValue(key, out var node))
            {
                return -1;
            }
            usageOrder.Remove(node);
            usageOrder.AddFirst(node);
            return node.Value.Value;
        }

        public void Put(int key, int value)
        {
            if (nodes.TryGetValue(key, out var existing))
            {
                usageOrder.Remove(existing);
            }
            else if (nodes.Count == capacity)
            {
                var leastRecent = usageOrder.Last!;
                usageOrder.RemoveLast();
                nodes.Remove(leastRecent.Value.Key);
            }
            nodes[key] = usageOrder.AddFirst((key, value));
        }
    }

    public static void Demo()
    {
        var cache = new FastLru(capacity: 2);
        cache.Put(1, 100);
        cache.Put(2, 200);
        Print("Get(1)", cache.Get(1), expected: 100);
        cache.Put(3, 300);
        Console.WriteLine("Put(3) on a full cache removes key 2, because key 1 was used more recently.");
        Print("Get(2)", cache.Get(2), expected: -1);
        cache.Put(4, 400);
        Console.WriteLine("Put(4) removes key 1.");
        Print("Get(1)", cache.Get(1), expected: -1);
        Print("Get(3)", cache.Get(3), expected: 300);
        Print("Get(4)", cache.Get(4), expected: 400);

        var simple = new SimpleLru(capacity: 2);
        simple.Put(1, 100);
        simple.Put(2, 200);
        simple.Get(1);
        simple.Put(3, 300);
        Print("SimpleLru gives the same answer, Get(2)", simple.Get(2), expected: -1);
    }
}
