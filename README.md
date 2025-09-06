# Prefab with collisions maker for Quaternius's Megakits


Transforms Quaternious source models and collisions and puts them into a Prefab asset, ready to use in unity

# User Guide

Select the models of quaternius that you want to combine the collision meshes for, then go to Tools/Apply collision Prefab
this is non-destructive, and only creates new prefabs at (OS Independant) "Assets/Prefabs/{model_name}_withCollisions

# Install guide

2 Ways

Download the Unitypackage file and import,

drag and drop the script anywhere inside your asset folder


# Notice 

The collision folder must be in the same folder as the model source fbx files

Some of quaternious's Models do not have collisions, or are seperated parts of the other models. This will still create a prefab, but wont apply a collider. 

the collisions are by name only, using Collision_ as the start for each of the colliders. if the naming scheme is the same, it will work for other packs.
if it isnt the same, you just need to change the (collision+(model) name part of the collisionpath to reflect the new naming scheme.


# Currently supported Packs

[Medieval Village Megakit](https://quaternius.com/packs/medievalvillagemegakit.html)



Enjoy!
