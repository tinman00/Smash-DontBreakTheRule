using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemStorage : MonoBehaviour
{
    private PickUp[] pickups;
    private int cnt = 0;

    [SerializeField] private Image skl1;
    [SerializeField] private Image skl2;

    [SerializeField] private Sprite None;
    [SerializeField] private Sprite Shield;
    [SerializeField] private Sprite Mace;
    [SerializeField] private Sprite Bomb;
    [SerializeField] private Sprite Shoe;
    void Start()
    {
        pickups = new PickUp[2] { PickUp.None, PickUp.None };
        cnt = 0;
    }
    private void Update() {
        if (cnt == 2 && RuleManager.instance.HasRule[(int)Rule.Have2Item]) {
            RuleManager.instance.BreakRule(Rule.Have2Item, GetComponent<Player>());
        }
        switch(pickups[0]) {
            case PickUp.None:
                skl1.sprite = None;
                break;
            case PickUp.Shield:
                skl1.sprite = Shield;
                break;
            case PickUp.Mace:
                skl1.sprite = Mace;
                break;
            case PickUp.Bomb:
                skl1.sprite = Bomb;
                break;
            case PickUp.Shoe:
                skl1.sprite = Shoe;
                break;
        }
        switch(pickups[1]) {
            case PickUp.None:
                skl2.sprite = None;
                break;
            case PickUp.Shield:
                skl2.sprite = Shield;
                break;
            case PickUp.Mace:
                skl2.sprite = Mace;
                break;
            case PickUp.Bomb:
                skl2.sprite = Bomb;
                break;
            case PickUp.Shoe:
                skl2.sprite = Shoe;
                break;
        }
    }
    public bool CanAdd() {
        return cnt != 2;
    }
    public void Add(PickUp pickup) {
        if (pickups[0] == PickUp.None)
            pickups[0] = pickup;
        else
            pickups[1] = pickup;
        cnt++;
    }
    public PickUp Use(int num) {
        PickUp ret;
        ret = pickups[num];
        if (num == 0) {
            pickups[0] = PickUp.None;
        } else {
            pickups[1] = PickUp.None;
        }
        if (ret != PickUp.None) cnt--;
        return ret;
    }

}
