using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImYellowFish.Utility
{
    [System.Serializable]
    public class CooldownFlag
    {
        /// <summary>
        /// The value of the flag
        /// </summary>
        public bool value;

        /// <summary>
        /// The remaining time before deactivate. -1 if timer is not active.
        /// </summary>
        public float timer;

        /// <summary>
        /// Whether the timer is active
        /// </summary>
        private bool enableTimer;

        public CooldownFlag(bool value = false)
        {
            this.value = value;
        }

        public void SetValue(bool value)
        {
            this.value = value;
            timer = -1;
            enableTimer = false;
        }

        /// <summary>
        /// Activate the flag
        /// </summary>
        public void Activate()
        {
            value = true;
            timer = -1;
            enableTimer = false;
        }

        /// <summary>
        /// Activate the flag for some duration
        /// </summary>
        public void Activate(float duration)
        {
            value = true;
            timer = duration;
            enableTimer = true;
        }

        /// <summary>
        /// Deactivate the flag
        /// </summary>
        public void Deactivate()
        {
            value = false;
            timer = -1;
            enableTimer = false;
        }

        /// <summary>
        /// Update the timer.
        /// </summary>
        public void Update(float deltaTime)
        {
            if (enableTimer)
            {
                timer -= deltaTime;
                if (timer <= 0)
                {
                    Deactivate();
                }
            }
        }
    }
}