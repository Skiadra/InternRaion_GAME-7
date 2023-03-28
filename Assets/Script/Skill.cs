using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SkillTree;
using static Movement;

public class Skill : MonoBehaviour
{
    public int id;
    public object temp;
    public TMP_Text namae;
    public int[] connectedSkill;

    public void UpdateUI()
    {
        namae.text = $"{skillTree.skillNames[id]}"; //Ngubah text value jdi nama skill
        GetComponent<Image>().color = skillTree.unlocked[id] == true ? Color.red: Color.gray; //kalo dah diunlock jadi warna merah, belom jd abu
    }

    public void GetSkill()
    {
        //Check jika ada skill lain yg udah aktif maka getSKill tidak dijalankan
        for (int i = 0; i < skillTree.skillList.Count; i++)
        {
            if (skillTree.skillNames[i] != skillTree.skillNames[id] && skillTree.unlocked[i] == true)
            {
                return;
            }
        }
        //Jika udah keunlock, maka akan dilock kembali dan skillpoin akan dikembalikan
        if (skillTree.unlocked[id] == true)
        {
            skillTree.skillPoint +=1;
            skillTree.unlocked[id] = false;
            Invoke(skillTree.releaseEffect[id], 0f);
            skillTree.UpdateSkillUI();
            return;
        }
        if (skillTree.skillPoint < 1) return; //Kalo skill poin gak cukup return
        
        skillTree.skillPoint -=1; //SKill poin kurang 1
        Invoke(skillTree.skillEffect[id], 0f); //Aktivasi skill effect
        skillTree.unlocked[id] = true; //Unlock skill
        
        //Menonaktifkan skill lain kecuali skill yang dipilih
        for (int i = 0; i < skillTree.skillList.Count; i++)
        {
            if (skillTree.skillNames[i] != skillTree.skillNames[id])
            {
                skillTree.unlocked[i] = false;
            }
        }
        skillTree.UpdateSkillUI();
    }

    //Menambah Tinggi Jump
    void hJump()
    {
        move.jumpForce = 30f;
    }
    //Mengembalikan tinggi jump seperti semula
    void hJumpLock()
    {
        move.jumpForce = 20f;
    }

    //Enable double jump
    void dJump()
    {
        move.doubleJump = true;
    }
    //Disable double jump
    void dJumpLock()
    {
        move.doubleJump = false;
    }
}
