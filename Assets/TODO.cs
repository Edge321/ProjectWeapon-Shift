/* FOR PLAYER ATTACKING STUFF
 * You can just keyframe the enabled state of colliders, yes
 * If you want to key multiple colliders of the same type,  
 * you probably will want to add them to child objects so the animator can tell them apart.
 * 
 * FOR ANIMATION MOVING CHARACTER
 * https://docs.unity3d.com/Manual/RootMotion.html
 */

/* TASKS TO ACCOMPLISH!!!!!
 * FOV for enemies to spot players. Improve AI for following the player when spotted.
 *   Could make it so when AI loses sight of player, they know the player's location for a set amount of seconds and stop there.
 *     They will stay alert for a long time TBH
 * 
 * When engaging the player, melee enemies can be aggressive, analytical, or defensive
 *  Aggressive means walk around the player for a bit, then attack
 *  Analytical means walk around player for a while, then attack
 *  Defensive means mostly walk around the player, rarely attack
 *	 When the enemy is walking around player, get the rotation for around the player, and slowly walk towards them
 *	 
 * (In some levels) Make enemies have a patrol pattern <- Maybe
 * 
 * Configure collision box with all animations
 * 
 * Implement player getting hit animations
 * 
 * Refine how jump,attack,dodge, and maybe block interact with each other
 *  Fix the physics of it too
 *  
 * Refine enemy AI
 *	Have them monitor player for a bit after the player leaves the monitor area
 */