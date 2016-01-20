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
