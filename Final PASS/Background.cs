//Author: Jonathan Cojita
//File Name: Background.cs
//Project Name: PASS3
//Creation Date: January 3, 2022
//Modified Date: January 3, 2022
//Description: Class for gameplay Background

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Final_PASS
{
    class Background
    {
        //Stores an instance of the sprite batch
        SpriteBatch spriteBatch;

        //Stores the height of the game border
        int gameHeight;

        int speed = 1;

        //Stores the Texture and rectangle of the background
        Texture2D bgImg;
        Rectangle bg1Rec;
        Rectangle bg2Rec;

        //Pre: Needs an instance of the sprite batch, the texture of the backgroun and an int representing the game height
        //Post: None
        //Desc: Constructor for the background class
        public Background(SpriteBatch spriteBatch, Texture2D bgImg, int gameHeight)
        {
            //Stores values for all parameters
            this.spriteBatch = spriteBatch;
            this.gameHeight = gameHeight;
            this.bgImg = bgImg;

            //Initializes rectangles for both backgrounds
            bg1Rec = new Rectangle(0, 0, (int)(bgImg.Width * 0.36), (int)(bgImg.Height * 0.36));
            bg2Rec = new Rectangle(0, -(int)(bgImg.Height * 0.36), (int)(bgImg.Width * 0.36), (int)(bgImg.Height * 0.36));
        }


        //Pre: None
        //Post: None
        //Desc: Draws both backgrounds
        public void Draw()
        {
            spriteBatch.Draw(bgImg, bg1Rec, Color.White);
            spriteBatch.Draw(bgImg, bg2Rec, Color.White);
        }


        //Pre: None
        //Post: None
        //Desc: Scrolls the background images
        public void Move()
        {
            //Updates position of both rectangles
            bg1Rec.Y = bg1Rec.Y + speed;
            bg2Rec.Y = bg2Rec.Y + speed;

            //Checks if background 1 or background 2 are fully covering the screen
            if(bg1Rec.Y + bg1Rec.Height == gameHeight)
            {
                //Resets the position of background 2
                bg2Rec.Y = -bg2Rec.Height;
            }
            else if (bg2Rec.Y + bg2Rec.Height == gameHeight)
            {
                //Resets the position of background 1
                bg1Rec.Y = -bg1Rec.Height;
            }
        }
    }
}
