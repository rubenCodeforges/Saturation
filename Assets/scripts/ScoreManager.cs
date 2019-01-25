using System;
using System.Collections;
using Http;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text ScoreCounter;
    public GameObject ScoreBoard;
    public GameObject ScoreEntryPrefab;
    public GameObject PlayerNameInput;
    public GameObject RestartButton;
    public GameObject title;

    private float score = 0f;
    private BoardModel currentPlayer;
    private bool isTimerRunning = true;

    public void ShowScores()
    {
        title.SetActive(true);
        ScoreBoard.SetActive(true);
        StartCoroutine(LoadScore());
    }

    public void disableTimer()
    {
        isTimerRunning = false;
    }

    public void ShowPlayerInput()
    {
        PlayerNameInput.SetActive(true);
    }

    public void OnPlayerSubmit(Text PlayerName)
    {
        if (currentPlayer == null)
        {
            currentPlayer = new BoardModel();
            currentPlayer.userName = PlayerName.text;
            currentPlayer.score = score.ToString();
            currentPlayer.userMachineId = SystemInfo.deviceUniqueIdentifier;
        }
        else
        {
            currentPlayer.score = score.ToString();
        }

        PlayerNameInput.SetActive(false);
        RestartButton.SetActive(true);
        StartCoroutine(UpdateScore(currentPlayer.userName, currentPlayer.score));
        Debug.Log(currentPlayer.userName);
    }

    private void Start()
    {
        StartCoroutine(BoardService.LoadScores());
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            score += Time.deltaTime;
            ScoreCounter.text = "Time: " + (int) Math.Floor(score) + " sec";
        }
    }

    private IEnumerator LoadScore()
    {
        yield return StartCoroutine(BoardService.LoadScores());
        int position = 1;
        foreach (BoardModel boardModel in BoardService.GetScores())
        {
            GameObject score = Instantiate(ScoreEntryPrefab);
            score.transform.SetParent(ScoreBoard.transform);
            score.transform.Find("UserName").GetComponent<Text>().text = position + ". " + boardModel.userName;
            score.transform.Find("Time").GetComponent<Text>().text = boardModel.score;
            score.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            position++;
        }
    }

    private IEnumerator UpdateScore(string userName, string score)
    {
        yield return StartCoroutine(BoardService.SaveOrUpdateScores(userName, score));
        ShowScores();
    }
}