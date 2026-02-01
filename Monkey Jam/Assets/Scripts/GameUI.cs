using System;
using MonkeyJam.Entities;
using MonkeyJam.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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

        EventManager.Instance.OnPlayerDied += OnPlayerDied;
        EventManager.Instance.OnPlayerStaminaUpdated += OnPlayerStaminaUpdated;
        EventManager.Instance.OnPlayerPosession += OnPlayerPosession;
    }

    // Update is called once per frame
    /*void Update()
    {
        staminaSlider.value = playerScript._currentStamina;
    }*/

    private void OnPlayerPosession(EnemyData data, int maxStamina)
    {
        staminaSlider.value = maxStamina;
        staminaSlider.maxValue = maxStamina;
    }

    private void OnPlayerStaminaUpdated(int currentStamina, int maxStamina)
    {
        staminaSlider.maxValue = maxStamina;
        //staminaSlider.value = (float)currentStamina / (float)maxStamina; // Was used to using filled images instead of sliders lmao
        staminaSlider.value = currentStamina;
    }

    private void OnPlayerDied()
    {
        //Oh shit he ded
    }

    private void OnDisable() //Cleaning up any event connections in case this shit is on every scene and gets destroyed during scene transition.
    //Basically just prevents memory leaks in these trying times.
    {
        EventManager.Instance.OnPlayerDied -= OnPlayerDied;
        EventManager.Instance.OnPlayerPosession -= OnPlayerPosession;
        EventManager.Instance.OnPlayerStaminaUpdated -= OnPlayerStaminaUpdated;
    }
}
