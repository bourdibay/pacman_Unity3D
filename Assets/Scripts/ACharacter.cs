using UnityEngine;
using System.Collections;

public abstract class ACharacter : MonoBehaviour, ICharacter
{
	public abstract void checkCollisionWithCharacters ();

	protected OTAnimatingSprite script_anim;
	private bool? _eatable = false;

	public bool? Eatable {
		get { return _eatable; }
		set { 
			_eatable = value;
		}
	}

	public enum Direction
	{
		LEFT,
		RIGHT,
		UP,
		DOWN
	}

	public Direction direction { get; set; } // actual direction
	// Direction we try to go (but may be not applied because of a wall)
	public Direction directionToTry { get; set; }
	public Vector3 InitCoord { get; set; }
	public Vector3 GraphicPosition { get; set; }

	public void Start ()
	{
		script_anim = GetComponent<OTAnimatingSprite> ();
	}

	public Direction OppositeDirection (Direction dir)
	{
		switch (dir) {
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

	public bool hasIntersection (out bool[] dir)
	{
		dir = new bool[] { false, false, false, false};
		if (GameController.Instance.map [checkOutOfBound (PosY + 1, 0), checkOutOfBound (PosX, 1)] != GameController.MapElement.WALL)
			dir [(int)Direction.UP] = true;
		if (GameController.Instance.map [checkOutOfBound (PosY - 1, 0), checkOutOfBound (PosX, 1)] != GameController.MapElement.WALL)
			dir [(int)Direction.DOWN] = true;
		if (GameController.Instance.map [checkOutOfBound (PosY, 0), checkOutOfBound (PosX + 1, 1)] != GameController.MapElement.WALL)
			dir [(int)Direction.RIGHT] = true;
		if (GameController.Instance.map [checkOutOfBound (PosY, 0), checkOutOfBound (PosX - 1, 1)] != GameController.MapElement.WALL)
			dir [(int)Direction.LEFT] = true;

		if (direction == Direction.RIGHT || direction == Direction.LEFT) {
			if (dir [(int)Direction.DOWN] == true || dir [(int)Direction.UP] == true)
				return true;
		} else if (direction == Direction.UP || direction == Direction.DOWN) {
			if (dir [(int)Direction.RIGHT] == true || dir [(int)Direction.LEFT] == true)
				return true;
		}
		return false;
	}

	public bool isBloqued ()
	{
		bool[] inter;
		if (hasIntersection (out inter) == false) {
			switch (direction) {
			case Direction.RIGHT:
				if (inter [(int)Direction.RIGHT] == false)
					return true;
				break;
			case Direction.LEFT:
				if (inter [(int)Direction.LEFT] == false)
					return true;
				break;
			case Direction.UP:
				if (inter [(int)Direction.UP] == false)
					return true;
				break;
			case Direction.DOWN:
				if (inter [(int)Direction.DOWN] == false)
					return true;
				break;
			}
		}
		return false;
	}

	private int checkOutOfBound (int v, int dim)
	{
		if (v < 0)
			return GameController.Instance.map.GetLength (dim) - 1;
		if (v >= GameController.Instance.map.GetLength (dim))
			return 0;
		return v;
	}

	private int checkOutOfBound (int v, int dim, ref Vector3 vec)
	{
		if (v < 0) {
			if (dim == 1)
				vec = new Vector3 (vec.x + (1.0f / Frame) * Frame * GameController.Instance.map.GetLength (dim), vec.y, vec.z);
			else if (dim == 0)
				vec = new Vector3 (vec.x, vec.y + (1.0f / Frame) * Frame * GameController.Instance.map.GetLength (dim), vec.z);

			return GameController.Instance.map.GetLength (dim) - 1;
		}
		if (v >= GameController.Instance.map.GetLength (dim)) {
			if (dim == 1)
				vec = new Vector3 (vec.x - (1.0f / Frame) * Frame * GameController.Instance.map.GetLength (dim), vec.y, vec.z);
			else if (dim == 0)
				vec = new Vector3 (vec.x, vec.y - (1.0f / Frame) * Frame * GameController.Instance.map.GetLength (dim), vec.z);
			return 0;
		}
		return v;
	}

	private int _posX = 0;
	public int PosX {
		get { return _posX; }
		set {
			_posX = checkOutOfBound (value, 1);
		}
	}

	private int _posY = 0;
	public int PosY {
		get { return _posY; }
		set {
			_posY = checkOutOfBound (value, 0);
		}
	}
	
	// Specifie the moving speed of the characters
	static public readonly int FRAME_FRIGHTENED = 16;
	static public readonly int FRAME_GHOST = 10;
	static public readonly int FRAME_PACMAN = 7;
	static public readonly int FRAME_ELROY = 7;
	static public readonly int FRAME_ELROY_CURSE = 6;

	public int Frame { get; set; }

	private int _currFrame = 0;

	public int currentFrame {
		get { return _currFrame; }
		set { _currFrame = value; }
	}

	public void rotateCharacter ()
	{
		switch (direction) {
		case Direction.UP:
			script_anim.rotation = 90.0f;
			break;
		case Direction.DOWN:
			script_anim.rotation = -90.0f;
			break;
		case Direction.LEFT:
			this.transform.rotation = new Quaternion (0.0f, 180.0f, 0.0f, 0.0f);
			break;
		case Direction.RIGHT:
			this.transform.rotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
			break;

		}
	}

	// true = ok
	// false = wall
	public bool moveToDirection ()
	{
		int checkX = PosX;
		int checkY = PosY;
		Vector3 toVector = transform.position;
		int newX = PosX;
		int newY = PosY;

		if (_currFrame == 0) {
			switch (directionToTry) {
			case Direction.DOWN:
				if (GameController.Instance.map [checkOutOfBound (PosY - 1, 0), PosX] != GameController.MapElement.WALL)
					direction = directionToTry;
				break;
			case Direction.UP:
				if (GameController.Instance.map [checkOutOfBound (PosY + 1, 0), PosX] != GameController.MapElement.WALL)
					direction = directionToTry;
				break;
			case Direction.RIGHT:
				if (GameController.Instance.map [PosY, checkOutOfBound (PosX + 1, 1)] != GameController.MapElement.WALL)
					direction = directionToTry;
				break;
			case Direction.LEFT:
				if (GameController.Instance.map [PosY, checkOutOfBound (PosX - 1, 1)] != GameController.MapElement.WALL)
					direction = directionToTry;
				break;
			}

		}

		switch (direction) {
		case Direction.RIGHT:
			checkX += 1;
			toVector = new Vector3 (transform.position.x + (1.0f / Frame), transform.position.y, 0.0f);
			newX = PosX + 1;
			break;
		case Direction.LEFT:
			checkX -= 1;
			toVector = new Vector3 (transform.position.x - (1.0f / Frame), transform.position.y, 0.0f);
			newX = PosX - 1;
			break;
		case Direction.UP:
			checkY += 1;
			toVector = new Vector3 (transform.position.x, transform.position.y + (1.0f / Frame), 0.0f);
			newY = PosY + 1;
			break;
		case Direction.DOWN:
			checkY -= 1;
			toVector = new Vector3 (transform.position.x, transform.position.y - (1.0f / Frame), 0.0f);
			newY = PosY - 1;
			break;
		}

		Vector3 tmp = toVector;
		checkX = checkOutOfBound (checkX, 1, ref toVector);
		checkY = checkOutOfBound (checkY, 0, ref toVector);
		newX = checkOutOfBound (newX, 1);
		newY = checkOutOfBound (newY, 0);

		if (GameController.Instance.map [checkY, checkX] != GameController.MapElement.WALL) {
			if (_currFrame == Frame - 1) {
				transform.position = toVector;
			} else {
				transform.position = tmp;
			}

			if (_currFrame == Frame - 1) {
				_currFrame = 0;
				GameController.Instance.map [PosY, PosX] = GameController.MapElement.NOTHING;
				PosX = newX;
				PosY = newY;
				GameController.Instance.map [PosY, PosX] = GameController.MapElement.PLAYER;
			} else
				++_currFrame;
			return true;
		}
		return false;
	}

	public void moveToPoint (int toY, int toX)
	{
		bool[] inter;
		if (hasIntersection (out inter)/* && _currFrame == Frame - 1*/) {
			int diffX = PosX - toX;
			int diffY = PosY - toY;

			if (diffY < 0 && inter [(int)Direction.UP] == true && direction != Direction.DOWN)
				directionToTry = Direction.UP;
			else if (diffY > 0 && inter [(int)Direction.DOWN] == true && direction != Direction.UP)
				directionToTry = Direction.DOWN;
			else if (diffX < 0 && inter [(int)Direction.RIGHT] == true && direction != Direction.LEFT)
				directionToTry = Direction.RIGHT;
			else if (diffX > 0 && inter [(int)Direction.LEFT] == true && direction != Direction.RIGHT)
				directionToTry = Direction.LEFT;
			else {
				while (true) {
					Direction rd = (Direction)Random.Range (0, 4);
					if (direction != OppositeDirection (rd)) {
						if (inter [(int)rd] == true)
							directionToTry = rd;
						break;
					}
				}
			}
		} else if (isBloqued ()/* && _currFrame == Frame - 1*/) {
			switch (direction) {
			case Direction.UP:
				directionToTry = Direction.DOWN;
				break;
			case Direction.DOWN:
				directionToTry = Direction.UP;
				break;
			case Direction.LEFT:
				directionToTry = Direction.RIGHT;
				break;
			case Direction.RIGHT:
				directionToTry = Direction.LEFT;
				break;
			}
		}
	}

	public void frightenedBehaviour ()
	{
		bool[] inter;
		if (hasIntersection (out inter)/* && _currFrame == Frame - 1*/) {
			while (true) {
				Direction rd = (Direction)Random.Range (0, 4);

				if (direction != OppositeDirection (rd)) {
					if (inter [(int)rd] == true)
						directionToTry = rd;
					break;
				}
			}
		} else if (isBloqued ()) {
			directionToTry = OppositeDirection (direction);
		}
	}

	public void mayGoToRespawn (string spriteContainer, string anim)
	{
		if (Eatable == null && PosX == (int)GameController.Instance.RespawnPoint.x &&
            PosY == (int)GameController.Instance.RespawnPoint.y) {
			Vector3 tmp = transform.position;
			script_anim.spriteContainer = GameObject.Find (spriteContainer).GetComponent<OTContainer> ();
			script_anim.animation = GameObject.Find (anim).GetComponent<OTAnimation> ();
			script_anim.size = new Vector2 (1, 1);
			Eatable = false;
			transform.position = tmp;
		}
	}

	public void SetToGhost (string spriteContainer, string anim)
	{
		while (currentFrame != 0)
			moveToDirection ();
		currentFrame = 0;
		Frame = FRAME_GHOST;
		Vector3 tmp = transform.position; 
		script_anim.spriteContainer = GameObject.Find (spriteContainer).GetComponent<OTContainer> ();
		script_anim.animation = GameObject.Find (anim).GetComponent<OTAnimation> ();
		script_anim.size = new Vector2 (1, 1);
		Eatable = false;
		transform.position = tmp;
	}

	public void SetToEye (string spriteContainer, string anim)
	{
		while (currentFrame != 0)
			moveToDirection ();
		currentFrame = 0;
		Frame = FRAME_GHOST;
		Vector3 tmp = transform.position; 
		script_anim.spriteContainer = GameObject.Find (spriteContainer).GetComponent<OTContainer> ();
		script_anim.animation = GameObject.Find (anim).GetComponent<OTAnimation> ();
		script_anim.size = new Vector2 (1.0f, 1.0f);
		Eatable = null;
		transform.position = tmp;
	}

	public void SetToFrightened (string spriteContainer, string anim)
	{
		while (currentFrame != 0)
			moveToDirection ();
		Vector3 tmp = transform.position;
		currentFrame = 0;
		Frame = FRAME_FRIGHTENED;
		Eatable = true;
		script_anim.spriteContainer = GameObject.Find (spriteContainer).GetComponent<OTContainer> ();
		script_anim.animation = GameObject.Find (anim).GetComponent<OTAnimation> ();
		script_anim.size = new Vector2 (1.0f, 1.0f);
		transform.position = tmp;
	}

	public void ResetCharacter ()
	{
		while (currentFrame != 0)
			moveToDirection ();
		currentFrame = 0;
		PosX = (int)InitCoord.x;
		PosY = (int)InitCoord.y;
		transform.position = GraphicPosition;
	}
}
