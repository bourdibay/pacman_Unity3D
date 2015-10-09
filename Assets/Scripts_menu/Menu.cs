using UnityEngine;
using System.Collections;
using Assets.Scripts;

namespace Assets.Scripts_menu
{
    [RequireComponent(typeof(AudioSource))]
    public class Menu : MonoBehaviour
    {
        #region Public members to be set through the Unity editor
        public AudioClip openingMusic;
        public GUISkin skinToApply;
        public float widthWindow = 500.0f;
        public float heightWindow = 600.0f;
        public float widthButton = 200.0f;
        public float heightButton = 100.0f;
        public float paddingBetweenButtons = 90.0f;
        #endregion

        private Rect window_;

        private enum MenuSection
        {
            HOMEPAGE,
            HIGHSCORE
        }
        private MenuSection currentMenuSection_ = MenuSection.HOMEPAGE;
        private readonly string[] menuSectionNames_ = {
                                              "Main menu",
                                              "HighScore"
                                          };

        static public string[] Levels = {
                                        "map",
                                        "map1"
                                    };
        static public int LevelsLength = Levels.Length;

        static public void LoadMenu()
        {
            Application.LoadLevel("menu");
        }

        void Start()
        {
            window_ = new Rect(Screen.width / 2.0f - widthWindow / 2.0f, Screen.height / 2.0f - heightWindow / 2.0f, widthWindow, heightWindow);
            GetComponent<AudioSource>().PlayOneShot(openingMusic);
        }

        void printHighScore()
        {
            int nbScores = 1;
            const float heightScoreLine = 70.0f;
            float widthScoreLine = widthButton;

            foreach (int score in ScoreManager.Instance.Scores)
            {
                ++nbScores;
                GUI.Label(new Rect(widthWindow / 2.0f - widthButton / 2.0f, (heightScoreLine * nbScores), widthScoreLine, heightScoreLine), score.ToString());
            }
            ++nbScores;
            if (GUI.Button(new Rect(widthWindow / 2.0f - widthButton / 2.0f, (heightScoreLine * nbScores), widthScoreLine, 50.0f), "Return"))
            {
                currentMenuSection_ = MenuSection.HOMEPAGE;
            }
        }

        /// <summary>
        /// The WindowFunction for Gui.Window
        /// Called during the creation of the window.
        /// </summary>
        /// <param name='id'>
        /// Identifier.
        /// </param>
        void doWindow(int id)
        {
            GUILayout.Label(menuSectionNames_[(int)currentMenuSection_]);
            GUILayout.BeginArea(new Rect(0.0f, 0.0f, widthWindow, heightWindow));

            if (currentMenuSection_ == MenuSection.HOMEPAGE)
            {
                if (GUI.Button(new Rect(widthWindow / 2.0f - widthButton / 2.0f, paddingBetweenButtons, widthButton, heightButton), "Play"))
                {
                    currentMenuSection_ = MenuSection.HOMEPAGE;
                    LevelsLoader.LoadFirstLevel();
                }
                if (GUI.Button(new Rect(widthWindow / 2.0f - widthButton / 2.0f, paddingBetweenButtons * 2, widthButton, heightButton), "HighScore"))
                {
                    currentMenuSection_ = MenuSection.HIGHSCORE;
                }
                if (GUI.Button(new Rect(widthWindow / 2.0f - widthButton / 2.0f, paddingBetweenButtons * 3, widthButton, heightButton), "Quit"))
                {
                    Application.Quit();
                }
            }
            else if (currentMenuSection_ == MenuSection.HIGHSCORE)
            {
                printHighScore();
            }
            GUILayout.EndArea();
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        void OnGUI()
        {
            GUI.skin = skinToApply;
            window_ = GUI.Window(0, window_, doWindow, "");
        }

    }
}