using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    class RoiContainer
    {
        public List<RoiInformation> fishes;
    }

    [System.Serializable]
    class RoiInformation
    {
        public string className;
        public string roi;
    }

    [System.Serializable]
    class GameInfoContainer
    {
        public string gameId;
    }

    [System.Serializable]
    class GameScoreContainer
    {
        public GameScore score;
    }

    [System.Serializable]
    public class GameScore
    {
        public float score;
        public string message;
    }



    public string ThorServer;

    public string ProjectId;

    public List<string> ClassNames;

    public List<GameObject> ClassPrefabs;

    public string GameId;

    // Use this for initialization
    void Start()
    {
#if true
        StartCoroutine(GetSomeData());
#else

        
#endif
    }

    IEnumerator GetSomeData()
    {
        var fishyUrl = string.Format("http://{0}/Game/NewGame", ThorServer);
        using (UnityWebRequest request = UnityWebRequest.Get(fishyUrl))
        {
            yield return request.SendWebRequest();

            if (request.isDone && !request.isNetworkError && !request.isHttpError)
            {
                var json = request.downloadHandler.text;
                var gameInfo = JsonUtility.FromJson<GameInfoContainer>(json);

                GameId = gameInfo.gameId;
            }
        }


        var si = GetComponent<SpeechInputSource>();
        //var words = new List<KeywordAndKeyCode>(si.Keywords);

        // Dictionary to map Thor's class names to our prefabs.

        var prefabs = new Dictionary<string, GameObject>();
        for (int i = 0; i < ClassNames.Count; i++)
        {
            prefabs.Add(ClassNames[i], ClassPrefabs[i]);
            //words.Add(new KeywordAndKeyCode() { Keyword = ClassNames[i] });
        }

        //si.Keywords = words.ToArray();


        fishyUrl = string.Format("http://{0}/Game/Fishes", ThorServer);
        using (UnityWebRequest request = UnityWebRequest.Get(fishyUrl))
        {
            yield return request.SendWebRequest();

            if (request.isDone && !request.isNetworkError && !request.isHttpError)
            {
                var json = request.downloadHandler.text;
                var fishes = JsonUtility.FromJson<RoiContainer>(json);

                Debug.Log("Got " + fishes.fishes.Count + " fishes");

                if (fishes.fishes.Count > 0)
                {
                    for (int i = 0; i < fishes.fishes.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(fishes.fishes[i].className))
                        {
                            var circle = Random.insideUnitCircle * 2;
                            var objectPosition = new Vector3(circle.x, 0, circle.y + 2.5f);
                            var newFishy = Instantiate(prefabs[fishes.fishes[i].className]);
                            newFishy.transform.position = objectPosition;
                            newFishy.transform.Rotate(0, (Random.value * 25) - 12.5f, 0);
                            newFishy.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 1.0f);

                            // TODO: Store the ROI for this object within the gameobject as extra data
                            var newRoi = newFishy.AddComponent<Roi>();
                            newRoi.RoiId = fishes.fishes[i].roi;
                        }
                    }

                    yield break;
                }
            }

            Debug.Log("Got nowt off liveID, will generate some random stuffs");

            // Fallback in case of error
            var localfishes = new RoiContainer
            {
                fishes = new List<RoiInformation>()
                    {
                        new RoiInformation { className = "pollock", roi = "123" },
                        new RoiInformation { className = "pollock", roi = "124" },
                        new RoiInformation { className = "whiting", roi = "125" },
                        new RoiInformation { className = "whiting", roi = "125" },
                        new RoiInformation { className = "whiting", roi = "125" },
                        new RoiInformation { className = "whiting", roi = "125" },
                        new RoiInformation { className = "whiting", roi = "125" },
                    }
            };

            for (int i = 0; i < localfishes.fishes.Count; i++)
            {
                if (!string.IsNullOrEmpty(localfishes.fishes[i].className))
                {
                    var circle = Random.insideUnitCircle * 2;
                    var objectPosition = new Vector3(circle.x, 0, circle.y + 2.5f);
                    var newFishy = Instantiate(prefabs[localfishes.fishes[i].className]);
                    newFishy.transform.position = objectPosition;
                    newFishy.transform.Rotate(0, (Random.value * 25) - 12.5f, 0);
                    newFishy.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 1.0f);

                    var newRoi = newFishy.AddComponent<Roi>();
                    newRoi.RoiId = localfishes.fishes[i].roi;
                }
            }

        }
    }

    public IEnumerator GameFinished()
    {
        var fishyUrl = string.Format("http://{0}/Game/GetScore?gameId={1}", ThorServer, GameId);
        using (UnityWebRequest request = UnityWebRequest.Get(fishyUrl))
        {
            yield return request.SendWebRequest();

            if (request.isDone && !request.isNetworkError && !request.isHttpError)
            {
                var json = request.downloadHandler.text;
                var scoreInfo = JsonUtility.FromJson<GameScoreContainer>(json);

                var go = GameObject.Find("Scoreboard");
                var scr = go.GetComponent<GameScores>();

                scr.Score = scoreInfo.score.score;
                scr.Message = scoreInfo.score.message;

                SceneManager.LoadScene(2);


            }
        }
    }
}
