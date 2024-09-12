//Author: Jonathan Cojita
//File Name: Enemy3.cs
//Project Name: PASS3
//Creation Date: January 13, 2022
//Modified Date: January 14, 2022
//Description: Class for 3rd type of enemy, doesn't move, but shoots in a continuos stream in one direction

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;
using Helper;

namespace Final_PASS
{
    class Enemy3 : Enemy
    {
        //Pre: Needs all valies from parent class, inherits all of them
        //Post: None
        //Desc: Constructor class for Enemy3
        public Enemy3(SpriteBatch sb, Texture2D enemyImg, Vector2 EnemyPos, int dirX, int dirY, int baseHeight, Animation enemyAnim, Rectangle projRec) : base(sb, enemyImg, EnemyPos, dirX, dirY, baseHeight, enemyAnim, projRec)
        {
            //Sets shoot delay and loads shoot timer
            shootDelay = 60;
            shootTimer = new Timer(shootDelay, true);

            //Sets max health and current health
            maxHealth = 400;
            health = maxHealth;        
        }


        //Pre: Enemy is active, shoot delay timer is not active
        //Post: Returns new projectile from stack
        //Desc: Overrides from enemy class, shoots a projectile from the magezine stack
        public override Projectile Shoot()
        {
            //Creates a temporary projectile
            Projectile currentProj;

            //Checks if projectile stack is empty
            if (projectiles.IsEmpty() == false)
            {
                //Reloads the projecile stack
                Reload();
            }

            //Reduces health by one point
            health = health - 1;

            //Resets shoot timer
            shootTimer.ResetTimer(true);

            //Stores top projectile from stack
            currentProj = projectiles.Pop();

            //Sets direction, colour and start position of projectile
            currentProj.SetDirX(dirX);
            currentProj.SetDirY(dirY);
            currentProj.SetColour(Color.Orange);
            currentProj.SetStartPos(new Vector2(enemyAnim.destRec.Center.X - projRec.Width / 3, enemyAnim.destRec.Y));

            //Returns temporary projectile
            return currentProj;
        }
    }
}
