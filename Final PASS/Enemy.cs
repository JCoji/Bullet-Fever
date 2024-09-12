//Author: Jonathan Cojita
//File Name: Enemy.cs
//Project Name: PASS3
//Creation Date: January 11, 2022
//Modified Date: January 11, 2022
//Description: Generic enemy class, parent class of all other enemy classes

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
    class Enemy
    {
        //Constants storing direction
        protected const int NEGATIVE = -1;
        protected const int STOP = 0;
        protected const int POSITIVE = 1;

        //Stores an instance of the sprite batch
        protected SpriteBatch sb;

        //Stores the current directtion of the enemy/enemy projectile
        protected int dirX = NEGATIVE;
        protected int dirY = NEGATIVE;

        //Stores texture and animation
        protected Texture2D enemyImg;
        protected Animation enemyAnim;

        //Stores current position, spawn position and relative/exact attacking position
        protected Vector2 pos;
        protected Vector2 startPos;
        protected int baseHeight;

        //Stores projectile rectangle
        protected Rectangle projRec;

        //Stores projectile speed
        int projSpeed = 3;

        //Stores shoot delay timer related data
        public Timer shootTimer;
        public Timer reloadTimer;
        protected int shootDelay;

        //Stores whether enemy is alive and active or not
        public bool isActive = false;
        public bool isAlive = true;

        //Stores movement speed
        protected int speed = 1;

        //Stores current health and max health
        protected int health = 0;
        public int maxHealth;
    
        //Stores maximum projectile count and projectile stack
        public static int projCount = 100;
        protected Magazine projectiles = new Magazine(projCount);


        //Pre: Needs instance of sprite batch, texture of enemy and projectile, vector representing position, 2 integers representding direction, an animation for the sprite and a rectnagle for the projectile
        //Post: None
        //Desc: Constructor for the Enemy Class
        public Enemy(SpriteBatch sb, Texture2D enemyImg, Vector2 enemyPos, int dirX, int dirY, int baseHeight, Animation enemyAnim, Rectangle projRec)
        {
            //Stores all paramaters
            this.sb = sb;
            this.enemyImg = enemyImg;
            pos = enemyPos;
            startPos = enemyPos;
            this.dirX = dirX;
            this.dirY = dirY;
            this.baseHeight = baseHeight;
            this.enemyAnim = enemyAnim;
            this.projRec = projRec;

            //Loops until reaching desired projectile count
            for (int i = 0; i < projCount; i++)
            {
                //Pushes current projectile to magezine stack
                projectiles.Push(new Projectile(projRec, enemyPos, 0, 1, projSpeed));
            }
        }


        //Pre: None
        //Post: Rectangle respresenting the enemy
        //Desc: Returns destRec for enemy
        public Rectangle GetRec()
        {
            return enemyAnim.destRec;
        }

        //Pre: None
        //Post: Returns enemies current health
        //Desc: Gets enemy's current healt
        public int GetHealth()
        {
            return health;
        }


        //Pre: None
        //Post: Returns enemy's total projectile count
        //Desc: Gets enemy's total projectile count
        public int GetProjCount()
        {
            return projCount;
        }


        //Pre: Needs new health value
        //Post: None
        //Desc: Sets enemy's health to given value
        public void SetHealth(int health)
        {
            this.health = health;
        }


        //Pre: None
        //Post: Bool representing if projectile stack is empty
        //Desc: Checks and returns if projectile stack is empty
        public bool IsProjEmpty()
        {
            //Checks if stack is empty, returns result
            if(projectiles.IsEmpty() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Pre: Needs current game time
        //Post: None
        //Desc: Updates all the enemis timers
        public void UpdateTimer(GameTime gameTime)
        {
            //Checks if shoot timer is active
            if(shootTimer.IsActive() == true)
            {
                //Updates shoot timer
                shootTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            //Checks if shoot timer is finished
            if(shootTimer.IsFinished() == true)
            {
                //Deactivates shoot timer
                shootTimer.Deactivate();
            }
        }

        //Pre: None
        //Post: None
        //Desc: Moves new enemy onto the screen
        public void Enter()
        {
            //Checks if enemy has reached desired y-value
            if (enemyAnim.destRec.Y == baseHeight)
            {
                //Activates enemy
                isActive = true;
            }
            else
            {
                //Moves enemy towards desired y-value
                enemyAnim.destRec.Y = enemyAnim.destRec.Y + 2 * speed;
            }
        }


        //Pre: Needs integer representing the game width
        //Post: Returns Enemy's new position
        //Desc: Virtual movement class for enemies
        public virtual Rectangle Move(int gameWidth)
        {
            return enemyAnim.destRec;
        }


        //Pre: None
        //Post: Returns top projectile in stack
        //Desc: Virtual shoot class for enemies
        public virtual Projectile Shoot()
        {
            return projectiles.Pop();
        }


        //Pre: None
        //Post: None
        //Desc: Virtual reload class for enemies
        public virtual void Reload()
        {
            //Loops until reaching desired projectil count
            for (int i = 0; i < projCount; i++)
            {
                //Add a new projectile to the stack
                projectiles.Push(new Projectile(projRec, new Vector2(0,0), 0, 1, projSpeed));
            }
        }


        //Pre: none
        //Post: none
        //Desc: Draws enemy
        public void Draw()
        {
            //Checks if enemy is alive
            if(isAlive == true)
            {
                //Draws enemy
                enemyAnim.Draw(sb, Color.White, Animation.FLIP_NONE);
            }
            
        }


        //Pre: Needs current Game Time
        //Post:  none
        //Desc: Updates animation for enemy
        public void UpdateAnim(GameTime gameTime)
        {
            enemyAnim.Update(gameTime);
        }


        //Pre: Needs current game time
        //Post: None
        //Desc: Virtual function that is used to update shoot angle of enemy
        public virtual void UpdateAngle(GameTime gameTime)
        {
        }


        //Pre: None
        //Post: None
        //Desc: Resests position of enemy to spawn position
        public void RestartPos()
        {
            //Resets Position
            pos.X = startPos.X;
            pos.Y = startPos.Y;

            //Resets animation position
            enemyAnim.destRec.X = (int)startPos.X;
            enemyAnim.destRec.Y = (int)startPos.Y;
        }
    }
}
