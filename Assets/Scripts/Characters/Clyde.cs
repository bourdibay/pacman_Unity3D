using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Clyde : Ghost
    {
        public Clyde()
        {
            SpriteContainerNormal = "clyde";
            AnimationNormal = "clyde_anim";
            SpriteContainerFrightened = "scaredGhost";
            AnimationFrightened = "scaredGhost_anim";
            SpriteContainerEaten = "eye_clyde";
            AnimationEaten = "eye_clyde_anim";
        }

        public override void SetDirectionToEatPacman(Pacman pacman)
        {
            // Flee Pacman + or - according to the distance with him.
            int diffX = X - pacman.X;
            int diffY = Y - pacman.Y;

            int absX = (diffX < 0) ? (-diffX) : diffX;
            int absY = (diffY < 0) ? (-diffY) : diffY;

            const int TILE_ECART = 8;
            int toX, toY;

            // absolute value
            if (absX >= TILE_ECART && absY >= TILE_ECART)
            {
                MoveToPoint(pacman.X, pacman.Y);
                // go to pacman
            }
            else if (absX <= TILE_ECART)
            {
                if (diffX < 0) // I am on the left
                {
                    toX = X - absX;
                    if (toX < 0)
                        toX = 0;
                    toY = Y;
                    MoveToPoint(toX, toY);
                    // go to left
                }
                else if (diffX >= 0)
                {
                    toX = X + absX;
                    if (toX > LevelElements.GetWidthLength())
                        toX = LevelElements.GetWidthLength() - 1;
                    toY = Y;
                    MoveToPoint(toX, toY);
                    //go to right
                }
            }
            else if (absY <= TILE_ECART)
            {
                if (diffY < 0)
                {
                    toX = X;
                    toY = Y - absY;
                    if (toY < 0)
                        toY = 0;
                    MoveToPoint(toX, toY);
                    // go down
                }
                else if (diffY >= 0)
                {
                    toX = X;
                    toY = Y + absY;
                    if (toY < LevelElements.GetHeightLength())
                        toY = LevelElements.GetHeightLength();
                    MoveToPoint(toX, toY);
                    //go up
                }
            }
        }
    }
}
