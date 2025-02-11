using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenScript : MonoBehaviour
{
    private Transform player;
    public AudioClip hitSound;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy")){
            if(other.GetComponent<SamuraiScript>().hurtTimer <= 0){
                SoundFXManager.instance.PlaySoundFXClip(hitSound, transform, 1f);
                other.GetComponent<SamuraiScript>().Hurt();
                Destroy(this.gameObject);
            }
        }
    }

    void Update(){
        if(Math.Abs(transform.position.x-player.transform.position.x) > 5){
            Destroy(this.gameObject);
        }
    }
}
