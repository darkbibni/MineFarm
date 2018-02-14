using System;
using UnityEngine;

[System.Serializable]
public class UserData {

    public string id;
    public string name;
    public int gold;
    public int diamond;
    public DateTime lastTimeMined;

    public static UserData CreateFromJSON(string jsonString)
    {
        //Example of jsonString : "{\"id\":\"User22\",\"name\":\"User22\",\"gold\":16,\"diamond\":3,\"lastTimeMined\":\"2018 - 02 - 12T13: 38:33.298149 + 01:00\"}";

        UserData user = JsonUtility.FromJson<UserData>(jsonString);

        return user;
    }
    
    public override string ToString()
    {
        return "["+id+"]" + " " + name + " " + gold + " golds - " + diamond + " diamonds.";
    }
    
}
