using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelElements
    {
        public bool IsFruit = false;
        private bool[] recapFruit_ = { false, false };

        public GameObject FruitGO;
        private GameObject _fruitGOI;

        public enum MapElement
        {
            WALL,
            NOTHING,
            POINT,
            FRUIT,
            BIGPOINT,
            GHOST,
            PLAYER,
            RESPAWN
        }

        public MapElement[,] Map;


        private int nbPoints_ = 0;
        public int NbPoints
        {
            get { return nbPoints_; }
            set { nbPoints_ = value; }
        }


        /*
                private Fruit _fruit = null;
                public Fruit FruitChar
                {
                    get { return _fruit; }
                    set { _fruit = value; }
                }*/

        public Vector2 FruitCoord { get; set; }
        public Vector2 RespawnCoord { get; set; }

        public LevelElements(int widthMap, int heightMap)
        {
            Map = new MapElement[widthMap, heightMap];
        }
        void Start()
        {
           /* SoundBackground = new OTSound("pacman_background");
            SoundBackground.Idle();
            SoundBackground.Loop();
            SoundBackground.Play();
            _pointToReachForLife = 1;*/
        }

        public int GetHeightLength()
        {
            return Map.GetLength(1);
        }
        public int GetWidthLength()
        {
            return Map.GetLength(0);
        }

    }
}
