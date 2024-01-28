using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleManager : MonoBehaviour
{
    public float rulePeriod = 40f;
    public float newRuleTime = 20f;

    public static RuleManager instance;
    public RuleDescription description;
    public RuleDescription annouce;
    public Text countText;
    public Animator breakAnnounce;
    private Animator anim;
    private Rule tmp;
    private int count = 0;
    public bool[] HasRule;

    private void Awake() {
        if (instance == null) instance = this;
    }
    void Start()
    {
        newRuleTime += Time.time;
        HasRule = new bool[20];
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > newRuleTime) {
            List<Rule> seq = new();
            for (int i = 1; i <= 11; i++) {
                if (!HasRule[i]) {
                    seq.Add((Rule)i);
                }
            }
            if (seq.Count != 0) {
                var newRule = seq[Random.Range(0, seq.Count)];
                AddRule(newRule);
                newRuleTime += rulePeriod;
            }
        }
    }

    public void AddRule(Rule r) {
        tmp = r;
        anim.SetTrigger("New");
        if (description.current != Rule.None)
            count++;
        HasRule[(int)r] = true;
    }

    public void UpdateRule() {
        countText.text = count.ToString();
        description.SetRule(tmp);
    }

    public void ClearRule() {
        count = 0;
        HasRule = new bool[20];
        description.SetRule(Rule.None);
    }

    public void BreakRule(Rule r) {
        ClearRule();
        annouce.SetRule(r);
        breakAnnounce.SetTrigger("Show");
    }
}
