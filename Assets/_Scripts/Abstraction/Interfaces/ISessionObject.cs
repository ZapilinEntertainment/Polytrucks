namespace ZE.Polytrucks
{
    public interface ISessionObject 
    {
        public void OnSessionStart();
        public void OnSessionEnd();
        public void OnSessionPause();
        public void OnSessionResume();
    }
}
