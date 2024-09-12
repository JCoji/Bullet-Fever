//Author: Jonathan Cojita
//File Name: Projectile.cs
//Project Name: PASS3
//Creation Date: December 29, 2022
//Modified Date: January 2, 2022
//Description: Class for player, stores and maintains almost all player related functions and variables

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Animation2D;
using Helper;

namespace Final_PASS
{
    class Player
    {
        //Stores constants representing direction
        const int NEGATIVE = -1;
        const int STOP = 0;
        const int POSITIVE = 1;

        //Stores Instance of keboard state
        KeyboardState kb;

        //Storees and instance of the sprite batch
        SpriteBatch sb;

        //Stores player speed and curren direction
        int speed = 5;
        int dirX = STOP;
        int dirY = STOP;

        //Stores the player orb's current visibility
        bool showHeart = false;

        //Stores shoot delay timer as well as its delay time
        public Timer shootTimer;
        int shootDelay = 100;

        //Stores maximun number of lives, all current lives and their texture
        public int lives = 5;
        List<Rectangle> lifeRecs = new List<Rectangle>();
        Texture2D lifeImg;

        //Stores Invisinsibility timer, invisibility timer length and if player is currently active
        public Timer iTimer;
        int iDelay = 1000;
        public bool isActive = true;

        //Stores the current colour of the player
        public Color color = Color.White;

        //Stores the maximum number of projectiles in player stack, the speed of projectiles and their damage per hit
        static int projCount = 25;
        int projSpeed = 25;
        int damage = 1;

        //Stores the texture of the player and player heart
        Texture2D playerImg;
        Texture2D heartImg;

        //Stores the player's spawn position and its current position
        Vector2 spawnPos;
        Vector2 playerPos;

        //Stores the animation of the player, and rectangle of the orb and projectile
        Animation playerAnim;
        Rectangle pHeartRec;
        Rectangle pProjRec;

        //Stores all player projectiles and their starting position
        public Magazine projectiles = new Magazine(projCount);
        Vector2 projStartLoc;


        //Pre: Needs and instance of the sprite batch, texures of the player, orb and player projectile, vectore representing player's position, Animation for the player and rectangles for the orb and projectile
        //Post: None
        //Desc: Consteuctor for the player class
        public Player(SpriteBatch sb, Texture2D playerImg, Texture2D heartImg, Texture2D lifeImg, Vector2 playerPos, Animation playerAnim, Rectangle pHeartRec, Rectangle pProjRec)
        {
            //Stores all paramater values
            this.sb = sb;
            this.playerImg = playerImg;
            this.heartImg = heartImg;
            this.lifeImg = lifeImg;
            this.playerPos = playerPos;
            spawnPos = playerPos;
            this.playerAnim = playerAnim;
            this.pHeartRec = pHeartRec;
            this.pProjRec = pProjRec;

            //Loads both timers
            shootTimer = new Timer(shootDelay, true);
            iTimer = new Timer(iDelay, false);

            //Loops until reaching max number of lives
            for (int i = 0; i < lives; i++)
            {
                //adds a new life to the list
                lifeRecs.Add(new Rectangle((i * lifeImg.Width * 4) + 10, 10, lifeImg.Width * 4, lifeImg.Height * 4));
            }

            //Sets the projectile postion to the player's poition and loads on full stack of prejectiles
            projStartLoc = new Vector2(playerAnim.destRec.Center.X, playerAnim.destRec.Y);
            ReloadProj();
        }

        
        //Pre: None
        //Post: Returns player's damage
        //Desc: Gets player's damage per hit value
        public int GetDamage()
        {
            return damage;
        }


        //Pre: None
        //Post: Returns player projectile count
        //Desc: Recturns player's maximum projectile count
        public int GetProjCount()
        {
            return projCount;
        }

        //Pre: None
        //Post: None
        //Desc: Resets speed back to original value
        private void ResetSpeed()
        {
            speed = 5;
        }


        //Pre: None
        //Post: None
        //Desc: Moves player and updates position
        public Animation Move()
        {
            //Updates current keyboard state
            kb = Keyboard.GetState();

            //Checks if left shift is currently being held down
            if (kb.IsKeyDown(Keys.LeftShift))
            {
                //Slows down movement speed and sets player orb to visible
                speed = 2;
                showHeart = true;
            }
            else
            {
                //Resturns player speed to orignal value, and sets player orb to invisible
                ResetSpeed();
                showHeart = false;
            }

            //Checks if either D or A is being held down
            if (kb.IsKeyDown(Keys.D))
            {
                //Changes current x-direction to positive
                dirX = POSITIVE;
            }
            else if (kb.IsKeyDown(Keys.A))
            {
                //Changes current x-direction to negative
                dirX = NEGATIVE;
            }
            else
            {
                //Stops player movement in x-direction
                dirX = STOP;
            }

            //Checks if whether S or W is being held down
            if (kb.IsKeyDown(Keys.S))
            {
                //Changes current y-direction to positive
                dirY = POSITIVE;
            }
            else if (kb.IsKeyDown(Keys.W))
            {
                //Changes current y-direction to negative
                dirY = NEGATIVE;
            }
            else
            {
                //Stops player movement in y-direction
                dirY = STOP;
            }

            //Update position and animation
            playerPos.X = playerPos.X + (dirX * speed);
            playerPos.Y = playerPos.Y + (dirY * speed);
            playerAnim.destRec.X = (int)playerPos.X;
            playerAnim.destRec.Y = (int)playerPos.Y;

            //Returns updated animation
            return playerAnim;
        }


        //Pre: Space is currently pressed and shoot delay timer is not actrive, Needs player animation
        //Post: Returns top projectile from stack
        //Desc: Player shoots top projectile from stack
        public Projectile Shoot(Animation playerAnim)
        {
            //Creates a temporaru projectile
            Projectile currentProj;

            //Checks if the projectile stack is currently empty
            if(projectiles.IsEmpty() == true)
            {
                //Reloads projectile stack
                ReloadProj();
            }

            //Resets the shoot delay timer
            shootTimer.ResetTimer(true);

            //Stores the top projectile from the stack
            currentProj = projectiles.Pop();

            //Sets the start position of the projectile
            currentProj.SetStartPos(new Vector2(playerAnim.destRec.Center.X - pProjRec.Width / 3, playerAnim.destRec.Y));

            //Returns the current projecile
            return currentProj;
        }


        //Pre: Needs current game time
        //Post: None
        //Desc: Updates all player timers
        public void UpdateTimer(GameTime gameTime)
        {
            //Checks if shoot timer is currently active
            if (shootTimer.IsActive() == true)
            {
                //Updates shoot timer
                shootTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            //Checks if shoot timer is finished
            if (shootTimer.IsFinished() == true)
            {
                //Deactivates shoot timer
                shootTimer.Deactivate();
            }

            //Checks if invinsibility timer is active
            if (iTimer.IsActive() == true)
            {
                //Updates invincibility timer
                iTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            //Checks if invincibility timer is finshed
            if (iTimer.IsFinished() == true)
            {
                //Deactivates invinsibility timer
                iTimer.Deactivate();
            }
        }


        //Pre: None 
        //Post: Resturns orbs updated position
        //Desc: Updates player orb position to player's current position
        public Rectangle MoveHeart()
        {
            //Sets orb position to player's center
            pHeartRec.X = playerAnim.destRec.Center.X - pHeartRec.Width / 2;
            pHeartRec.Y = playerAnim.destRec.Center.Y - pHeartRec.Height / 2;

            //Returns updated position
            return pHeartRec;
        }


        //Pre: None
        //Post: None
        //Desc: Detects if player is colliding with screen border and updates position accordingly
        public void WallColl(int gameWidth, int gameHeight)
        {
            //Checks if player is colliding with right wall or left wall
            if (playerPos.X <= 0)
            {
                //Moves player back
                playerPos.X = playerPos.X + speed;
            }
            else if (playerAnim.destRec.Right >= gameWidth)
            {
                //Moves player back
                playerPos.X = playerPos.X - speed;
            }

            //Checks if player is colliding with top or bottom wall
            if (playerPos.Y <= 0)
            {
                //Moves player back
                playerPos.Y = playerPos.Y + speed;
            }
            else if (playerAnim.destRec.Bottom >= gameHeight)
            {
                //Moves player back
                playerPos.Y = playerPos.Y - speed;
            }

        }


        //Pre: None
        //Post: None
        //Desc: Reloads projectile stack
        private void ReloadProj()
        {
            //Loops until reaching max projectile count
            for (int i = 0; i < projCount; i++)
            {
                //Pushes new projectile to stack
                projectiles.Push(new Projectile(pProjRec, projStartLoc, 0, -1, projSpeed));
            }
        }


        //Pre: None
        //Post: None
        //Desc: Draws player and player related sprites
        public void Draw()
        {
            //Draws player animation
            playerAnim.Draw(sb, color, Animation.FLIP_NONE);

            //Checks if orb is currently visible
            if (showHeart == true)
            {
                //Draws player orb
                sb.Draw(heartImg, pHeartRec, color);
            }

            //Loops untile reaching current number of lives
            for (int i = 0; i < lives; i++)
            {
                //Draws current player life
                sb.Draw(lifeImg, lifeRecs[i], Color.White);
            }
        }


        //Pre: None
        //Post: None
        //Desc: Changes player values when a hit has been detected
        public void Hit()
        {
            //Removes one life from the life count and activates invinsibility timer
            lives--;
            iTimer.ResetTimer(true);

            //Sets player colour to gray
            color = Color.Gray;
            
            //Resets player position back to spawn position
            playerPos.X = spawnPos.X;
            playerPos.Y = spawnPos.Y;
        }
    }
}
