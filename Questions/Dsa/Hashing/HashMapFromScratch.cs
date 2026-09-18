namespace CodingQuestions.Dsa.Hashing;

[Q(1_04_04, "Implement a HashMap from Scratch", Medium,
"Build a hash map with Put, Get and Remove without using Dictionary. Handle collisions with separate chaining and grow when it gets full.")]
public static class HashMapFromScratch
{
    public class MyHashMap<TKey, TValue> where TKey : notnull
    {
        // Each bucket is a linked list of the entries whose hash lands there (separate chaining).
        LinkedList<(TKey Key, TValue Value)>[] buckets = new LinkedList<(TKey, TValue)>[8];
        public int Count { get; private set; }

        int IndexOf(TKey key, int size) => (key.GetHashCode() & int.MaxValue) % size;

        public void Put(TKey key, TValue value)
        {
            var bucket = buckets[IndexOf(key, buckets.Length)] ??= new();
            for (var node = bucket.First; node != null; node = node.Next)
                if (node.Value.Key.Equals(key)) { node.Value = (key, value); return; }
            bucket.AddLast((key, value));
            if (++Count > buckets.Length * 3 / 4) Resize(); // load factor 0.75
        }

        public bool TryGet(TKey key, out TValue value)
        {
            foreach (var (k, v) in buckets[IndexOf(key, buckets.Length)] ?? new())
                if (k.Equals(key)) { value = v; return true; }
            value = default!;
            return false;
        }

        public bool Remove(TKey key)
        {
            var bucket = buckets[IndexOf(key, buckets.Length)];
            for (var node = bucket?.First; node != null; node = node.Next)
                if (node.Value.Key.Equals(key)) { bucket!.Remove(node); Count--; return true; }
            return false;
        }

        // Double the table and re-insert everything. Amortized O(1) per Put.
        void Resize()
        {
            var bigger = new LinkedList<(TKey, TValue)>[buckets.Length * 2];
            foreach (var bucket in buckets)
                foreach (var entry in bucket ?? new())
                    (bigger[IndexOf(entry.Key, bigger.Length)] ??= new()).AddLast(entry);
            buckets = bigger;
            Console.WriteLine($"  resized to {bigger.Length} buckets");
        }
    }

    public static void Run()
    {
        var map = new MyHashMap<string, int>();
        foreach (var (word, i) in "one two three four five six seven eight nine ten".Split(' ').Select((w, i) => (w, i + 1)))
            map.Put(word, i);

        Check("Count", map.Count, 10);
        Check("Get(\"seven\")", map.TryGet("seven", out var seven) ? seven : -1, 7);
        map.Put("seven", 77);
        Check("Get(\"seven\") after update", map.TryGet("seven", out seven) ? seven : -1, 77);
        Check("Remove(\"two\")", map.Remove("two"), true);
        Check("Get(\"two\") after remove", map.TryGet("two", out _), false);
        Check("Count", map.Count, 9);
    }
}
