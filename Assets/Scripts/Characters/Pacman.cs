using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Pacman : ACharacter
    {
        // Specify the moving speed of the characters
        static public readonly int FRAME_PACMAN = 7;

        public const int INITIAL_NB_LIFE = 3;

        static private int nbLife_ = INITIAL_NB_LIFE;
        public int NbLife
        {
            get { return nbLife_; }
            set { nbLife_ = value; }
        }
        public Pacman()
        {
            Frame = FRAME_PACMAN;
            Eatable = EatableState.CAN_BE_EATEN;
        }

        public bool IsDead()
        {
            return NbLife <= 0;
        }

        private Queue<Collider> objectCollision_ = new Queue<Collider>();
        public Queue<Collider> ObjectIncollision { get { return objectCollision_; } }

        void OnTriggerEnter(Collider hit)
        {
            ObjectIncollision.Enqueue(hit);
        }
    }
}
