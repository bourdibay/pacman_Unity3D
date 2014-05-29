using UnityEngine;
using System.Collections;

public interface ICharacter {

    bool isBloqued();
    bool hasIntersection(out bool[] dir);
    void rotateCharacter();
    bool moveToDirection();
    void moveToPoint(int y, int x);

    void checkCollisionWithCharacters();
    void frightenedBehaviour();
    void mayGoToRespawn(string spriteContainer, string anim);
    void SetToGhost(string spriteContainer, string anim);
    void SetToEye(string spriteContainer, string anim);
    void SetToFrightened(string spriteContainer, string anim);

    void ResetCharacter();
}
