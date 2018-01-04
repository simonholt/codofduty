using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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


    public string ThorServer;

    public string ProjectId;

    public List<string> ClassNames;

    public List<GameObject> ClassPrefabs;

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


        var fishyUrl = string.Format("http://{0}/Game/Fishes", ThorServer);
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
                            var newModelPoint = Instantiate(prefabs[fishes.fishes[i].className]);
                            newModelPoint.transform.position = objectPosition;
                            newModelPoint.transform.Rotate(0, (Random.value * 25) - 12.5f, 0);
                            newModelPoint.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 1.0f);

                            // TODO: Store the ROI for this object within the gameobject as extra data
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
                    var newModelPoint = Instantiate(prefabs[localfishes.fishes[i].className]);
                    newModelPoint.transform.position = objectPosition;
                    newModelPoint.transform.Rotate(0, (Random.value * 25) - 12.5f, 0);
                    newModelPoint.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 1.0f);

                    // TODO: Store the ROI for this object within the gameobject as extra data
                }
            }

        }
    }
}
