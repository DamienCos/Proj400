using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;


namespace Blocker
{
    public class SoundManager : GameComponent
    {
        #region Private fields
        private ContentManager _content;

        private Dictionary<string, SoundEffect> soundBank; 

        private SoundEffectInstance[] _playingSounds = new SoundEffectInstance[MaxSounds];
        private string[,] soundNames;
        //private bool _isMusicPaused = false;

        //private bool _isFading = false;
        //private MusicFadeEffect _fadeEffect;
        #endregion

        // Change MaxSounds to set the maximum number of simultaneous sounds that can be playing.
        private const int MaxSounds = 32;
      
        /// <summary>
        /// Gets or sets the master volume for all sounds. 1.0f is max volume.
        /// </summary>
        public  float SoundVolume
        {
            get { return SoundEffect.MasterVolume; }
            set { SoundEffect.MasterVolume = value; }
        }

        /// <summary>
        /// Gets or sets the master volume for all sounds. 1.0f is max volume.
        /// </summary>
        public float MusicVolume
        {
            get { return SoundEffect.MasterVolume; }
            set { SoundEffect.MasterVolume = value; }
        }
        /// <summary>
        /// Gets whether a song is playing or paused (i.e. not stopped).
        /// </summary>
        //public bool IsSongActive { get { return _currentSong != null && MediaPlayer.State != MediaState.Stopped; } }

        /// <summary>
        /// Gets whether the current song is paused.
        /// </summary>
        //public bool IsSongPaused { get { return _currentSong != null && _isMusicPaused; } }

        /// <summary>
        /// Creates a new Audio Manager. Add this to the Components collection of your Game.
        /// </summary>
        /// <param name="game">The Game</param>
        public SoundManager(Game1 game)
            : base(game)
        {
            _content = new ContentManager(game.Content.ServiceProvider, game.Content.RootDirectory);
        }

        /// <summary>
        /// Creates a new Audio Manager. Add this to the Components collection of your Game.
        /// </summary>
        /// <param name="game">The Game</param>
        /// <param name="contentFolder">Root folder to load audio content from</param>
        public SoundManager(Game1 game, string contentFolder)
            : base(game)
        {
            _content = new ContentManager(game.Content.ServiceProvider, contentFolder);
        }

        /// <summary>
        /// Loads a SoundEffect into the AudioManager.
        /// </summary>
        /// <param name="soundName">Name of the sound to load</param>
        /// <param name="soundPath">Path to the song asset file</param>
        public void LoadSound()
        {
            string soundLocation = "Sounds/";
            soundNames = new string[,]
            {
                {"RedButton", "red"},
                {"DrumLoop", "lev2"},
                {"DrumLoop2", "lev3"},
                {"Click", "click"},
                {"Menu", "select"}
            };
            soundBank = new Dictionary<string, SoundEffect>();
            for (int i = 0; i < soundNames.GetLength(0); i++)
            {
                SoundEffect se = _content.Load<SoundEffect>(soundLocation + soundNames[i, 0]);
                soundBank.Add(soundNames[i, 1], se);
            }
        }

        /// <summary>
        /// Unloads all loaded songs and sounds.
        /// </summary>
        public void UnloadContent()
        {
            _content.Unload();
        }

       

      
        /// <summary>
        /// Plays the sound of the given name at the given volume.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        public void PlaySound(string soundName)
        {
            PlaySound(soundName, 0.0f, 0.0f);
        }

        /// <summary>
        /// Plays the sound of the given name with the given parameters.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        /// <param name="pitch">Pitch, -1.0f (down one octave) to 1.0f (up one octave)</param>
        /// <param name="pan">Pan, -1.0f (full left) to 1.0f (full right)</param>
        public void PlaySound(string soundName, float pitch, float pan)
        {
            SoundEffect sound;

            if (!soundBank.TryGetValue(soundName, out sound))
            {
                throw new ArgumentException(string.Format("Sound '{0}' not found", soundName));
            }

            int index = GetAvailableSoundIndex();

            if (index != -1)
            {
                _playingSounds[index] = sound.CreateInstance();
                _playingSounds[index].Volume = SoundVolume;
                _playingSounds[index].Pitch = pitch;
                _playingSounds[index].Pan = pan;
                _playingSounds[index].Play();

                if (!Enabled)
                {
                    _playingSounds[index].Pause();
                }
            }
        }

        public float GetVol()
        {
            return SoundVolume;
        }

        /// <summary>
        /// Plays the sound of the given name with the given parameters.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="SongVolume">Volume, 0.0f to 1.0f</param>
        /// <param name="pitch">Pitch, -1.0f (down one octave) to 1.0f (up one octave)</param>
        /// <param name="pan">Pan, -1.0f (full left) to 1.0f (full right)</param>
        public void PlaySound(string soundName, bool isLooped)
        {
            SoundEffect sound;

            if (!soundBank.TryGetValue(soundName, out sound))
            {
                throw new ArgumentException(string.Format("Sound '{0}' not found", soundName));
            }

            int index = GetAvailableSoundIndex();

            if (index != -1)
            {
                _playingSounds[index] = sound.CreateInstance();
                _playingSounds[index].Volume = MusicVolume; // looped sounds are the level tracks
                _playingSounds[index].Pitch = -1f;
                _playingSounds[index].Pan = -1f;
                _playingSounds[index].IsLooped = isLooped;
                _playingSounds[index].Play();
               
                if (!Enabled)
                {
                    _playingSounds[index].Pause();
                }
            }
        }

        /// <summary>
        /// Stops all currently playing sounds.
        /// </summary>
        public void StopAllSounds()
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i] != null)
                {
                    _playingSounds[i].Stop();
                    _playingSounds[i].Dispose();
                    _playingSounds[i] = null;
                }
            }
        }

        /// <summary>
        /// Called per loop unless Enabled is set to false.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Stopped)
                {
                    _playingSounds[i].Dispose();
                    _playingSounds[i] = null;
                }
            }

         
            base.Update(gameTime);
        }

        // Pauses all music and sound if disabled, resumes if enabled.
        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (Enabled)
            {
                for (int i = 0; i < _playingSounds.Length; ++i)
                {
                    if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Paused)
                    {
                        _playingSounds[i].Resume();
                    }
                }
            }
            else
            {
                for (int i = 0; i < _playingSounds.Length; ++i)
                {
                    if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Playing)
                    {
                        _playingSounds[i].Pause();
                    }
                }
            }

            base.OnEnabledChanged(sender, args);
        }

        // Acquires an open sound slot.
        private int GetAvailableSoundIndex()
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }

        #region MusicFadeEffect
        private struct MusicFadeEffect
        {
            public float SourceVolume;
            public float TargetVolume;

            private TimeSpan _time;
            private TimeSpan _duration;

            public MusicFadeEffect(float sourceVolume, float targetVolume, TimeSpan duration)
            {
                SourceVolume = sourceVolume;
                TargetVolume = targetVolume;
                _time = TimeSpan.Zero;
                _duration = duration;
            }

            public bool Update(TimeSpan time)
            {
                _time += time;

                if (_time >= _duration)
                {
                    _time = _duration;
                    return true;
                }

                return false;
            }

            public float GetVolume()
            {
                return MathHelper.Lerp(SourceVolume, TargetVolume, (float)_time.Ticks / _duration.Ticks);
            }
        }
        #endregion
    }
}
    /// <summary>
    /// Options for SoundManager.CancelFade
    /// </summary>
    public enum FadeCancelOptions
    {
        /// <summary>
        /// Return to pre-fade volume
        /// </summary>
        Source,
        /// <summary>
        /// Snap to fade target volume
        /// </summary>
        Target,
        /// <summary>
        /// Keep current volume
        /// </summary>
        Current

    }
