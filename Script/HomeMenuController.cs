using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HomeMenuController : MonoBehaviour
{

    [SerializeField] Transform Player;
    [SerializeField] Transform House;
    [SerializeField] GameObject _canvas;
    public void PlayGame()
    {
        House.gameObject.SetActive(true);
        Player.GetComponent<TeleportationProvider>().enabled = true;
        Player.GetComponent<ContinuousMoveProviderBase>().enabled = true;
        _canvas.SetActive(false);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    private void Awake()
    {
        House.gameObject.SetActive(false);
        Player.GetComponent<TeleportationProvider>().enabled = false;
        Player.GetComponent<ContinuousMoveProviderBase>().enabled = false;
    }
}
