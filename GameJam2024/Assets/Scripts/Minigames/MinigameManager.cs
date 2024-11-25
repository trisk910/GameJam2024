using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public enum MinigameType
    {
        SimonSays       
    }

    [Header("Minigame Settings")]
    public MinigameType selectedMinigame = MinigameType.SimonSays;
    public List<GameObject> buttons;
    public Material highlightMaterial; 
    public Material defaultMaterial; 
    public int initialSequenceLength = 3;
    public float sequenceDisplayTime = 1f;
    public int maxRounds = 4;

    private List<int> sequence = new List<int>();
    private int currentRound = 0;
    private int playerProgress = 0;
    private bool isGameActive = false;
    private bool isGameOver = false;

    void Start()
    {
        ResetGame();
    }

    private void ResetGame()
    {
        sequence.Clear();
        currentRound = 0;
        playerProgress = 0;
        isGameActive = false;
        isGameOver = false;
    }

    public void ButtonPressed(int buttonIndex)
    {
        if (isGameOver)
            return;

        if (!isGameActive)
        {
            StartGame();
            return;
        }

        if (buttonIndex == sequence[playerProgress])
        {
            playerProgress++;

            if (playerProgress == sequence.Count)
            {
                currentRound++;
                playerProgress = 0;

                if (currentRound >= maxRounds)
                {
                    GameWon();
                }
                else
                {
                    StartCoroutine(DisplaySequence());
                }
            }
        }
        else
        {
            GameOver();
        }
    }

    private void StartGame()
    {
        if (isGameOver) return;

        isGameActive = true;
        sequence.Clear();
        currentRound = 0;
        playerProgress = 0;
        StartCoroutine(DisplaySequence());
    }

    private IEnumerator DisplaySequence()
    {
        yield return new WaitForSeconds(1f); 
        for (int i = 0; i < currentRound + initialSequenceLength; i++)
        {
            if (i >= sequence.Count)
            {
                sequence.Add(Random.Range(0, buttons.Count));
            }

            int buttonIndex = sequence[i];
            HighlightButton(buttonIndex);
            yield return new WaitForSeconds(sequenceDisplayTime);
            ResetButtonMaterial(buttonIndex);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void HighlightButton(int buttonIndex)
    {
        buttons[buttonIndex].GetComponent<Renderer>().material = highlightMaterial;
    }

    private void ResetButtonMaterial(int buttonIndex)
    {
        buttons[buttonIndex].GetComponent<Renderer>().material = defaultMaterial;
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        isGameActive = false;
        isGameOver = true;
    }

    private void GameWon()
    {
        Debug.Log("Completed!");
        isGameActive = false;
        isGameOver = true;
    }
}
