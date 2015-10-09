using UnityEngine;
using System.Collections;

namespace Assets.Scripts_menu
{

    public class callScoreSave : MonoBehaviour
    {

        // Use this for initialization
        void Awake()
        {
            ScoreManager t = ScoreManager.Instance;
            // disable the warning
            t.enabled = true;
        }
    }
}
