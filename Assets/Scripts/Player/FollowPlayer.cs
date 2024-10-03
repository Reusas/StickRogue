using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    bool shouldFollow = false;
    Transform player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldFollow)
        {
            transform.position = new Vector3(player.position.x,player.position.y +6f,-10);
        }
    }

    public void StartFollowPlayer(Transform _player)
    {
        shouldFollow = true;
        player = _player;
    }
}
