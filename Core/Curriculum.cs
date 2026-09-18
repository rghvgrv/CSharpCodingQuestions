namespace CSharpCodingQuestions.Core;

/// <summary>The learning path: sections and topics in the order they should be studied.</summary>
public static class Curriculum
{
    public static readonly IReadOnlyDictionary<string, string> SectionTitles = new Dictionary<string, string>
    {
        ["Dsa"] = "DSA",
        ["Tree"] = "Tree",
        ["Parallelism"] = "Parallelism",
    };

    /// <summary>Topic folders under Questions/, in study order. Each folder has a README.md that explains the topic.</summary>
    public static readonly string[] Topics =
    [
        "Dsa/Basics",
        "Dsa/Arrays",
        "Dsa/Strings",
        "Dsa/Hashing",
        "Dsa/SearchingAndSorting",
        "Dsa/LinkedLists",
        "Dsa/StacksAndQueues",
        "Dsa/RecursionAndBacktracking",
        "Dsa/BitManipulation",
        "Dsa/Heaps",
        "Dsa/Greedy",
        "Dsa/DynamicProgramming",
        "Dsa/Graphs",
        "Tree/BinaryTreeBasics",
        "Tree/BinaryTreeProblems",
        "Tree/BinarySearchTree",
        "Tree/AdvancedTrees",
        "Parallelism/ThreadBasics",
        "Parallelism/Synchronization",
        "Parallelism/TasksAndAsync",
        "Parallelism/DataParallelism",
        "Parallelism/ConcurrentCollections",
        "Parallelism/ClassicProblems",
    ];
}
