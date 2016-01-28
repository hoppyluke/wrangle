# Wrangle

Parses command-line arguments.

## To Dictionary

```csharp
var args = Wrangle.Dictionary.From(new[] {"hello", "world", "lets", "go"});
Debug.Assert(args["hello"] == "world");
Debug.Assert(args["lets"] == "go");

var args2 = Wrangle.Dictionary.From(new[] { "/optional", "prefix", "/is", "supported"}, "/");
Debug.Assert(args2["optional"] == "prefix");
Debug.Assert(args2["is"] == "supported");

var args3 = Wrangle.Dictionary.FromPairs(":", new[] {"apples:oranges", "pears:bananas"});
Debug.Assert(args3["apples"] == "oranges");
Debug.Assert(args3["pears"] == "bananas");
```

## To Object/Struct

```csharp
public class Orders
{
    public int WarpFactor { get; set; }
    public PhaserSetting Phasers { get; set; }
}

public enum PhaserSetting
{
    Stun,
    Tickle
}

var orders = Wrangle.Instance<Orders>.From(new[] {"-WarpFactor", "5", "-Phasers", "Stun"});
Debug.Assert(orders.WarpFactor == 5);
Debug.Assert(orders.Phasers == PhaserSetting.Stun);
```
Supports properties of these types:
* any simple type: `bool`, `char`, `byte`, `sbyte`, `short`, `ushort`,
  `int`, `uint`, `long`, `ulong`, `float`, `double`, `decimal`
* `string`
* `DateTime`, `TimeSpan`, `DateTimeOffset`, `Guid`
* any `enum`; you can use either the name or numeric value (but not combinations of bit flags)

Properties must have public `get` and `set` accessors.
