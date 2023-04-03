using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    public int[] highScores = new int[10];

    string currentDirectory;
    public string scoreFileName = "highscores.txt";

    public bool enimyAlive;

    public CutManager cutManager;


    //-------------------------------------------------------------
    //-------------------------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        currentDirectory = Application.dataPath;
        Debug.Log("Our current directory is: " + currentDirectory);
        LoadScoresFromFile();

        enimyAlive = true;
    }

    //-------------------------------------------------------------
    //-------------------------------------------------------------



    // Update is called once per frame
    void Update()
    {
        
    }

    //-------------------------------------------------------------
    //-------------------------------------------------------------



    public void SaveScoresToFile()
    {
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\" + scoreFileName);

        for(int i = 0; i <highScores.Length; i++)
        {
            fileWriter.WriteLine(highScores[i]);
        }
        fileWriter.Close();
        Debug.Log("High scores written to " + scoreFileName);
    }
    //-------------------------------------------------------------
    public void AddScoreToHighScores(int newScore)
    {
        int desiredIndex = -1;

        for(int i = 0; i< highScores.Length; i++)
        {
            if (highScores[i] > newScore || highScores[i] == 0)
            {
                desiredIndex = i;
                break;
            }
        }

        if(desiredIndex < 0)
        {
            Debug.Log("Score of " + newScore + " not high enough for high scores list.", this);
            //enimyAlive = false; //changes bool to activate cutscene
            //make it disable a bool in movement script to stop the player from walking then play cutscene
            return;
        }

        for (int i = highScores.Length - 1; i > desiredIndex; i--)
        {
            highScores[i] = highScores[i - 1];
        }

        highScores[desiredIndex] = newScore;    
    }
    //-------------------------------------------------------------
    public void LoadScoresFromFile()
    {
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);

        if(fileExists)
        {
            Debug.Log("Found high score file " + scoreFileName);
        }
        else
        {
            Debug.Log("File does not exist. Scores will not be loaded");
            return;
        }

        highScores = new int[highScores.Length];

        StreamReader fileReader = new StreamReader(currentDirectory + "\\" + scoreFileName);

        int scoreCount = 0;

        while(fileReader.Peek() != 0 && scoreCount < highScores.Length)
        {
            string fileLine = fileReader.ReadLine();

            int readScore = -1;

            bool didParse = int.TryParse(fileLine, out readScore);

            if (didParse)
            {
                highScores[scoreCount] = readScore;
            }
            else
            {
                Debug.Log("Score is invalid");
                highScores[scoreCount] = 0;
            }

            scoreCount++;
        }

        fileReader.Close();
    }
}
