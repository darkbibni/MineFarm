using UnityEngine;

[System.Serializable]
public class RewardData  {

    public int goldObtained;
    public int diamondObtained;
    public int amethystObtained;

    public static RewardData CreateFromJSON(string jsonString)
    {
        RewardData user = JsonUtility.FromJson<RewardData>(jsonString);

        return user;
    }
}
