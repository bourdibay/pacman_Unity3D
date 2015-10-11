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

        // Set by LevelsLoader in Awake().
        public LevelElements LevelElements;

        private OTView _view = null;

        void Awake()
        {
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

        static int nbPointsToReachForNewLife_ = 1;
        private Texture2D lifeHUD_;
        private Texture2D fruitHUD_;

        void Start()
        {
            soundPlayer_.Start();
            soundPlayer_.PlayGameMusicInLoop();

            lifeHUD_ = (Texture2D)Resources.Load("life");
            fruitHUD_ = (Texture2D)Resources.Load("sprites/cherry");
            nbPointsToReachForNewLife_ = 1;
            
            timer_.InvincibilityTimer.TimerCompleted += new EventHandler(InvincibilityTimeOver);
            timer_.FruitTimer.TimerCompleted += new EventHandler(FruitTimeOver);
        }

        void InvincibilityTimeOver(object sender, EventArgs e)
        {
            characters_.SetNormalCharacterStates();
        }
        void FruitTimeOver(object sender, EventArgs e)
        {
            LevelElements.DestroyFruit();
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

            CurrentScore = characters_.CheckPacmanEatGhost(soundPlayer_, CurrentScore);
            if (characters_.PacmanHasBeenEaten(soundPlayer_))
            {
                timer_.InvincibilityTimer.IsRunning = false;
                characters_.ResetAllPositions();
            }

            if (IsGameOver())
            {
                GoBackToMenu();
            }

            if (LevelElements.NbPoints <= 0)
            {
                if (!LevelsLoader.LoadNextLevel())
                {
                    GoBackToMenu();
                }
            }

            if (!LevelElements.HasEatenFruit &&
                (LevelElements.NbPoints == 70 ||
                LevelElements.NbPoints == 170) &&
                !LevelElements.IsFruitInstantiated())
            {
                LevelElements.PopUpFruit();
                timer_.FruitTimer.IsRunning = true;
            }

            GetNewLifeIfNeeded();

            if (!soundPlayer_.SoundIsPlaying())
            {
                soundPlayer_.PlayGameMusicInLoop();
            }

            // Zoom in or out the boardgame.
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

        private void GetNewLifeIfNeeded()
        {
            // +1 life each 10 000 pts
            if ((int)(CurrentScore / 10000) >= nbPointsToReachForNewLife_)
            {
                characters_.Pacman.NbLife += (((int)(CurrentScore / 10000) - nbPointsToReachForNewLife_) + 1);
                nbPointsToReachForNewLife_ = (int)(CurrentScore / 10000) + 1;
            }
        }

        private void GoBackToMenu()
        {
            ScoreManager.Instance.addScore(CurrentScore);
            CurrentScore = 0;
            characters_.Pacman.NbLife = Pacman.INITIAL_NB_LIFE;
            Menu.LoadMenu();
        }

        private void IncreasePointsWhenObjectEaten(Collider objectInCollision)
        {
            if (objectInCollision.tag == "point")
            {
                --LevelElements.NbPoints;
                CurrentScore += 10;
                Destroy(objectInCollision.gameObject);
                soundPlayer_.PlayChomp();
            }
            else if (objectInCollision.tag == "bigPoint")
            {
                --LevelElements.NbPoints;
                CurrentScore += 50;
                Destroy(objectInCollision.gameObject);

                characters_.SetInvincibilityTime();
                timer_.InvincibilityTimer.IsRunning = true;
                soundPlayer_.PlayInvincible();
            }
            else if (objectInCollision.tag == "fruit")
            {
                CurrentScore += 100;
                LevelElements.DestroyFruit();
                LevelElements.HasEatenFruit = true;
                timer_.FruitTimer.IsRunning = false;
                soundPlayer_.PlayFruitEaten();
            }
        }

        private bool IsGameOver()
        {
            return characters_.Pacman.IsDead();
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0.0f, 0.0f, 100.0f, 20.0f), "Score: " + CurrentScore);
            GUI.Label(new Rect(0.0f, 20.0f, 100.0f, 20.0f), "Points left: " + LevelElements.NbPoints);

            float x = 0.0f;
            for (int i = 0; i < characters_.Pacman.NbLife; ++i)
             {
                 GUI.Label(new Rect(x, Screen.height - 50.0f, 50.0f, 50.0f), lifeHUD_);
                 x += 50.0f;
             }
            if (LevelElements.HasEatenFruit)
            {
                GUI.Label(new Rect(Screen.width - 50.0f, Screen.height - 50.0f, 50.0f, 50.0f), fruitHUD_);
            }
        }
    }

}
