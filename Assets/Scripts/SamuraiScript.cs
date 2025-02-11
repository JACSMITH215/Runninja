using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiScript : MonoBehaviour
{
    public int hurtTimer = 0;
    public void Attack(){
        GetComponent<Animator>().SetBool("attack", true);
    }

    public void Hurt(){
        GetComponent<Animator>().SetBool("hurt", true);
        hurtTimer = 60;
    }

    void Update(){
        if(hurtTimer>0){
            hurtTimer--;
            if(hurtTimer==0){
                Destroy(this.gameObject);
            }
        }
    }
}
