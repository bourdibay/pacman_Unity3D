using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Inky : Ghost
    {
        public GameObject BlinkyAsset;
        private Ghost Blinky = null;

        public Inky()
        {
            SpriteContainerNormal = "inky";
            AnimationNormal = "inky_anim";
            SpriteContainerFrightened = "scaredGhost";
            AnimationFrightened = "scaredGhost_anim";
            SpriteContainerEaten = "eye_inky";
            AnimationEaten = "eye_inky_anim";
        }

        public override void SetDirectionToEatPacman(Pacman pacman)
        {
            if (Blinky == null)
            {
                Blinky = BlinkyAsset.GetComponent<Blinky>();
            }

            // Goes to Pacman's direction + 2 cases + Blinky - Pacamn
            {
                int tmpX = pacman.X;
                int tmpY = pacman.Y;

                switch (pacman.ActualDirection)
                {
                    case Direction.DOWN:
                        for (int ec = 2; ec > 0; --ec)
                            if (tmpY - 1 > 0)
                                tmpY--;
                        break;
                    case Direction.UP:
                        for (int ec = 2; ec > 0; --ec)
                            if (tmpY + 1 < LevelElements.GetHeightLength())
                                tmpY++;
                        break;
                    case Direction.LEFT:
                        for (int ec = 2; ec > 0; --ec)
                            if (tmpX - 1 > 0)
                                tmpX--;
                        break;
                    case Direction.RIGHT:
                        for (int ec = 2; ec > 0; --ec)
                            if (tmpX + 1 < LevelElements.GetWidthLength())
                                tmpX++;
                        break;
                }

                tmpY = ((Blinky.Y - tmpY) + tmpY);
                tmpX = ((Blinky.X - tmpX) + tmpX);

                MoveToPoint(tmpX, tmpY);
            }
        }
    }
}
