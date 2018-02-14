using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType {

    GOLD,
    DIAMOND,
    AMEHTYST
}

public class ResourceUtility
{
    public static string GetName(ResourceType resource)
    {
        switch(resource)
        {
            case ResourceType.GOLD: return "Gold";
            case ResourceType.DIAMOND: return "Diamond";
            case ResourceType.AMEHTYST: return "Amethyst";
            default: return "";
        }
    }
}
