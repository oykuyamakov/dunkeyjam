using System.Collections;
using System.Collections.Generic;
using DiceImplementations;
using Roro.Scripts.Sounds.Core;
using UnityCommon.Runtime.Utility;
using UnityEngine;
using Utility;

namespace DimensionImplementations.Dimensions
{
    public class MonsterDimension : DimensionController
    {
        [SerializeField] 
        private SpriteRenderer m_InteractImage;

        [SerializeField]
        protected Sprite MonsterEatSprite;
        [SerializeField]
        protected Sprite MonsterNormalSprite;

        protected override void Awake()
        {
            base.Awake();
            m_Dimension = Dimension.Fish;
            m_DimensionFailAction = new TimedAction(OnWrong, m_GeneralSettings.GameStartCountdown,m_GeneralSettings.FishDimensionPeriod);
        }
        
        protected override void OnDicePlaced()
        {
            base.OnDicePlaced();
            
            OnCorrect();
        }

        public override void OnWrong()
        {
            base.OnWrong();
            
            SoundManager.Instance.PlayOneShot(SoundDatabase.Get().MonsterHugnry);

        }

        public override void OnCorrect()
        {
            base.OnCorrect();
            
            SoundManager.Instance.PlayOneShot(SoundDatabase.Get().MonsterChew);
            
            StartCoroutine(AnimateMonster());
        }

        private IEnumerator AnimateMonster()
        {
            m_InteractImage.sprite = MonsterEatSprite;
            yield return new WaitForSeconds(0.2f);
            m_InteractImage.sprite = MonsterNormalSprite;
            yield return new WaitForSeconds(0.2f);
            m_InteractImage.sprite = MonsterEatSprite;
            yield return new WaitForSeconds(0.2f);
            m_InteractImage.sprite = MonsterNormalSprite;

        }
    }
}
