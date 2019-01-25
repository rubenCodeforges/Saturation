using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Http
{
    public static class BoardService
    {
        private static readonly string URL = "https://codeforges-sandbox.herokuapp.com/board/Saturation";
        private static readonly string GAME_NAME = "Saturation";

        private static BoardModel[] scores;

        public static IEnumerator LoadScores()
        {
            UnityWebRequest uwr = UnityWebRequest.Get(URL);
            yield return uwr.SendWebRequest();
            HandleResponse(uwr, () => scores = JsonHelper.FromJson<BoardModel>(uwr.downloadHandler.text));
        }

        public static IEnumerator SaveOrUpdateScores(string userName, string score)
        {
            var newScore = FindOrCreateScore(userName, score);
            var uwr = UnityWebRequest.Post(URL, JsonUtility.ToJson(newScore));

            yield return uwr.SendWebRequest();

            HandleResponse(uwr, () =>
            {
                List<BoardModel> scoreList = scores.ToList();

                for (int i = 0; i < scoreList.Count; i++)
                {
                    BoardModel response = JsonUtility.FromJson<BoardModel>(uwr.downloadHandler.text);
                    if (scoreList[i].userMachineId == newScore.userMachineId)
                    {
                        scoreList[i] = response;
                    }
                    else
                    {
                        scoreList.Add(response);
                    }
                }

                scores = scoreList.ToArray();
            });
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
            }
            else
            {
                callback();
                Debug.Log("Response received");
            }
        }

        private static BoardModel FindOrCreateScore(string userName, string score)
        {
            BoardModel foundScore = scores.ToList().Find(model => model.userMachineId == SystemInfo.deviceUniqueIdentifier);
            
            if (foundScore != null)
            {
                foundScore.score = score;
                foundScore.userName = userName;
                return foundScore;
            }
            
            BoardModel newScore = new BoardModel();
            newScore.gameName = GAME_NAME;
            newScore.userName = userName;
            newScore.userMachineId = SystemInfo.deviceUniqueIdentifier;
            newScore.score = score;
            return newScore;
        }
    }
}