//Author: Jonathan Cojita
//File Name: Enemy1.cs
//Project Name: PASS3
//Creation Date: January 11, 2022
//Modified Date: January 12, 2022
//Description: Class for 1st type of enemy, moves in a Sine-wave and shoots straight downwards

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
    class Enemy1 : Enemy
    {
        //Pre: Needs all values all parent class has, inherits all of them
        //Post: None
        //Desc: Constructor class for Enemy1
        public Enemy1(SpriteBatch sb, Texture2D enemyImg, Vector2 EnemyPos, int dirX, int dirY, int baseHeight, Animation enemyAnim, Rectangle projRec) : base(sb, enemyImg, EnemyPos, dirX, dirY, baseHeight, enemyAnim, projRec)
        {
            //Sets the value of delay between shots and loads timer
            shootDelay = 250;
            shootTimer = new Timer(shootDelay, true);

            //Sets max health value and matches current  health to max health
            maxHealth = 20;
            health = maxHealth;
        }

        //Pre: Needs int representing game width
        //Post: Returns rectangle with new position
        //Desc: Overrides from enemy class, updates the enemy's position
        public override Rectangle Move(int gameWidth)
        {
            //Checks which direction the enemy is currently moving in
            if(dirX == POSITIVE)
            {
                //Checks if enemy is colliding with screen border on right side
                if(enemyAnim.destRec.X + enemyAnim.destRec.Width == gameWidth)
                {
                    //Changes direction
                    dirX = NEGATIVE;
                }
            }
            else if(dirX == NEGATIVE)
            {
                //Checks if enemy is colliding with screen border on left side
                if (enemyAnim.destRec.X == 0)
                {
                    //Changes direction
                    dirX = POSITIVE;
                }
            }

            //Updates x and y position
            pos.X = pos.X + (dirX * speed);
            pos.Y =  (float)(20 * Math.Sin(0.125 * pos.X)) + baseHeight;

            //Updates animation's position using updated postion
            enemyAnim.destRec.X = (int)pos.X;
            enemyAnim.destRec.Y = (int)pos.Y;

            //Retunrs updated rectangle
            return enemyAnim.destRec;
        }


        //Pre: Enemy is active, shoot delay timer is not active
        //Post: Returns new projectile from stack
        //Desc: Overrides from enemy class, shoots a projectile from the magezine stack
        public override Projectile Shoot()
        {
            //Creates a temporary projectile
            Projectile currentProj;

            //Checks if projectile stack is currently empty
            if (projectiles.IsEmpty() == false)
            {
                //Reloads entire projectile stack
                Reload();
            }

            //Resets the shoot timer
            shootTimer.ResetTimer(true);

            //Sets temp. projectile to top projectile in stack
            currentProj = projectiles.Pop();

            //Updates position and colour of projectile
            currentProj.SetColour(Color.Red);
            currentProj.SetStartPos(new Vector2(enemyAnim.destRec.Center.X - projRec.Width / 3, enemyAnim.destRec.Y));

            //Returns projectile
            return currentProj;            
        }
    }
}
