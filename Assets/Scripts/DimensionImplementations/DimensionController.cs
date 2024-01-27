using System;
using System.Text;
using Based2.TimeManagement.Core;
using DiceImplementations;
using Roro.Scripts.SettingImplementations;
using Roro.Scripts.Sounds.Core;
using TMPro;
using UnityCommon.Modules;
using UnityCommon.Runtime.Utility;
using UnityEngine;
using Utility;

namespace DimensionImplementations
{
    public abstract class DimensionController : MonoBehaviour
    {
        [SerializeField] protected TextMeshPro m_TimerText;
        [SerializeField] protected SpriteRenderer m_DummyCorrect;
        [SerializeField] protected SpriteRenderer m_DummyResetFalse;
        
        protected Dimension m_Dimension;
        
        protected Dice m_InDice;
        
        protected Conditional m_PlacementConditional;

        protected bool m_DicePlaced;

        protected TimedAction m_DimensionFailAction;

        protected GeneralSettings m_GeneralSettings;

        protected virtual void Awake()
        {
            m_GeneralSettings = GeneralSettings.Get();
            m_DummyCorrect.enabled = false;
            m_DummyResetFalse.enabled = false;
        }

        protected virtual void Update()
        {
            m_DimensionFailAction?.Update(Time.deltaTime);
            
            if(m_DimensionFailAction == null) 
                return;
            m_TimerText.text = m_DimensionFailAction.Remaining.ToString("0");
        }

        public virtual void OnCorrect()
        {
            Debug.Log("Correct");

            m_DummyCorrect.enabled = true;
            
            Conditional.Wait(0.2f).Do(ResetRealm);
        }

        public virtual void OnNeutral()
        {
            Debug.Log("Neutral");
            
            Conditional.Wait(0.2f).Do(ResetRealm);
        }

        public virtual void OnWrong()
        {
            Debug.Log("Wrong");
            
            m_DummyResetFalse.enabled = true;
            
            Conditional.Wait(0.2f).Do(ResetRealm);
        }

        protected virtual void ResetRealm()
        {
            m_DummyResetFalse.enabled = false;
            m_DummyCorrect.enabled = false;
        }

        protected virtual void OnDicePlaced()
        {
            if(m_InDice == null)
                return;
            
            m_DicePlaced = true;
            
            m_DimensionFailAction.ResetToPeriod();
            
            m_InDice.GetDestroyed();
            
            SoundManager.Instance.PlayOneShot(SoundDatabase.Get().DiceLock);

            
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Dice"))
            {
                m_InDice = other.GetComponent<Dice>();
                m_InDice.Place();

                m_PlacementConditional = Conditional.While(() => m_InDice != null && Input.GetMouseButton(0)).Do(
                    () =>
                    {
                        Debug.Log("Waiting for Dice to be placed");
                    }).OnComplete(() =>
                {
                    Debug.Log(m_InDice);
                    if (m_InDice != null && !Input.GetMouseButton(0))
                    {
                        OnDicePlaced();
                    }
                });
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Dice"))
            {
                if (!m_DicePlaced)
                {
                    m_PlacementConditional.Cancel();
                }
                else
                {
                    m_InDice = null;
                }
            }
        }
    }
}
