# Rimworld-GraffitiMod

Modification for the game Rimworld that adds graffiti to the game. Please refer to /About/About.xml for details.


## License

Please note that the code in this mod relies and even duplicates parts of the code of the original game by Ludeon Studios. Therefor I am in no position to license this in any way.

Regarding my own work: Feel free to use this mod or parts of it for any purpose, fork it, integrate it, remake it, build upon it or reupload it. If you mention my name and/or notify me, I'd be happy (but not a requirement).


## Project status

First version released + some bugfixes.

New features are planned:
* Mental break
* Ideology icon integration
* Commission a pawn to create custom wall art (that is not filth)

## How to use
The following assemblies from your local Rimworld installation have to be added to your IDE:
* Data\Managed\Assembly-CSharp.dll
* Data\Managed\UnityEngine.dll


These four folders must be included to form the mod: 
* 1.3
* About
* Defs
* Textures

How to build:
* /1.3/Assemblies/GraffitiMod.dll is built from the scripts in /source/
* The folder /external files/ contains the textures as *.psd and a reference file for the atlas used to make the art fit.