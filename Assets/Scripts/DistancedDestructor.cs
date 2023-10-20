using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancedDestructor : MonoBehaviour
{

    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }
    void Update()
    {
        if(!player)
        {
            player = GameObject.Find("Player");
            return;
        }
        if(player.transform.position.x - transform.position.x > 15f)
        {
            Destroy(gameObject);
        }
    }
}
