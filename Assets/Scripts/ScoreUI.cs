using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceText;

    void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            float d = GameManager.Instance.currentDistance;
            distanceText.text = Mathf.FloorToInt(d).ToString() + "m";
        }
    }
}
