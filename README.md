# UnsaveableParser

## Usage

This is a parser for source engine save files which creates an in-memory representation during parsing. It is currently a super duper work in progress and only explicitly supports Portal 1 (although it does seem to work with Portal 2 👀). Here's an example of how you could use this to get the time of a save file:
```cs
SourceSave save = new SourceSave("your favorite save file.sav");
save.Parse();
// get the server state file (there might be many (don't ask))
SaveGameStateFile sFile = save.StateFiles.OfType<SaveGameStateFile>().First();
// get the appropriate field from the "header"
Time t = sFile.SaveHeader.GetFieldOrDefault<Time>("time__USE_VCR_MODE");  // just use "time" for p2 saves
Console.WriteLine(t);
```
```
35.864998
```
In general, there's lots of juicy stuff in a save file and it's not necessarily the most easy thing to find what you need. The best way to find a specific thing that you're looking for is to do a verbose dump of the parsed data to a text file and ctrl+f your way through that and then struggle to figure out how to get it from the code later.
```cs
using var w = new IndentedTextWriter(new FileStream("your favorite file.txt", FileMode.Create));
save.AppendToWriter(w);
```
This will create a fully readable text representation of the parsed data (note that there's still lots of stuff that isn't parsed yet). A sample of this verbose dump can be found [here](verbose-sample.txt).

## Structure

A save file is actually composed of multiple files which have `.hl1`, `.hl2`, and `.hl3` extensions; usually there is just one of each. So far, this project has been focused around parsing the `.hl1` files, which seems to contain the server-side state of the game. I believe `.hl2` files contain the client-state state and most (all?) of the time the `.hl3` file is empty. 

In game, the vast majority of the data is written and read with **datamaps.** A datamap contains a list of fields in a class/struct, each of which is represented with a **type description.** The type description has the name of the field, some additional flags, (which I don't use atm) how many elements there are (it can represent an array), and the data type of the field (or a custom read/write function). I tried to copy this process closely (as much as seemed reasonable in C# anyway). There are several classes that inherit from the `DataMapGenerator` class which contain the instructions for creating many datamaps (this is done during run time before parsing). During parsing, each parsed field is stored as a `ParsedSaveField` object, each parsed map is stored as a `ParsedDataMap` object, and each parsed `.hl` file is stored as a `EmbeddedStateFile` object.
