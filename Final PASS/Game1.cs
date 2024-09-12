//Author: Jonathan Cojita
//File Name: Game1.cs
//Project Name: PASS3
//Creation Date: December 29, 2022
//Modified Date: January 21, 2022
//Description: Bullet hell style game with pre-loaded levels and enemies. Game1 is the psuedo main class for entire game

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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Game State related constants
        const int MENU = 0;
        const int INSTRUCTIONS = 2;
        const int GAME = 3;
        const int GAMEOVER = 4;

        //Stores cursor position constants
        const int PLAY_BUTTON_POS = 125;
        const int INSTRUCT_BUTTON_POS = 225;
        const int EXIT_BUTTON_POS = 325;

        //Stores the games current game states
        int gameState = MENU;

        //Stores the game window's width and height
        int gameWidth = 600;
        int gameHeight = 800;
        
        //Stores various graphics and hardware states
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private KeyboardState kb;
        private KeyboardState prevKb;

        //Stores all Menu Related textures
        private Texture2D menuBgImg;
        private Texture2D titleImg;
        private Texture2D playButtonImg;
        private Texture2D instructButtonImg;
        private Texture2D exitButtonImg;
        private Texture2D cursorImg;

        //Stores cursor speed
        private int cursorSpeed = 100;

        //Stores all game related Rectangles
        private Rectangle menuBgRec;
        private Rectangle titleRec;
        private Rectangle playButtonRec;
        private Rectangle instructButtonRec;
        private Rectangle exitButtonRec;
        private Rectangle cursorRec;

        //Stores all timer values relating to game time
        SpriteFont TimerFont;
        private Timer gameTimer;
        Vector2 timerLoc = new Vector2(450, 10);
        string timerOpt = "0";

        //Stores texture and rectangle used in Instruction game state
        private Texture2D instructBgImg;
        private Rectangle instructBgRec;

        //Stores Game over state related textures and rectangles
        private Texture2D loseImg;
        private Rectangle loseRec;
        private Texture2D winImg;
        private Rectangle winRec;

        //Stores values used in Game over state
        private bool didWin = false;
        private SpriteFont resultFont;
        private string resultOpt = "";
        private Vector2 resultLoc = new Vector2(250, 400);

        //Stores texture and rectangle of background
        private Background bg;
        private Texture2D bgImg;
        
        //Stores all player related textures and rectangles
        private Texture2D playerImg;
        private Animation playerAnim;
        private Texture2D pHeartImg;
        private Rectangle pHeartRec;
        private Texture2D pProjImg;
        private Rectangle pProjRec;
        private List<Projectile> activePProj = new List<Projectile>();
        private Texture2D pLifeImg;

        //Stores player related values
        private Vector2 playerPos = new Vector2(275, 700);
        private Player player;
        private int maxLives = 5;

        //Stores Quad Tree related values
        private QuadTree pCollTree;
        private List<Projectile> nearBullets;

        //Stores all enemy type textures
        private Texture2D enemy1Img;
        private Texture2D enemy2Img;
        private Texture2D enemy3Img;

        //Stores enemy projectiles and their texture
        private Texture2D enemyProjImg;
        private List<Projectile> activeEProj = new List<Projectile>();


        //Stores all enemies reguarless of type
        private List<Enemy> enemies = new List<Enemy>();

        //Stores stages and stage related values
        private Stage[] stages = new Stage[5];
        private int curStage = 0;
        private int deadEnemies;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = gameWidth;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = gameHeight;   // set this value to the desired height of your window
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Loads all menu related textures
            menuBgImg = Content.Load<Texture2D>("Images/Background/Menu");
            titleImg = Content.Load<Texture2D>("Images/Sprites/TitleImg");
            playButtonImg = Content.Load<Texture2D>("Images/Sprites/PlayImg");
            instructButtonImg = Content.Load<Texture2D>("Images/Sprites/InstructImg");
            exitButtonImg = Content.Load<Texture2D>("Images/Sprites/Exit");
            cursorImg = Content.Load<Texture2D>("Images/Sprites/Cursor");

            //Loads all menu related rectangles
            menuBgRec = new Rectangle(0, 0, menuBgImg.Width, menuBgImg.Height);
            titleRec = new Rectangle(gameWidth / 2 - 2 * titleImg.Width, 100, 4 * titleImg.Width, 4 * titleImg.Height);
            playButtonRec = new Rectangle(gameWidth / 2 - 2 * playButtonImg.Width, 350, 4 * playButtonImg.Width, 4 * playButtonImg.Height);
            instructButtonRec = new Rectangle(gameWidth / 2 - 2 * instructButtonImg.Width, 450, 4 * instructButtonImg.Width, 4 * instructButtonImg.Height);
            exitButtonRec = new Rectangle(gameWidth / 2 - 2 * exitButtonImg.Width, 550, 4 * exitButtonImg.Width, 4 * exitButtonImg.Height);
            cursorRec = new Rectangle(25, PLAY_BUTTON_POS, 4 * cursorImg.Width, 4 * cursorImg.Width);

            //Loads instruction image and rectangle
            instructBgImg = Content.Load<Texture2D>("Images/Background/InstructBG");
            instructBgRec = new Rectangle(0, 0, (int)(instructBgImg.Width * 1.143), (int)(instructBgImg.Height * 1.143));

            //Loads Game over related images and rectangles
            winImg = Content.Load<Texture2D>("Images/Background/WinImg");
            winRec = new Rectangle(0, 0, (int)(winImg.Width * 1.143), (int)(winImg.Height * 1.143));
            loseImg = Content.Load<Texture2D>("Images/Background/loseImg");
            loseRec = new Rectangle(0, 0, (int)(loseImg.Width * 1.143), (int)(loseImg.Height * 1.143));

            //Loads font used in game over state
            resultFont = Content.Load<SpriteFont>("Fonts/DebugFont");
            
            //Loads timer and its font used during Gameplay state
            TimerFont = Content.Load<SpriteFont>("Fonts/DebugFont");
            gameTimer = new Timer(Timer.INFINITE_TIMER, true);

            //Loads the background texture and rectangle used in GAMEPLAY
            bgImg = Content.Load<Texture2D>("Images/Background/Background");
            bg = new Background(spriteBatch, bgImg, gameHeight);

            //Loads all player related textures, animations and rectangles
            playerImg = Content.Load<Texture2D>("Images/Sprites/Player");
            playerAnim = new Animation(playerImg, 3, 1, 3, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 15, playerPos, 1.3f, true);
            pHeartImg = Content.Load<Texture2D>("Images/Sprites/PlayerHeart");
            pLifeImg = Content.Load<Texture2D>("Images/Sprites/PlayerLife");
            pHeartRec = new Rectangle(playerAnim.destRec.Center.X - (int)(pHeartImg.Width * 0.6) / 2, playerAnim.destRec.Center.Y - (int)(pHeartImg.Height * 0.6) / 2, (int)(pHeartImg.Width * 0.6), (int)(pHeartImg.Height * 0.6));
            pProjImg = Content.Load<Texture2D>("Images/Sprites/PlayerProj");
            pProjRec = new Rectangle(playerAnim.destRec.Left, playerAnim.destRec.Top, pProjImg.Width * 2, pProjImg.Height * 2);

            //Loads enemies' animation and projectile texture
            enemy1Img = Content.Load<Texture2D>("Images/Sprites/Enemy/Enemy1Front");
            enemy2Img = Content.Load<Texture2D>("Images/Sprites/Enemy/Enemy2");
            enemy3Img = Content.Load<Texture2D>("Images/Sprites/Enemy/Enemy3");
            enemyProjImg = Content.Load<Texture2D>("Images/Sprites/Enemy/EnemyProj");

            //Loads instance of player class
            player = new Player(spriteBatch, playerImg, pHeartImg, pLifeImg, playerPos, playerAnim, pHeartRec, pProjRec);

            //Loads instance of quad tree
            pCollTree = new QuadTree(new Rectangle(0, 0, gameWidth, gameHeight));

            //Initializes 5 instances of stage class, refrencing different files to load
            stages[0] = new Stage(spriteBatch, "Stage1.txt", enemyProjImg, enemy1Img, enemy2Img, enemy3Img);
            stages[1] = new Stage(spriteBatch, "Stage2.txt", enemyProjImg, enemy1Img, enemy2Img, enemy3Img);
            stages[2] = new Stage(spriteBatch, "Stage3.txt", enemyProjImg, enemy1Img, enemy2Img, enemy3Img);
            stages[3] = new Stage(spriteBatch, "Stage4.txt", enemyProjImg, enemy1Img, enemy2Img, enemy3Img);
            stages[4] = new Stage(spriteBatch, "Stage5.txt", enemyProjImg, enemy1Img, enemy2Img, enemy3Img);

            //Loads data from each stage class
            for (int i = 0; i < stages.Length; i++)
            {
                stages[i].Load();
            }
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Updates both keyboard states to current values
            prevKb = kb;
            kb = Keyboard.GetState();

            // TODO: Add your update logic here

            //Checks which gamestate the game is currently in
            switch (gameState)
            {
                case MENU:
                    //Calls function that updates menu page                               
                    UpdateMenu();
                    break;

                case INSTRUCTIONS:
                    //Calls function that updates instructions page
                    UpdateInstructions();
                    break;

                case GAME:
                    //Calls function that updates gameplay
                    UpdateGame(gameTime);
                    break;

                case GAMEOVER:
                    //Calls function that updates game over page
                    UpdateGameOver();
                    break;
            }
            
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Starts Spritebatch
            spriteBatch.Begin();

            //Draw Current game state
            switch (gameState)
            {
                case MENU:
                    //Calls function to Draw Menu page
                    DrawMenu();
                    break;

                case INSTRUCTIONS:
                    //Calls function to Draw intstructions page
                    DrawInstuctions();
                    break;

                case GAME:
                    //Calls function to Draw gameplay
                    DrawGame();
                    break;

                case GAMEOVER:
                    //Calls function to Draw game over page
                    DrawGameOver();
                    break;
            }

            //spriteBatch.DrawString(TimerFont, fps, new Vector2(1, 1), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }


        //Pre: Game is in Menu state
        //Post: None
        //Desc: Updates the game while on the menu page
        public void UpdateMenu()
        {
            //Calls function to reset needed game related values
            RestartGame();
            
            //Controls cursor position by checking if either 'w' or 's' is being held down, and if the cursor is able to move further upwards or downwards
            if (kb.IsKeyDown(Keys.W) && !prevKb.IsKeyDown(Keys.W) && cursorRec.Y != PLAY_BUTTON_POS)
            {
                //Moves cursor upwards 1 time by cursor speed
                cursorRec.Y = cursorRec.Y - cursorSpeed;
            }
            else if(kb.IsKeyDown(Keys.S) && !prevKb.IsKeyDown(Keys.S) && cursorRec.Y != EXIT_BUTTON_POS)
            {
                //Moves cursor downwards 1 time by cursor speed
                cursorRec.Y = cursorRec.Y + cursorSpeed;
            }

            //Checks if enter has been pressed, only allows for one input
            if(kb.IsKeyDown(Keys.Enter) && !prevKb.IsKeyDown(Keys.Enter))
            {

                //Checks which button the cursor is currently aligned with
                switch (cursorRec.Y)
                {
                    case PLAY_BUTTON_POS:
                        //Changes gamestate to gameplay
                        gameState = GAME;
                        break;
                    
                    case INSTRUCT_BUTTON_POS:
                        //Changes gamestate to instructions
                        gameState = INSTRUCTIONS;
                        break;
                    
                    //Exit button
                    case EXIT_BUTTON_POS:
                        //Exits the game
                        Exit();
                        break;
                }
            }
        }


        //Pre: Game is in intructions state
        //Post: None
        //Desc: Updates the game while on instructions page
        public void UpdateInstructions()
        {
            //Checks if enter has been pressed, only allows one input
            if (kb.IsKeyDown(Keys.Enter) && !prevKb.IsKeyDown(Keys.Enter))
            {
                //Changes game state to menu
                gameState = MENU;
            }
        }


        //Pre: Game has ended, game is in Game Over state
        //Post: None
        //Desc: Updates game while in game over state
        public void UpdateGameOver()
        {
            //Checks if the player won or lost the game
            if(didWin == false)
            {
                //Changes text to losing text
                resultOpt = "Stage: " + (curStage + 1);
            }
            else
            {
                //Changes text to winning text
                resultOpt = "" + gameTimer.GetTimePassedAsString(Timer.FORMAT_MIN_SEC_MIL);
            }

            //Checks if enter has been pressed, only allows one input
            if (kb.IsKeyDown(Keys.Enter) && !prevKb.IsKeyDown(Keys.Enter))
            {
                //Changes game state to meni
                gameState = MENU;
            }
        }

        
        //Pre: Game is in Gameplay state
        //Post: None
        //Desc: Updates entire game loop while in game state
        public void UpdateGame(GameTime gameTime)
        {
            //Scrolls background
            bg.Move();

            //Updates timer and timer output
            gameTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
            timerOpt = "TIME: " + gameTimer.GetTimePassedAsString(Timer.FORMAT_MIN_SEC_MIL);

            //Updates player's animations and timers
            playerAnim.Update(gameTime);
            player.UpdateTimer(gameTime);

            //Checks if player invinvibility timer is currently active
            if(player.iTimer.IsActive() == false)
            {   
                //if timer is not active, changes player's colour back to original colours
                player.color = Color.White;
            }

            //Updates player and player orb position
            playerAnim = player.Move();
            pHeartRec = player.MoveHeart();

            //Checks if player is out of lives or if all stages have been cleared
            if(player.lives == 0)
            {
                //Ends game and stores that player lost
                didWin = false;
                gameState = GAMEOVER;
            }
            else if (curStage == stages.Length)
            {
                //Ends game and stores that player won
                didWin = true;
                gameState = GAMEOVER;
            }
            else
            {
                //Updates current enemy list to the current stage's enemies
                enemies = stages[curStage].GetEnemies();
            }

            //Checks if space is currently being pressed down
            if (kb.IsKeyDown(Keys.Space))
            {
                //Checks if player's shoot delay timer is currently active
                if (player.shootTimer.IsActive() == false)
                {
                    //Adds a new player projectile to the current active projectile list
                    activePProj.Add(player.Shoot(playerAnim));
                }
            }

            //Loops through every currently active player projectile
            for (int i = 0; i < activePProj.Count(); i++)
            {
                //Moves projectile
                activePProj[i].SetRec(activePProj[i].Move());

                //Checks if the projectile has left the game screen
                if (activePProj[i].GetRec().Intersects(new Rectangle(0, 0, gameWidth, gameHeight)) == false)
                {
                    //Removes and deactivates projectile
                    activePProj.RemoveAt(i);
                }
            }

            //Checks if all enemies have been defeated
            if (enemies.Count == deadEnemies)
            {
                //Resets defeated enemu count and itterates the current stage
                deadEnemies = 0;
                curStage++;
            }

            //Loops through all active enemies and updates all their main functionality
            for (int i = 0; i < enemies.Count; i++)
            {
                //Updates enemy's animation, timer and shoot angle if applicable
                enemies[i].UpdateAnim(gameTime);
                enemies[i].UpdateTimer(gameTime);
                enemies[i].UpdateAngle(gameTime);

                //Only executes code if current enemy is alive
                if (enemies[i].isAlive == true)
                {
                    //Checks if the enemy is currently attacking state
                    if (enemies[i].isActive == true)
                    {
                        //Updates enemy's position
                        enemies[i].Move(gameWidth);

                        //Chekcs if shoot delay timer is currently active
                        if (enemies[i].shootTimer.IsActive() == false)
                        {
                            //Adds projectile to current active enemy projectiles
                            activeEProj.Add(enemies[i].Shoot());
                        }

                        //Checks if any player projectiles are currently active
                        if (activePProj.Count > 0)
                        {
                            //Loops through all active player projectiles
                            for (int j = 0; j < activePProj.Count; j++)
                            {
                                //Checks if current active projectile has hit the current enemy
                                if (enemies[i].GetRec().Intersects(activePProj[j].GetRec()) == true)
                                {
                                    //Removes the player projectile and redcues enemy's health
                                    activePProj.Remove(activePProj[j]);
                                    enemies[i].SetHealth(enemies[i].GetHealth() - player.GetDamage());
                                }
                            }
                        }
                    }
                    //If enemy is not currently active, exceutes non-active functionality
                    else
                    {
                        //Enters enemy into the screen
                        enemies[i].Enter();
                    }

                    //Checks if current enemy's health is zero
                    if(enemies[i].GetHealth() == 0)
                    {
                        //Sets the enemy to not alive and itterates the defeat count
                        deadEnemies++;                    
                        enemies[i].isAlive = false;
                    }
                }
            }

            //Calls function checking if enemy is currently colliding with screen border
            player.WallColl(gameWidth, gameHeight);

            //Re-instates quad tree, updating its data
            pCollTree = new QuadTree(new Rectangle(0, 0, gameWidth, gameHeight));

            //Loops through every currently active enemy projectile
            for (int i = 0; i < activeEProj.Count; i++)
            {
                //Updates position of projectile
                activeEProj[i].Move();

                //Inserts current projectile into the Quad Tree
                pCollTree.insert(activeEProj[i]);

                //Checks if current enemy projectile is out of the game screne
                if (activeEProj[i].GetRec().Intersects(new Rectangle(0, 0, gameWidth, gameHeight)) == false)
                {
                    //Removes the projectile
                    activeEProj.RemoveAt(i);
                }
            }

            //Stores all potentially colliding enemy projectiles by querying the Quad Tree
            nearBullets = pCollTree.Query(playerAnim.destRec);

            //Loops through every possibly colliding projectile
            for (int i = 0; i < nearBullets.Count; i++)
            {
                //Checks if projectile is colliding with the player orb
                if (pHeartRec.Intersects(nearBullets[i].GetRec()) == true && player.iTimer.IsActive() == false)
                {   
                    //Clears the screen of enemy projectiles and calls a function to update player to a hit state
                    activeEProj.Clear();
                    player.Hit();
                }
            }
        }


        //Pre: Game is in Menu state
        //Post: None
        //Desc: Draws all menu related objects
        public void DrawMenu()
        {
            //Draws all objects
            spriteBatch.Draw(menuBgImg, menuBgRec, Color.White);
            spriteBatch.Draw(titleImg, titleRec, Color.White);
            spriteBatch.Draw(playButtonImg, playButtonRec, Color.White);
            spriteBatch.Draw(instructButtonImg, instructButtonRec, Color.White);
            spriteBatch.Draw(exitButtonImg, exitButtonRec, Color.White);
            spriteBatch.Draw(cursorImg, cursorRec, Color.White);
        }

        //Pre: Game is in instruction state
        //Post: None
        //Desc: Draws all instruction related objects
        public void DrawInstuctions()
        {
            //Draws insturction page background
            spriteBatch.Draw(instructBgImg, instructBgRec, Color.White);
        }


        //Pre: Game is in Game Over state
        //Post: None
        //Desc: Draws all Game Over related objects
        public void DrawGameOver()
        {
            //Checks if the player won the game or not
            if(didWin == false)
            {
                //Draws losing image
                spriteBatch.Draw(loseImg, loseRec, Color.White);
            }
            else
            {
                //Draws winning image
                spriteBatch.Draw(winImg, winRec, Color.White);
            }

            //Draws result text
            spriteBatch.DrawString(resultFont, resultOpt, resultLoc, Color.White);
        }

        //Pre: Game is in Gameplay state
        //Post: None
        //Desc: Draws all Gameplay related objects
        public void DrawGame()
        {
            //Draws background
            bg.Draw();        

            //Checks if current active player projectiles is not zero
            if (activePProj.Count() != 0)
            {
                //Loops through all active projectiles
                for (int i = 0; i < activePProj.Count; i++)
                {
                    //Draws current player projectile
                    spriteBatch.Draw(pProjImg, activePProj[i].GetRec(), Color.White);
                }
            }

            //Checks if current enemy projectiles is not zero
            if (activeEProj.Count() != 0)
            {
                //Loops through all enemy projectiles
                for (int i = 0; i < activeEProj.Count; i++)
                {
                    //Draws current enemy pproejtile
                    spriteBatch.Draw(enemyProjImg, activeEProj[i].GetRec(), activeEProj[i].GetColor());
                }
            }

            //Loops through every active enemy
            for (int i = 0; i < enemies.Count; i++)
            {
                //Draws current enemy
                enemies[i].Draw();
            }

            //Calls function to draw player and player orb
            player.Draw();

            //Draws text displaying game time
            spriteBatch.DrawString(TimerFont, timerOpt, timerLoc, Color.Black);
        }

        //Pres: Game is in menu state
        //Post: None
        //Desc: Resets needed game values
        public void RestartGame()
        {
            //Resets various stage related values
            activeEProj.Clear();
            deadEnemies = 0;
            curStage = 0;

            //Resets player's lives
            player.lives = maxLives;

            //Resets game timer
            gameTimer.ResetTimer(true);

            //Loops through all stages
            for (int i = 0; i < stages.Length; i++)
            {
                //Loops through all enemies in current stage
                for (int j = 0; j < stages[i].GetEnemies().Count; j++)
                {
                    //Resets various enemy related values
                    stages[i].GetEnemies()[j].SetHealth(stages[i].GetEnemies()[j].maxHealth);
                    stages[i].GetEnemies()[j].RestartPos();
                    stages[i].GetEnemies()[j].isAlive = true;
                    stages[i].GetEnemies()[j].isActive = false;
                }
            }
        }
    } 
}
