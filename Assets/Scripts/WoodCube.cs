using System;
using UnityEngine;

public class WoodCube : MonoBehaviour
{
    private GameManager _gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        var position = gameObject.transform.position;
        var horizontalOverflow = Math.Abs(position.x - _gameManager.basicPositionX) > 3;
        var verticalOverflow = Math.Abs(position.y - _gameManager.basicPositionY) > 3;
        
        if (horizontalOverflow || verticalOverflow)
        {
            Destroy(gameObject);
        }
    }
}
