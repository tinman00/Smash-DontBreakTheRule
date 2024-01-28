using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject DisappearVFX;
    public void destroy() {
        Instantiate(DisappearVFX, transform.position, new Quaternion());
        Destroy(gameObject);
    }

    private void Update() {
        transform.rotation = new Quaternion();
    }
}
