//Author: Jonathan Cojita
//File Name: QuadTree.cs
//Project Name: PASS3
//Creation Date: January 6, 2022
//Modified Date: January 7, 2022
//Description: Class for Quad Tree Algorithm

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
    class QuadTree
    {
        //Stores the original boundary
        public Rectangle boundary;

        //Stores the bullets currently within the boundary as well as the max capacity allowed within boundary
        public List<Projectile> curBullets = new List<Projectile>();
        private int capacity = 4;

        //Stores whether boundary has been divided yet or not
        public bool divided = false;

        //Stores children Quad Trees representing the 4 smaller boundaries it divides into
        public QuadTree northEast;
        public QuadTree northWest;
        public QuadTree southEast;
        public QuadTree southWest;

        //Pre: Needs a rectaongle representing a boundary
        //Post: None
        //Desc Constructor class for quad tree
        public QuadTree(Rectangle boundary)
        {
            //Sets the boundary as given parameter
            this.boundary = boundary;
        }


        //Pre: Needs a projectile
        //Post: None
        //Desc: Inserts a new projectile into the tree and updates tree as nessasary
        public void insert(Projectile bullet)
        {
            //Chekcs if inserted bullet is within the bounds of the rectangle
            if(boundary.Intersects(bullet.GetRec()) == false)
            {
                //If not in bounds, simply exits function
                return;
            }

            //Checks if boundary is at capacity
            if(curBullets.Count() < capacity)
            {
                //Inserts given projectile to stored list
                curBullets.Add(bullet);
            }
            else
            {
                //Checks if boundary has been divded yet or not
                if(divided == false)
                {
                    //Calls subdivide function
                    Subdivide();                  
                }

                //Recursively calls insert function with children trees
                northEast.insert(bullet);
                northWest.insert(bullet);
                southEast.insert(bullet);
                southWest.insert(bullet);
            }
        }


        //Pre: Boundary has not been divided yet
        //Post: None
        //Desc: Loads children trees by dividing boundary into 4 rectangles and using them as new boundaries for child trees
        private void Subdivide()
        {
            //Loads all child trees
            northEast = new QuadTree(new Rectangle(boundary.X + boundary.Width / 2, boundary.Y, boundary.Width / 2, boundary.Height / 2));
            northWest = new QuadTree(new Rectangle(boundary.X, boundary.Y, boundary.Width / 2, boundary.Height / 2));
            southEast = new QuadTree(new Rectangle(boundary.X + boundary.Width / 2, boundary.Y + boundary.Height / 2, boundary.Width / 2, boundary.Height / 2));
            southWest = new QuadTree(new Rectangle(boundary.X, boundary.Y + boundary.Height / 2, boundary.Width / 2, boundary.Height / 2));

            //Stores that tree has been divided
            divided = true;

        }


        //Pre: Rectangle representing search area
        //Post: Returns a list of projectiles found within range
        //Desc: Search algorithm that checks if any projectiles in the tree are contained within a given rectangle range
        public List<Projectile> Query(Rectangle range)
        {
            //creates a temporary list that stores all foun projectiles within range
            List<Projectile> found = new List<Projectile>();

            //Checks if current boundary even intersects with given range
            if (boundary.Intersects(range) == false)
            {
                //If not intersecting, return empty list
                return found;
            }
            else
            {
                //Loops through every bullet in current boundary
                for (int i = 0; i < curBullets.Count(); i++)
                {
                    //Checks if boundary intersects with range
                    if(range.Contains(curBullets[i].GetRec()) == true)
                    {
                        //Adds the current bullet to the found list
                        found.Add(curBullets[i]);
                    }
                }

                //Checks if the tree has been divided
                if(divided == true)
                {
                    //Recursively calls child trees to also query given range
                    found.AddRange(northEast.Query(range));
                    found.AddRange(northWest.Query(range));
                    found.AddRange(southEast.Query(range));
                    found.AddRange(southWest.Query(range));
                }

                //Returns finished list of all found projectiles within range
                return found;
            }
        }
    } 
}
