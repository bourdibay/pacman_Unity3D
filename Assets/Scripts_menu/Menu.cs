using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Menu : MonoBehaviour
{
    public AudioClip SoundOpening;

    public GUISkin mySkin;

    public float widthWindow = 500.0f;
    public float heightWindow = 600.0f;

    public float widthButton = 200.0f;
    public float heightButton = 100.0f;

    public float ecartButton = 90.0f;

    private Rect _window;

    private enum Page
    {
        INDEX,
        HIGHSCORE
    }
    private Page _page = Page.INDEX;
    private readonly string[] _pageName = { 
                                              "Main menu",
                                              "HighScore"
                                          };

    static public string[] Levels = {
                                        "map",
                                        "map1",
                                        "map2",
                                        "map3"
                                    };
    static public int LenLevels = Levels.Length;

    void Start()
    {
        _window = new Rect(Screen.width / 2.0f - widthWindow / 2.0f, Screen.height / 2.0f - heightWindow / 2.0f, widthWindow, heightWindow);
        audio.PlayOneShot(SoundOpening);
    }

    void printHighScore()
    {
        int i = 1;
        const float h = 70.0f;
        float w = widthButton;

        foreach (int sc in ScoreManager.Instance.Scores)
        {
            ++i;
            GUI.Label(new Rect(widthWindow / 2.0f - widthButton / 2.0f,  (h * i), w, h), sc.ToString());
        }
        ++i;
        if (GUI.Button(new Rect(widthWindow / 2.0f - widthButton / 2.0f, (h * i), w, 50.0f), "Return"))
        {
            _page = Page.INDEX;
        }
    }

	/// <summary>
	/// The WindowFunction for Gui.Window
	/// </summary>
	/// <param name='id'>
	/// Identifier.
	/// </param>
    void doWindow(int id)
    {
        GUILayout.Label(_pageName[(int)_page]);
        GUILayout.BeginArea(new Rect(0.0f, 0.0f, widthWindow, heightWindow));

        if (_page == Page.INDEX)
        {
            if (GUI.Button(new Rect(widthWindow / 2.0f - widthButton / 2.0f, ecartButton, widthButton, heightButton), "Play"))
            {
                _page = Page.INDEX;
                Application.LoadLevel("level");
            }
            if (GUI.Button(new Rect(widthWindow / 2.0f - widthButton / 2.0f, ecartButton * 2, widthButton, heightButton), "HighScore"))
            {
                _page = Page.HIGHSCORE;
            }
            if (GUI.Button(new Rect(widthWindow / 2.0f - widthButton / 2.0f, ecartButton * 3, widthButton, heightButton), "Quit"))
            {
                Application.Quit();
            }
        }
        else if (_page == Page.HIGHSCORE)
        {
            printHighScore();
        }
        GUILayout.EndArea();
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    void OnGUI()
    {
        GUI.skin = mySkin;
        _window = GUI.Window(0, _window, doWindow, "");
    }

}
