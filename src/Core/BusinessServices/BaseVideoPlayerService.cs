namespace PrankChat.Mobile.Core.BusinessServices
{
    public abstract class BaseVideoPlayerService : IVideoPlayerService
    {
        public virtual void Dispose()
        {
        }

        public abstract bool Muted { get; set; }

        public abstract IVideoPlayer Player { get; }

        public abstract void Play(string uri, int id);

        public abstract void Play();

        public abstract void Pause();

        public abstract void Stop();
    }
}
