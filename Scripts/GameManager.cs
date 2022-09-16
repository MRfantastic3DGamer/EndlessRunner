using Objects.Ground_Pieces;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null) {
                    GameObject obj = new GameObject ();
                    obj.name = typeof(GameManager).Name;
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    
    public GameObject player;
    
    public virtual void Awake ()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad (this.gameObject);
        } else {
            Destroy (gameObject);
        }
    }
    
    
}