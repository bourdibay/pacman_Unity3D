using UnityEngine;
using System.Collections;

public class Clyde : ACharacter, IGhost
{
    // Use this for initialization
    new void Start()
    {
        base.Start();
        direction = Direction.UP;
        directionToTry = direction;
        Eatable = false;
        Frame = FRAME_GHOST;
    }

    public void EatPacman()
    {
        if (Eatable == false)
        {
            if (PosX == GameController.Instance.PlayerChar.PosX &&
                PosY == GameController.Instance.PlayerChar.PosY)
            {
                GameController.Instance.PlayerChar.Eaten();

                GameController.Instance.BlinkyChar.ResetCharacter();
                GameController.Instance.BlinkyChar.SetToGhost("blinky", "blinky_anim");
                GameController.Instance.PinkyChar.ResetCharacter();
                GameController.Instance.PinkyChar.SetToGhost("pinky", "pinky_anim");
                GameController.Instance.InkyChar.ResetCharacter();
                GameController.Instance.InkyChar.SetToGhost("inky", "inky_anim");
                GameController.Instance.ClydeChar.ResetCharacter();
                GameController.Instance.ClydeChar.SetToGhost("clyde", "clyde_anim");

            }
        }
    }

    public override void checkCollisionWithCharacters()
    {
        EatPacman();
    }

    // Update is called once per frame
    void Update()
    {
        script_anim.size = new Vector2(1, 1);

        if (Eatable == null)
            moveToPoint((int)GameController.Instance.RespawnPoint.y, (int)GameController.Instance.RespawnPoint.x);
        else if (Eatable == true)
            frightenedBehaviour();
        else // on fuit le pacman + ou - selon la distance
        {
            int diffX = PosX - GameController.Instance.PlayerChar.PosX;
            int diffY = PosY - GameController.Instance.PlayerChar.PosY;

            int absX = (diffX < 0) ? (-diffX) : diffX;
            int absY = (diffY < 0) ? (-diffY) : diffY;

            const int TILE_ECART = 8;
            int toX, toY;

            // val absolue
            if (absX >= TILE_ECART && absY >= TILE_ECART)
            {
                moveToPoint(GameController.Instance.PlayerChar.PosY, GameController.Instance.PlayerChar.PosX);
                // go to pacman
            }
            else if (absX <= TILE_ECART)
            {
                if (diffX < 0) // je suis a gauche
                {
                    toX = PosX - absX;
                    if (toX < 0)
                        toX = 0;
                    toY = PosY;
                    moveToPoint(toY, toX);
                    // go to left
                }
                else if (diffX >= 0)
                {
                    toX = PosX + absX;
                    if (toX > GameController.Instance.map.GetLength(1))
                        toX = GameController.Instance.map.GetLength(1) - 1;
                    toY = PosY;
                    moveToPoint(toY, toX);
                    //go to right
                }
            }
            else if (absY <= TILE_ECART)
            {
                if (diffY < 0)
                {
                    toX = PosX;
                    toY = PosY - absY;
                    if (toY < 0)
                        toY = 0;
                    moveToPoint(toY, toX);
                    // go to down
                }
                else if (diffY >= 0)
                {
                    toX = PosX;
                    toY = PosY + absY;
                    if (toY < GameController.Instance.map.GetLength(0))
                        toY = GameController.Instance.map.GetLength(0);
                    moveToPoint(toY, toX);
                    //go to up
                }
            }
           // moveToPoint(tmpY, tmpX);

        }

        moveToDirection();
        rotateCharacter();
        checkCollisionWithCharacters();

        // if go to respawn
        mayGoToRespawn("clyde", "clyde_anim");
    }
}
