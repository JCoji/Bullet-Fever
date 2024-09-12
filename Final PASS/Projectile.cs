//Author: Jonathan Cojita
//File Name: Projectile.cs
//Project Name: PASS3
//Creation Date: January 4, 2022
//Modified Date: January 4, 2022
//Description: Class for projectile, used for both player and enemy projectiles

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Animation2D;

namespace Final_PASS
{
    class Projectile
    {
        //Stores the projectiles rectoangle and starting position
        Rectangle projRec;
        Vector2 startPos;

        //Stores the projectiles direction and speed
        double dirX;
        double dirY;
        int speed;

        //Stores the projectiles visibility and colour
        bool isVisible = false;
        Color color = Color.White;

        //Pre: Recatngle storing current position, vector representing starting position, 2 doubles representing X and Y position, and an integer storing speed
        //Post:
        //Desc:
        public Projectile(Rectangle projRec, Vector2 startPos, double dirX, double dirY, int speed)
        {
            //Stores all data given my parameters
            this.projRec = projRec;
            this.startPos = startPos;
            this.dirX = dirX;
            this.dirY = dirY;
            this.speed = speed;          
        }


        //Pre: None
        //Post: Returns current dectRec
        //Desc: Gets current destRec
        public Rectangle GetRec()
        {
            return projRec;
        }


        //Pre: None
        //Post: Returns starting position
        //Desc: Gets vector representing starting position
        public Vector2 GetStartPos()
        {
            return startPos;
        }


        //Pre: None
        //Post: returns color
        //Desc: gets projectiles current color
        public Color GetColor()
        {
            return color;
        }


        //Pre: None
        //Post: Returns visibility
        //Desc: Gets bool representing projectile's current visibility
        public bool GetVisibility()
        {
            return isVisible;
        }


        //Pre: None
        //Post: Returns speed
        //Desc: gets integer representing the projectiles speed
        public int GetSpeed()
        {
            return speed;
        }


        //Pre: Rectangle representing destRec
        //Post: None
        //Desc: Sets current projectile rectangle to give value
        public void SetRec(Rectangle projRec)
        {
            this.projRec = projRec;
        }


        //Pre: Needs a color
        //Post: None
        //Desc: Sets colour of projectile to given value
        public void SetColour(Color color)
        {
            this.color = color;
        }


        //Pre: Needs a vector representing start position
        //Post: None
        //Desc: Sets start position of projectile to given value
        public void SetStartPos(Vector2 startPos)
        {
            projRec.X = (int)startPos.X;
            projRec.Y = (int)startPos.Y;
        }


        //Pre: Needs a double representing x-direction
        //Post: None
        //Desc: Sets current x-direction to given value
        public void SetDirX(double dirX)
        {
            this.dirX = dirX;
        }


        //Pre: Needs a double representing y-direction
        //Post: None
        //Desc: Sets current y-direction to given value
        public void SetDirY(double dirY)
        {
            this.dirY = dirY;
        }


        //Pre: Projectile is active
        //Post: Returns updated destRec
        //Desc: None
        public Rectangle Move()
        {
            //Updates Projectile position
            projRec.X = projRec.X + (int)(dirX * speed);
            projRec.Y = projRec.Y + (int)(dirY * speed);

            //Returns updated rectanlge
            return projRec;
        }
    }
}
