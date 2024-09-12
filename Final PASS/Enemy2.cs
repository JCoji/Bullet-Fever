//Author: Jonathan Cojita
//File Name: Enemy2.cs
//Project Name: PASS3
//Creation Date: January 12, 2022
//Modified Date: January 13, 2022
//Description: Class for 2nd type of enemy, doesn't move, but shoots in a circular pattern

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
    class Enemy2 : Enemy
    {
        //Stores all shoot anfle related values
        private float shootAngle;
        int rotSpeed = 10;
        int radius = 50;

        
        //Pre: Needs all values from parent class, inherits all of them
        //Post: None
        //Desc: Constructor class for Enemy2
        public Enemy2(SpriteBatch sb, Texture2D enemyImg, Vector2 EnemyPos, int dirX, int dirY, int baseHeight, Animation enemyAnim, Rectangle projRec) : base(sb, enemyImg, EnemyPos, dirX, dirY, baseHeight, enemyAnim, projRec)
        {
            //Sets the shoot delay and loads timer
            shootDelay = 10;
            shootTimer = new Timer(shootDelay, true);

            //Sets max health and current health
            maxHealth = 100;
            health = maxHealth;         
        }


        //Pre: Enemy is active, shoot delay timer is not active
        //Post: Returns new projectile from stack
        //Desc: Overrides from enemy class, shoots a projectile from the magezine stack
        public override Projectile Shoot()
        {
            //Creates a temporary projectile
            Projectile currentProj;

            //Checks if projectile stack is emppty
            if (projectiles.IsEmpty() == false)
            {
                //Reloads stack
                Reload();
            }

            //Resets shoot timer
            shootTimer.ResetTimer(true);

            //Stores top projectile in stack
            currentProj = projectiles.Pop();

            //Sets the start position of the projectile to the center of the enemy
            currentProj.SetStartPos(new Vector2(enemyAnim.destRec.Center.X - projRec.Width / 3 + radius * (float)Math.Sin(shootAngle), enemyAnim.destRec.Center.Y + radius * (float)Math.Cos(shootAngle)));

            //Sets the direction and colour of the projectile
            currentProj.SetDirX(Math.Sin(shootAngle));
            currentProj.SetDirY(Math.Cos(shootAngle));
            currentProj.SetColour(Color.Cyan);
            
            //Returns temp. projectile
            return currentProj;
        }


        //Pre: Needs game time
        //Post: None
        //Desc Overrides the parent class function, updates the angle at which the enemy shoots at
        public override void UpdateAngle(GameTime gameTime)
        {
            shootAngle = shootAngle + (float)gameTime.ElapsedGameTime.TotalSeconds * rotSpeed;
        }
    }
}

