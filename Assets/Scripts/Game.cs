using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using Assets.Scripts_menu;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        static public int CurrentScore { get; set; }

        private GameTimer timer_ = new GameTimer();
        private SoundPlayer soundPlayer_ = new SoundPlayer();

        private CharactersMediator characters_ = new CharactersMediator();
        public CharactersMediator Characters { get { return characters_; } }

        private OTView _view = null;

        void Awake()
        {
            Debug.Log("Awake Game");
            GameObject obj = GameObject.Find("View");
            if (obj != null)
            {
                _view = obj.GetComponent<OTView>();
            }
            if (_view == null)
            {
                Debug.LogError("Error: OTView cannot be found.");
            }
        }

       // static int nbPointsToReachForNewLife_ = 1;
        private Texture2D lifeHUD_;
        private Texture2D fruitHUD_;

        void Start()
        {
            Debug.Log("Start Game");
            lifeHUD_ = (Texture2D)Resources.Load("life");
            fruitHUD_ = (Texture2D)Resources.Load("sprites/cerise");
            // nbPointsToReachForNewLife_ = 1;

            characters_.Pacman.NbLife = Pacman.INITIAL_NB_LIFE;
            
            //TODO: should not be there... We need to keep the scoring in the next levels.
            CurrentScore = 0;

            timer_.InvincibilityTimer.TimerCompleted += new EventHandler(InvincibilityTimeOver);
        }

        void InvincibilityTimeOver(object sender, EventArgs e)
        {
            Debug.Log("IN THE CALLBACK INVINCIBILITY");
            characters_.SetNormalCharacterStates();
        }

        void Update()
        {
            characters_.MovePacman();
            characters_.MoveAllGhosts(timer_);

            Queue<Collider> objectsEaten = characters_.GetObjectsEncounteredByPacman();
            foreach (Collider obj in objectsEaten)
            {
                if (obj != null)
                {
                    IncreasePointsWhenObjectEaten(obj);
                }
            }

            characters_.CheckPacmanEatGhost();
            if (characters_.PacmanHasBeenEaten())
            {
                characters_.ResetAllPositions();
            }

            if (IsGameOver())
            {
                GoBackToMenu();
            }

            if (characters_.Pacman.LevelElements.NbPoints <= 0)
            {
                if (!LevelsLoader.LoadNextLevel())
                {
                    GoBackToMenu();
                }
            }

            /*
            // +1 life each 10 000 pts
            if ((int)(CurrentScore / 10000) >= _pointToReachForLife)
            {
                PlayerChar.NbLife += (((int)(CurrentScore / 10000) - _pointToReachForLife) + 1);
                _pointToReachForLife = (int)(CurrentScore / 10000) + 1;
            }


            if (SoundBackground.isPlaying == false)
            {
                SoundBackground.Play();
                SoundBackground.Loop();
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
                    sp.position = FruitCoord;
                    sp.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                    sp.depth = 0;
                    sp.transform.position = new Vector3(FruitCoord.x, FruitCoord.y, 0.0f);
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
            */

            if (Input.GetKey(KeyCode.A))
            {
                if (!LevelsLoader.LoadNextLevel())
                {
                    Debug.Log("menu load level");
                    Menu.LoadMenu();
                }
            }

            if (Input.GetKey(KeyCode.KeypadPlus))
            {
                if (_view != null)
                {
                    _view.zoom += 0.1f;
                }
            }
            if (Input.GetKey(KeyCode.KeypadMinus))
            {
                if (_view != null)
                {
                    _view.zoom -= 0.1f;
                }
            }

            timer_.Update();
        }

        private void GoBackToMenu()
        {
            ScoreManager.Instance.addScore(CurrentScore);
            Menu.LoadMenu();
        }

        private void IncreasePointsWhenObjectEaten(Collider objectInCollision)
        {
            if (objectInCollision.tag == "point")
            {
                --characters_.Pacman.LevelElements.NbPoints;
                CurrentScore += 10;
                //SoundChomp.Play();
                Destroy(objectInCollision.gameObject);
            }
            else if (objectInCollision.tag == "bigPoint")
            {
                --characters_.Pacman.LevelElements.NbPoints;
                CurrentScore += 50;
                //SoundIntermission.Play();
                Destroy(objectInCollision.gameObject);

                characters_.SetInvincibilityTime();
                timer_.InvincibilityTimer.IsRunning = true;

            }
            else if (objectInCollision.tag == "fruit")
            {
                CurrentScore += 100;
                //SoundEatFruit.PlayClone();
                Destroy(objectInCollision.gameObject);
            }
        }

        private bool IsGameOver()
        {
            return characters_.Pacman.IsDead();
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0.0f, 0.0f, 100.0f, 20.0f), "Score: " + CurrentScore);
            GUI.Label(new Rect(0.0f, 20.0f, 100.0f, 20.0f), "Points left: " + characters_.Pacman.LevelElements.NbPoints);

            float x = 0.0f;
            for (int i = 0; i < characters_.Pacman.NbLife; ++i)
             {
                 GUI.Label(new Rect(x, Screen.height - 50.0f, 50.0f, 50.0f), lifeHUD_);
                 x += 50.0f;
             }
            GUI.Label(new Rect(Screen.width - 50.0f, Screen.height - 50.0f, 50.0f, 50.0f), fruitHUD_);
        }
    }

}
