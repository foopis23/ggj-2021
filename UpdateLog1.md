# After Game Jam Clean Up Update

!!!INTRO HERE!!!

## Change Log
- HUD Changes
  - Reduced Lag when Updated HUD
  - Add Compass To HUD
  - Add Dialogue for Intractable Items
- Bug Fixes
  - Fixed damage flash on enemies that are taking sustained damage 
- General Clean Up

## Overlay (HUD) Fix
To explain this issue, I need to setup some context. Before the Jam even start, we were tossing around loose idea. Most things like genres we would be interested in and extra challenges that would be fun. In the hope of finding inspiration, I checked the global game jam's diversifier page, and there it was. "Are we ASCIIng too much?" A diversifier that required that all of the games graphics be made in ASCII. After talking about it, we decided to go for it. I looked around and settled on using something I found on the Unity Asset store called Awesome ASCII Effect\*.

We probably didn't notice the problem until we were in the finally 24 hours of the jam just because no one had time to implement a hud. Once I finally did, I noticed that every time I took damage the game would freeze for about a second. At this point, it could probably be anything. I do some debugging the performance profiler, and I see the problem. Every time we update the hud, it has to render a texture the size of the entire screen on the CPU.

Unfortunately, we could not solve this issue during the game jam. I just didn't have enough knowledge about Unity graphics to have a solution off the top of my head. But now the fix is here, and its actually really simple. Now when changes are made to the HUD, we set the overlay dirty, and store what area has been effected. Once all of the changes to hud are done that frame, we render a texture for just the effected region, use a graphics card process to copy that texture to a location on the overlay texture.

**If that doesn't mean anything to you, that is okay**. In simple terms, CPU does significantly less work now. The trade off being, the graphics card needs to do its fucking job for once. But I don't think anyone is complaining about that trade off.

## General Clean Up
When you are programming for a game jam things get messy. 


\* At the time of writing, Awesome ASCII Effect is now deprecated on the Unity Asset store. I don't really know why, but it wasn't in early February 2021. 

