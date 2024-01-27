using System;
using DiceImplementations;
using Sirenix.OdinInspector;
using UnityCommon.Runtime.Utility;
using UnityEngine;

namespace DimensionImplementations
{
    public class DimensionArea : MonoBehaviour
    {
        [SerializeField][EnumToggleButtons]
        private Dimension m_Dimension;

        public Dimension Dimension => m_Dimension;
    
    }
}
