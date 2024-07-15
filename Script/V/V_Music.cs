using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class V_Music : MonoBehaviour
{
    [SerializeField] M_Musiic m_Musiic;
    [SerializeField] AudioSource audio;
    [SerializeField] private ToggleGroup toggleGroup;  

    private ScrollRect ScrollRect;
    private GameObject Content;
    private GameObject Item;

    public EventSystem eventSystem;

    private TMP_Dropdown dropdown;


    private GameObject dropdownList;
    private GameObject blocker;
    bool handlingDropdownChange;

    private void Start()
    {
        audio = Camera.main.GetComponent<AudioSource>();

        // Cache EventSystem once
        eventSystem = EventSystem.current;

        dropdown = GetComponent<TMP_Dropdown>();

        InstantiateItem(m_Musiic.m_MusiicList.Count);
    }
    void OnToggleValueChanged(bool newValue)
    {
        Debug.Log("Toggle value changed to: " + newValue);
    }
    // Function to create item objects based on the song list
    private void InstantiateItem(int length)
    {
        if (m_Musiic == null || m_Musiic.m_MusiicList == null)
        {
            Debug.LogWarning("Music list not initialized.");
            return;
        }

        dropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (var musicItem in m_Musiic.m_MusiicList)
        {
            options.Add(new TMP_Dropdown.OptionData(musicItem.name));
        }

        dropdown.AddOptions(options);

        dropdown.onValueChanged.AddListener(OnDropdownChange);

        dropdown.value = 0;
    }

    private void OnDropdownChange(int value)
    {
        if (handlingDropdownChange)
        {
            return;
        }
        handlingDropdownChange = true;

   

        var selectedItem = m_Musiic.m_MusiicList[value];
        audio.clip = selectedItem;
        audio.Play();
        Debug.Log("Selected item: " + selectedItem.name);
        handlingDropdownChange = false;

    }
    bool isCoroutineRunning;


    private void Update()
    {
        if (!isCoroutineRunning &&  eventSystem.IsPointerOverGameObject() && eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject.name == "Dropdown")
        {
            StartCoroutine(FindList());
        }
    }

    private IEnumerator FindList()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(0.1f);
        dropdownList = GameObject.Find("Dropdown List");
        blocker = GameObject.Find("Blocker");

        if (dropdownList != null && blocker != null)
        {
            dropdownList.GetComponent<Canvas>().sortingOrder = 1;
            blocker.GetComponent<Canvas>().sortingOrder = 1;
        }
        isCoroutineRunning = false;
    }

}
