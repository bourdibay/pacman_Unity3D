using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class EntityTimer
    {
        public readonly float timeMax_;
        private float timeLeft_;

        public float TimeLeft
        {
            get { return timeLeft_; }
            set { timeLeft_ = value; }
        }

        private bool isRunning_ = false;
        public bool IsRunning
        {
            get { return isRunning_; }
            set
            {
                isRunning_ = value;
                timeLeft_ = timeMax_;
            }
        }

        public event EventHandler TimerCompleted;
        protected virtual void OnTimerCompleted(EventArgs e)
        {
            EventHandler handler = TimerCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Update()
        {
            if (IsRunning)
            {
                TimeLeft -= Time.deltaTime;
                if (TimeLeft <= 0.0f)
                {
                    IsRunning = false;
                    OnTimerCompleted(null);
                }
            }
        }

        public EntityTimer(float time)
        {
            timeMax_ = time;
            TimeLeft = timeMax_;
            IsRunning = false;
        }
    }

    public class GameTimer
    {
        private List<EntityTimer> timers = new List<EntityTimer>();
        public EntityTimer InvincibilityTimer = new EntityTimer(5.0f);
        public EntityTimer FruitTimer = new EntityTimer(10.0f);

        public GameTimer()
        {
            timers.Add(InvincibilityTimer);
            timers.Add(FruitTimer);
        }

        // TODO: separate running timers to speed up this loop.
        public void Update()
        {
            foreach (EntityTimer timer in timers)
            {
                timer.Update();
            }
        }
    }
}
