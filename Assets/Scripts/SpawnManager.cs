using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject woodBranchPrefab;
    public GameObject woodCubePrefab;
    private GameManager _gameManager;
    private int _branchLimit = 5;
    private readonly Queue<PlayerPosition> _branchQueue = new Queue<PlayerPosition>();

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        
        SpawnBranches();
    }

    public void SpawnWoodCube(PlayerPosition position)
    {
        var newPosition = new Vector3(
            _gameManager.basicPositionX, 
            _gameManager.basicPositionY + 1.0f, 
            _gameManager.basicPositionZ + 0.6f
        );
        var newCube = Instantiate(
            woodCubePrefab, newPosition, new Quaternion()
        );

        var directionFactor = position == PlayerPosition.Left ? -1 : 1;
        var force = new Vector3(directionFactor * 10, 10, 0);
        newCube.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }

    public void SpawnBranches()
    {
        if (_branchQueue.Count == 0)
        {
            _branchQueue.Enqueue(PlayerPosition.Right);
        }
        else
        {
            _branchQueue.Dequeue();
        }
        RemoveBranches();

        var spawnCount = _branchLimit - _branchQueue.Count;
        for (int i = 1; i <= spawnCount; i++)
        {
            var isLeft = Random.Range(0.0f, 1.0f) < 0.5f;
            var position = isLeft ? PlayerPosition.Left : PlayerPosition.Right;

            if (_branchQueue.Count == 0)
            {
                position = PlayerPosition.Right;
            }
            
            _branchQueue.Enqueue(position);
        }

        for (int i = 0; i < _branchQueue.Count; i++)
        {
            var position = _branchQueue.ElementAt(i);
            SpawnBranch(i, position);

            if (position == _gameManager.playerPosition && i == 0)
            {
                _gameManager.GameOver();
            }
        }
    }

    public void RespawnBranches()
    {
        _branchQueue.Clear();
        RemoveBranches();
        SpawnBranches();
    }

    private void RemoveBranches()
    {
        var objects = GameObject.FindGameObjectsWithTag("Branch");
        foreach (var woodBranch in objects)
        {
            Destroy(woodBranch);
        }
    }

    private void SpawnBranch(int index, PlayerPosition position)
    {
        var degreeZ = position == PlayerPosition.Left ? 0 : 180;
        var directionFactor = position == PlayerPosition.Left ? -1 : 1;
        
        var positionX = _gameManager.basicPositionX + 0.5f * directionFactor;
        var positionY = _gameManager.basicPositionY * index * 2 + 1.0f;
        var positionZ = _gameManager.basicPositionZ + 2.1f;
        var newPosition = new Vector3(positionX, positionY, positionZ);
        
        Instantiate(woodBranchPrefab, newPosition, Quaternion.Euler(0, 0, degreeZ));
    }
}
