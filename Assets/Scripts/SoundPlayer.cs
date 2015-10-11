using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class SoundPlayer
    {
        OTSound background_;
        OTSound ghostEaten_;
        OTSound pacmanEaten_;
        OTSound fruitEaten_;
        OTSound chomp_;
        OTSound invincible_;

        public void Start()
        {
            background_ = new OTSound("pacman_background");
            background_.Idle();

            ghostEaten_ = new OTSound("pacman_eat_ghost");
            ghostEaten_.Idle();
            pacmanEaten_ = new OTSound("pacman_dead");
            pacmanEaten_.Idle();
            fruitEaten_ = new OTSound("pacman_eat_fruit");
            fruitEaten_.Idle();
            chomp_ = new OTSound("pacman_chomp");
            chomp_.Idle();
            invincible_ = new OTSound("pacman_invincible");
            invincible_.Idle();
        }

        public void PlayGameMusicInLoop()
        {
            background_.Loop();
            background_.Play();
        }

        public void PlayPacmanEaten()
        {
            pacmanEaten_.Play();
        }
        public void PlayFruitEaten()
        {
            fruitEaten_.Play();
        }
        public void PlayGhostEaten()
        {
            ghostEaten_.Play();
        }
        public void PlayChomp()
        {
            chomp_.Play();
        }
        public void PlayInvincible()
        {
            invincible_.Play();
        }

        public bool SoundIsPlaying()
        {
            return chomp_.isPlaying || invincible_.isPlaying ||
                fruitEaten_.isPlaying || pacmanEaten_.isPlaying || background_.isPlaying;
        }
    }
}
