using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour {

    // Use this for initialization
    private void Start()
    {
        var go = GameObject.Find("Scoreboard");
        var scr = go.GetComponent<GameScores>();

        var tm = GetComponent<TextMesh>();

        var message = scr.Message.Split(' ');
        string constructedText = string.Empty;
        for(int i = 0; i < message.Length; i++)
        {
            constructedText += message[i] + " ";
            if (i % 4 == 0 && i > 0)
                constructedText += "\r\n";
        }
        tm.text = string.Format("{0}%{1}{2}", scr.Score, "\r\n", constructedText);
    }
}
