using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Blinky : Ghost
    {
        static public readonly int FRAME_ELROY = 7;
        static public readonly int FRAME_ELROY_CURSE = 6;

        public Blinky()
        {
            SpriteContainerNormal = "blinky";
            AnimationNormal = "blinky_anim";
            SpriteContainerFrightened = "scaredGhost";
            AnimationFrightened = "scaredGhost_anim";
            SpriteContainerEaten = "eye_blinky";
            AnimationEaten = "eye_blinky_anim";
        }

        public override void SetDirectionToEatPacman(Pacman pacman)
        {
            if (LevelElements.NbPoints > 10 && LevelElements.NbPoints <= 20 && Frame != FRAME_ELROY)
            {
                FinishCurrentMove();
                Frame = FRAME_ELROY;
            }
            else if (LevelElements.NbPoints <= 10 && Frame != FRAME_ELROY_CURSE)
            {
                FinishCurrentMove();
                Frame = FRAME_ELROY_CURSE;
            }
            MoveToPoint(pacman.X, pacman.Y);
        }
    }
}
