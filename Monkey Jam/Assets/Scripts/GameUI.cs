using MonkeyJam.Entities;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Player playerScript;
    [SerializeField] private Slider staminaSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(playerScript == null)
        {
            playerScript = Object.FindFirstObjectByType<Player>();
        }
        if(staminaSlider == null)
        {
            staminaSlider = GetComponentInChildren<Slider>();
        }

        staminaSlider.maxValue = playerScript._currentStamina;
    }

    // Update is called once per frame
    void Update()
    {
        staminaSlider.value = playerScript._currentStamina;
    }
}
