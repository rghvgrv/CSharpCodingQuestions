namespace CodingQuestions.Tree.AdvancedTrees;

[Q(2_04_03, "Segment Tree (Range Sum + Point Update)", Hard,
"Support two operations on an array in O(log n) each: update a single element, and query the sum of a range [l, r].")]
public static class SegmentTree
{
    // Each tree node stores the sum of a segment. Root = whole array; children = its two halves.
    // Stored in an array like a heap: node i has children 2i and 2i+1. Size 4n is always enough.
    public class RangeSum
    {
        readonly int n;
        readonly long[] tree;

        public RangeSum(int[] a)
        {
            n = a.Length;
            tree = new long[4 * n];
            Build(a, 1, 0, n - 1);
        }

        void Build(int[] a, int node, int lo, int hi)
        {
            if (lo == hi) { tree[node] = a[lo]; return; }
            int mid = (lo + hi) / 2;
            Build(a, 2 * node, lo, mid);
            Build(a, 2 * node + 1, mid + 1, hi);
            tree[node] = tree[2 * node] + tree[2 * node + 1];
        }

        public void Update(int index, int value) => Update(1, 0, n - 1, index, value);

        void Update(int node, int lo, int hi, int index, int value)
        {
            if (lo == hi) { tree[node] = value; return; }
            int mid = (lo + hi) / 2;
            if (index <= mid) Update(2 * node, lo, mid, index, value);
            else Update(2 * node + 1, mid + 1, hi, index, value);
            tree[node] = tree[2 * node] + tree[2 * node + 1];
        }

        public long Query(int l, int r) => Query(1, 0, n - 1, l, r);

        long Query(int node, int lo, int hi, int l, int r)
        {
            if (r < lo || hi < l) return 0;              // no overlap
            if (l <= lo && hi <= r) return tree[node];   // full overlap
            int mid = (lo + hi) / 2;                     // partial: ask both children
            return Query(2 * node, lo, mid, l, r) + Query(2 * node + 1, mid + 1, hi, l, r);
        }
    }

    public static void Run()
    {
        var st = new RangeSum([1, 3, 5, 7, 9, 11]);
        Check("Sum(1..3)", st.Query(1, 3), 15L);
        Check("Sum(0..5)", st.Query(0, 5), 36L);
        st.Update(1, 10);
        Check("after a[1] = 10, Sum(1..3)", st.Query(1, 3), 22L);
        Check("Sum(4..4)", st.Query(4, 4), 9L);
    }
}
