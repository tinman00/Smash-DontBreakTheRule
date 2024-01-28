using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUp{
    None,
    Shield,
    Mace,
    Shoe,
    Bomb
}

public class Pickup : MonoBehaviour
{
    public GameObject Anim;
    public float duration = 10f;
    [SerializeField] PickUp type;

    private float startTime = 0f;

    private void Start() {
        startTime = Time.time;
    }

    private void Update() {
        if (Time.time - startTime >= duration) {
            Pick();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            var plr = other.gameObject.GetComponent<Player>();
            if (!plr.CanPick()) return;
            plr.AddPickup(type);
            Pick();
            if (type != PickUp.None && RuleManager.instance.HasRule[(int)Rule.PickItem]) {
                RuleManager.instance.BreakRule(Rule.PickItem, plr);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player") {
            var plr = other.gameObject.GetComponent<Player>();
            if (!plr.CanPick()) return;
            plr.AddPickup(type);
            Pick();
            if (type != PickUp.None && RuleManager.instance.HasRule[(int)Rule.PickItem]) {
                RuleManager.instance.BreakRule(Rule.PickItem, plr);
            }
        }
    }

    void Pick() {
        Instantiate(Anim, transform.position, new Quaternion());
        Destroy(gameObject);
    }
}
