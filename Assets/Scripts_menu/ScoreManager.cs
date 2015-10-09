using UnityEngine;
using System.Collections.Generic;
using System;


namespace Assets.Scripts_menu
{
    public class ScoreManager : MonoBehaviour
    {
        public static readonly string SCORE_KEY = "Score";

        public static readonly string[] SCORE_KEYS = {
                                              SCORE_KEY + "1",
                                              SCORE_KEY + "2",
                                              SCORE_KEY + "3",
                                              SCORE_KEY + "4",
                                              SCORE_KEY + "5"
                                          };

        private List<int> scoresSaved_ = new List<int>();
        public List<int> Scores
        {
            get { return scoresSaved_; }
        }

        private static ScoreManager instance_ = null;
        public static ScoreManager Instance
        {
            get
            {
                if (instance_ == null)
                {
                    instance_ = new GameObject("ScoreManager").AddComponent<ScoreManager>();
                }
                return instance_;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            // Load the score from<the preferences.
            foreach (string score in SCORE_KEYS)
            {
                int scoreValue = PlayerPrefs.GetInt(score);
                if (scoreValue != 0)
                {
                    scoresSaved_.Add(scoreValue);
                }
            }
        }

        void OnDestroy()
        {
            int i = 0;
            foreach (int score in scoresSaved_)
            {
                PlayerPrefs.SetInt(SCORE_KEYS[i++], score);
            }
        }

        public void addScore(int score)
        {
            scoresSaved_.Add(score);
            scoresSaved_.Sort((x, y) => y.CompareTo(x)); // sort in reverse order
            while (scoresSaved_.Count > SCORE_KEYS.Length)
            {
                scoresSaved_.RemoveAt(scoresSaved_.Count - 1);
            }
        }

    }
}
