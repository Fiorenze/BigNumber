# BigNumber
 Created this a while back since I couldn't find something like I needed for a clicker game, but ended up not using it. See the demo to test it
 
 - Creates a variable 'BigNumber' which acts similar to any number variable (float, double etc.), displays 3 digits and a suffix. Can be expanded or modified.
 - Supports math operations (+ - * /) between BigNumber/BigNumber and BigNumber/float
 - Supports comparison (== != < > <= >=) between BigNumbers
 - Supports ToString() (prints; 100M, 35.25ag etc.)
 - Supports TryParse() (parses; "15550K" as 15.5M or BigNumber(15.5, 2))
 - Number types are "K, M, B, T, aa, ab, ..., ga"
 - Number types and maximum value can be modified, go for 'quintillion' or something instead of 'bb' if you want (index of suffixes are used to determine digits so they should be safe to change)
 - Or you may add a lot more suffixes and it will work

```csharp
// Simply define a BigNumber and you are good to go
public BigNumber bigNumber = new BigNumber(1, 0); // amount, exponent (^3)
// BigNumber(1,1) is 1K or 1000
// BigNumber(12, 10) is 12af or 12 * 10^30
```
