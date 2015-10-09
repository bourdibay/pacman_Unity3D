using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Pinky : Ghost
    {

        public Pinky()
        {
            SpriteContainerNormal = "pinky";
            AnimationNormal = "pinky_anim";
            SpriteContainerFrightened = "scaredGhost";
            AnimationFrightened = "scaredGhost_anim";
            SpriteContainerEaten = "eye_pinky";
            AnimationEaten = "eye_pinky_anim";
        }

        public override void SetDirectionToEatPacman(Pacman pacman)
        {
            // Goes to the Pacman's direction + 3 cases.
            int tmpX = pacman.X;
            int tmpY = pacman.Y;
            switch (pacman.ActualDirection)
            {
                case Direction.DOWN:
                    for (int ec = 3; ec > 0; --ec)
                        if (tmpY - 1 > 0)
                            tmpY--;
                    break;
                case Direction.UP:
                    for (int ec = 3; ec > 0; --ec)
                        if (tmpY + 1 < LevelElements.GetHeightLength())
                            tmpY++;
                    break;
                case Direction.LEFT:
                    for (int ec = 3; ec > 0; --ec)
                        if (tmpX - 1 > 0)
                            tmpX--;
                    break;
                case Direction.RIGHT:
                    for (int ec = 3; ec > 0; --ec)
                        if (tmpX + 1 < LevelElements.GetWidthLength())
                            tmpX++;
                    break;
            }
            MoveToPoint(tmpX, tmpY);
        }
    }
}
