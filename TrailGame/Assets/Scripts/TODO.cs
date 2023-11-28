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
 *  - add icon for pivot tile
 *  - refactor  
 *  - paint bucket tile splashes color in all adjacent tiles
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