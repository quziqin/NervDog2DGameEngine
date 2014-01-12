using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using NervDog.Soul.Defs;
using NervDog.Utilities;

namespace NervDog.Managers
{
    public class AuditionManager
    {
        private static readonly AuditionManager _instance = new AuditionManager();
        private readonly Dictionary<string, Song> _BGMs;
        private readonly Dictionary<string, SoundEffect> _soundEffects;

        private AuditionManager()
        {
            _soundEffects = new Dictionary<string, SoundEffect>();
            _BGMs = new Dictionary<string, Song>();

            MediaPlayer.IsRepeating = true;
        }

        public static AuditionManager Instance
        {
            get { return _instance; }
        }

        public Dictionary<string, SoundEffect> SoundEffects
        {
            get { return _soundEffects; }
        }

        public Dictionary<string, Song> BGMS
        {
            get { return _BGMs; }
        }

        public void LoadAuditionResource(string fileName)
        {
            var def = XmlHelper.LoadFromFile<AuditionDef>(fileName);

            foreach (string i in def.SoundEffects)
            {
                var se = XNADevicesManager.Instance.ContentManager.Load<SoundEffect>(i);
                _soundEffects.Add(i, se);
            }

            foreach (string i in def.Songs)
            {
                var song = XNADevicesManager.Instance.ContentManager.Load<Song>(i);
                _BGMs.Add(i, song);
            }
        }

        public void PlayBGM(string name)
        {
            MediaPlayer.Play(_BGMs[name]);
        }

        public void PauseBGM()
        {
            MediaPlayer.Pause();
        }
    }
}