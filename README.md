# novus
A new grammar for a new .Net language.

Some years ago, in the early 1990s I took an opportunity to design and write an optimizing compiler for the PL/I language that could run on and generate code for, Windows NT. The compiler became a pretty solid implementation of the PL/I Subset G as found on DEC and Stratus minicomputers. To write the compiler I chose the C language (initially Borland C). The compiler was eventually able to produce NT compatible 32-bit COFF object modules that were linkable with the standard MS linker and a small boostrap/runtime written in assembler.

PL/I was a language I'd used for many years and being familair with it was a help, but I also used the ANSI standard too (which I had to order by mail, a 1" thick printed book) and several other resources including a short but valuable paper by Robert Freiburghouse who was a member of the GE Multics team and responsible for developing that systems PL/I compiler.

Developing a compiler for PL/I by writing one in C brought the differences in the languages into stark contrast, one needs a pretty good insight into each language too. I found myself wondering "how do I do this in C" or "it would be good if PL/I could do that" and so on.

Each language was more than capable of being used to write an operating system, each langauge supported pointers so list processing and tree manipulation was readily done in each language. There were numerous differences of course too but by far the most prominent difference was that PL/I has no concept of a "reserved word" a word that cannot be used for a function or variable.

It turns out that PL/I did this for a very good reason which is to allow new keywords to be introduced into the language in the future that might just be the same as some variable or function present in some customer's source code. The "no reserved words" policy meant that no matter what new keywords were introduced into PL/I it would always be backward compatible, it could always compile customer code absolutely fine even if that code had used a new keyword as a function or variable name.

It became clear that the designers of PL/I had put a lot of thought into how to make the language extensible, being able to freely add new keywords without much effort and no risk of breaking backward compatibility was a design goal, not an afterthought.

It is this idea that has sparked my interest in a grammar that could also be free of reserved words yet also be capable of developing the same functionality that C# can produce.

This repo contains code that is in one way or another, related to the design of a grammar that is not based on C or it's derivatives. The grammar is intended to be able to create a language implementation that can do all that C# can do but offer improvements to how we extend the language over time.

This include the ability to almost routinely add new keywords and the ability to add new language functions (think C#'s nameof or sizeof) without ever having to reserve these names. 

My blog contains articles related to this and this is where most of the in-depth analysis can be found.

[A new grammar for C# - Part 1.](https://korporalkernel.wordpress.com/2021/10/19/a-new-grammar-for-c/)

[A new grammar for C# - Part 2.](https://korporalkernel.wordpress.com/2021/10/31/taking-stock/)

One of the main objectives is to simplify the process by which we can extend the language, make it easier to add new features and reduce the idiosyncracies that often arise in languages derived from C. The language can be extended by both adding new keywords or by adding new (what I term) "bultin" functions.

This is currently a research effort, so feedback is valued and encouraged.


