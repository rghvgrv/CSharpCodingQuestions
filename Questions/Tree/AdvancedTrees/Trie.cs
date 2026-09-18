namespace CSharpCodingQuestions.Questions.Tree.AdvancedTrees;

[Question(Order = 1, Title = "Trie (Prefix Tree)", Level = Medium, Problem = """
    Store words so you can quickly check `Contains(word)`, `StartsWith(prefix)`, and list every word with a prefix (autocomplete).
    """)]
public static class Trie
{
    [Approach(Name = "List of Words", Time = "O(total letters) per lookup", Space = "O(total letters)", Idea = """
        Keep the words in a list and check them one by one. Every lookup gets slower as you add more words.
        """)]
    public class WordList
    {
        private readonly List<string> words = new();

        public void Add(string word) => words.Add(word);

        public bool Contains(string word) => words.Contains(word);

        public bool StartsWith(string prefix) => words.Any(word => word.StartsWith(prefix));
    }

    [Approach(Name = "Trie", Time = "O(length of the word) per lookup", Space = "O(total letters)", Idea = """
        A tree where each edge is a letter. Words that begin the same way share the same path: "car", "card" and "care" all go through c → a → r.
        A flag marks the nodes where a real word ends.

        - `Add`: follow the letters, creating missing nodes; flag the last one.
        - `Contains`: follow the letters; the last node must be flagged.
        - `StartsWith`: just follow the letters; the path must exist.

        The speed depends only on the length of the word, not on how many words are stored.
        """)]
    public class PrefixTree
    {
        private class Node
        {
            public Dictionary<char, Node> Children { get; } = new();
            public bool IsEndOfWord { get; set; }
        }

        private readonly Node root = new();

        public void Add(string word)
        {
            Node node = root;
            foreach (char letter in word)
            {
                if (!node.Children.ContainsKey(letter))
                {
                    node.Children[letter] = new Node();
                }
                node = node.Children[letter];
            }
            node.IsEndOfWord = true;
        }

        public bool Contains(string word)
        {
            Node? node = Walk(word);
            return node != null && node.IsEndOfWord;
        }

        public bool StartsWith(string prefix) => Walk(prefix) != null;

        public List<string> Autocomplete(string prefix)
        {
            var words = new List<string>();
            Node? start = Walk(prefix);
            if (start != null)
            {
                CollectWords(start, prefix, words);
            }
            return words;
        }

        private Node? Walk(string text)
        {
            Node node = root;
            foreach (char letter in text)
            {
                if (!node.Children.TryGetValue(letter, out Node? next))
                {
                    return null;
                }
                node = next;
            }
            return node;
        }

        private void CollectWords(Node node, string soFar, List<string> words)
        {
            if (node.IsEndOfWord)
            {
                words.Add(soFar);
            }
            foreach (var (letter, child) in node.Children.OrderBy(pair => pair.Key))
            {
                CollectWords(child, soFar + letter, words);
            }
        }
    }

    public static void Demo()
    {
        var trie = new PrefixTree();
        foreach (string word in new[] { "car", "card", "care", "cat", "dog", "dot" })
        {
            trie.Add(word);
        }
        Console.WriteLine("Added: car, card, care, cat, dog, dot");
        Print("Contains(\"card\")", trie.Contains("card"), expected: true);
        Print("Contains(\"ca\")", trie.Contains("ca"), expected: false);
        Print("StartsWith(\"ca\")", trie.StartsWith("ca"), expected: true);
        Print("StartsWith(\"x\")", trie.StartsWith("x"), expected: false);
        Print("Autocomplete(\"car\")", trie.Autocomplete("car"), expected: new[] { "car", "card", "care" });
        Print("Autocomplete(\"do\")", trie.Autocomplete("do"), expected: new[] { "dog", "dot" });

        var list = new WordList();
        list.Add("car");
        Print("WordList Contains(\"car\")", list.Contains("car"), expected: true);
    }
}
