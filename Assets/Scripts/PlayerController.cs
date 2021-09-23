using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private GameObject _tree;
    public GameObject player;
    private GameManager _gameManager;

    private AudioSource _playerAudio;
    public AudioClip swingClip;
    
    private Animator _animator;
    private static readonly int AttackTrigger = 
        Animator.StringToHash("Attack_trigger");
    private static readonly int DeathTrigger = 
        Animator.StringToHash("Death_trigger");
    private static readonly int RespawnTrigger = 
        Animator.StringToHash("Respawn_trigger");

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        _tree = GameObject.Find("Tree");
        _animator = GetComponent<Animator>();
        _playerAudio = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameManager.gameActive) { return; }
        
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            SetPosition(PlayerPosition.Left);
        } 
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            SetPosition(PlayerPosition.Right);
        }

        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            var touchX = touch.position.x;
            if (touch.phase == TouchPhase.Began)
            {
                if (touchX < (float)Screen.width / 2)
                {
                    SetPosition(PlayerPosition.Left);
                }
                else
                {
                    SetPosition(PlayerPosition.Right);
                }
            }
        }
    }

    public void ResetPosition()
    {
        _gameManager.playerPosition = PlayerPosition.Left;
        var position = player.transform.position;
        var positionX = _tree.transform.position.x + 1;
        player.transform.rotation = Quaternion.Euler(0, -90, 0);
        player.transform.position = new Vector3(positionX, position.y, position.z);
    }

    private void SetPosition(PlayerPosition position)
    {
        if (!_gameManager.gameActive) { return; }
        _gameManager.playerPosition = position;

        var treeScaleX = _tree.transform.localScale.x;
        var offsetFactor = position == PlayerPosition.Left ? 1 : -1;
        
        var positionX = offsetFactor * treeScaleX + _gameManager.basicPositionX;
        var playerPosition = player.transform.position;
        var positionY = playerPosition.y;
        var positionZ = playerPosition.z;
        
        var newPosition = new Vector3(positionX, positionY, positionZ);
        player.transform.position = newPosition;

        var degreeY = position == PlayerPosition.Left ? -90 : 90;
        player.transform.rotation = Quaternion.Euler(0, degreeY, 0);
        
        SwingSword(position);
    }

    private void SwingSword(PlayerPosition position)
    {
        _animator.SetTrigger(AttackTrigger);
        _playerAudio.PlayOneShot(swingClip, 1);
        _gameManager.OnPlayerSwingSword(position);
    }

    public void KillPlayer()
    {
        _animator.SetTrigger(DeathTrigger);
    }

    public void RespawnPlayer()
    {
        _animator.SetTrigger(RespawnTrigger);
    }
}
