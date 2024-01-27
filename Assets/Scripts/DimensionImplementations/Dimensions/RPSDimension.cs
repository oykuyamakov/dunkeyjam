using System.Collections.Generic;
using DiceImplementations;
using TMPro;
using UnityCommon.Modules;
using UnityCommon.Runtime.Utility;
using UnityEngine;

namespace DimensionImplementations
{
    public class RpsDimension : DimensionController
    {
        [SerializeField] 
        private Sprite m_Rock;
        [SerializeField] 
        private Sprite m_Paper;
        [SerializeField] 
        private Sprite m_Scissor;

        [SerializeField] 
        private SpriteRenderer m_HandImage;

        private HashSet<int> m_RequiredRolls;
        protected override void Awake()
        {
            base.Awake();
            m_Dimension = Dimension.RPS;
            m_DimensionFailAction = new TimedAction(OnWrong, m_GeneralSettings.GameStartCountdown,m_GeneralSettings.RPSPeriod);
            
            SetRealm();
        }
        
        private void SetRealm()
        {
            m_RequiredRolls = new HashSet<int>();
            var rand = UnityEngine.Random.Range(1, 7);

            if (rand == 1 || rand == 6)
            {
                m_RequiredRolls.Add(1);
                m_RequiredRolls.Add(6);
                m_HandImage.sprite = m_Rock;
            }else if (rand == 2 || rand == 5)
            {
                m_RequiredRolls.Add(2);
                m_RequiredRolls.Add(5);
                m_HandImage.sprite = m_Paper;
            }else if (rand == 4 || rand == 3)
            {
                m_RequiredRolls.Add(4);
                m_RequiredRolls.Add(3);
                m_HandImage.sprite = m_Scissor;
            }
        }

        protected override void ResetRealm()
        {
            base.ResetRealm();
            SetRealm();
        }

        protected override void OnDicePlaced()
        {
            base.OnDicePlaced();
            
            if (m_RequiredRolls.Contains(m_InDice.GetRollResult()))
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
