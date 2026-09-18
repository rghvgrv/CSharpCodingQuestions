namespace CSharpCodingQuestions.Questions.Tree.AdvancedTrees;

[Question(Order = 2, Title = "Range Sums With Updates (Segment & Fenwick Trees)", Level = Hard, Problem = """
    An array keeps changing. Support two operations, both many times:

    - `Update(index, value)`: set one item
    - `Sum(left, right)`: the sum of the items from `left` to `right` (inclusive)
    """)]
public static class RangeSumQueries
{
    [Approach(Name = "Loop Over the Range", Time = "Update O(1), Sum O(n)", Space = "O(n)", Idea = """
        Store the array as it is, and add up the range on every `Sum`. Fast updates, slow sums.
        """)]
    public class SimpleArray(int[] numbers)
    {
        private readonly int[] values = (int[])numbers.Clone();

        public void Update(int index, int value) => values[index] = value;

        public long Sum(int left, int right)
        {
            long total = 0;
            for (int i = left; i <= right; i++)
            {
                total += values[i];
            }
            return total;
        }
    }

    [Approach(Name = "Prefix Sums", Time = "Update O(n), Sum O(1)", Space = "O(n)", Idea = """
        Store `prefix[i]` = the sum of the first `i` items. Then `Sum(left, right) = prefix[right + 1] - prefix[left]`, one subtraction.
        But an update changes every prefix after it, so it has to rebuild them. Fast sums, slow updates.
        """)]
    public class PrefixSums
    {
        private readonly int[] values;
        private readonly long[] prefix;

        public PrefixSums(int[] numbers)
        {
            values = (int[])numbers.Clone();
            prefix = new long[values.Length + 1];
            Rebuild(0);
        }

        public void Update(int index, int value)
        {
            values[index] = value;
            Rebuild(index);
        }

        public long Sum(int left, int right) => prefix[right + 1] - prefix[left];

        private void Rebuild(int from)
        {
            for (int i = from; i < values.Length; i++)
            {
                prefix[i + 1] = prefix[i] + values[i];
            }
        }
    }

    [Approach(Name = "Fenwick Tree (Binary Indexed Tree)", Time = "Update O(log n), Sum O(log n)", Space = "O(n)", Idea = """
        A clever array where each slot stores the sum of a **block** of items. The block size comes from the index's lowest 1-bit: `index & -index`.

        - Prefix sum up to `i`: add slots while stripping the lowest bit (`i -= i & -i`). About `log n` slots.
        - Update: add the change to every slot that covers the index (`i += i & -i`). Also about `log n` slots.

        `Sum(left, right) = PrefixSum(right) - PrefixSum(left - 1)`. The tree is 1-indexed inside.
        """)]
    public class FenwickTree
    {
        private readonly long[] tree;
        private readonly int[] values;

        public FenwickTree(int[] numbers)
        {
            values = new int[numbers.Length];
            tree = new long[numbers.Length + 1];
            for (int i = 0; i < numbers.Length; i++)
            {
                Update(i, numbers[i]);
            }
        }

        public void Update(int index, int value)
        {
            long change = value - values[index];
            values[index] = value;
            for (int i = index + 1; i < tree.Length; i += i & -i)
            {
                tree[i] += change;
            }
        }

        public long Sum(int left, int right) => PrefixSum(right) - PrefixSum(left - 1);

        private long PrefixSum(int index)
        {
            long total = 0;
            for (int i = index + 1; i > 0; i -= i & -i)
            {
                total += tree[i];
            }
            return total;
        }
    }

    [Approach(Name = "Segment Tree", Time = "Update O(log n), Sum O(log n)", Space = "O(n)", Idea = """
        A binary tree where each node stores the sum of a segment: the root covers the whole array, its children cover each half, and so on down to single items.

        - `Sum(left, right)`: if a node's segment is fully inside the range, use its stored sum; if it's fully outside, ignore it; otherwise ask both children.
          Only about `2 log n` nodes are visited.
        - `Update`: change the leaf, then fix the sums on the path back up to the root.

        More code than a Fenwick tree, but it also handles minimum, maximum and other range questions.
        """)]
    public class SegmentTree
    {
        private readonly long[] tree;
        private readonly int size;

        public SegmentTree(int[] numbers)
        {
            size = numbers.Length;
            tree = new long[4 * size];   // 4n is always enough room
            Build(numbers, 1, 0, size - 1);
        }

        public void Update(int index, int value) => Update(1, 0, size - 1, index, value);

        public long Sum(int left, int right) => Sum(1, 0, size - 1, left, right);

        // Node i's children are 2i and 2i + 1, like a heap.
        private void Build(int[] numbers, int node, int low, int high)
        {
            if (low == high)
            {
                tree[node] = numbers[low];
                return;
            }
            int middle = (low + high) / 2;
            Build(numbers, 2 * node, low, middle);
            Build(numbers, 2 * node + 1, middle + 1, high);
            tree[node] = tree[2 * node] + tree[2 * node + 1];
        }

        private void Update(int node, int low, int high, int index, int value)
        {
            if (low == high)
            {
                tree[node] = value;
                return;
            }
            int middle = (low + high) / 2;
            if (index <= middle)
            {
                Update(2 * node, low, middle, index, value);
            }
            else
            {
                Update(2 * node + 1, middle + 1, high, index, value);
            }
            tree[node] = tree[2 * node] + tree[2 * node + 1];
        }

        private long Sum(int node, int low, int high, int left, int right)
        {
            if (right < low || high < left)
            {
                return 0;
            }
            if (left <= low && high <= right)
            {
                return tree[node];
            }
            int middle = (low + high) / 2;
            return Sum(2 * node, low, middle, left, right) + Sum(2 * node + 1, middle + 1, high, left, right);
        }
    }

    public static void Demo()
    {
        int[] numbers = [1, 3, 5, 7, 9, 11];
        var simple = new SimpleArray(numbers);
        var prefix = new PrefixSums(numbers);
        var fenwick = new FenwickTree(numbers);
        var segment = new SegmentTree(numbers);
        Console.WriteLine($"Array: {Formatter.Format(numbers)}");

        long[] Sums(int left, int right) => [simple.Sum(left, right), prefix.Sum(left, right), fenwick.Sum(left, right), segment.Sum(left, right)];

        Print("Sum(1, 3) from all four", Sums(1, 3), expected: new long[] { 15, 15, 15, 15 });
        Print("Sum(0, 5) from all four", Sums(0, 5), expected: new long[] { 36, 36, 36, 36 });

        simple.Update(1, 10);
        prefix.Update(1, 10);
        fenwick.Update(1, 10);
        segment.Update(1, 10);
        Console.WriteLine("Update(1, 10): the array is now [1, 10, 5, 7, 9, 11]");
        Print("Sum(1, 3) from all four", Sums(1, 3), expected: new long[] { 22, 22, 22, 22 });
        Print("Sum(4, 5) from all four", Sums(4, 5), expected: new long[] { 20, 20, 20, 20 });
    }
}
