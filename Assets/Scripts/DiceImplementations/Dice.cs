using System;
using System.Collections.Generic;
using DimensionImplementations;
using Roro.Scripts.Sounds.Core;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityCommon.Runtime.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Utility;

namespace DiceImplementations
{
    [RequireComponent(typeof(BoxCollider))]
    public class Dice : MonoBehaviour
    {
        [SerializeField]
        protected List<DiceDimensionPair> m_DimensionDices;

        private Dictionary<Dimension, GameObject> m_DiceObjectsByDimensions = new Dictionary<Dimension, GameObject>();

        private Dictionary<Vector3, int> m_ValuesByFaces = new Dictionary<Vector3, int>();
        
        private int m_CurrentValue = 0;
        
        private Dimension m_CurrentDimension;

        private bool m_InBowl;
        public bool m_Freezed;
        
        private void Awake()
        {

        }

        public void Initialize()
        {
            m_InBowl = false;
            
            m_DiceObjectsByDimensions = new Dictionary<Dimension, GameObject>();
            
            for (int i = 0; i < m_DimensionDices.Count; i++)
            {
                //Debug.Log(m_DimensionDices[i].Dimension);
                m_DiceObjectsByDimensions.Add(m_DimensionDices[i].Dimension, m_DimensionDices[i].DiceObject);
            }
            Reset();
        }

        public void SwitchDimension(Dimension dimension)
        {
            Debug.Log(dimension);
            m_CurrentDimension = dimension;
            
            SoundManager.Instance.PlayOneShot(SoundDatabase.Get().DiceSwitchDimension);
            
            foreach (var t in m_DimensionDices)
            {
                t.DiceObject.GetComponent<MeshRenderer>().enabled = false;
            }
            
            m_DiceObjectsByDimensions[dimension].GetComponent<MeshRenderer>().enabled = true;
        }
        

        public int GetRollResult()
        {
            return m_CurrentValue;
        }

        private void Update()
        {
            if(GetComponent<Rigidbody>().velocity.magnitude < 0.01f)
                CheckRollResult();


            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f, layerMask: LayerMask.GetMask("DimensionArea")))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("DimensionArea"))
                {
                    var area = hit.collider.GetComponent<DimensionArea>();
                    SwitchDimension(area.Dimension);
                }
            }

        }


        public void Move(Vector3 position)
        {
            transform.position = position.WithY(transform.position.y);
        }

        public void Place()
        {
            
        }

        public void CheckRollResult()
        {
            if(!m_InBowl || m_Freezed)
                return;

            m_Freezed = true;
            
            if(transform.forward.y > 0.9f)
            {
                m_CurrentValue = 1;
            }else if (transform.forward.y < -0.9f)
            {
                m_CurrentValue = 6;
            }else if (transform.right.y > 0.9f)
            {
                m_CurrentValue = 2;
            }else if (transform.right.y < -0.9f)
            {
                m_CurrentValue = 5;
            }else if (transform.up.y > 0.9f)
            {
                m_CurrentValue = 4;
            }else if (transform.up.y < -0.9f)
            {
                m_CurrentValue = 3;
            }
            
            GetComponent<Rigidbody>().isKinematic = true;
            
            SoundManager.Instance.PlayOneShot(SoundDatabase.Get().DiceHitBowl);

            Debug.Log(m_CurrentValue);
        }
        
        public void GetSpawned()
        {
            
        }
        
        public void GetDestroyed()
        {
            DiceSpawner.Instance.ReturnDice(this);
        }

        public void Reset()
        {
            SwitchDimension(Dimension.Original);
            m_CurrentDimension = Dimension.Original;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Bowl"))
            {
                m_InBowl = true;
            }
        }


    }

    [Serializable]
    public class DiceDimensionPair
    {
        [EnumToggleButtons][SerializeField]
        public Dimension Dimension;
        
        public GameObject DiceObject;
    }

    [Serializable]
    public struct DiceFacePair
    {
        public Vector3 Direction;
        public int Value;
    }
    
    public enum Dimension
    {
        Original,
        Fish,
        RPS,
        DatingSim,
        RPG,
        Slot,
        Monster,
        Cowboy
    }
}




//0,-1,1 forward
//
