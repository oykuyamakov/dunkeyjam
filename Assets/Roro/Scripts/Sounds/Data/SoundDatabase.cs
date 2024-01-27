using System.Collections.Generic;
using Roro.Scripts.Sounds.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utility
{
    [CreateAssetMenu(fileName = " SoundDatabase")]
    public class SoundDatabase : ScriptableObject
    {
        private static SoundDatabase _SoundDatabase;

        private static SoundDatabase soundDatabase
        {
            get
            {
                if (!_SoundDatabase)
                {
                    _SoundDatabase = Resources.Load<SoundDatabase>($"Databases/SoundDatabase");

                    if (!_SoundDatabase)
                    {
#if UNITY_EDITOR
                        Debug.Log("SoundDatabase not found AND NOT creating and a new one");
                        //_GeneralSettings = CreateInstance<SoundDatabase>();
                        // var path = "Assets/Resources/Settings/SoundDatabase.asset";
                        // AssetDatabaseHelpers.CreateAssetMkdir(_SoundDatabase, path);
#else
        				//		throw new Exception("SoundDatabase could not be loaded");
#endif
                    }
                }

                return _SoundDatabase;
            }
        }

        public static SoundDatabase Get()
        {
            return soundDatabase;
        }

        public Sound LevelWinSound;
        public Sound LevelFailSound;
        public Sound LevelStartSound;
        
        public Sound LevelMusic;

        public Sound DicePick;
        public Sound DiceLock;
        public Sound DiceSwitchDimension;
        public Sound DiceHitBowl;
        public Sound DiceSpawn;
        
        public Sound MonsterHugnry;
        public Sound MonsterChew;
        
        public Sound Fish;
        public Sound NoFish;
        
        public List<Sound> WomanHappies;
        public List<Sound> WomanSads;
        public List<Sound> WomanNeutrals;
        
        public Sound GetRandomWoamnHappy()
        {
            return WomanHappies[Random.Range(0, WomanHappies.Count)];
        }
         public Sound GetRandomWomanSad()
        {
            return WomanHappies[Random.Range(0, WomanSads.Count)];
        }
         public Sound GetRandomWomanNeutral()
        {
            return WomanHappies[Random.Range(0, WomanNeutrals.Count)];
        }
        
    }
}