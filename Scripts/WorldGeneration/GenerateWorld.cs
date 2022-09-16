using System.Collections.Generic;
using Objects.Ground_Pieces;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

public class GenerateWorld : MonoBehaviour
{
    public GameObject groundParent;
    public List<GroundPieceObject> groundPieces;
    private readonly Queue<GroundPiece> _placedGroundPieces = new Queue<GroundPiece>();
    private readonly RandomObject<GroundPieceObject> _randomGroundObject = new RandomObject<GroundPieceObject>();
    public Vector3 startPoint = Vector3.down;
    public int startingNoOfPieces = 5;
    public int requiredNoOfPieces = 7;
    
    private void Start()
    {
        _randomGroundObject.AddList(groundPieces);

        for (int i = 0; i < startingNoOfPieces; i++)
        {
            PlaceRandomGroundPiece();
        }
    }
    
    private void PlaceGroundPiece(GroundPieceObject pieceObject)
    {
        // Parameters
        Vector3 offset = pieceObject.offset;
        Vector3 size = pieceObject.size;
        GameObject mesh = pieceObject.mesh;
        Vector3 position = startPoint;
        
        // Instantiate the piece
        position.z = startPoint.z + offset.z + size.z / 2;
        GameObject thisPiece = Instantiate(mesh, position, Quaternion.Euler(Vector3.forward));
        thisPiece.transform.parent = groundParent.transform;
        
        
        // Set up the piece
        GroundPiece groundPiece = thisPiece.AddComponent<GroundPiece>();
        groundPiece.generateWorld = this;
        groundPiece.movementRange = pieceObject.movementRange;
        _placedGroundPieces.Enqueue(groundPiece);
        //  collider
        BoxCollider boxCollider = thisPiece.transform.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(50, 50, pieceObject.size.z);
        boxCollider.isTrigger = true;
        
        // set up for next start point
        startPoint.x = position.x;
        startPoint.y = position.y;
        startPoint.z = position.z + offset.z + size.z / 2;
    }

    [Button]
    public void NextStep()
    {
        PlaceRandomGroundPiece();
        DeleteAPiece();
    }
    
    [Button]
    private void PlaceRandomGroundPiece() => PlaceGroundPiece(_randomGroundObject.getRandom());

    [Button]
    private void DeleteAPiece()
    {
        GroundPiece g = _placedGroundPieces.Peek();
        if (g.left)
        {
            _placedGroundPieces.Dequeue();
            Destroy(g.gameObject);
        }
    }
}