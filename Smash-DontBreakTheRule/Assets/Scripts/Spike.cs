using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            var plr = other.gameObject.GetComponent<Player>();
            plr.Attacked(20, new(), null);
        }
    }
    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            var plr = other.gameObject.GetComponent<Player>();
            plr.Attacked(20, new(), null);
        }
    }
}
