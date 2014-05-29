using UnityEngine;
using System.Collections;

public class BigPoint : APoint {


    public override void doAction()
    {
        GameController.Instance.NbPoint--;
        GameController.Instance.PlayerChar.Eatable = false;

        if (GameController.Instance.BlinkyChar.Eatable == false)
            GameController.Instance.BlinkyChar.SetToFrightened("scaredGhost", "scaredGhost_anim");
        if (GameController.Instance.PinkyChar.Eatable == false)
            GameController.Instance.PinkyChar.SetToFrightened("scaredGhost", "scaredGhost_anim");
        if (GameController.Instance.InkyChar.Eatable == false)
            GameController.Instance.InkyChar.SetToFrightened("scaredGhost", "scaredGhost_anim");
        if (GameController.Instance.ClydeChar.Eatable == false)
            GameController.Instance.ClydeChar.SetToFrightened("scaredGhost", "scaredGhost_anim");

        GameController.Instance.LaunchTimer = true;
    }
}
