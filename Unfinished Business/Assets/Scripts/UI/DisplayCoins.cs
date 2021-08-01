using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayCoins : MonoBehaviour
{
    [Tooltip("Used to make the money amount look bigger on the screen.")]
    [SerializeField] float modifier = 100f;
    [Tooltip("The tag that the player has.")]
    [SerializeField] string playerTag = "Player";

    private TextMeshProUGUI text;
    private PlayerHealth playerHeathRef;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        playerHeathRef = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        UpdateText();
    }

    // Read the player's money amount and update the UI accordingly
    private void UpdateText ()
    {
        Debug.Log(text);
        text.text = "$" + (playerHeathRef.Money * modifier).ToString();
    }
}
