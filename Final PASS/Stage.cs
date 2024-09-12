//Author: Jonathan Cojita
//File Name: Stage.cs
//Project Name: PASS3
//Creation Date: January 14, 2022
//Modified Date: January 16, 2022
//Description: Reads data from a .txt file and loads enemies based on file data

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
    class Stage
    {
        //Stores maximun number of enemies that can be stored in 1 stage
        const int MAX_ENEMY_COUNT = 15;

        //Stores instance of sprite batch
        SpriteBatch spriteBatch;

        //Stores readable file and it's name
        StreamReader inFile;
        string fileName;

        //Stores a list of enemies
        private List<Enemy> enemies = new List<Enemy>();
        
        //Stores enemy and enemy projectile textures
        Texture2D projImg;
        Texture2D enemy1Img;
        Texture2D enemy2Img;
        Texture2D enemy3Img;

        //Stores all enemy values that will be updated by file
        string[] enemyCount = new string[3];
        string[] posX = new string[MAX_ENEMY_COUNT];
        string[] posy = new string[MAX_ENEMY_COUNT];
        string[] dirX = new string[MAX_ENEMY_COUNT];
        string[] dirY = new string[MAX_ENEMY_COUNT];
        string[] baseHeights = new string[MAX_ENEMY_COUNT];

        //Stores rectangle and animation of enemy projectiles and enemy sprites
        Rectangle projRec;
        List<Animation> enemy1Anims = new List<Animation>();
        List<Animation> enemy2Anims = new List<Animation>();
        List<Animation> enemy3Anims = new List<Animation>();

        //Pre: Stage class has been called, instance of sprite batch, a file name, a texture of each enemy and their projectile
        //Post: None
        //Desc: Constructor class of Stage class
        public Stage(SpriteBatch spriteBatch, string fileName, Texture2D projImg, Texture2D enemy1Img, Texture2D enemy2Img, Texture2D enemy3Img)
        {
            //Stores all parameters
            this.spriteBatch = spriteBatch;
            this.fileName = fileName;
            this.projImg = projImg;
            this.enemy1Img = enemy1Img;
            this.enemy2Img = enemy2Img;
            this.enemy3Img = enemy3Img;

            //Stores value of each line of file
            string[] line = new string[6];
            
            //Opens readable stage file containing enemy data
            inFile = File.OpenText(fileName);

            //Loops through every line of file
            for (int i = 0; i < line.Length; i++)
            {
                //Reads and stores current line's data
                line[i] = inFile.ReadLine();
            }
            
            //Closes file
            inFile.Close();

            //Stores and splits all file line data
            enemyCount = line[0].Split(',');
            posX = line[1].Split(',');
            posy = line[2].Split(',');
            dirX = line[3].Split(',');
            dirY = line[4].Split(',');
            baseHeights = line[5].Split(',');
        }


        //Pre: Instance of stage class is present
        //Post: A list of enemies stored by current stage instance
        //Desc: Returns a list of enemies owned by the current stage
        public List<Enemy> GetEnemies()
        {
            return enemies;
        }

        //Pre: Instance of stage class is present
        //Post: None
        //Desc: Loads all enemies based on prev. stored file data
        public void Load()
        {
            //Creates a new rectangle for enemy projectile
            projRec = new Rectangle(0, 0, projImg.Width, projImg.Height);


            //Loops until desired number of type 1 enemy is loaded
            for (int i = 0; i < Convert.ToInt32(enemyCount[0]); i++)
            {
                //Loads a new enemy1 animation
                enemy1Anims.Add(new Animation(enemy1Img, 3, 1, 3, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 10, new Vector2(Convert.ToInt32(posX[i]), Convert.ToInt32(posy[i])), 1.3f, true));

                //Adds enemy to enemy list, uses file data
                enemies.Add(new Enemy1(spriteBatch, enemy1Img, new Vector2(Convert.ToInt32(posX[i]), Convert.ToInt32(posy[i])), Convert.ToInt32(dirX[i]), Convert.ToInt32(dirY[i]), Convert.ToInt32(baseHeights[i]), enemy1Anims[i], projRec));
            }

            //Loops until desired number of type 2 enemy is loaded
            for (int i = 0; i < Convert.ToInt32(enemyCount[1]); i++)
            {
                //Loads a new enemy2 animation
                enemy2Anims.Add(new Animation(enemy2Img, 3, 1, 3, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 10, new Vector2(Convert.ToInt32(posX[i + 5]), Convert.ToInt32(posy[i + 5])), 1.3f, true));

                //Adds enemy to enemy list, uses file data
                enemies.Add(new Enemy2(spriteBatch, enemy2Img, new Vector2(Convert.ToInt32(posX[i + 5]), Convert.ToInt32(posy[i + 5])), Convert.ToInt32(dirX[i + 5]), Convert.ToInt32(dirY[i + 5]), Convert.ToInt32(baseHeights[i + 5]), enemy2Anims[i], projRec));
            }

            //Loops until desired number of type 3 enemy is loaded
            for (int i = 0; i < Convert.ToInt32(enemyCount[2]); i++)
            {
                //Loads a new enemy2 animation
                enemy3Anims.Add(new Animation(enemy3Img, 3, 1, 3, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 10, new Vector2(Convert.ToInt32(posX[i + 10]), Convert.ToInt32(posy[i + 10])), 1.3f, true));

                //Adds enemy to enemy list, uses file data
                enemies.Add(new Enemy3(spriteBatch, enemy3Img, new Vector2(Convert.ToInt32(posX[i + 10]), Convert.ToInt32(posy[i + 10])), Convert.ToInt32(dirX[i + 10]), Convert.ToInt32(dirY[i + 10]), Convert.ToInt32(baseHeights[i + 10]), enemy3Anims[i], projRec));
            }
        }
    }
}
