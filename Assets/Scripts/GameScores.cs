using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScores : MonoBehaviour {

    public float Score;

    public string Message;

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
    }

    public void Reset()
    {
        Score = 0.0f;
        Message = string.Empty;

    }

}