using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    private float smoothTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    public static float limitLeft = -9;
    public static float limitRight = 232;
    public static float limitDown = -10;
    public static float limitUp = 10.5f;

    // Update is called once per frame
    void FixedUpdate () {
            Vector3 targetPosition = new Vector3(
                Mathf.Clamp(player.transform.position.x,limitLeft,limitRight),
                Mathf.Clamp(player.transform.position.y,limitDown,limitUp),0)
                + new Vector3(3, 0, -5);

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
