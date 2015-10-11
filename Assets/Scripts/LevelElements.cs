using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelElements
    {
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

        // Fruits
        public Vector2 FruitCoord { get; set; }
        public GameObject FruitObject;
        public bool HasEatenFruit = false;
        public GameObject fruitInstance_ = null;

        public Vector2 RespawnCoord { get; set; }

        public LevelElements(int widthMap, int heightMap)
        {
            Map = new MapElement[widthMap, heightMap];
        }

        public int GetHeightLength()
        {
            return Map.GetLength(1);
        }
        public int GetWidthLength()
        {
            return Map.GetLength(0);
        }
        public void PopUpFruit()
        {
            fruitInstance_ = (GameObject)LevelsLoader.Instantiate(FruitObject);
            if (fruitInstance_ != null)
            {
                OTFilledSprite sp = fruitInstance_.GetComponent<OTFilledSprite>();
                sp.position = FruitCoord;
                sp.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                sp.depth = 0;
                sp.transform.position = new Vector3(FruitCoord.x, FruitCoord.y, 0.0f);
                sp.size = new Vector2(1, 1);
            }
        }

        public bool IsFruitInstantiated()
        {
            return fruitInstance_ != null;
        }

        public void DestroyFruit()
        {
            if (fruitInstance_ != null)
            {
                LevelsLoader.DestroyImmediate(fruitInstance_.gameObject);
                fruitInstance_ = null;
            }
        }
    }
}
