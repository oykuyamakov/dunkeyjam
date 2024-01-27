using DiceImplementations;
using UnityCommon.Runtime.Utility;
using UnityEngine;

namespace DimensionImplementations.Dimensions
{
    public class CowboyDimension : DimensionController
    {
        [SerializeField] 
        private Sprite m_Happy;
        [SerializeField] 
        private Sprite m_Sad;

        [SerializeField] 
        private SpriteRenderer m_InteractImage;

        protected override void Awake()
        {
            base.Awake();
            m_Dimension = Dimension.Cowboy;
            m_DimensionFailAction = new TimedAction(OnWrong, m_GeneralSettings.GameStartCountdown,m_GeneralSettings.CowboyPeriod);

            m_InteractImage.sprite = m_Sad;
            SetRealm(0);
        }
        
        private void SetRealm(int roll)
        {
            switch (roll)
            {
                case 1:
                case 2:
                case 5:
                    m_InteractImage.sprite = m_Happy;
                    break;
                case 6:
                case 4:
                case 3:
                    m_InteractImage.sprite = m_Sad;
                    OnWrong();
                    break;
                default:
                    m_InteractImage.sprite = m_Sad;
                    break;
            }
        }

        public override void OnWrong()
        {
            base.OnWrong();
        }

        protected override void OnDicePlaced()
        {
            base.OnDicePlaced();
            
            SetRealm(m_InDice.GetRollResult());
        }


        public override void OnCorrect()
        {
            Debug.Log("Correct");
            base.OnCorrect();
        }  
    }
}
