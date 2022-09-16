using UnityEngine;

namespace Objects.Ground_Pieces
{
    [CreateAssetMenu(fileName = "Ground Piece Object", menuName = "Objects/Ground Piece Object")]
    public class GroundPieceObject : ScriptableObject
    {
        public GameObject mesh;
        public Vector3 offset = new Vector3(0, 0, 0);
        public Vector3 size = new Vector3(10, 10, 40);
        public float movementRange = 10f;
        public float dificulty = 0.5f;
    }
}