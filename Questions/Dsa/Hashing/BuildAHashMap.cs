namespace CSharpCodingQuestions.Questions.Dsa.Hashing;

[Question(Order = 4, Title = "Build Your Own Hash Map", Level = Medium, Problem = """
    Build a map from string keys to int values with `Put(key, value)`, `Get(key)` (returns -1 if missing) and `Remove(key)`,
    without using `Dictionary`. This shows what a real hash map does inside.
    """)]
public static class BuildAHashMap
{
    [Approach(Name = "List of Pairs", Time = "O(n) per operation", Space = "O(n)", Idea = """
        Keep every (key, value) pair in one list. To find a key, check the pairs one by one.
        It works, but every `Get` gets slower as the map grows.
        """)]
    public class ListMap
    {
        private readonly List<(string Key, int Value)> pairs = new();

        public void Put(string key, int value)
        {
            for (int i = 0; i < pairs.Count; i++)
            {
                if (pairs[i].Key == key)
                {
                    pairs[i] = (key, value);
                    return;
                }
            }
            pairs.Add((key, value));
        }

        public int Get(string key)
        {
            foreach (var pair in pairs)
            {
                if (pair.Key == key)
                {
                    return pair.Value;
                }
            }
            return -1;
        }

        public void Remove(string key)
        {
            pairs.RemoveAll(pair => pair.Key == key);
        }
    }

    [Approach(Name = "Buckets (Separate Chaining)", Time = "O(1) on average", Space = "O(n)", Idea = """
        Split the pairs into many small lists called **buckets**. A key's **hash code** decides its bucket,
        so `Get` only searches one short bucket instead of everything.

        - Two keys in the same bucket is a **collision**. They simply share that bucket's list.
        - When the map gets too full (more than 0.75 items per bucket), double the number of buckets and move every pair.
          That keeps buckets short, which is why the average cost stays `O(1)`.
        """)]
    public class ChainedHashMap
    {
        private List<(string Key, int Value)>[] buckets = CreateBuckets(8);
        private int count;

        public void Put(string key, int value)
        {
            var bucket = buckets[BucketIndex(key, buckets.Length)];
            for (int i = 0; i < bucket.Count; i++)
            {
                if (bucket[i].Key == key)
                {
                    bucket[i] = (key, value);
                    return;
                }
            }

            bucket.Add((key, value));
            count++;
            if (count > buckets.Length * 3 / 4)
            {
                Grow();
            }
        }

        public int Get(string key)
        {
            foreach (var pair in buckets[BucketIndex(key, buckets.Length)])
            {
                if (pair.Key == key)
                {
                    return pair.Value;
                }
            }
            return -1;
        }

        public void Remove(string key)
        {
            count -= buckets[BucketIndex(key, buckets.Length)].RemoveAll(pair => pair.Key == key);
        }

        private void Grow()
        {
            var bigger = CreateBuckets(buckets.Length * 2);
            foreach (var bucket in buckets)
            {
                foreach (var pair in bucket)
                {
                    bigger[BucketIndex(pair.Key, bigger.Length)].Add(pair);
                }
            }
            buckets = bigger;
        }

        private static int BucketIndex(string key, int bucketCount)
        {
            int hash = key.GetHashCode() & int.MaxValue;   // clear the sign bit so it's never negative
            return hash % bucketCount;
        }

        private static List<(string Key, int Value)>[] CreateBuckets(int size)
        {
            var result = new List<(string Key, int Value)>[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = new List<(string Key, int Value)>();
            }
            return result;
        }
    }

    public static void Demo()
    {
        var map = new ChainedHashMap();
        string[] fruits = ["apple", "banana", "cherry", "date", "elderberry", "fig", "grape", "honeydew", "kiwi", "lemon"];
        for (int i = 0; i < fruits.Length; i++)
        {
            map.Put(fruits[i], i + 1);
        }
        Console.WriteLine("Put 10 fruits with values 1 to 10 (the map grew from 8 to 16 buckets on the way).");

        Print("Get(\"cherry\")", map.Get("cherry"), expected: 3);
        map.Put("cherry", 30);
        Print("Get(\"cherry\") after Put(\"cherry\", 30)", map.Get("cherry"), expected: 30);
        map.Remove("banana");
        Print("Get(\"banana\") after Remove", map.Get("banana"), expected: -1);
        Print("Get(\"mango\") (never added)", map.Get("mango"), expected: -1);

        var slow = new ListMap();
        slow.Put("apple", 1);
        Print("ListMap Get(\"apple\")", slow.Get("apple"), expected: 1);
    }
}
