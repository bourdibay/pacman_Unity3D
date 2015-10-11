using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class ACharacter : MonoBehaviour
    {
        // Set by LevelsLoader in Awake()
        public LevelElements LevelElements;
        protected OTAnimatingSprite script_anim;

        private int x_ = 0;
        public int X
        {
            get { return x_; }
            set
            {
                x_ = CheckOutOfBound(value, LevelElements.GetWidthLength());
            }
        }

        private int y_ = 0;
        public int Y
        {
            get { return y_; }
            set
            {
                y_ = CheckOutOfBound(value, LevelElements.GetHeightLength());
            }
        }

        protected int CheckOutOfBound(int v, int limit)
        {
            if (v < 0)
            {
                return limit - 1;
            }
            if (v >= limit)
            {
                return 0;
            }
            return v;
        }

        protected int CheckOutOfBoundWidth(int v, ref Vector3 vec)
        {
            if (v < 0)
            {
                vec = new Vector3(vec.x + (1.0f / Frame) * Frame * LevelElements.GetWidthLength(), vec.y, vec.z);
                return LevelElements.GetWidthLength() - 1;
            }
            if (v >= LevelElements.GetWidthLength())
            {
                vec = new Vector3(vec.x - (1.0f / Frame) * Frame * LevelElements.GetWidthLength(), vec.y, vec.z);
                return 0;
            }
            return v;
        }
        protected int CheckOutOfBoundHeight(int v, ref Vector3 vec)
        {
            if (v < 0)
            {
                vec = new Vector3(vec.x, vec.y + (1.0f / Frame) * Frame * LevelElements.GetHeightLength(), vec.z);
                return LevelElements.GetHeightLength() - 1;
            }
            if (v >= LevelElements.GetHeightLength())
            {
                vec = new Vector3(vec.x, vec.y - (1.0f / Frame) * Frame * LevelElements.GetHeightLength(), vec.z);
                return 0;
            }
            return v;
        }

        public enum EatableState
        {
            CAN_BE_EATEN,
            HAS_BEEN_EATEN,
            CANNOT_BE_EATEN
        }
        private EatableState eatable_ = EatableState.CANNOT_BE_EATEN;
        public EatableState Eatable
        {
            get { return eatable_; }
            set { eatable_ = value; }
        }

        public enum Direction
        {
            LEFT,
            RIGHT,
            UP,
            DOWN
        }

        public Direction ActualDirection { get; set; } // actual direction

        // Direction we try to go (but may be not applied because of a wall)
        public Direction DirectionToTry { get; set; }
        public Vector3 InitialCoord { get; set; }
        public Vector3 GraphicCoord { get; set; }

        public void Start()
        {
            script_anim = GetComponent<OTAnimatingSprite>();
        }

        public Direction GetOppositeDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.UP:
                    return Direction.DOWN;
                case Direction.DOWN:
                    return Direction.UP;
                case Direction.LEFT:
                    return Direction.RIGHT;
                case Direction.RIGHT:
                    return Direction.LEFT;
            }
            return Direction.RIGHT;
        }

        public int Frame { get; set; }

        private int currentNumberOfFrame_ = 0;
        public int CurrentNmberOfFrame
        {
            get { return currentNumberOfFrame_; }
            set { currentNumberOfFrame_ = value; }
        }

        public void Rotate()
        {
            switch (ActualDirection)
            {
                case Direction.UP:
                    script_anim.rotation = 90.0f;
                    break;
                case Direction.DOWN:
                    script_anim.rotation = -90.0f;
                    break;
                case Direction.LEFT:
                    this.transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
                    break;
                case Direction.RIGHT:
                    this.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                    break;
            }
        }

        public void MoveToDirection()
        {
            // Convert DirectionToTry to ActualDirection
            if (currentNumberOfFrame_ == 0)
            {
                switch (DirectionToTry)
                {
                    case Direction.DOWN:
                        if (LevelElements.Map[X, CheckOutOfBound(Y - 1, LevelElements.GetHeightLength())] != LevelElements.MapElement.WALL)
                            ActualDirection = DirectionToTry;
                        break;
                    case Direction.UP:
                        if (LevelElements.Map[X, CheckOutOfBound(Y + 1, LevelElements.GetHeightLength())] != LevelElements.MapElement.WALL)
                            ActualDirection = DirectionToTry;
                        break;
                    case Direction.RIGHT:
                        if (LevelElements.Map[CheckOutOfBound(X + 1, LevelElements.GetWidthLength()), Y] != LevelElements.MapElement.WALL)
                            ActualDirection = DirectionToTry;
                        break;
                    case Direction.LEFT:
                        if (LevelElements.Map[CheckOutOfBound(X - 1, LevelElements.GetWidthLength()), Y] != LevelElements.MapElement.WALL)
                            ActualDirection = DirectionToTry;
                        break;
                }
            }

            Vector3 changedPosition = transform.position;
            int newX = X;
            int newY = Y;
            switch (ActualDirection)
            {
                case Direction.RIGHT:
                    changedPosition = new Vector3(transform.position.x + (1.0f / Frame), transform.position.y, 0.0f);
                    newX = X + 1;
                    break;
                case Direction.LEFT:
                    changedPosition = new Vector3(transform.position.x - (1.0f / Frame), transform.position.y, 0.0f);
                    newX = X - 1;
                    break;
                case Direction.UP:
                    changedPosition = new Vector3(transform.position.x, transform.position.y + (1.0f / Frame), 0.0f);
                    newY = Y + 1;
                    break;
                case Direction.DOWN:
                    changedPosition = new Vector3(transform.position.x, transform.position.y - (1.0f / Frame), 0.0f);
                    newY = Y - 1;
                    break;
            }

            Vector3 tmp = changedPosition;

            newX = CheckOutOfBoundWidth(newX, ref changedPosition);
            newY = CheckOutOfBoundHeight(newY, ref changedPosition);

            if (LevelElements.Map[newX, newY] != LevelElements.MapElement.WALL)
            {
                if (CurrentNmberOfFrame == Frame - 1)
                {
                    transform.position = changedPosition;
                }
                else
                {
                    transform.position = tmp;
                }

                if (currentNumberOfFrame_ == Frame - 1)
                {
                    currentNumberOfFrame_ = 0;
                    LevelElements.MapElement formerElement = LevelElements.Map[X, Y];
                    LevelElements.Map[X, Y] = LevelElements.MapElement.NOTHING;
                    X = newX;
                    Y = newY;
                    LevelElements.Map[X, Y] = formerElement;
                }
                else
                {
                    ++CurrentNmberOfFrame;
                }
            }
            Rotate(); // Rotate the character with the correct sprite.
        }

        protected void FinishCurrentMove()
        {
            while (CurrentNmberOfFrame != 0)
            {
                MoveToDirection();
            }
        }

        public void ResetCharacter()
        {
            FinishCurrentMove();
            CurrentNmberOfFrame = 0;
            X = (int)InitialCoord.x;
            Y = (int)InitialCoord.y;
            transform.position = GraphicCoord;
        }
    }
}
