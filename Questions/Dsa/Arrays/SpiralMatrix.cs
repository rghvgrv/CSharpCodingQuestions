namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_19, "Spiral Matrix", Medium,
"Return all elements of an m × n matrix in spiral order (right, down, left, up, repeat).")]
public static class SpiralMatrix
{
    // Shrink four boundaries after walking each side. Time O(m·n)
    public static List<int> Solve(int[][] m)
    {
        var result = new List<int>();
        int top = 0, bottom = m.Length - 1, left = 0, right = m[0].Length - 1;
        while (top <= bottom && left <= right)
        {
            for (int c = left; c <= right; c++) result.Add(m[top][c]);
            top++;
            for (int r = top; r <= bottom; r++) result.Add(m[r][right]);
            right--;
            if (top <= bottom)
                for (int c = right; c >= left; c--) result.Add(m[bottom][c]);
            bottom--;
            if (left <= right)
                for (int r = bottom; r >= top; r--) result.Add(m[r][left]);
            left++;
        }
        return result;
    }

    public static void Run()
    {
        Check("3×3", Solve([[1, 2, 3], [4, 5, 6], [7, 8, 9]]), [1, 2, 3, 6, 9, 8, 7, 4, 5]);
        Check("3×4", Solve([[1, 2, 3, 4], [5, 6, 7, 8], [9, 10, 11, 12]]), [1, 2, 3, 4, 8, 12, 11, 10, 9, 5, 6, 7]);
    }
}
