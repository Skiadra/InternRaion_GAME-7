using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(Movement mov, SkillTree st)
    {
        BinaryFormatter formatter = new BinaryFormatter(); //Instansiasi binaryformatter
        string path = Application.persistentDataPath + "/playerMovement.rai"; //path data yang akan disimpan
        FileStream fs = new FileStream(path, FileMode.Create); //Instansiasi filestream untuk membuat file pada path
        PlayerData data = new PlayerData(mov, st); //Instansiasi player data

        formatter.Serialize(fs, data); //mengubah data menjadi binari
        fs.Close();
        Debug.Log("Saved");
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/playerMovement.rai"; //Path tempat data disimpan
        if (File.Exists(path)) // Kalau file exsist di path
        {
            BinaryFormatter formatter = new BinaryFormatter(); //Instansiasi binaryformatter
            FileStream fs = new FileStream(path, FileMode.Open); //Instansiasi filestream untuk membuat file pada path

            PlayerData data = formatter.Deserialize(fs) as PlayerData; //Mengubah data yang ada dari binari ke PlayerData
            fs.Close();
            return data; //Return player data
        } else
        {
            Debug.LogError("Save file not found");
            return null;
        }
    }
}
