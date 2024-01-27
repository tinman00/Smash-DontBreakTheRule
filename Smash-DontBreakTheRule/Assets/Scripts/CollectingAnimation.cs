using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingAnimation : MonoBehaviour
{
    public void Disappear() {
        Destroy(gameObject);
    }
}
