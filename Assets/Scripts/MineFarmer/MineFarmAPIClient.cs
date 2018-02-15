using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Http;
using UnityEngine;
using UnityEngine.Networking;

public struct UserSession
{
    public string userId;
    public string token;
}

/// <summary>
/// Manage api request.
/// </summary>
public class MineFarmAPIClient : MonoBehaviour {
    
    public MineFarmGameManager gameMgr;

    [Header("Web API configuration")]
    [Tooltip("Url of the web api server (needs the port)")]
    public string url = "http://10.51.0.52:8022"; // Use localhost else.

    private UserSession currentSession;
    
    /// <summary>
    /// Return true if the session is correct.
    /// </summary>
    public bool SessionCorrect {
        get {
            return currentSession.token != null && currentSession.userId != null;
        }
    }

    public void ResetSession()
    {
        currentSession.userId = null;
        currentSession.token = null;
    }

    #region API Request methods

    /// <summary>
    /// Try to register to the web api.
    /// </summary>
    /// <param name="nickname"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public IEnumerator TryToRegister(string nickname, string password)
    {
        string passwordJson = "\"" + password + "\"";
        UnityWebRequest www = UnityWebRequest.Put(url +"/api/User/" + nickname, passwordJson);

        Request request = new Request("PUT", url + "/api/User/" + nickname);
        request.Text = passwordJson;

        AddRequestHeaders(www);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            gameMgr.uiMgr.MainMenuFeedback("No network connection :(");
        }

        else if(www.isHttpError)
        {
            gameMgr.uiMgr.MainMenuFeedback(ManagerErrorCode(www.responseCode));
        }

        else
        {
            //Debug.Log("Status code : "+ www.responseCode +"\n"+ www.downloadHandler.text);

            // Feedback registration succeed !
            gameMgr.uiMgr.MainMenuFeedback("You are now registered !");
        }
    }

    /// <summary>
    /// Try to connect to the web api.
    /// </summary>
    /// <param name="nickname"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task TryToConnect(string nickname, string password)
    {
        string passwordJson = "\"" + password + "\"";

        // UnityWebRequest encode the body so ... we use that custom Http Request script.
        Request www = new Request("POST", url + "/api/User/" + nickname)
        {
            Text = passwordJson
        };
        AddRequestHeaders(www);

        // www.Send can throw exception and stop the calling method without try/catch.
        try
        {
            await www.Send();
        }

        catch(Exception)
        {
            gameMgr.uiMgr.MainMenuFeedback("Can't connect...");
            return;
        }

        var response = www.response;

        if (response != null)
        {
            response.EnsureSuccessStatusCode();

            string messageReceived = Encoding.UTF8.GetString(response.bytes);

            //Debug.Log("Token : " + messageReceived);
            
            // Store informations about current user.
            currentSession = new UserSession()
            {
                userId = nickname,
                token = messageReceived
            };

            StartCoroutine(RetrievePlayerData());
            StartCoroutine(RetrieveServerData());

            gameMgr.DisplayGamePanel();
        }
    }

    /// <summary>
    /// Try to buy a rock.
    /// </summary>
    /// <returns></returns>
    public IEnumerator BuyRock()
    {
        UnityWebRequest www = UnityWebRequest.Put(url + "/api/Gameplay/Shop/Rock/" + currentSession.userId, "Try to buy a rock");
        AddRequestHeaders(www);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            gameMgr.uiMgr.ShopFeedback("Network error D:");
        }

        else if(www.isHttpError)
        {
            gameMgr.uiMgr.ShopFeedback("You don't have enough golds O:");
        }

        else
        {
            gameMgr.UnlockRock();

            // Refresh inventory.
            StartCoroutine(RetrievePlayerData());
            
            gameMgr.uiMgr.ShopFeedback("You have unlocked a new rock :)");
        }
    }

    /// <summary>
    /// Retrieve the inventory of the player when he connects to the game.
    /// </summary>
    /// <returns></returns>
    public IEnumerator RetrievePlayerData()
    {
        UnityWebRequest www = UnityWebRequest.Get(url + "/api/Gameplay/" + currentSession.userId);
        AddRequestHeaders(www);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }

        else
        {
            string json = www.downloadHandler.text;
            
            // Create user info from json.
            gameMgr.UserData = UserData.CreateFromJSON(json);

            Debug.Log("Status code : " + www.responseCode + "\n" + json);
        }
    }

    /// <summary>
    /// Retrieve the informations about cooldown and other if neededs...
    /// </summary>
    /// <returns></returns>
    public IEnumerator RetrieveServerData()
    {
        UnityWebRequest www = UnityWebRequest.Get(url + "/api/Gameplay/Cooldowns");
        AddRequestHeaders(www);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }

        else
        {
            string response = www.downloadHandler.text;
            string[] strings = response.Split('/');

            float[] cds = new float[strings.Length];
            for (int i = 0; i < cds.Length; i++)
            {
                cds[i] = float.Parse(strings[i]);
            }

            gameMgr.cooldowns = cds;
        }
    }


    /// <summary>
    /// Try to mine. Can return the mine id or the time to mine.
    /// </summary>
    /// <returns></returns>
    public IEnumerator TryToMine(int rockIndex)
    {
        if(!gameMgr.CanMineRock(rockIndex))
        {
            gameMgr.uiMgr.rocks[rockIndex].FeedbackCantMine();
            yield break;
        }

        // USE http://localhost.fiddler:80/api/Gameplay/Farm/ to observe the request in details. Remove to reduce latency !!!
        UnityWebRequest www = UnityWebRequest.Put(url+"/api/Gameplay/Farm/" + currentSession.userId +"/" + rockIndex, "Try to mine");
        
        AddRequestHeaders(www);
        // Add token to request header.
        www.SetRequestHeader("x-token", currentSession.token);

        yield return www.SendWebRequest();
        
        Debug.Log(www.url);

        // Handle error or button pressed to fast !
        if (www.isNetworkError || www.isHttpError)
        {
            if(www.downloadHandler != null)
            {
                Debug.Log("Status code : " + www.responseCode + "\n" + www.downloadHandler.text);
            }

            else
            {
                Debug.LogWarning(www.error);
            }
        }

        // Handle mine farm
        else
        {
            Debug.Log("Status code : " + www.responseCode + "\n" + www.downloadHandler.text);

            RewardData reward = RewardData.CreateFromJSON(www.downloadHandler.text);
            
            gameMgr.Mine(rockIndex, reward);
        }
    }

    public IEnumerator GetLeaderboard(ResourceType resource)
    {
        UnityWebRequest www = UnityWebRequest.Get(url + "/api/Gameplay/Top/" + ResourceUtility.GetName(resource));
        AddRequestHeaders(www);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }

        else
        {
            string json = www.downloadHandler.text;

            // Create user info from json.
            UserData[] top = JsonHelper.getJsonArray<UserData>(json);

            Debug.Log("Status code : " + www.responseCode + "\n" + json);
            
            gameMgr.uiMgr.UpdateLeaderboard(resource, top);
        }
    }

    #endregion

    private string ManagerErrorCode(long statusCode)
    {
        string response = "OULAH";

        switch(statusCode)
        {
            case 403: response = "WHAT ARE YOU DOING ? >:("; break;
            case 404: response = "I don't found what you search :("; break;
            case 502: response = "Server OFF :("; break;
        }

        return response;
    }

    /// <summary>
    /// Add some request headers.
    /// </summary>
    /// <param name="www"></param>
    private void AddRequestHeaders(UnityWebRequest www)
    {
        www.SetRequestHeader("User-Agent", "Unity");
        www.SetRequestHeader("Content-type", "application/json");

        if (currentSession.token != null && !currentSession.token.Equals(""))
        {
            www.SetRequestHeader("x-token", currentSession.token);
        }
    }

    /// <summary>
    /// Add some request headers.
    /// </summary>
    /// <param name="www"></param>
    private void AddRequestHeaders(Request www)
    {
        www.AddHeader("User-Agent", "Unity");
        www.AddHeader("Content-type", "application/json");

        if (currentSession.token != null && !currentSession.token.Equals(""))
        {
            www.AddHeader("x-token", currentSession.token);
        }
    }
}
