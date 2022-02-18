# La-Mulana 2 Garb Key Reset Mod

## Changes made to the game
- Removes a check during startup that deletes and re-gives garbs and garb keys based on achievement data
- Ignores Steam achievement completion status and instead assumes you have not completed the given requirements for garb keys.
- Selecting New Game from the title screen will:
	- Take away all currently owned garbs as well as the keys used to open them (info stored in system2.dat)
	- Delete the global glossary stored in system1.dat (so the garb key is not automatically re-awarded from the title screen until you complete the glossary once again)
	- Clear the system flag in system2.dat that corresponds to previously beating the game (again, so that the garb key is not automatically re-awarded)
- The "Beat the Game" key, which is usually awarded at the end of the credits, is additionally awarded after defeating the final boss.
	- This change was made so that collecting all the garbs (and subsequently beating the game) was possible without watching the credits sequence twice.

## Before installing and patching...
- Since this mod edits your La-Mulana 2 system files, it is highly recommended to create a backup of these files elsewhere.
	- The system files are generally located in `C:/Users/<Username>/AppData/LocalLow/NIGORO/LaMulana2/System`
- Since the changes specifically involve interactions with Steam's achievement system, this will not work on any other version of the game.
- Glossary entries are only added to the global glossary when collected. If you, for instance, start a new game (thus deleting your global glossary) and then load an existing save, you may not be able to complete the global glossary from that save alone.
- If you clear your garbs and keys using this mod and then switch back to the regular game, the check for Steam achievements should re-award you all your keys, but will not recover any of the garbs you had - you will still have to return to each garb chest to open it.
	- To avoid any potential issues recovering garbs or keys, please back up your system files.

## Installing
1. Download the release from https://github.com/Planeswater/LaMulana2KeyResetMod/releases
2. Copy all files from the `LaMulana2KeyResetMod/Monomod` folder to `LaMulana2_Data/Managed`
3. Drag `Assembly-CSharp.dll` onto `monomod.exe`
4. Back up or rename `LaMulana2_Data/Managed/Assembly-CSharp.dll`
5. Rename the output file from `MONOMODDED_Assembly-CSharp.dll` to `Assembly-CSharp.dll`

## Uninstalling
1. Delete the modified `LaMulana2_Data/Managed/Assembly-CSharp.dll`
2. Remove `Assembly-CSharp.mm.dll` and all monomod files from `LaMulana2_Data/Managed`
3. Either restore the original `Assembly-CSharp.dll` from your backup, or use Steam's local file verification to redownload the original file
