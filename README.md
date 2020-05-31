# Gravitative Cable Car
Gravitative Cable Car mod for Cities Skylines.

Make cable cars subject to gravity. This is a mod.  

In the vanilla game cable cars move unrealistically because the top/wheel part and the cabin part are combined together so the game physics handles them in the same way. In reality these two parts have different movements. This mod makes them rotate/sway in different ways when rendering so it looks more realistic. Cable car assets need to be modified to get the best result.  

## Warning
This mod was written in the prefix method of Harmony so it will be incompatible with other mods if they also change how cable cars are rendered. Right now this is just a proof-of-concept thing and cable cars are not popular so....I'll rewrite it using the transpiler method when I have more time to learn about it and a known incompatibility with another mod occurs.

## Release Note
This mod is still in the proof-of-concept stage so expect frequent changes.

v0.3 - Disable sway of the top/wheel part. [This short video](https://www.youtube.com/watch?v=ljHAi7EXB8k) explains the difference.

v0.2 - Disable sway on the Y-axis so cable cars don't jump up and down on the cable  
       This one is hard to explain in screenshots so I made a short [video clip](https://www.youtube.com/watch?v=B6mj6Y95eYk) to show the difference.  

v0.1 - Proof of concept. Limit the rotation of meshes for rendering so it looks more gravitative.  
       See the screenshots on the workshop [page](https://steamcommunity.com/sharedfiles/filedetails/?id=2094830335).  

## Required Items
Harmony: this mod overwrites a function the game uses to render cable cars. boformer's [Harmony mod](https://steamcommunity.com/workshop/filedetails/?id=2040656402) makes this a lot easier.  

Mass Transit DLC: You need this DLC to use cable cars.

## Goal & Future Plan
The goal of this mod is to make cable cars move more realistically(from the viewpoint of the players) without messing too much with game physics. More features might be added.

## To-do
- Rewrite using the transpiler method.

## Contact
Steam workshop [link](https://steamcommunity.com/sharedfiles/filedetails/?id=2094830335)  

You can use Github's issues tab or via Steam. This repo is not under my main Github account so you probably will get a faster response if you send me a Steam message or leave a comment on the workshop page.  

## FAQ

### Does it work with existing cable cars on the workshop?
Unfortunately at the moment only my own assets would rotate fully in the intended way because I already modified them for testing:  
[Modified Vanilla Cable Car](https://steamcommunity.com/sharedfiles/filedetails/?id=2095140359)  
[Cable Car TR - LHT](https://steamcommunity.com/sharedfiles/filedetails/?id=2104173456)  
[Cable Car TR - RHT](https://steamcommunity.com/sharedfiles/filedetails/?id=2114149504)  
[Colored Cable Car](https://steamcommunity.com/sharedfiles/filedetails/?id=2088780350)  

Vanilla cable cars and other cable car assets, without modification, would have the whole vehicle rotated like the third screenshot shows(see the wheels). I think this still looks better than the original. Now we have the smaller top/wheel part, not the cabin part, against the law of physics, and this is nearly unnoticeable when you watch from a long distance.  

### Incompatibility with other mod?
No known incompatibility yet. This mod was written in the prefix method of Harmony so it will be incompatible with other mods if they also change how cable cars are rendered. Right now this is just a proof-of-concept thing and cable cars are not popular so.... I'll rewrite it using the transpiler method when I have more time to learn about it and a known incompatibility with another mod occurs.

### How do I make cable cars sway more/less?
This mod doesn't change how cable cars actually move. Use [Advanced Vehicle Options](https://steamcommunity.com/workshop/filedetails/?id=1548831935) to change the spring and damper values.

### Performance
Minimal and unnoticible. This mod only does a few simple things. This mod only makes the game render cable car instances with a modified version of the RenderInstance method in the Vehicle class. The performance difference should be minimal and unnoticeable.

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
Any comment/feedback is welcome! This mod needs to be rewritten in a more proper way to avoid potential incompatibility with other mods and I'd like to collect more issue/bug reports and suggestion before I start rewriting it. If you read the source code and find any issue or know a better way to make things work, feel free to comment below or use the issues tab on Github. Pull requests are also welcome.  

### How do I make my own cable car assets compatible with this mod?
Because the top/wheel part should stay on the cable and rotate/sway differently, the mod needs to know where the top/wheel part is. This is done by using submeshes. So just put the top/wheel part of your cable car in submesh 1. Vehicle shaders don't support submesh LOD so combine sumbesh LODs with the main LOD. By default the cable car template uses the ship shader for the main mesh and uses the car shader for submesh for whatever reason. So without any tweaks the game might think the wheel/top part is a car wheel and spins it. I use modtools to change to car shaders for all meshs, and use vertex painting to stop the shader from spinning things. If you have any question about creating cable car assets feel free to send me a message. I'm not an experienced asset creator but I might be able to help.
Check out my [**Cable Car Asset Creation Guide**](https://github.com/sway2020/GravitativeCableCar/tree/master/cable_car_asset_guide) for more information.
