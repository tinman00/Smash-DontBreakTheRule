using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject shockWave;
    public float radius = 5.2f;
    public void Shock() {
        var wave = Instantiate(shockWave, transform.position, new Quaternion());
        wave.GetComponent<ShockWave>().CallShockWave();
    }
    public void Explode() {
        foreach (var plr in Player.list) {
            var dis = plr.transform.position - transform.position;
            if (dis.magnitude <= radius) {
                plr.Attacked(20, new(), null);
            }
        }
    }
    public void destroy() {
        Destroy(gameObject);
    }
}
