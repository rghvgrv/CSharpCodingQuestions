# Bit Manipulation

Computers store numbers in binary, as 0s and 1s. Bit operators work on those bits directly, which gives tiny and very fast solutions to some problems.

## Binary in 30 seconds

Each position is a power of two: `13` = `8 + 4 + 1` = `1101` in binary.

```csharp
Console.WriteLine(Convert.ToString(13, 2));   // "1101"
int fromBinary = 0b1101;                      // 13
```

## The operators

| Operator | Name | Example | Result |
|---|---|---|---|
| `a & b` | AND: 1 if both bits are 1 | `1100 & 1010` | `1000` |
| `a \| b` | OR: 1 if either bit is 1 | `1100 \| 1010` | `1110` |
| `a ^ b` | XOR: 1 if the bits differ | `1100 ^ 1010` | `0110` |
| `~a` | NOT: flip every bit | `~0` | `-1` |
| `a << k` | Shift left = multiply by 2ᵏ | `3 << 2` | `12` |
| `a >> k` | Shift right = divide by 2ᵏ | `12 >> 2` | `3` |

## Handy tricks

- Is bit `i` set? `(n & (1 << i)) != 0`
- Is `n` even? `(n & 1) == 0`
- Remove the lowest 1-bit: `n & (n - 1)`
- XOR facts: `x ^ x = 0` and `x ^ 0 = x`, so pairs cancel out.

These tricks are fast and use no extra memory. Clear, normal code is usually better unless speed or memory really matters.
