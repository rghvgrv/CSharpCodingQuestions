# Strings

A string is text: a sequence of characters. Most string problems are array problems in disguise, so counting, two pointers and sliding windows all work here too.

## Strings in C#

```csharp
string word = "hello";
char first = word[0];            // 'h'
int length = word.Length;        // 5
string upper = word.ToUpper();   // "HELLO"
char[] letters = word.ToCharArray();
string back = new string(letters);
```

**Strings are immutable.** You can't change a string; every change creates a new one. So this loop is slow, because it copies the whole text each time:

```csharp
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += "a";   // O(n) copy every time → O(n²) total
}
```

Use `StringBuilder` when you build text piece by piece:

```csharp
var builder = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    builder.Append('a');   // O(1) on average
}
string text = builder.ToString();
```

## Useful tricks

- **Count letters** with an array of 26 numbers: `counts[letter - 'a']++`.
- **Two pointers** from both ends: palindromes and reversing.
- **Sliding window**: longest or shortest substring with some property.
- **Sorting the letters** gives a key that all anagrams share: "listen" and "silent" both become "eilnst".

## Cost of common operations

| Operation | Cost |
|---|---|
| `text[i]` | `O(1)` |
| `text.Substring(start, length)` | `O(length)` |
| `a + b` (concatenation) | `O(a.Length + b.Length)` |
| `text.Contains(word)` | `O(n · m)` worst case |
