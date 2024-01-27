using System;
using System.Collections;
using System.Collections.Generic;
using DiceImplementations;
using Roro.Scripts.Sounds.Core;
using UnityCommon.Runtime.Utility;
using UnityEngine;
using Utility;

namespace DimensionImplementations
{
    public class FishingDimension : DimensionController
    {
        [SerializeField] 
        private SpriteRenderer m_AnimationImage;
        
        [SerializeField] 
        private List<Sprite> m_Animation;

        private HashSet<int> m_RequiredRolls;
        protected override void Awake()
        {
            base.Awake();
            m_Dimension = Dimension.Fish;
            m_DimensionFailAction = new TimedAction(OnWrong, m_GeneralSettings.GameStartCountdown,m_GeneralSettings.FishDimensionPeriod);
        }

        protected override void Update()
        {
            base.Update();
            
            //animate 
            
            m_AnimationImage.sprite = m_Animation[(int) (Time.time * 5) % m_Animation.Count];
        }

        public override void OnWrong()
        {
            base.OnWrong();
            
            SoundManager.Instance.PlayOneShot(SoundDatabase.Get().NoFish);

        }

        public override void OnCorrect()
        {
            base.OnCorrect();
            
            SoundManager.Instance.PlayOneShot(SoundDatabase.Get().Fish);

        }

        protected override void OnDicePlaced()
        {
            base.OnDicePlaced();
            
            if (m_InDice.GetRollResult() == 6)
            {
                OnCorrect();
            }
            else
            {
                OnWrong();
            }
        }

    }
}
