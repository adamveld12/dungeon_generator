Dungeon Generator
=================
[![Gitter](https://badges.gitter.im/Join Chat.svg)](https://gitter.im/adamveld12/dungeon_generator?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Generates dungeons of arbitrary size. Useful for Roguelike/Top down/Action RPGs/Anything that needs random dungeons.

![image](http://i.imgur.com/nMbhpjX.png)


## How To Use

This library doesn't have any third party dependencies and is easy to use:

```C#
   var size = MapSize.Huge;
   var seed = 1024u;
   var dungeon = Generator.Generate(size, seed);
```

The return value from Generator.Generate() gives you an ITIleMap, which is just a wrapper around a 2D array of UShorts.


## License

MIT
