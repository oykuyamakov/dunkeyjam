using System;
using System.Collections.Generic;
using Pooling;
using Roro.Scripts.Misc;
using Roro.Scripts.SettingImplementations;
using Roro.Scripts.Sounds.Core;
using Unity.Mathematics;
using UnityCommon.Runtime.Utility;
using UnityCommon.Singletons;
using UnityEngine;
using Utility;

namespace DiceImplementations
{
    public class DiceSpawner : SingletonBehaviour<DiceSpawner>
    {
        [SerializeField]
        private Dice m_OriginalDicePrefab;
        // [SerializeField]
        // private Dice m_FishDimensionPrefab;
        
        private Queue<Dice> m_OriginalDiceQueue = new Queue<Dice>();
        private Queue<Dice> m_FishDiceQueue = new Queue<Dice>();
        
        private TimedAction m_OriginalDiceSpawnAction;

        private GeneralSettings m_GS;
        
        private void Awake()
        {
            //TODO
            if(!SetupInstance())
                return;
            
            InitializeDicePools();
            
            m_GS  =GeneralSettings.Get();

            m_OriginalDiceSpawnAction = new TimedAction(SpawnOriginalDice,m_GS.FirstDiceSpawnDelay, m_GS.DiceSpawnPeriod);
        }

        private void Update()
        {
            m_OriginalDiceSpawnAction.Update(Time.deltaTime);
        }

        private void InitializeDicePools()
        {
            m_OriginalDiceQueue = new Queue<Dice>();
            for (int i = 0; i < 200; i++)
            {
                var dice = GameObject.Instantiate(m_OriginalDicePrefab, Vector3.up * 100, Quaternion.identity, this.transform);
                m_OriginalDiceQueue.Enqueue(dice);
                dice.Initialize();
                dice.gameObject.SetActive(false);
            }
        }
        
        private void SpawnOriginalDice()
        {
            SoundManager.Instance.PlayOneShot(SoundDatabase.Get().DiceSpawn);

            
            var dice = m_OriginalDiceQueue.Dequeue();
            dice.gameObject.SetActive(true);
            dice.transform.position = Camera.main.transform.position + Vector3.up;
            var randomRotation = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
            dice.transform.rotation = Quaternion.Euler(randomRotation);
            dice.GetComponent<Rigidbody>().AddForce(Vector3.down * 500 , ForceMode.Impulse);
            dice.Reset();
        }
        
        public void ReturnDice(Dice dice)
        {
            dice.Reset();
            dice.gameObject.SetActive(false);
            m_OriginalDiceQueue.Enqueue(dice);
        }
    }
}
