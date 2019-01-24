using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Http
{
    public class BoardService
    {
        private static readonly string URL = "https://codeforges-sandbox.herokuapp.com/board/Saturation";
        
        public static IEnumerator GetRequest()
        {
            UnityWebRequest uwr = UnityWebRequest.Get(URL);
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                BoardModel[] scores = JsonHelper.FromJson<BoardModel>(uwr.downloadHandler.text);
                Debug.Log("Received: " + uwr.downloadHandler.text);
                Debug.Log("Received: " + scores[0].gameName);
            }
        }

        public static IEnumerable<BoardModel[]> getScores()
        {
            UnityWebRequest uwr = UnityWebRequest.Get(URL);
            uwr.SendWebRequest();
            yield return JsonHelper.FromJson<BoardModel>(uwr.downloadHandler.text);
        }
    }
}