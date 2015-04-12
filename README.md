Dungeon Generator
=================
[![Gitter](https://badges.gitter.im/Join Chat.svg)](https://gitter.im/adamveld12/dungeon_generator?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Generates dungeons. Useful for Roguelike/Top down RPGs/Action RPGs/Anything that needs random dungeons.

![image](http://i.imgur.com/nMbhpjX.png)

Built against .Net Framework 4.5 in VS 2013. You can surely build this in mono but I don't care enough to do that.

This library takes absolutely no dependencies (unless System counts as one to you), so there is that.


Right now the library is setup to use multiple dungeon generation algorithms, and you can implement your own by
deriving from the IDungeonGeneratorStrategy interface, and plugging it into the Generator class.

Pull requests welcome, please just don't use singletons and keep thread safety in mind.
