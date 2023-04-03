using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public bool canDoubleJump;
    public bool canDashReset;
    public bool canAbsorb;
    public float jumpForce;
    public float maxFallSpeed;
    public int maxWallJump;
    public int activeSceneIndex;
    public bool[] unlockedSkill;
    public int points;
    public float[] position;
    public int[] skillPointsEachLevel;

    public PlayerData (Movement mov, SkillTree st)
    {
        canDoubleJump = mov.doubleJump; //Save kemampuan double jump
        canDashReset = mov.canDashReset; //Save kemampuan reset dash
        jumpForce = mov.jumpForce; //Save kemampuan high jump
        canAbsorb = mov.absorb; //Save kemampuan absorb
        maxFallSpeed = mov.maxFallSpeed; //Save kemampuan slow drop
        maxWallJump = mov.maxWallJump; //Save banyak walljump
        activeSceneIndex = SceneManager.GetActiveScene().buildIndex; //Save last active scene

        //Save unlocked skill
        unlockedSkill = new bool[st.unlocked.Length];
        for (int i = 0; i < unlockedSkill.Length; i++)
        {
            unlockedSkill[i] = st.unlocked[i];
        }
        //Save skill point
        points = st.skillPoint;

        //Save player position
        position = new float[2];

        position[0] = mov.transform.position.x;
        position[1] = mov.transform.position.y;

        skillPointsEachLevel = st.addSkillPoints;
    }
}
