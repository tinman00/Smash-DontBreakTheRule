using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Rule {
    None = 0,
    Turn = 1,
    Stop = 2,
    Jump = 3,
    StayLeft = 4,
    StayRight = 5,
    StandingNotOnGold = 6,
    PickItem = 7,
    NotEmote = 8,
    UseItem = 9,
    Have2Item = 10,
    NotHurtOpponent = 11
}

public class RuleDescription : MonoBehaviour
{
    public Text text;

    public Rule current = Rule.None;

    private string[] rules;

    private void Awake() {
        rules = new string[] {
        "",
        "Don't turn 5 times in 8s",
        "Don't stop for 3s",
        "Jump in every 4s",
        "Don't stay at the left",
        "Don't stay at the right",
        "Land only on gold",
        "Don't pick an item",
        "Laugh in every 5s",
        "Don't use items",
        "Don't have 2 items",
        "Don't hurt your opponent"
        };
    }
    public void SetRule(Rule rule) {
        current = rule;
    }

    void Update()
    {
        text.text = rules[(int)current];
    }
}
