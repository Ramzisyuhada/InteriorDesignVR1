using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class V_Music : MonoBehaviour
{
    [SerializeField] M_Musiic m_Musiic;
    [SerializeField] AudioSource audio;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] Slider slider;
    [SerializeField] Sprite _icon;
    [SerializeField] Image Gambar;
    private ScrollRect ScrollRect;
    private GameObject Content;
    private GameObject Item;
    private static Sprite _icon1;
    public EventSystem eventSystem;

    private TMP_Dropdown dropdown;


    private GameObject dropdownList;
    private GameObject blocker;
    bool handlingDropdownChange;

   
    private void Start()
    {


        _icon1 = Gambar.sprite;
        audio = Camera.main.GetComponent<AudioSource>();
        if (audio.clip != null)
        {
            slider.value = audio.volume;
        }

            // Cache EventSystem once
            eventSystem = EventSystem.current;

        dropdown = GetComponent<TMP_Dropdown>();
        
        InstantiateItem(m_Musiic.m_MusiicList.Count);
        slider.onValueChanged.AddListener(delegate { Volume_music(); });

    }


    void DestroyCanvas()
    {
        
    }
    public void PlayMusic()
    {
        if(audio.isPlaying)
        {
            audio.Pause();
            Gambar.sprite = _icon1;

        }
        else
        {
            Gambar.sprite = _icon;
            audio.Play();

        }
    }
    private void Volume_music()
    {
        audio.volume = slider.value;
    }
    void OnToggleValueChanged(bool newValue)
    {
        Debug.Log("Toggle value changed to: " + newValue);
    }
    private void InstantiateItem(int length)
    {
        if (m_Musiic == null || m_Musiic.m_MusiicList == null)
        {
            Debug.LogWarning("Music tidak di inisialisasi.");
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
        if (!audio.isPlaying)
        {
            Gambar.sprite = _icon1;

        }

        slider.value = audio.volume;
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
