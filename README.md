# novus
A new grammar for a new .Net language.

This repo contains code that is in one way or another, related to the design of a grammar that is not based on C or it's derivatives. The grammar is intended to be able to create a language implementation that can do all that C# can do.

My [blog](https://korporalkernel.wordpress.com/2021/10/19/a-new-grammar-for-c/) contains articles related to this and this is where most of the in-depth analysis can be found.

One of the main objectives is to simplify the process by which we can extend the language, make it easier to add new features and reduce the idiosyncracies that often arise in languages derived from C.

Another goal is to improve consistent and symmetry, a simple example being where C# allows this 

```cs
public class record
{


}
```

but not this:

```cs
public record class
{


}
```


