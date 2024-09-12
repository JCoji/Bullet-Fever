//Author: Jonathan Cojita
//File Name: QuadTree.cs
//Project Name: PASS3
//Creation Date: January 5, 2022
//Modified Date: January 5, 2022
//Description: Class for Magezine, stack that stores proectiles

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
    class Magazine
    {
        //Stores and array of all projectiles currently owned by stack as well as the current size of the stack
        private Projectile[] bullets;
        private int size;


        //Pre: Needs the maximun size of the stack
        //Post: None
        //Desc: Constructor class for the Magezine class
        public Magazine(int count)
        {
            //Loads projectile array using the size given by parameter, sets current size to zero
            bullets = new Projectile[count];
            size = 0;
        }


        //Pre: Needs a projectile
        //Post: None
        //Desc: Adds a new Projectile to the stack
        public void Push(Projectile bullet)
        {
            //Checks if the stack has room
            if(size < bullets.Length)
            {
                //Adds a new projectile to the array, itterates size count
                bullets[size] = bullet;
                size++;
            }
        }


        //Pre: None
        //Post: Returns top bullet in stack
        //Desc: Returns top projectile from stack and removes it
        public Projectile Pop()
        {
            //Creates temporary projectile
            Projectile bullet = null;

            //Checks if the stack is currently empty or not
            if(IsEmpty() == false)
            {
                //Sotres the top bullet in the stack, redueces size count by 1
                bullet = bullets[size - 1];
                size--;
            }

            //Returns temporary projectile
            return bullet;
        }


        //Pre: None
        //Post: None
        //Desc: Returns bool representing if the stack is currently empty
        public bool IsEmpty()
        {
            //Checks stack is empty, returns result
            if(size == 0)
            {
                return true;
            }
            else
            {
                return false;
            }        
        }
    }
}
