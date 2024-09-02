using Runtime.Enums;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [System.Serializable]
    public struct GameSoundData
    {
        public GameSoundType gameSoundType;
        public AudioClip audioClip;
        public bool hasRandomPitch;
        public bool hasGlissando;
    }
}