using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    private Transform player;
    public bool defeated = false;
    [SerializeField] private Rigidbody2D bossHead;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        defeated = false;
        bossHead.bodyType = RigidbodyType2D.Static;
    }

    public void Defeat(){
        defeated = true;
        bossHead.bodyType = RigidbodyType2D.Dynamic;
        bossHead.gravityScale = 1f;
        bossHead.velocity = new Vector2(5, 4);
        GameManager.instance.canPause = false;
        GameManager.instance.StartSlowMotion();
    }

    void Update()
    {
        if(defeated){
            if(!GameManager.instance.win){
                if(player.transform.position.y - bossHead.transform.position.y > 4){
                    GameManager.instance.win = true;
                    GameManager.instance.StopSlowMotion();
                    Destroy(bossHead);
                }
            }
        }
    }
}
