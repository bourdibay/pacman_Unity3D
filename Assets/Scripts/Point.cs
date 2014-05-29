using UnityEngine;
using System.Collections;

public class Point : APoint {

    public override void doAction()
    {
        GameController.Instance.NbPoint--;
    }
}
