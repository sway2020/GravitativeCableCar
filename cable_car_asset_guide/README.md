# Cable Car Asset Creation Guide
This guide shows the steps I did to make my colored cable car asset, and how to make it work with my [gravitative cable car](https://steamcommunity.com/workshop/filedetails/?id=2094830335) mod. This guide assumes you already know how to make other types of asset so many steps are skipped. It only covers some extra things needed to be done to make a cable car asset.  
  
![](https://i.imgur.com/tQLi7fI.png)

## Introduction
At the moment there are only 4 custom cable car assets on the workshop, and only my asset would work 100% with my mod. I hope this guide will make more people interested in creating cable car assets. There might be some wrong information in the guide because I have no prior experience in making vehicle assets, and these are just the steps I did to make it work. 

The cable car template has many weird default behaviors, and I'll explain how to fix them with some extra steps in this guide. My asset only uses 1 submesh(for the top/wheel part), if you plan to use more submeshes(like using the rotor shader for the glasses), there will be some extra steps needed to be done and not covered in this guide.


## Model Preparation
You model needs some preparation, or the cable car template in the asset editor is gonna do some crazy things to it. Cable cars would have some weird color/shading, and it might even spin like car wheels. It took me a lot of time to figure things out. 

I use Blender 2.82a so things might be a little different if you use other software.

First import the model of the vanilla cable car into your project. I uploaded the model already and you can download it [here](https://drive.google.com/open?id=1d3u-SkFMAYJlrl3E123xp199TNqTamey).

This is how it looks like in blender.  

![](https://i.imgur.com/CLiX7w1.jpg)

Keep it as a reference for the size and position of your cable car asset. The wheels should be at the same height as the vanilla ones, so your cable car will stay on the cables. If your cable car is larger than the vanilla one, if might not fit the cable car stations.


Make your model and texture in the same way as you would for other vehicle assets. Make sure the top/wheel part is separated from other parts if you want it to be compatible with the gravitative cable car mod. The mod needs the top/wheel part to be in a submesh. This is how my main model look like:

Cabin Part  
![](https://i.imgur.com/KvQftTj.jpg)

Top/Wheel Part  
![](https://i.imgur.com/ocFtp4c.jpg)

Don't merge them. This is just how it look overall  
![](https://i.imgur.com/hCeQS1j.jpg)


Vehicle shaders don't suppor submesh LOD, so just combine everything together in the main LOD model:  

![](https://i.imgur.com/AgJr1Pl.jpg)

By default the cable car temple uses the ship shader for the main mesh, and uses the default car shader for the submeshs. The default car wheel detection mechanics for cars don't well work for cable cars. Cable cars have wheels above them, not below them. So we need to use vertex painting to fix the problem. More useful info on this can be found on Ronyx69's [website](https://cslmodding.info/shader/vehicle-wheels/).

I'm okay with non-spinning wheel, so I just vertex paint everything in blue. If you want spinning wheels, I think it's possible but you need to do some extra steps(I never tried it).  

![](https://i.imgur.com/wMBzYiu.jpg)

Export your model as usual. OBJ doesn't support vertex color so use FBX or DAE.   

Now I have:  
main.fbx (main mesh for the cabin part)   
top.fbx (main mesh for the top/wheel part)  
main_lod.fbx (LOD mesh)  

## Import

Subscribe to BloodyPenguin's [ModTools](https://steamcommunity.com/sharedfiles/filedetails/?id=450877484). Copy your model and texture files to the import folder. Go to the asset editor and import main.fbx as a new prop. If you already have experience in making car and train assets, you probably already know what we are going to do. It is only a little bit different.

It will show up below the ground which is fine. Save it and remember the filename and asset name you choose. I used the default New Asset.    
![](https://i.imgur.com/Q9uJnyb.jpg)

Repeat the same steps for top.fbx. I named it New Asset1  
![](https://i.imgur.com/dDp4Icy.jpg)

Restart the game. Make sure ModTools is enabled. Go to the asset editor and import main.fbx as a vehicle using the cable car template.  
![](https://i.imgur.com/yhnRJoI.jpg)

Here you should see some weird shading on the imported model. This is because by the default the cable car template uses the shader for ships on the main mesh so the vanilla cable cars look wet.  
![](https://i.imgur.com/9tHeqxv.jpg)

Click ctrl-E to open ModTools' scene explorer. Click  Show edited prefab.  
![](https://i.imgur.com/8imnx6d.jpg)

Scroll down and find m_material, click the + and expand it.  
![](https://i.imgur.com/OT8Q8K6.jpg) 

Scroll down and you can see it uses the shader for ships which doesn't make any sense.  
![](https://i.imgur.com/Ts9azTW.jpg)

Change it to the default vehicle shader.
You can also write a script to change the shader to save time in the future.  
![](https://i.imgur.com/CUy9USy.jpg)

Now the weird shading is gone. Also you can use color variations with this shader.  
![](https://i.imgur.com/MKCY05w.jpg)

Open ModTools' scene explorer again. Find m_lodMaterial. By default the LOD mesh also uses ship shader. Expand it and change the shader to the default vehicle shader too (same steps as the main mesh). You will get an error message after you change the shader but it's fine.    
![](https://i.imgur.com/VnHiRkl.jpg)

Add top.fbx as a submesh. Gravitative cable car mod needs the top/wheel part to be the first submesh.  
![](https://i.imgur.com/mw4Kj3X.jpg)

The cable car template uses the default vehicle shader for submeshes so we don't need to change the shader for the submeshes. If you want to use the rotor shader for other submeshes, there are probably some extra things to do. I think Ronyx69's script for car windows would still work but I never tried it.

If you save it now as a new cable car asset and test it in the game, **some parts of your cable car are gonna rotate randomly like propellers.** This is caused by the default car wheel detection mechanics of the shader. 

I wrote a [**script**](https://github.com/sway2020/GravitativeCableCar/blob/master/cable_car_asset_guide/cable_car_script.cs) to sovlve the problem. The script was modified from Ronyx69's car wheel script.  

If you saved the prop version with a different name, the code needs to be changed slightly(check out the comments)

Click F7 to open ModTool's debug console. Copy and paste the script. Click run.
If there are no new red error message after you run the script, you can save your cable car asset and test it in the game.   
![](https://i.imgur.com/FRwYFle.jpg)

Subscribe to my [gravitative cable car mod](https://steamcommunity.com/workshop/filedetails/?id=2094830335), and see if the mod works with your cable car or any unexpected thing happens.
Also check if it works fine when the mod is disabled. It will help me improve the mod.

## Next Steps
If you find any mistakes, or you have any suggeston, feel free to contact me and I'll update this guide.  

If you make a cable car asset that is compatible with my mod, send me a message so I can put your workshop link in the mod description. 

If you make a cable car asset with rotor shaders and would like to share the steps, I can combine it into this guide.  