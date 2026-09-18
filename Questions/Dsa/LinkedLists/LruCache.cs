namespace CodingQuestions.Dsa.LinkedLists;

[Q(1_06_11, "LRU Cache", Medium,
"Design a Least Recently Used cache with Get(key) and Put(key, value) in O(1). When full, evict the least recently used key.")]
public static class LruCache
{
    // Dictionary gives O(1) lookup; a doubly linked list keeps usage order (front = most recent).
    // .NET's LinkedList<T> is doubly linked, and a stored LinkedListNode can be removed in O(1).
    public class Lru(int capacity)
    {
        readonly Dictionary<int, LinkedListNode<(int Key, int Value)>> map = [];
        readonly LinkedList<(int Key, int Value)> order = new();

        public int Get(int key)
        {
            if (!map.TryGetValue(key, out var node)) return -1;
            order.Remove(node);
            order.AddFirst(node);
            return node.Value.Value;
        }

        public void Put(int key, int value)
        {
            if (map.TryGetValue(key, out var node)) order.Remove(node);
            else if (map.Count == capacity)
            {
                Console.WriteLine($"  evict key {order.Last!.Value.Key}");
                map.Remove(order.Last.Value.Key);
                order.RemoveLast();
            }
            map[key] = order.AddFirst((key, value));
        }
    }

    public static void Run()
    {
        var cache = new Lru(2);
        cache.Put(1, 1);
        cache.Put(2, 2);
        Check("Get(1)", cache.Get(1), 1);
        cache.Put(3, 3); // evicts 2 (1 was used more recently)
        Check("Get(2)", cache.Get(2), -1);
        cache.Put(4, 4); // evicts 1
        Check("Get(1)", cache.Get(1), -1);
        Check("Get(3)", cache.Get(3), 3);
        Check("Get(4)", cache.Get(4), 4);
    }
}
