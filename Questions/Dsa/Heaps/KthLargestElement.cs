namespace CodingQuestions.Dsa.Heaps;

[Q(1_10_02, "Kth Largest Element", Medium,
"Find the kth largest element in an unsorted array. Solve it with a size-k min-heap, then with QuickSelect.")]
public static class KthLargestElement
{
    // Keep only the k largest seen so far in a min-heap; its top is the kth largest. Time O(n log k)
    public static int WithHeap(int[] nums, int k)
    {
        var heap = new PriorityQueue<int, int>();
        foreach (int x in nums)
        {
            heap.Enqueue(x, x);
            if (heap.Count > k) heap.Dequeue();
        }
        return heap.Peek();
    }

    // QuickSelect: partition like quicksort but only recurse into the side holding the answer. Average O(n)
    public static int QuickSelect(int[] nums, int k)
    {
        int target = nums.Length - k, lo = 0, hi = nums.Length - 1; // kth largest = index n-k in sorted order
        while (true)
        {
            int r = Random.Shared.Next(lo, hi + 1);
            (nums[r], nums[hi]) = (nums[hi], nums[r]);
            int store = lo;
            for (int i = lo; i < hi; i++)
                if (nums[i] < nums[hi]) (nums[i], nums[store]) = (nums[store++], nums[i]);
            (nums[store], nums[hi]) = (nums[hi], nums[store]);

            if (store == target) return nums[store];
            if (store < target) lo = store + 1; else hi = store - 1;
        }
    }

    public static void Run()
    {
        Check("WithHeap([3,2,1,5,6,4], k=2)", WithHeap([3, 2, 1, 5, 6, 4], 2), 5);
        Check("WithHeap([3,2,3,1,2,4,5,5,6], k=4)", WithHeap([3, 2, 3, 1, 2, 4, 5, 5, 6], 4), 4);
        Check("QuickSelect([3,2,1,5,6,4], k=2)", QuickSelect([3, 2, 1, 5, 6, 4], 2), 5);
        Check("QuickSelect([3,2,3,1,2,4,5,5,6], k=4)", QuickSelect([3, 2, 3, 1, 2, 4, 5, 5, 6], 4), 4);
    }
}
