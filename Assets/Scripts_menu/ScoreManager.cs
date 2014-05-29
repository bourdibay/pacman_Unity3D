using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private List<int> _scores = new List<int>();
    public List<int> Scores
    {
        get { return _scores; }
    }

    private static ScoreManager _instance = null;
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("ScoreManager").AddComponent<ScoreManager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        foreach (string sc in SCORE_KEYS)
        {
            int v = PlayerPrefs.GetInt(sc);
            if (v != 0)
            {
                _scores.Add(v);
            }
        }
    }

    void OnDestroy()
    {
        int i = 0;
        foreach (int sc in _scores)
        {
            if (i < SCORE_KEYS.Length) // in case of...
                PlayerPrefs.SetInt(SCORE_KEYS[i], sc);
            ++i;
        }
    }

    public void addScore(int sc)
    {
        int len = SCORE_KEYS.Length;
        if (len > _scores.Count)
            len = _scores.Count;
        for (int i = 0; i < len; ++i)
        {
            if (sc > _scores[i])
            {
                _scores.Insert(i, sc);
                if (_scores.Count > SCORE_KEYS.Length)
                    _scores.RemoveAt(SCORE_KEYS.Length);
                return;
            }
        }

        if (len < SCORE_KEYS.Length)
        {
            _scores.Add(sc);
            if (_scores.Count > SCORE_KEYS.Length)
                _scores.RemoveAt(SCORE_KEYS.Length);
        }
    }

}
