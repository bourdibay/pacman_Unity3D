using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    static OTSound SoundBackground;

	// the singleton of the gameController, readonly
	private static GameController _instance = null;
	public static GameController Instance
	{
		get 
		{
			if (_instance == null) 
			{
				_instance = new GameObject ("GameControllerGlobal").AddComponent<GameController> ();
			}
			return _instance;
		}
	}
    static private int level = 0;
    public int LevelNum { get { return level; } }

    public int Score { get; set; }

    private int _nbPoint = 0;
    public int NbPoint
    {
        get { return _nbPoint; }
        set { _nbPoint = value; }
    }
    public int MaxPoint = 0;

    public Vector2 FruitPoint { get; set; }
    public Vector2 RespawnPoint { get; set; }

    public static float TIME_INVINCIBLE = 5.0f;

    public float TimeInvinsible = TIME_INVINCIBLE;
    private bool _launchTimer = false;
    public bool LaunchTimer
    {
        get { return _launchTimer; }
        set { _launchTimer = value; TimeInvinsible = TIME_INVINCIBLE; }
    }


	private Pacman _player = null;
	public Pacman PlayerChar {
		get { return _player; }
		set { _player = value; }
	}

    private Blinky _blinky = null;
    public Blinky BlinkyChar
    {
        get { return _blinky; }
        set { _blinky = value; }
    }

    private Pinky _pinky = null;
    public Pinky PinkyChar
    {
        get { return _pinky; }
        set { _pinky = value; }
    }

    private Inky _inky = null;
    public Inky InkyChar
    {
        get { return _inky; }
        set { _inky = value; }
    }

    private Clyde _clyde = null;
    public Clyde ClydeChar
    {
        get { return _clyde; }
        set { _clyde = value; }
    }


    private Fruit _fruit = null;
    public Fruit FruitChar
    {
        get { return _fruit; }
        set { _fruit = value; }
    }

    private OTView _view = null;
	
	public enum MapElement {
		WALL,
		NOTHING,
		POINT,
		FRUIT,
		BIGPOINT,
		GHOST,
		PLAYER,
        RESPAWN
	}
	
	public MapElement[,] map;

    public bool IsFruit = false;
    private bool[] _recapFruit = { false, false };
    private float _timerFruit = 10.0f;
    public GameObject FruitGO;
    private GameObject _fruitGOI;


    private Texture2D _life;
    private Texture2D _fruitDisplay;

	void Awake() {
        DontDestroyOnLoad(this.gameObject);
		GameObject obj = GameObject.Find("View");
		if (obj != null) {
			_view = obj.GetComponent<OTView>();
		}
		if (_view == null) {
			Debug.LogError("Error: OTView cannot be found.");	
		}
	}

    static int _pointToReachForLife = 1;

    void Start()
    {
        Score = 0;
        LaunchTimer = false;
        _life = (Texture2D)Resources.Load("life");
        _fruitDisplay = (Texture2D)Resources.Load("sprites/cerise");
        SoundBackground = new OTSound("pacman_background");
        SoundBackground.Idle();
        SoundBackground.Loop();
        SoundBackground.Play();
        _pointToReachForLife = 1;
    }

	void Update() {

        // +1 life each 10 000 pts
        if ((int)(Score / 10000) >= _pointToReachForLife)
        {
            PlayerChar.NbLife += (((int)(Score / 10000) - _pointToReachForLife) + 1);
            _pointToReachForLife = (int)(Score / 10000) + 1;
        }


        if (SoundBackground.isPlaying == false)
        {
            SoundBackground.Play();
            SoundBackground.Loop();
        }

        if (PlayerChar.NbLife <= 0)
        {
            System.Threading.Thread.Sleep(2000);
            ScoreManager.Instance.addScore(Score); 
            level = 0;
            Score = 0;
            _pointToReachForLife = 1;
            SoundBackground.Stop();
            Destroy(_instance);
            _instance = null;
            Application.LoadLevel("menu");
        }
        else if (NbPoint <= 0)
        {
            System.Threading.Thread.Sleep(2000);
            ++level;
            if (level < Menu.LevelsLength)
                Application.LoadLevel("level");
            else
            {
                ScoreManager.Instance.addScore(Score);
                SoundBackground.Stop();
                level = 0;
                _pointToReachForLife = 1;
                Score = 0;
                Destroy(_instance);
                _instance = null;
                Application.LoadLevel("menu");
            }
        }

        if (LaunchTimer == true)
        {
            TimeInvinsible -= Time.deltaTime;
            if (TimeInvinsible < 0.0f) // end of the invinsibility
            {
                if (BlinkyChar.Eatable == true)
                    BlinkyChar.SetToGhost("blinky", "blinky_anim");
                if (PinkyChar.Eatable == true)
                    PinkyChar.SetToGhost("pinky", "pinky_anim");
                if (InkyChar.Eatable == true)
                    InkyChar.SetToGhost("inky", "inky_anim");
                if (ClydeChar.Eatable == true)
                    ClydeChar.SetToGhost("clyde", "clyde_anim");
                if (PlayerChar.Eatable == false)
                    PlayerChar.Eatable = true;

                TimeInvinsible = TIME_INVINCIBLE;
                LaunchTimer = false;
            }
        }

        if (IsFruit == false && ((NbPoint == 70 && _recapFruit[1] == false) || (NbPoint == 170 && _recapFruit[0] == false)))
        {
            if (NbPoint == 170)
                _recapFruit[0] = true;
            else
                _recapFruit[1] = true;

            _timerFruit = 10.0f;
            IsFruit = true;
            _fruitGOI = (GameObject)Instantiate(FruitGO);
            if (_fruitGOI != null)
            {
                OTFilledSprite sp = _fruitGOI.GetComponent<OTFilledSprite>();
                sp.position = FruitPoint;
                sp.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                sp.depth = 0;
                sp.transform.position = new Vector3(FruitPoint.x, FruitPoint.y, 0.0f);
                sp.size = new Vector2(1, 1);
            }

        }

        if (IsFruit == true)
        {
            _timerFruit -= Time.deltaTime;
            if (_timerFruit <= 0.0f)
            {
                Destroy(_fruitGOI);
                _fruitGOI = null;
                // destroy fruit
            }
        }



		if (Input.GetKey(KeyCode.KeypadPlus)) {
			if (_view != null) {
				_view.zoom += 0.1f;
			}
		}
		if (Input.GetKey(KeyCode.KeypadMinus)) {
			if (_view != null) {
				_view.zoom -= 0.1f;
			}
		}
	}

    void OnGUI()
    {
        GUI.Label(new Rect(0.0f, 0.0f, 100.0f, 20.0f), "Score: " + Score);
        GUI.Label(new Rect(0.0f, 20.0f, 100.0f, 20.0f), "Point left: " + NbPoint);

        float x = 0.0f;
        for (int i = 0; i < PlayerChar.NbLife; ++i)
        {
            GUI.Label(new Rect(x, Screen.height - 50.0f, 50.0f, 50.0f), _life);
            x += 50.0f;
        }
        GUI.Label(new Rect(Screen.width - 50.0f, Screen.height - 50.0f, 50.0f, 50.0f), _fruitDisplay);
    }
}
