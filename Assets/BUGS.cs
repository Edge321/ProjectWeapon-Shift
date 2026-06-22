/*
 * Some attacks won't register on an enemy
 *  Could be due to the enemy getting animation length from player giving the enemy longer invulnerability
 *		If so, fix by enemy getting the player's attack animation.
 *			If player spams first attack, enemy will then be unhittable. Combine method with existing one
 *  
 * When player goes from moving to idle, a small frame can allow the player to attack
 * while the animation is transitioning, making the player move before the attac animation ends
 *	Make the attack animation be able to interrupt anything
 *	
 * Collision with player and obstacles do not work correctly
 *	Player can phase through objects
 *	Find a way for objects to never allow the player inside them
 *	https://stackoverflow.com/questions/67989772/problem-with-character-passing-through-objects-unity3d
 * 
 * Player's camera will twitch if player is rotating camera in a circle after switching camera modes
 *	Ask unity discord if anyone has had this issue
 *	
 * Player's camera is jittery when player jumps
 * Player is jittery when moving around while targeting an enemy
 *	
 * Target Camera stays in same spot as well as Third-Person Camera, even with transform changing functions
 *  Ask unity discord
 *  Make Third-Person Camera switch to values of Target Camera, so there is only one camera
 */