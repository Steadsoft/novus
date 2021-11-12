# novus grammar
This is the somewhat informal/incomplete grammar expressed using BNF 'like' notation. This is a work in progress and changes as the code changes, the code drives the grammar and the grammar drives the code, this is because this is an experimental undertaking.

The grammar bears some resemblance to C# but is not as granular and the names of non-terminals are not the same even for the same constructs. 

This [document](https://www.cs.vu.nl/grammarware/browsable/CSharp/grammar.html) was found helpful too.

`*` - Denotes zero or more occurences

`+` - Denotes one or more occurences

`?` - Denotes an optional term

```
compilation-unit:
   using-directive*  
   namespace-member-declaration*
```
```
namespace-member-declaration:
   namespace-declaration
   type-declaration
```   
```
namespace-declaration:
    "namespace" qualified-identifier [namespace-body] ";"?
```    
```
namespace-body:
    "{" using-directive* namespace-member-declaration* "}"
```

The `type-declaration` is where we differ from C#:

```
type-declaration:
   "type"  [type-options] type-body
```

```
type-options
   "class"
   "struct"
   "record"
   "singlet"
   "new"
   "public"
   "protected"
   "internal"
   "private"
   "abstract"
   "sealed"
```

The rules regarding what options can be combined and any ordering of them, is not defined in this grammar (it is defined but not in this doc)