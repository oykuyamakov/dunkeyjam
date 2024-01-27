using DiceImplementations;
using Roro.Scripts.Sounds.Core;
using UnityEngine;
using Utility;

namespace PlayerManagement
{
    public class InputController : MonoBehaviour
    {

        private Dice m_HoldedDice;
        public float RaycastDistance = 10f;
        private Vector3 GetMousePoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create ray from camera into the world through the mouse cursor position.
 
            RaycastHit hitInfo; // Empty var to store info about the thing we hit

            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, RaycastDistance, LayerMask.GetMask("Ground")))
            {
                
            } // If raycast hit a collider

            return hitInfo.point;
        }
        
        
        
        private void Update()
        {
            
            CastRay();
            
            m_HoldedDice?.Move(GetMousePoint());
            
            
            if (Input.GetMouseButtonUp(0))
            {
                m_HoldedDice = null;
            }
        }
        
        private void CastRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create ray from camera into the world through the mouse cursor position.
 
            RaycastHit hitInfo; // Empty var to store info about the thing we hit
 
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, RaycastDistance,  LayerMask.GetMask("Dice"))) // If raycast hit a collider...
            {

                if (Input.GetMouseButtonDown(0))
                {
                    SoundManager.Instance.PlayOneShot(SoundDatabase.Get().DicePick);

                    m_HoldedDice = hitInfo.transform.GetComponent<Dice>();
                }
                //dice.Move(GetMousePoint());
            }
            else
            {
                // Not hitting anything
            }
        }

    }
}
