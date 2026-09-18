namespace CodingQuestions.Tree.AdvancedTrees;

[Q(2_04_01, "Trie (Prefix Tree)", Medium,
"Implement a Trie with Insert, Search (whole word), StartsWith (prefix) and autocomplete (all words with a prefix).")]
public static class Trie
{
    // Each node = one character position; children keyed by the next character.
    // Operations are O(word length), independent of how many words are stored.
    public class PrefixTree
    {
        class Node { public readonly Dictionary<char, Node> Next = []; public bool IsWord; }
        readonly Node root = new();

        public void Insert(string word)
        {
            var n = root;
            foreach (char c in word)
            {
                if (!n.Next.TryGetValue(c, out var child)) n.Next[c] = child = new Node();
                n = child;
            }
            n.IsWord = true;
        }

        Node? Walk(string s)
        {
            var n = root;
            foreach (char c in s)
                if (!n.Next.TryGetValue(c, out n)) return null;
            return n;
        }

        public bool Search(string word) => Walk(word)?.IsWord == true;
        public bool StartsWith(string prefix) => Walk(prefix) != null;

        public List<string> Autocomplete(string prefix)
        {
            var result = new List<string>();
            void Collect(Node n, string sofar)
            {
                if (n.IsWord) result.Add(sofar);
                foreach (var (c, child) in n.Next.OrderBy(kv => kv.Key)) Collect(child, sofar + c);
            }
            if (Walk(prefix) is { } start) Collect(start, prefix);
            return result;
        }
    }

    public static void Run()
    {
        var t = new PrefixTree();
        foreach (var w in new[] { "apple", "app", "application", "apt", "banana", "band" }) t.Insert(w);
        Check("Search(\"app\")", t.Search("app"), true);
        Check("Search(\"appl\")", t.Search("appl"), false);
        Check("StartsWith(\"appl\")", t.StartsWith("appl"), true);
        Check("StartsWith(\"c\")", t.StartsWith("c"), false);
        Check("Autocomplete(\"ap\")", t.Autocomplete("ap"), ["app", "apple", "application", "apt"]);
        Check("Autocomplete(\"ban\")", t.Autocomplete("ban"), ["banana", "band"]);
    }
}
