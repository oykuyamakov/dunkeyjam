using System.Collections.Generic;
using DiceImplementations;
using Roro.Scripts.Sounds.Core;
using UnityCommon.Runtime.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace DimensionImplementations
{
    public class DatingDimension : DimensionController
    {

        [SerializeField] 
        private Sprite m_Happy;
        [SerializeField] 
        private Sprite m_Sad;
        [SerializeField] 
        private Sprite m_Neutral;

        [SerializeField] 
        private SpriteRenderer m_InteractImage;

        protected override void Awake()
        {
            base.Awake();
            m_Dimension = Dimension.DatingSim;
            m_DimensionFailAction = new TimedAction(OnWrong, m_GeneralSettings.GameStartCountdown,m_GeneralSettings.DatingPeriod);

            m_InteractImage.sprite = m_Neutral;

            SetRealm(0);
        }
        
        private void SetRealm(int roll)
        {
            switch (roll)
            {
                case 1:
                case 6:
                    OnNeutral();
                    break;
                case 2:
                case 5:
                    OnCorrect();
                    break;
                case 4:
                case 3:
                    OnWrong();
                    break;
                default:
                    m_InteractImage.sprite = m_Neutral;
                    break;
            }
        }

        public override void OnNeutral()
        {
            SoundManager.Instance.PlayOneShot(SoundDatabase.Get().GetRandomWomanNeutral());

            base.OnNeutral();
            
            m_InteractImage.sprite = m_Neutral;
        }

        public override void OnWrong()
        {
            SoundManager.Instance.PlayOneShot(SoundDatabase.Get().GetRandomWomanSad());

            
            base.OnWrong();
            
            m_InteractImage.sprite = m_Sad;
        }
        
        public override void OnCorrect()
        {
            m_InteractImage.sprite = m_Happy;
            SoundManager.Instance.PlayOneShot(SoundDatabase.Get().GetRandomWoamnHappy());

            Debug.Log("Correct");
            base.OnCorrect();
        }  

        protected override void OnDicePlaced()
        {
            base.OnDicePlaced();
            
            SetRealm(m_InDice.GetRollResult());
        }


        
    }
}