using UnityEngine;
using System.Collections;

using System.IO;

public class LoaderLevel : MonoBehaviour
{

	private string LevelFilename;
	public GameObject point;
	public GameObject bigPoint;
	public GameObject fruit;
	public GameObject wall;
	public GameObject blinky;
	public GameObject player;
	public GameObject pinky;
	public GameObject inky;
	public GameObject clyde;
	public const char pointChar = '.';
	public const char wallChar = '-';
	public const char blinkyChar = 'B';
	public const char pinkyChar = 'P';
	public const char inkyChar = 'I';
	public const char clydeChar = 'C';
	public const char playerChar = 'J';
	public const char bigPointChar = '*';
	public const char fruitChar = 'f';
	public const char respawnChar = 'r';

	void Awake ()
	{

		GameController.Instance.NbPoint = 0;

		LevelFilename = Menu.Levels [GameController.Instance.LevelNum];

		GameController.Instance.FruitChar = fruit.GetComponent<Fruit> ();
		GameController.Instance.FruitGO = fruit;

		GameController.Instance.BlinkyChar = blinky.GetComponent<Blinky> ();
		GameController.Instance.PinkyChar = pinky.GetComponent<Pinky> ();
		GameController.Instance.InkyChar = inky.GetComponent<Inky> ();
		GameController.Instance.ClydeChar = clyde.GetComponent<Clyde> (); 
		GameController.Instance.PlayerChar = player.GetComponent<Pacman> ();

		TextAsset levelFile = (TextAsset)Resources.Load ("Levels/" + LevelFilename, typeof(TextAsset));
		
		if (levelFile == null) {
			Debug.LogError ("fail fichier");	
		}
		
		StringReader reader_tmp = new StringReader (levelFile.text);
		int len = 0;
		int lenx = 0;
		if (reader_tmp == null) {
			Debug.LogError ("map not found or not readable");
		} else {
			for (string line = reader_tmp.ReadLine(); line != null; line = reader_tmp.ReadLine()) {
				lenx = line.Length;
				++len;
			}
			GameController.Instance.map = new GameController.MapElement[len, lenx];
		}
		
		StringReader reader = new StringReader (levelFile.text);		
		if (reader == null) {
			Debug.LogError ("puzzles.txt not found or not readable");
		} else {
			
			float x = 0.0f;
			float y = 0.0f;
			
			int mx = 0;
			int my = len - 1;
			
			for (string line = reader.ReadLine(); line != null; line = reader.ReadLine()) {
				foreach (char c in line) {
					GameObject toInstance = null;
					switch (c) {
					case wallChar :
						toInstance = wall;
						GameController.Instance.map [my, mx] = GameController.MapElement.WALL;
						break;
					case pointChar :
						toInstance = point;
						GameController.Instance.NbPoint++;
						GameController.Instance.map [my, mx] = GameController.MapElement.POINT;
						break;
					case blinkyChar:
						toInstance = blinky;
						GameController.Instance.map [my, mx] = GameController.MapElement.GHOST;
						break;
					case pinkyChar:
						toInstance = pinky;
						GameController.Instance.map [my, mx] = GameController.MapElement.GHOST;
						break;
					case inkyChar:
						toInstance = inky;
						GameController.Instance.map [my, mx] = GameController.MapElement.GHOST;
						break;
					case clydeChar:
						toInstance = clyde;
						GameController.Instance.map [my, mx] = GameController.MapElement.GHOST;
						break;
					case playerChar:
						toInstance = player;
						GameController.Instance.map [my, mx] = GameController.MapElement.PLAYER;
						break;
					case bigPointChar:
						GameController.Instance.NbPoint++;
						toInstance = bigPoint;
						GameController.Instance.map [my, mx] = GameController.MapElement.BIGPOINT;
						break;
					case respawnChar:
						GameController.Instance.RespawnPoint = new Vector2 (mx, my);
						GameController.Instance.map [my, mx] = GameController.MapElement.RESPAWN;
						break;
					case fruitChar:
						GameController.Instance.FruitPoint = new Vector2 (x, y);
						GameController.Instance.map [my, mx] = GameController.MapElement.FRUIT;
						break;
					default:
						GameController.Instance.map [my, mx] = GameController.MapElement.NOTHING;
						break;
					}
					
					if (toInstance != null) {
						GameObject pt = null;
						// wall, point
						if (c != playerChar && c != blinkyChar && c != pinkyChar && c != inkyChar && c != clydeChar) {
							pt = (GameObject)Instantiate (toInstance);
							if (pt != null) {
								OTFilledSprite sp = pt.GetComponent<OTFilledSprite> ();

								sp.position = new Vector2 (x, y);
								sp.transform.rotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
								sp.depth = 0;
								sp.transform.position = new Vector3 (x, y, 0.0f);

								if (c == pointChar)
									sp.size = new Vector2 (0.3f, 0.3f);
								else if (c == bigPointChar)
									sp.size = new Vector2 (0.6f, 0.6f);
								else
									sp.size = new Vector2 (1, 1);
							}
						} else { // animated charac
							switch (c) {
							case playerChar:
								{
									pt = player;//GameObject.Find("Player");
									Pacman scriptPos = pt.GetComponent<Pacman> ();
									if (scriptPos != null) {
										scriptPos.PosX = mx;
										scriptPos.PosY = my;
										scriptPos.InitCoord = new Vector3 (mx, my, 0.0f);
										scriptPos.GraphicPosition = new Vector3 (x, y, 0.0f);
									}
									break;
								}
							case blinkyChar:
								{
									pt = blinky;
									Blinky scriptPos = pt.GetComponent<Blinky> ();
									if (scriptPos != null) {
										scriptPos.PosX = mx;
										scriptPos.InitCoord = new Vector3 (mx, my, 0.0f);
										scriptPos.GraphicPosition = new Vector3 (x, y, 0.0f);
										scriptPos.PosY = my;
									}

								}
								break;
							case pinkyChar:
								{
									pt = pinky;
									Pinky scriptPos = pt.GetComponent<Pinky> ();
									if (scriptPos != null) {
										scriptPos.PosX = mx;
										scriptPos.InitCoord = new Vector3 (mx, my, 0.0f);
										scriptPos.GraphicPosition = new Vector3 (x, y, 0.0f);
										scriptPos.PosY = my;
									}
								}
								break;
							case inkyChar:
								{
									pt = inky;
									Inky scriptPos = pt.GetComponent<Inky> ();
									if (scriptPos != null) {
										scriptPos.PosX = mx;
										scriptPos.InitCoord = new Vector3 (mx, my, 0.0f);
										scriptPos.GraphicPosition = new Vector3 (x, y, 0.0f);
										scriptPos.PosY = my;
									}
								}
								break;
							case clydeChar:
								{
									pt = clyde;
									Clyde scriptPos = pt.GetComponent<Clyde> ();
									if (scriptPos != null) {
										scriptPos.PosX = mx;
										scriptPos.InitCoord = new Vector3 (mx, my, 0.0f);
										scriptPos.GraphicPosition = new Vector3 (x, y, 0.0f);
										scriptPos.PosY = my;
									}
								}
								break;
							}
							// if character, we set position, rotation, etc...
							OTAnimatingSprite sp = pt.GetComponent<OTAnimatingSprite> ();

							sp.position = new Vector2 (x, y);
							sp.transform.rotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
							sp.depth = 0;
							sp.transform.position = new Vector3 (x, y, 0.0f);
							sp.size = new Vector2 (1, 1);
						}
					}
					x += 1.0f;
					++mx;
				}
				x = 0.0f;
				y -= 1.0f;

				mx = 0;
				--my;
			}
		}
		GameController.Instance.MaxPoint = GameController.Instance.NbPoint;
		Application.targetFrameRate = 30;
	}
}
