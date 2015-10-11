using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Ghost : ACharacter
    {
        // Specify the moving speed of the characters
        static public readonly int FRAME_FRIGHTENED = 16;
        static public readonly int FRAME_GHOST = 10;

        public abstract void SetDirectionToEatPacman(Pacman pacman);

        public string SpriteContainerNormal = "";
        public string AnimationNormal = "";
        public string SpriteContainerFrightened = "";
        public string AnimationFrightened = "";
        public string SpriteContainerEaten = "";
        public string AnimationEaten = "";

        public Ghost()
        {
            Frame = FRAME_GHOST;
            Eatable = EatableState.CANNOT_BE_EATEN;
        }

        public void SetNormalState()
        {
            FinishCurrentMove();
            CurrentNmberOfFrame = 0;
            Frame = FRAME_GHOST;
            Vector3 positionBackup = transform.position;
            script_anim.spriteContainer = GameObject.Find(SpriteContainerNormal).GetComponent<OTContainer>();
            script_anim.animation = GameObject.Find(AnimationNormal).GetComponent<OTAnimation>();
            script_anim.size = new Vector2(1.0f, 1.0f);
            Eatable = EatableState.CANNOT_BE_EATEN;
            transform.position = positionBackup;
        }

        public void SetEatenState()
        {
            FinishCurrentMove();
            CurrentNmberOfFrame = 0;
            Frame = FRAME_GHOST;
            Vector3 positionBackup = transform.position;
            script_anim.spriteContainer = GameObject.Find(SpriteContainerEaten).GetComponent<OTContainer>();
            script_anim.animation = GameObject.Find(AnimationEaten).GetComponent<OTAnimation>();
            script_anim.size = new Vector2(1.0f, 1.0f);
            Eatable = EatableState.HAS_BEEN_EATEN;
            transform.position = positionBackup;
        }

        public void SetFrightenedState()
        {
            FinishCurrentMove();
            Vector3 positionBackup = transform.position;
            CurrentNmberOfFrame = 0;
            Frame = FRAME_FRIGHTENED;
            Eatable = EatableState.CAN_BE_EATEN;
            script_anim.spriteContainer = GameObject.Find(SpriteContainerFrightened).GetComponent<OTContainer>();
            script_anim.animation = GameObject.Find(AnimationFrightened).GetComponent<OTAnimation>();
            script_anim.size = new Vector2(1.0f, 1.0f);
            transform.position = positionBackup;
        }

        public bool HasIntersection(out bool[] dir)
        {
            dir = new bool[] { false, false, false, false };
            if (LevelElements.Map[
                CheckOutOfBound(X, LevelElements.GetWidthLength()),
                CheckOutOfBound(Y + 1, LevelElements.GetHeightLength())
                ] != LevelElements.MapElement.WALL)
            {
                dir[(int)Direction.UP] = true;
            }
            if (LevelElements.Map[
                CheckOutOfBound(X, LevelElements.GetWidthLength()),
                CheckOutOfBound(Y - 1, LevelElements.GetHeightLength())
                ] != LevelElements.MapElement.WALL)
            {
                dir[(int)Direction.DOWN] = true;
            }
            if (LevelElements.Map[
                CheckOutOfBound(X + 1, LevelElements.GetWidthLength()),
                CheckOutOfBound(Y, LevelElements.GetHeightLength())
                ] != LevelElements.MapElement.WALL)
            {
                dir[(int)Direction.RIGHT] = true;
            }
            if (LevelElements.Map[
                CheckOutOfBound(X - 1, LevelElements.GetWidthLength()),
                CheckOutOfBound(Y, LevelElements.GetHeightLength())
                ] != LevelElements.MapElement.WALL)
            {
                dir[(int)Direction.LEFT] = true;
            }

            if (ActualDirection == Direction.RIGHT || ActualDirection == Direction.LEFT)
            {
                if (dir[(int)Direction.DOWN] == true || dir[(int)Direction.UP] == true)
                {
                    return true;
                }
            }
            else if (ActualDirection == Direction.UP || ActualDirection == Direction.DOWN)
            {
                if (dir[(int)Direction.RIGHT] == true || dir[(int)Direction.LEFT] == true)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetFrightenedMoveStrategy()
        {
            bool[] inter;
            if (HasIntersection(out inter))
            {
                while (true)
                {
                    Direction rd = (Direction)UnityEngine.Random.Range(0, 4);

                    if (ActualDirection != GetOppositeDirection(rd))
                    {
                        if (inter[(int)rd] == true)
                            DirectionToTry = rd;
                        break;
                    }
                }
            }
            else if (IsStuck())
            {
                DirectionToTry = GetOppositeDirection(ActualDirection);
            }
        }

        public bool IsStuck()
        {
            bool[] inter;
            if (HasIntersection(out inter) == false)
            {
                switch (ActualDirection)
                {
                    case Direction.RIGHT:
                        if (inter[(int)Direction.RIGHT] == false)
                            return true;
                        break;
                    case Direction.LEFT:
                        if (inter[(int)Direction.LEFT] == false)
                            return true;
                        break;
                    case Direction.UP:
                        if (inter[(int)Direction.UP] == false)
                            return true;
                        break;
                    case Direction.DOWN:
                        if (inter[(int)Direction.DOWN] == false)
                            return true;
                        break;
                }
            }
            return false;
        }

        public void MoveToPoint(int toX, int toY)
        {
            bool[] inter;
            if (HasIntersection(out inter))
            {
                int diffX = X - toX;
                int diffY = Y - toY;

                if (diffY < 0 && inter[(int)Direction.UP] == true && ActualDirection != Direction.DOWN)
                    DirectionToTry = Direction.UP;
                else if (diffY > 0 && inter[(int)Direction.DOWN] == true && ActualDirection != Direction.UP)
                    DirectionToTry = Direction.DOWN;
                else if (diffX < 0 && inter[(int)Direction.RIGHT] == true && ActualDirection != Direction.LEFT)
                    DirectionToTry = Direction.RIGHT;
                else if (diffX > 0 && inter[(int)Direction.LEFT] == true && ActualDirection != Direction.RIGHT)
                    DirectionToTry = Direction.LEFT;
                else
                {
                    while (true)
                    {
                        Direction rd = (Direction)UnityEngine.Random.Range(0, 4);
                        if (ActualDirection != GetOppositeDirection(rd))
                        {
                            if (inter[(int)rd] == true)
                                DirectionToTry = rd;
                            break;
                        }
                    }
                }
            }
            else if (IsStuck())
            {
                switch (ActualDirection)
                {
                    case Direction.UP:
                        DirectionToTry = Direction.DOWN;
                        break;
                    case Direction.DOWN:
                        DirectionToTry = Direction.UP;
                        break;
                    case Direction.LEFT:
                        DirectionToTry = Direction.RIGHT;
                        break;
                    case Direction.RIGHT:
                        DirectionToTry = Direction.LEFT;
                        break;
                }
            }
        }
    }
}
