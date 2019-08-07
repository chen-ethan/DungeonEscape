using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem{
    public static void SavePlayer (PlayerController player, int i)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string file = "save" + i + ".save";
        string path =  Path.Combine(Application.persistentDataPath, "player.fun");
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
        
    }

    public static PlayerData LoadPlayer(int i)
    {
        string file = "save" + i+".save";
    
        string path = Path.Combine(Application.persistentDataPath, file);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file '"+file+"' not found in " + path);
            return null;
        }
    }
}
