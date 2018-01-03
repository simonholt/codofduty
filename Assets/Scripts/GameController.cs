using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    class RoiContainer
    {
        public List<RoiInformation> fishes;
    }

    class RoiInformation
    {
        public string className;
        public string roi;
    }


    public string ThorServer;

    public string ProjectId;

    public List<GameObject> ClassPrefabs;

    // Use this for initialization
    void Start()
    {
        // Get list of classes / ROIs from livid
        var fishes = new RoiContainer
        {
            fishes = new List<RoiInformation>()
            {
                new RoiInformation { className = "Whiting", roi = "123" },
                new RoiInformation { className = "Whiting", roi = "124" },
                new RoiInformation { className = "Pollock", roi = "125" },
                new RoiInformation { className = "Pollock", roi = "125" },
                new RoiInformation { className = "Pollock", roi = "125" },
                new RoiInformation { className = "Pollock", roi = "125" },
                new RoiInformation { className = "Pollock", roi = "125" },
            }
        };

        for (int i = 0; i < fishes.fishes.Count; i++)
        {
            var circle = Random.insideUnitCircle * 2;
            var objectPosition = new Vector3(circle.x, 0, circle.y + 2.5f);
            var newModelPoint = Instantiate(ClassPrefabs[0]);
            newModelPoint.transform.position = objectPosition;
            newModelPoint.transform.Rotate(0, (Random.value * 25) - 12.5f, 0);
            newModelPoint.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 1.0f);
        }
    }
}
