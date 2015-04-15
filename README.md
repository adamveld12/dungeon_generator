Dungeon Generator
=================
[![Gitter](https://badges.gitter.im/Join Chat.svg)](https://gitter.im/adamveld12/dungeon_generator?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Generates dungeons of arbitrary size. Useful for Roguelike/Top down/Action RPGs/Anything that needs random dungeons.

![image](http://i.imgur.com/yJjLThE.png)


## How To Use

This library doesn't have any third party dependencies and is easy to use:

```C#
   var size = MapSize.Huge;
   var seed = 1024u;
   var dungeon = Generator.Generate(size, seed);
```

The return value from Generator.Generate() gives you an ITIleMap, which is just a wrapper around a 2D array of UShorts.

If you want to use the tester console app that comes along with this, you need to make sure to change your font size to be
8x8. You can do this by right clicking on the window title bar icon and selecting "properties", then go to the "font" tab
and apply the following settings:

![font screen cap](http://i.imgur.com/RggSCgo.png)

If you don't do this, the characters will be taller than they are wide when the dungeon prints, skewing everything and making it
look wacky asf.


## License

MIT
