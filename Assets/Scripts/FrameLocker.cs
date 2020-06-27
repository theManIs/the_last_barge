namespace Assets.Scripts
{
    public class FrameLocker
    {
        public int LockFrames = 60;

        private int _frameLock = 0;

        public bool CheckFrame()
        {
            return _frameLock <= 0;
        }

        public void RefineFrame()
        {
            _frameLock--;
        }

        public void StartLock()
        {
            _frameLock = LockFrames;
        }
    }
}