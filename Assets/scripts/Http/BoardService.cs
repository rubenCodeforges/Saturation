using System;
using System.Collections;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Http
{
    public static class BoardService
    {
        private static readonly string URL = "https://codeforges-sandbox.herokuapp.com/board";
        private static readonly string GAME_NAME = "Saturation";

        private static BoardModel[] scores;

        public static IEnumerator LoadScores()
        {
            UnityWebRequest uwr = UnityWebRequest.Get(URL + "/" + GAME_NAME);
            yield return uwr.SendWebRequest();
            HandleResponse(uwr, () => scores = JsonHelper.FromJson<BoardModel>(uwr.downloadHandler.text));
        }

        public static IEnumerator SaveOrUpdateScores(string userName, string score)
        {
            var newScore = FindOrCreateScore(userName, score);
            Debug.Log("Sending: " + JsonUtility.ToJson(newScore));

            var request = sendPost(newScore);

            yield return request.SendWebRequest();

            HandleResponse(request, () => { });
        }


        public static BoardModel[] GetScores()
        {
            return scores;
        }

        private static void HandleResponse(UnityWebRequest uwr, Action callback)
        {
            if (uwr.isNetworkError)
            {
                Debug.Log("Error While Sending: " + uwr.error);
                throw new NetworkInformationException();
            }
            else
            {
                callback();
                Debug.Log("Response received");
            }
        }

        private static BoardModel FindOrCreateScore(string userName, string score)
        {
            BoardModel foundScore =
                scores.ToList().Find(model => model.userMachineId == SystemInfo.deviceUniqueIdentifier);

            if (foundScore != null)
            {
                foundScore.score = score;
                foundScore.userName = userName;
                return foundScore;
            }

            BoardModel newScore = new BoardModel();
            newScore.gameName = GAME_NAME;
            newScore.userName = userName;
            newScore.userMachineId = SystemInfo.deviceUniqueIdentifier == "n/a"
                ? generateIDCode()
                : SystemInfo.deviceUniqueIdentifier;
            newScore.score = score;
            return newScore;
        }

        private static UnityWebRequest sendPost<T>(T payload)
        {
            UnityWebRequest request = new UnityWebRequest(URL, "POST");
            UploadHandler uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(payload)));
            request.uploadHandler = uploadHandler;
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }

        private static string generateIDCode()
        {
            var desiredCodeLength = 15;
            var code = "";
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            while (code.Length < desiredCodeLength)
            {
                code += characters[Random.Range(0, characters.Length)];
            }

            code += DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            return code;
        }
    }
}