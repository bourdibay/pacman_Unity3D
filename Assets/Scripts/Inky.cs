using UnityEngine;
using System.Collections;

public class Inky : ACharacter, IGhost {

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
        else //direction = pacman + 2 + blinky-pacamn
        {
            int tmpX = GameController.Instance.PlayerChar.PosX;
            int tmpY = GameController.Instance.PlayerChar.PosY;

            switch (GameController.Instance.PlayerChar.direction)
            {
                case Direction.DOWN:
                    for (int ec = 2; ec > 0; --ec)
                        if (tmpY - 1 > 0)
                            tmpY--;
                    break;
                case Direction.UP:
                    for (int ec = 2; ec > 0; --ec)
                        if (tmpY + 1 < GameController.Instance.map.GetLength(0))
                            tmpY++;
                    break;
                case Direction.LEFT:
                    for (int ec = 2; ec > 0; --ec)
                        if (tmpX - 1 > 0)
                            tmpX--;
                    break;
                case Direction.RIGHT:
                    for (int ec = 2; ec > 0; --ec)
                        if (tmpX + 1 < GameController.Instance.map.GetLength(1))
                            tmpX++;
                    break;
            }

            tmpY = ((GameController.Instance.BlinkyChar.PosY - tmpY) + tmpY);
            tmpX = ((GameController.Instance.BlinkyChar.PosX - tmpX) + tmpX);

            moveToPoint(tmpY, tmpX);
        }

        moveToDirection();
        rotateCharacter();
        checkCollisionWithCharacters();

        // if go to respawn
        mayGoToRespawn("inky", "inky_anim");
    }
}
