using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockPlayerTrigger : MonoBehaviour
{
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            print("stun player");
            _player.GetStunned(2);
        }
    }
}