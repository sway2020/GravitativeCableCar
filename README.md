# GravitativeCableCar
Gravitative Cable Car mod for Cities Skylines.

This mod is still in the early proof-of-concept stage. Right now this experimental mod changes the rotation of a cable car vehicle for rendering, so it looks like they're subject to gravity, while the top/wheel part uses the original rotation. It doesn't change how cable cars actually move. Cable cars would sway in the original way so it still looks natural. Cable car assets need to be modified to get the best result.

## Warning
This mod was written in the prefix method of Harmony so it will be incompatible with other mods which also change how cable cars are rendered. Right now this is just a proof-of-concept thing and cable cars are not popular so....I'll rewrite it using the transpiler method when I have more time to learn about it and a known incompatibility with another mod occurs.

## Release Note
v0.1 - Proof of concept. Change the rotation of meshes for rendering so it looks more gravitative.

## Required Items
Harmony: this mod overwrites a function the game uses to render cable cars. boformer's [Harmony mod](https://steamcommunity.com/workshop/filedetails/?id=2040656402) makes this a lot easier.
Mass Transit DLC: You need this DLC to use cable cars.

## Goal & Future Plan
The goal of this mod is to make cable cars move more realistically(from the viewpoint of the players) without messing too much with game physics. More features might be added.

## To-do
- Rewrite using the transpiler method.

## Contact
Steam workhsop [link](https://steamcommunity.com/sharedfiles/filedetails/?id=2094830335)  
This is not my main Github account so you probably would get a faster response if you send me a Steam message

## FAQ

### Does it work with existing cable cars on the workshop?
Unfortunately at the moment only my own [colored cable car](https://steamcommunity.com/sharedfiles/filedetails/?id=2088780350) asset, and this [modified version of vanilla cable car](https://steamcommunity.com/sharedfiles/filedetails/?id=2095140359) would rotate fully in the intended way because I already modified them for testing.

Vanilla cable cars and other cable car assets, without modification, would have the whole vehicle rotated like the third screenshot shows(see the wheels). I think this still looks better than the original. Now we have the smaller top/wheel part, not the cabin part, against the law of physics, and this is nearly unnoticeable when you watch from a long distance. I wrote a short how-to guide at the end of FAQ for other asset creators who are interested in making their cable car assets compatible with this mod.

### Incompatibility with other mod?
Unknown. This mod is not widely tested yet. This mod was written in the prefix method of Harmony so it will be incompatible with other mods which also change how cable cars are rendered. Right now this is just a proof-of-concept thing and cable cars are not popular so....I'll rewrite it using the transpiler method when I have more time to learn about it and a known incompatibility with another mod occurs.

### Performance
This is a lightweight simple mod which renders cable car instances with a modified version of the RenderInstance method in the Vehicle class. The performance difference should be minimal and unnoticeable.

### Known Issues and Bugs
This mod is still in the early development stage and it's more of a proof of concept now. If you encounter any issue/bug or know how to fix it please let me know.

Issue 1. It seems like vehicle shaders don't support submesh LOD, and the submesh would disappear upon reaching LOD distance. In our case it means the top/wheel part of the cable car would disappear when it switches to LOD. I can think of 3 ways to tackle this issue:

A. Something similar to what the [Transparency LODs Fix](https://steamcommunity.com/workshop/filedetails/?id=922939393) mod does. This option gives the best result but it would increase performance burden.

B. Combine all submesh LODs with the main LOD. This means the top/wheel LOD would rotate together with other parts(not staying on the cables) so it looks a bit weird.

C. Just let the top/wheel part disappear when the game switches to LOD.

Option A needs more modding work. Asset creators can choose between B or C on their own just by re-organizing their assets. Right now my colored cable car asset uses option B because unlike transparent buildings, cable cars are small enough so it isn't obvious from a far distance. I'm still open to other options.

### Can this mod break my save file?
This mod does not touch you save data, game physics, or any other game data at all so it is unlikely this mod would break you save file. This mod only changes how cable cars are rendered in runtime and you can disable this mod any time. However it is always a good idea to back up your save file periodically. If it turns out this mod is the reason why your save file is broken, I'm not responsible for it although I will do my best to help you out.

### What can I do to help the development of this mod?
Any comment/feedback is welcome! I decided to publish this PoC 0.1 version so I can collect bug/issue reports and suggestions before I start rewriting this mod in a more proper way. Although I have prior experience in other programming languages, I am not familiar with C#, Harmony or Unity at all. If you read the source code and find any issue or know a better way to make things work, feel free to comment below or use the issues tab on Github.

### How do I make my own cable car assets compatible with this mod?
Right now the mod changes the quaternion rotation of the main mesh and sub-meshes for rendering. The only exception is sub-mesh 1 which still uses the original rotation. So just put the wheels/top part of your cable car in submesh 1. Any other meshes will have zero rotation on the x and z axes so it looks like they're subject to gravity. I plan to make this more customizable in the future and I'd love to get suggestion from other cable car asset creators. If you have any question feel free to send me a message.
