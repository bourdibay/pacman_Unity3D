using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using Assets.Scripts_menu;

namespace Assets.Scripts
{
    class LevelsLoader : MonoBehaviour
    {
        private string levelFilename_;

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

        public GameObject game;

        static private int levelCurrentlyPlaying_ = 0;
        public int CurrentLevel { get { return levelCurrentlyPlaying_; } }

        LevelElements level_;

        internal struct Vector2Int
        {
            public int x, y;
            public Vector2Int(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        static public void LoadFirstLevel()
        {
            levelCurrentlyPlaying_ = 0;
            Application.LoadLevel("level");
        }

        static public bool LoadNextLevel()
        {
            ++levelCurrentlyPlaying_;
            if (levelCurrentlyPlaying_ < Menu.LevelsLength)
            {
                Application.LoadLevel("level");
                return true;
            }
            levelCurrentlyPlaying_ = 0;
            return false;
        }

        void Awake()
        {
            Debug.Log("Awake LevelsLoader");

            levelFilename_ = Menu.Levels[CurrentLevel];
            TextAsset levelAsset = (TextAsset) Resources.Load("Levels/" + levelFilename_, typeof(TextAsset));
            if (levelAsset == null)
            {
                Debug.LogError("");
            }

            int widthMap, heightMap;
            GetMapDimension(levelAsset, out widthMap, out heightMap);

            StringReader reader = new StringReader(levelAsset.text);
            if (reader == null)
            {
                Debug.LogError("not found or not readable");
            }
            else
            {
                level_ = new LevelElements(widthMap, heightMap);

                Vector2 graphicCoord = new Vector2(0.0f, 0.0f);
                Vector2Int mapCoord = new Vector2Int(0, heightMap - 1);

                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    foreach (char c in line)
                    {
                        GameObject toInstance = TranslateCharToInstance(c, graphicCoord, mapCoord);
                        if (toInstance != null)
                        {
                            // wall, point
                            if (!IsACharacter(c))
                            {
                                InstantiateInanimateElement(toInstance, graphicCoord, c);
                            }
                            else
                            {
                                InstantiateCharacters(graphicCoord, mapCoord, c);
                            }
                        }
                        graphicCoord.x += 1.0f;
                        ++mapCoord.x;
                    }
                    graphicCoord.x = 0.0f;
                    graphicCoord.y -= 1.0f;

                    mapCoord.x = 0;
                    --mapCoord.y;
                }
                // GameController.Instance.MaxPoint = GameController.Instance.NbPoint;
                Application.targetFrameRate = 30;
            }
        }

        void Start()
        {
            Debug.Log("Start LevelsLoader");
        }

        private GameObject TranslateCharToInstance(char c, Vector2 graphicCoord, Vector2Int mapCoord)
        {
            GameObject toInstance = null;
            switch (c)
            {
                case wallChar:
                    toInstance = wall;
                    level_.Map[mapCoord.x, mapCoord.y] = LevelElements.MapElement.WALL;
                    break;
                case pointChar:
                    toInstance = point;
                    level_.Map[mapCoord.x, mapCoord.y] = LevelElements.MapElement.POINT;
                    ++level_.NbPoints;
                    break;
                case blinkyChar:
                    toInstance = blinky;
                    level_.Map[mapCoord.x, mapCoord.y] = LevelElements.MapElement.GHOST;
                    break;
                case pinkyChar:
                    toInstance = pinky;
                    level_.Map[mapCoord.x, mapCoord.y] = LevelElements.MapElement.GHOST;
                    break;
                case inkyChar:
                    toInstance = inky;
                    level_.Map[mapCoord.x, mapCoord.y] = LevelElements.MapElement.GHOST;
                    break;
                case clydeChar:
                    toInstance = clyde;
                    level_.Map[mapCoord.x, mapCoord.y] = LevelElements.MapElement.GHOST;
                    break;
                case playerChar:
                    toInstance = player;
                    level_.Map[mapCoord.x, mapCoord.y] = LevelElements.MapElement.GHOST;
                    break;
                case bigPointChar:
                    toInstance = bigPoint;
                    level_.Map[mapCoord.x, mapCoord.y] = LevelElements.MapElement.BIGPOINT;
                    ++level_.NbPoints;
                    break;
                case respawnChar:
                    level_.RespawnCoord = new Vector2(mapCoord.x, mapCoord.y);
                    level_.Map[mapCoord.x, mapCoord.y] = LevelElements.MapElement.RESPAWN;
                    break;
                case fruitChar:
                    level_.FruitCoord = new Vector2(graphicCoord.x, graphicCoord.y);
                    level_.Map[mapCoord.x, mapCoord.y] = LevelElements.MapElement.FRUIT;
                    break;
                default:
                    level_.Map[mapCoord.x, mapCoord.y] = LevelElements.MapElement.NOTHING;
                    break;
            }
            return toInstance;
        }

        private void GetMapDimension(TextAsset levelAsset, out int widthMap, out int heightMap)
        {
            StringReader reader_tmp = new StringReader(levelAsset.text);
            heightMap = 0;
            widthMap = 0;
            if (reader_tmp == null)
            {
                Debug.LogError("map not found or not readable");
            }
            else
            {
                for (string line = reader_tmp.ReadLine(); line != null; line = reader_tmp.ReadLine())
                {
                    widthMap = Math.Max(widthMap, line.Length);
                    ++heightMap;
                }
            }
        }

        private bool IsACharacter(char c)
        {
            return c == playerChar || c == blinkyChar || c == pinkyChar || c == inkyChar || c == clydeChar;
        }

        private void InstantiateInanimateElement(GameObject toInstance, Vector2 graphicCoord, char c)
        {
            GameObject instance = (GameObject)Instantiate(toInstance);
            if (instance != null)
            {
                OTFilledSprite sprite = instance.GetComponent<OTFilledSprite>();

                sprite.position = new Vector2(graphicCoord.x, graphicCoord.y);
                sprite.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                sprite.depth = 0;
                sprite.transform.position = new Vector3(graphicCoord.x, graphicCoord.y, 0.0f);

                if (c == pointChar)
                    sprite.size = new Vector2(0.3f, 0.3f);
                 else if (c == bigPointChar)
                    sprite.size = new Vector2(0.6f, 0.6f);
                else
                    sprite.size = new Vector2(1, 1);
            } else
            {
                Debug.LogWarning("Fail to instantiate" + c);
            }
        }

        private void InstantiateCharacters(Vector2 graphicCoord, Vector2Int mapCoord, char c)
        {
            GameObject instance = null;

            var initCharactersScript = new Action<ACharacter, Vector2, Vector2Int> ((vScriptPos, vGraphicCoord, vMapCoord) =>
            {
                if (vScriptPos != null)
                {
                    vScriptPos.LevelElements = level_;
                    vScriptPos.X = vMapCoord.x;
                    vScriptPos.Y = vMapCoord.y;
                    vScriptPos.InitialCoord = new Vector3(vMapCoord.x, vMapCoord.y, 0.0f);
                    vScriptPos.GraphicCoord = new Vector3(vGraphicCoord.x, vGraphicCoord.y, 0.0f);
                }
            });

            var gameInstance = game.GetComponent<Game>();
            switch (c)
            {
                case playerChar:
                    {
                        instance = player;
                        var pacmanInstance = instance.GetComponent<Pacman>();
                        gameInstance.Characters.Pacman = pacmanInstance;
                        initCharactersScript(pacmanInstance, graphicCoord, mapCoord);
                        break;
                    }
                case blinkyChar:
                    {
                        instance = blinky;
                        var blinkyInstance = instance.GetComponent<Blinky>();
                        gameInstance.Characters.Blinky = blinkyInstance;
                        initCharactersScript(blinkyInstance, graphicCoord, mapCoord);
                    }
                    break;
                case pinkyChar:
                    {
                        instance = pinky;
                        var pinkyInstance = instance.GetComponent<Pinky>();
                        gameInstance.Characters.Pinky = pinkyInstance;
                        initCharactersScript(pinkyInstance, graphicCoord, mapCoord);
                    }
                    break;
                case inkyChar:
                    {
                        instance = inky;
                        var inkyInstance = instance.GetComponent<Inky>();
                        gameInstance.Characters.Inky = inkyInstance;
                        initCharactersScript(inkyInstance, graphicCoord, mapCoord);
                    }
                    break;
                case clydeChar:
                    {
                        instance = clyde;
                        var clydeInstance = instance.GetComponent<Clyde>();
                        gameInstance.Characters.Clyde = clydeInstance;
                        initCharactersScript(clydeInstance, graphicCoord, mapCoord);
                    }
                    break;
            }
            if (instance == null)
            {
                Debug.LogError("Character's instance is not set. The char in the map is '" + c + "'");
                return;
            }

            // if character, we set position, rotation, etc...
            OTAnimatingSprite sp = instance.GetComponent<OTAnimatingSprite>();

            sp.position = new Vector2(graphicCoord.x, graphicCoord.y);
            sp.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            sp.depth = 0;
            sp.transform.position = new Vector3(graphicCoord.x, graphicCoord.y, 0.0f);
            sp.size = new Vector2(1, 1);
        }

    }
}
