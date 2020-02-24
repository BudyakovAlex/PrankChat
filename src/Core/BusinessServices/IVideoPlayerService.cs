using System;

namespace PrankChat.Mobile.Core.BusinessServices
{
    public interface IVideoPlayerService : IDisposable

    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IVideoPlayerService"/> sound is muted.
        /// </summary>
        bool Muted { get; set; }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        IVideoPlayer Player { get; }

        /// <summary>
        /// Plays the specified URI. Initialize playback, switch file.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="id">The video identifier.</param>
        void Play(string uri, int id);

        /// <summary>
        /// Plays this currently playing URI. Continue playing.
        /// </summary>
        void Play();

        /// <summary>
        /// Pauses current video.
        /// </summary>
        void Pause();

        /// <summary>
        /// Stops current video, release file.
        /// </summary>
        void Stop();
    }
}
