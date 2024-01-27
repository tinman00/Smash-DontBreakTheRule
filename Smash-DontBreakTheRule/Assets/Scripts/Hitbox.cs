using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private Player owner;
    [SerializeField] private int damage;
    private int ownerIdentity;
    private bool isActive;
    private List<GameObject> list;

    private void Awake() {
        ownerIdentity = owner.id;
        list = new();
    }

    private bool HadCollided(GameObject obj) {
        return list.Contains(obj);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!isActive) return;
        if (other.tag == "Player") {
            if (HadCollided(other.gameObject)) return;
            list.Add(other.gameObject);

            var plr = other.GetComponent<Player>();
            Vector2 direction = other.transform.position - owner.transform.position;
            plr.Attacked(damage, direction, owner);
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (!isActive) return;
        if (other.tag == "Player") {
            if (HadCollided(other.gameObject)) return;
            list.Add(other.gameObject);

            var plr = other.GetComponent<Player>();
            Vector2 direction = other.transform.position - owner.transform.position;
            plr.Attacked(damage, direction, owner);
        }
    }

    public void Activate() {
        isActive = true;
    }

    public void Deactivate() {
        isActive = false;
        list.Clear();
    }
}
