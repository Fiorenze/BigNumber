# BigNumber
 Created this a while back since I couldn't find something like I needed for a clicker game, but ended up not using it. See the demo to test it
 
 Creates a variable 'BigNumber' which acts similar to any number variable (float, double etc.), displays 3 digits and a suffix. Can be expanded or modified.
 Supports math operations (+ - * /) between BigNumber/BigNumber and BigNumber/float
 Supports comparison (== != < > <= >=) between BigNumbers
 Supports ToString() (prints; 100M, 35.25ag etc.)
 Number types are "K, M, B, T, aa, ab, ..., ga"
 Number types and maximum value can be modified
 Go for 'quintillion' or something instead of 'bb' if you want (index of suffixes are used to determine digits so they should be safe to change)
 Or you may add a lot more suffixes and it will work (Adjust the maximum value according to your needs)

```csharp
//You can edit this line at the beginning
private static readonly int MaxExponent = NumberTypes.stringToIndex("ga");

//Like this to make it automatically select highest possible suffix for maximum value of number
private static readonly int MaxExponent = NumberTypes.stringToIndex(NumberTypes.types[NumberTypes.types.Length - 1]);
```
