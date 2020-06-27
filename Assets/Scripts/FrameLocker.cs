using UnityEngine;

namespace Assets.Scripts
{
    public class FrameLocker
    {
        public int LockFrames = 0;
        public float LockSeconds = 0;

        private int _frameLock = 0;
        private float _lastSec = 0f;
        private float _inCooldown = 0f;

        public bool CheckFrame()
        {
            return _frameLock <= 0;
        }

        public void RefineFrame()
        {
            _frameLock--;
        }

        public void Countdown()
        {
            if (Time.time - _lastSec > 1)
            {
                _lastSec = Time.time;
                _inCooldown -= 1;
            }
        }

        public void StartCountdown()
        {
            _lastSec = Time.time;
            _inCooldown = LockSeconds;
        }

        public bool CheckTime()
        {
            return _inCooldown <= 0;
        }

        public void StartLock()
        {
            _frameLock = LockFrames;
        }
    }
}