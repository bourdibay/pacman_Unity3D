using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class SoundPlayer
    {
        static OTSound SoundBackground;

        void Start()
        {
            SoundBackground = new OTSound("pacman_background");
            SoundBackground.Idle();
            SoundBackground.Loop();
            SoundBackground.Play();
        }
    }
}
