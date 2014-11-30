Dungeon Generator
=================

Generates dungeons. Useful for Roguelike/Top down RPGs/Action RPGs/Anything that needs random dungeons.


Built against .Net Framework 4.5 in VS 2013. You can surely build this in mono but I don't care enough to do that.

This library takes absolutely no dependencies (unless System counts as one to you), so there is that.


Right now the library is setup to use multiple dungeon generation algorithms, and you can implement your own by
deriving from the IDungeonGeneratorStrategy interface, and plugging it into the Generator class.

Pull requests welcome, please just don't use singletons and keep thread safety in mind.
