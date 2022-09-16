using System;
using UnityEngine;

namespace Objects.Ground_Pieces
{
    public class GroundPiece : MonoBehaviour
    {
        public GenerateWorld generateWorld;
        public float movementRange;
        public bool reached = false;
        public bool left = false;

        private String _playerTag = "Player"; 
        
        private void OnTriggerEnter(Collider other)
        {
            reached = true;
            if(other.CompareTag(_playerTag))
                generateWorld.NextStep();
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag(_playerTag))
                left = true;
        }
    }
}
