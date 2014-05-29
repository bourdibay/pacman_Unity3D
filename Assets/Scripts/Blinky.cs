using UnityEngine;
using System.Collections;

public class Blinky : ACharacter, IGhost {

	// Use this for initialization
	new void Start () {
        base.Start();
        direction = Direction.RIGHT;
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
       /* if (PosX == GameController.Instance.PinkyChar.PosX &&
            PosY == GameController.Instance.PinkyChar.PosY)
        {
            direction = OppositeDirection(direction);
//            directionToTry = OppositeDirection(direction);
        }*/
    }


	// Update is called once per frame
	void Update () {
        script_anim.size = new Vector2(1, 1);

        if (Eatable == false && GameController.Instance.NbPoint > 10 && GameController.Instance.NbPoint <= 20 && Frame != FRAME_ELROY)
        {
            while (currentFrame != 0)
                moveToDirection();
            Frame = FRAME_ELROY;
        }
        else if (Eatable == false && GameController.Instance.NbPoint <= 10 && Frame != FRAME_ELROY_CURSE)
        {
            while (currentFrame != 0)
                moveToDirection();
            Frame = FRAME_ELROY_CURSE;
        }

        if (Eatable == null)
            moveToPoint((int)GameController.Instance.RespawnPoint.y, (int)GameController.Instance.RespawnPoint.x);
        else if (Eatable == true)
            frightenedBehaviour();
        else
            moveToPoint(GameController.Instance.PlayerChar.PosY, GameController.Instance.PlayerChar.PosX);

        moveToDirection();
        rotateCharacter();
        checkCollisionWithCharacters();

        // if go to respawn
        mayGoToRespawn("blinky", "blinky_anim");


	}



}
