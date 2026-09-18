namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_20, "Rotate Image (Matrix 90°)", Medium,
"Rotate an n × n matrix 90 degrees clockwise, in place.")]
public static class RotateImage
{
    // Clockwise rotation = transpose (swap across the diagonal) + reverse each row. Time O(n²), Space O(1)
    public static int[][] Solve(int[][] m)
    {
        int n = m.Length;
        for (int i = 0; i < n; i++)
            for (int j = i + 1; j < n; j++)
                (m[i][j], m[j][i]) = (m[j][i], m[i][j]);
        foreach (var row in m) Array.Reverse(row);
        return m;
    }

    public static void Run()
    {
        Check("[[1,2,3],[4,5,6],[7,8,9]]", Solve([[1, 2, 3], [4, 5, 6], [7, 8, 9]]), [[7, 4, 1], [8, 5, 2], [9, 6, 3]]);
        Check("[[1,2],[3,4]]", Solve([[1, 2], [3, 4]]), [[3, 1], [4, 2]]);
    }
}
