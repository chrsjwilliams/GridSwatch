/*
 *    _________  ________  ________  ________     
 *    |\___   ___\\   __  \|\   ___ \|\   __  \    
 *    \|___ \  \_\ \  \|\  \ \  \_|\ \ \  \|\  \   
 *        \ \  \ \ \  \\\  \ \  \ \\ \  \  \\\  \  
 *         \ \  \ \ \  \\\  \ \  \_\\ \  \  \\\  \ 
 *          \ \__\ \ \_______\ \_______\  \_______\
 *           \|__|  \|_______|\|_______|\|_______|
 *                                             
 *                                             
 *   
 *  GAME:
 *  - Fade in color effect!
 *  - How does the game know if it's in colorblind mode to display patterns?
 *  - paint bucket tile splashes color in all adjacent tiles
 *  - erasererases color in a tile direction
 *  - swerve tile - changes players direction
 *  - screen wrap tile
 *  - warp tiles
 *  
 *  
 *  
 *  - BUG: Player clips out of bounds on the higer end
 *          -   Could not reproduce (5/10/23 7:25 PM)
 *  - BUG: Lower intensity colors overwrite higher intensity ones
 *          - potentially fixed (5/10/23 8:47 PM)
 *
 * 
 * OPEN QUESTION:
 * - What is the level design pipeline?
 * - Are desaturated colors confusing?
 * 
 */ 