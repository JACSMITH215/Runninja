using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceScript : MonoBehaviour
{
    public bool canHit = false;
    private PlayerAxisMovement player;
    public AudioClip hitSound;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAxisMovement>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Lantern") && canHit){
            SoundFXManager.instance.PlaySoundFXClip(hitSound, transform, 1f);
            Destroy(other.gameObject);
            player.Jump();
            canHit = false;
        }
        if(other.gameObject.CompareTag("Enemy") && canHit){
            if(other.GetComponent<SamuraiScript>().hurtTimer <= 0){
                SoundFXManager.instance.PlaySoundFXClip(hitSound, transform, 1f);
                other.GetComponent<SamuraiScript>().Hurt();
                player.Jump();
                canHit = false;
            }
        }
    }
}
