using UnityEngine;
using System.Collections;

public class Fruit : APoint {
    public override void doAction()
    {
        GameController.Instance.IsFruit = false;
    }
}
