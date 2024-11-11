using UnityEngine;
using UnityEngine.Networking;
using TMPro; // Ensure you have this namespace for TextMeshPro
using System.Collections;

public class LoadPlayerData : MonoBehaviour
{
    // Assign these in the Inspector
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI inventoryText;

    private string url = "https://api.jsonbin.io/v3/b/6686a992e41b4d34e40d06fa";

    void Start()
    {
        StartCoroutine(GetJsonData());
    }

    private IEnumerator GetJsonData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                PlayerData data = JsonUtility.FromJson<PlayerData>(www.downloadHandler.text);
                
                // Display player data in UI
                playerNameText.text = "Player: " + data.record.playerName;
                levelText.text = "Level: " + data.record.level.ToString();
                healthText.text = "Health: " + data.record.health.ToString();
                positionText.text = $"Position: ({data.record.position.x}, {data.record.position.y}, {data.record.position.z})";

                // Display inventory items
                inventoryText.text = "Inventory:\n";
                foreach (var item in data.record.inventory)
                {
                    inventoryText.text += $"{item.itemName} - {item.quantity} x {item.weight}kg\n";
                }
            }
            else
            {
                Debug.LogError("Error fetching JSON data: " + www.error);
            }
        }
    }
}