Dungeon Generator
=================

Generates dungeons of arbitrary size. Useful for Roguelike/Top down/Action RPGs/Anything that needs random dungeons.

![image](http://i.imgur.com/BCAFZJ0.png)


## How To Use

This library doesn't have any third party dependencies and is easy to use:

```C#
   var size = MapSize.Huge;
   var seed = 1024u;
   var dungeon = Generator.Generate(size, seed);
```

The return value from Generator.Generate() gives you an ITileMap, which is just a wrapper around a 2D array of UShorts.


Optionally, you can give fine grained generation options

```C#
 Generate(MapSize.Huge, new GeneratorParams{
   Seed = 1024,  
   Doors = 1.0f,  // NOT IMPLLEMENTED the chance to add a door tile attribute to a room's exits
   Exits = true, // will generate and place an exit tile
   Loot = .25f,  // chance that a loot marker will be placed in a room, 0 - 1.0
   MobSpawns = .66f, // chance that a mob spawn will be placed in a cell, 0 - 1.0
   RoomChance = .66f, // chance that a cell will become a room instead of a corridor, 0 - 1.0
   MobsInRoomsOnly = false // if mobs should only spawn in rooms
 })
```

If you want to use the tester console app that comes along with this, you need to make sure to change your font size to be
8x8. You can do this by right clicking on the window title bar icon and selecting "properties", then go to the "font" tab
and apply the following settings:

![font screen cap](http://i.imgur.com/RggSCgo.png)

If you don't do this, the characters will be taller than they are wide when the dungeon prints, skewing everything and making it
look wacky asf.


## License

MIT
