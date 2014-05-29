using UnityEngine;
using System.Collections;

public class Pacman : ACharacter
{
    public OTSound SoundEatGhost;
    public OTSound SoundEaten;
    public OTSound SoundEatFruit;
    public OTSound SoundChomp;
    public OTSound SoundIntermission;

	// Use this for initialization
    void Awake () {
        direction = Direction.RIGHT;
        directionToTry = direction;
        Eatable = true;
        Frame = FRAME_PACMAN;
        OTAnimatingSprite sp = this.GetComponent<OTAnimatingSprite>();
        sp.size = new Vector2(1, 1);
    }

    new void Start()
    {
        base.Start();
        SoundEatGhost = new OTSound("pacman_eat_ghost");
        SoundEatGhost.Idle();
        SoundEaten = new OTSound("pacman_dead");
        SoundEaten.Idle();
        SoundEatFruit = new OTSound("pacman_eat_fruit");
        SoundEatFruit.Idle();
        SoundChomp = new OTSound("pacman_chomp");
        SoundChomp.Idle();
        SoundIntermission = new OTSound("pacman_intermission");
        SoundIntermission.Idle();
    }

    public int NbLife = 3;
    private int _nbEnergyzerRow = 0;
    private int[] _scoreEnergyzerGhost = {
                                             200,
                                             400,
                                             800,
                                             1600,
                                             12000
                                         };

    public void Eaten()
    {
        SoundEaten.Play();

        System.Threading.Thread.Sleep(1000);

        NbLife--;
        PosX = (int)InitCoord.x;
        PosY = (int)InitCoord.y;
        transform.position = GraphicPosition;
        currentFrame = 0;
    }

    public override void checkCollisionWithCharacters()
    {
        if (PosX == GameController.Instance.BlinkyChar.PosX &&
            PosY == GameController.Instance.BlinkyChar.PosY &&
            GameController.Instance.BlinkyChar.Eatable == true)
        {
            SoundEatGhost.PlayClone();
            GameController.Instance.BlinkyChar.SetToEye("eye_blinky", "eye_blinky_anim");
            GameController.Instance.Score += _scoreEnergyzerGhost[_nbEnergyzerRow];
            ++_nbEnergyzerRow;
        }
        if (PosX == GameController.Instance.PinkyChar.PosX &&
            PosY == GameController.Instance.PinkyChar.PosY &&
            GameController.Instance.PinkyChar.Eatable == true)
        {
            SoundEatGhost.PlayClone();
            GameController.Instance.PinkyChar.SetToEye("eye_pinky", "eye_pinky_anim");
            GameController.Instance.Score += _scoreEnergyzerGhost[_nbEnergyzerRow]; 
            ++_nbEnergyzerRow;
        }
        if (PosX == GameController.Instance.InkyChar.PosX &&
            PosY == GameController.Instance.InkyChar.PosY &&
            GameController.Instance.InkyChar.Eatable == true)
        {
            SoundEatGhost.PlayClone();
            GameController.Instance.InkyChar.SetToEye("eye_inky", "eye_inky_anim");
            GameController.Instance.Score += _scoreEnergyzerGhost[_nbEnergyzerRow]; 
            ++_nbEnergyzerRow;
        }
        if (PosX == GameController.Instance.ClydeChar.PosX &&
            PosY == GameController.Instance.ClydeChar.PosY &&
            GameController.Instance.ClydeChar.Eatable == true)
        {
            SoundEatGhost.PlayClone();
            GameController.Instance.ClydeChar.SetToEye("eye_clyde", "eye_clyde_anim");
            GameController.Instance.Score += _scoreEnergyzerGhost[_nbEnergyzerRow]; 
            ++_nbEnergyzerRow;
        }

        if (_nbEnergyzerRow == 4) // MAXI BONUS !!!!!
        {
            GameController.Instance.Score += _scoreEnergyzerGhost[_nbEnergyzerRow];
            _nbEnergyzerRow = 0;
        }
    }
 
    private void getTouch()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            directionToTry = Direction.UP;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            directionToTry = Direction.RIGHT;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            directionToTry = Direction.LEFT;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            directionToTry = Direction.DOWN;
        }

    }

	// Update is called once per frame
	void Update () {
        if (Eatable != false)
            _nbEnergyzerRow = 0;
        getTouch();
        moveToDirection();
        rotateCharacter();
        checkCollisionWithCharacters();
	}

    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "point")
        {
            Point pt = hit.GetComponent<Point>();
            if (pt != null)
            {
                pt.doAction();
                GameController.Instance.Score += pt.Point;
                SoundChomp.Play();
            }
            Destroy(hit.gameObject);
        }
        if (hit.tag == "bigPoint")
        {
            BigPoint pt = hit.GetComponent<BigPoint>();
            if (pt != null)
            {
                GameController.Instance.Score += pt.Point;
                pt.doAction();
                SoundIntermission.Play();
            }
            Destroy(hit.gameObject);
        } 
        if (hit.tag == "fruit")
        {
            Fruit pt = hit.GetComponent<Fruit>();
            if (pt != null)
            {
                GameController.Instance.Score += pt.Point;
                pt.doAction();
                SoundEatFruit.PlayClone();
            }
            Destroy(hit.gameObject);
        }
    }

}
