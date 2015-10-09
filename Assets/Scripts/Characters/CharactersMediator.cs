using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharactersMediator
    {
        private Pacman pacman_ = null;
        public Pacman Pacman
        {
            get { return pacman_; }
            set { pacman_ = value; }
        }
  
        enum GhostType
        {
            BLINKY,
            PINKY,
            INKY,
            CLYDE
        }
        private Ghost[] ghosts_ = new Ghost[Enum.GetNames(typeof(GhostType)).Length];

        public Ghost Blinky
        {
            get { return ghosts_[(int)GhostType.BLINKY]; }
            set { ghosts_[(int)GhostType.BLINKY] = value; }
        }
        public Ghost Pinky
        {
            get { return ghosts_[(int)GhostType.PINKY]; }
            set { ghosts_[(int)GhostType.PINKY] = value; }
        }
        public Ghost Inky
        {
            get { return ghosts_[(int)GhostType.INKY]; }
            set { ghosts_[(int)GhostType.INKY] = value; }
        }
        public Ghost Clyde
        {
            get { return ghosts_[(int)GhostType.CLYDE]; }
            set { ghosts_[(int)GhostType.CLYDE] = value; }
        }

        public void MovePacman()
        {
            getInput();
            Pacman.MoveToDirection();
        }

        public void MoveAllGhosts(GameTimer gameTimer)
        {
            foreach (Ghost ghost in ghosts_)
            {
                if (ghost.Eatable == ACharacter.EatableState.HAS_BEEN_EATEN)
                {
                    ghost.MoveToPoint((int)ghost.LevelElements.RespawnCoord.x, (int)ghost.LevelElements.RespawnCoord.y);
                }
                else if (ghost.Eatable == ACharacter.EatableState.CAN_BE_EATEN)
                {
                    ghost.SetFrightenedMoveStrategy();
                }
                else
                {
                    ghost.SetDirectionToEatPacman(Pacman);
                }
                ghost.MoveToDirection();

                if (ghost.Eatable == ACharacter.EatableState.HAS_BEEN_EATEN &&
                    ghost.X == (int)ghost.LevelElements.RespawnCoord.x &&
                    ghost.Y == (int)ghost.LevelElements.RespawnCoord.y)
                {
                    ghost.SetNormalState();
                }
            }
        }

        public Queue<Collider> GetObjectsEncounteredByPacman()
        {
            Queue<Collider> objectInCollision = new Queue<Collider>(Pacman.ObjectIncollision);
            Pacman.ObjectIncollision.Clear(); // TODO: find a better way to do this.
            return objectInCollision;
        }

        public bool PacmanHasBeenEaten()
        {
            if (Pacman.Eatable == ACharacter.EatableState.CAN_BE_EATEN)
            {
                foreach (Ghost ghost in ghosts_)
                {
                    if (ghost.Eatable == ACharacter.EatableState.CANNOT_BE_EATEN &&
                        ghost.X == Pacman.X && ghost.Y == Pacman.Y)
                    {
                        --Pacman.NbLife;
                        return true;
                    }
                }
            }
            return false;
        }
        public void CheckPacmanEatGhost()
        {
            if (Pacman.Eatable == ACharacter.EatableState.CANNOT_BE_EATEN)
            {
                foreach (Ghost ghost in ghosts_)
                {
                    if (ghost.Eatable == ACharacter.EatableState.CAN_BE_EATEN &&
                        ghost.X == Pacman.X && ghost.Y == Pacman.Y)
                    {
                        ghost.SetEatenState();
                        // TODO: bonus point
                    }
                }
            }
        }

        public void SetInvincibilityTime()
        {
            foreach (Ghost ghost in ghosts_)
            {
                if (ghost.Eatable == ACharacter.EatableState.CANNOT_BE_EATEN)
                {
                    ghost.SetFrightenedState();
                }
            }
            Pacman.Eatable = ACharacter.EatableState.CANNOT_BE_EATEN;
        }

        public void SetNormalCharacterStates()
        {
            foreach (Ghost ghost in ghosts_)
            {
                if (ghost.Eatable == ACharacter.EatableState.CAN_BE_EATEN)
                {
                    ghost.SetNormalState();
                }
            }
            Pacman.Eatable = ACharacter.EatableState.CAN_BE_EATEN;
        }

        public void ResetAllPositions()
        {
            Pacman.Eatable = ACharacter.EatableState.CAN_BE_EATEN;
            Pacman.ResetCharacter();
            foreach (Ghost ghost in ghosts_)
            {
                ghost.Eatable = ACharacter.EatableState.CANNOT_BE_EATEN;
                ghost.ResetCharacter();
            }
            SetNormalCharacterStates();
        }

        private void getInput()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                Pacman.DirectionToTry = ACharacter.Direction.UP;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                Pacman.DirectionToTry = ACharacter.Direction.RIGHT;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                Pacman.DirectionToTry = ACharacter.Direction.LEFT;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                Pacman.DirectionToTry = ACharacter.Direction.DOWN;
            }
        }

    }
}
