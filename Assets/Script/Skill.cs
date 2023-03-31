using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SkillTree;
using static Movement;
using System.Data;

public class Skill : MonoBehaviour
{
    public int id;
    public object temp;
    public TMP_Text namae;
    public int[] connectedSkill;
    public GameObject connector;

    public void UpdateUI()
    {
        namae.text = $"{skillTree.skillNames[id]}"; //Ngubah text value jdi nama skill
        GetComponent<Image>().color = skillTree.unlocked[id] == true ? Color.red: Color.gray; //kalo dah diunlock jadi warna merah, belom jd abu
        connector.GetComponent<Image>().color = skillTree.unlocked[id] == true ? Color.red: Color.gray; 
    }

    public void GetSkill()
    {
        //Check jika ada skill lain yg udah aktif maka getSKill tidak dijalankan
        if (id != skillTree.unlocked.Length-1)
        {
            if (skillTree.unlocked[id+1] == true && id%2 == 0)
            {
                return;
            }
        }

        if (id != 0)
        {
            if (skillTree.unlocked[id-1] == true && id%2 == 1)
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

        for (int i = 0; i < connectedSkill.Length; i++)
        {
            if (!skillTree.unlocked[connectedSkill[i]]) return;
        }

        skillTree.skillPoint -=1; //SKill poin kurang 1
        Invoke(skillTree.skillEffect[id], 0f); //Aktivasi skill effect
        skillTree.unlocked[id] = true; //Unlock skill
        
        skillTree.UpdateSkillUI();
    }

    //Menambah Tinggi Jump
    void hSplash()
    {
        move.jumpForce = 30f;
    }
    //Mengembalikan tinggi jump seperti semula
    void hSplashLock()
    {
        move.jumpForce = 20f;
    }

    //Enable double jump
    void dSplash()
    {
        move.doubleJump = true;
    }
    //Disable double jump
    void dSplashLock()
    {
        move.doubleJump = false;
    }

    void sDash()
    {
        move.canDashReset = true;
    }
    void sDashLock()
    {
        move.canDashReset = false;
    }

    void sDrop()
    {
        move.maxFallSpeed = -5f;
    }
    void sDropLock()
    {
        move.maxFallSpeed = -25f;
    }

    void absorb()
    {
        move.absorb = true;
    }

    void absorbLock()
    {
        move.absorb = false;
    }
    void slider()
    {
        move.maxWallJump = 2;
    }
    void sliderLock()
    {
        move.maxWallJump = 1;
    }
}
