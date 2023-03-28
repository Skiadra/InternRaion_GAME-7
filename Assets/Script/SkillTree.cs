using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public static SkillTree skillTree;
    private void Awake() => skillTree = this;
    public bool[] unlocked;
    public String[] skillNames;
    public String[] skillEffect;
    public String[] releaseEffect;

    public List<Skill> skillList;
    public GameObject skillHolder;
    public GameObject dashHolder;
    public int skillPoint;

    private void Start()
    {
        unlocked = new bool[2];
        skillNames = new[] {"Double Jump", "High Jump"};
        skillEffect = new[] {"dJump", "hJump"};
        releaseEffect = new[] {"dJumpLock", "hJumpLock"};

        foreach (var skill in skillHolder.GetComponentsInChildren<Skill>()) skillList.Add(skill); //Nambah list dengan skill yg ada

        for (int i = 0; i < skillList.Count; i++) skillList[i].id = i; //Ngisi id tiap skill
        
        UpdateSkillUI();
    }

    public void UpdateSkillUI()
    {
        foreach (var skill in skillList) skill.UpdateUI(); //Update UI tiap skill
    }

}
