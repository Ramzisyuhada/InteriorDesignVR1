using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;


public class TutorialController : MonoBehaviour
{
    [SerializeField] List<Sprite> Gambar = new List<Sprite>();
    public static Image displayImage;
    [SerializeField] GameObject _canvas;
    private static int currentIndex = 0;
    void Start()
    {

    }
    public void Close()
    {
        if (GameObject.Find("How To(Clone)") != null)Destroy(GameObject.Find("How To(Clone)"));
    }
   /* private void _Findcanvas()
    {
        displayImage =  GameObject.Find("How To(Clone)").transform.GetChild(2).GetChild(1).GetComponent<Image>();
    }
    void Update()
    {
        if (Gambar.Count > 0)
        {
            if (GameObject.Find("How To(Clone)") != null)
            {
                _Findcanvas();
                displayImage.sprite = Gambar[currentIndex];
            }
        }
    }*/

    public void _showCanvas()
    {
        if(GameObject.Find("How To(Clone)") == null)
        {
            GameObject canvas = Instantiate(_canvas);
            Vector3 cameraPosition = Camera.main.transform.position;
            Quaternion cameraRotation = Camera.main.transform.rotation;
            Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 2.5f;
            canvas.transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, targetPosition.z), 1f);

            canvas.transform.DORotate(cameraRotation.eulerAngles, 1f);

            canvas.transform.DORestart();

            DOTween.Play(canvas);
        }
        else
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            Quaternion cameraRotation = Camera.main.transform.rotation;
            Vector3 targetPosition = cameraPosition + cameraRotation * Vector3.forward * 2.5f;
            GameObject.Find("How To(Clone)").transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, targetPosition.z), 1f);

            GameObject.Find("How To(Clone)").transform.DORotate(cameraRotation.eulerAngles, 1f);

            GameObject.Find("How To(Clone)").transform.DORestart();

            DOTween.Play(GameObject.Find("How To(Clone)"));
        }
        if (GameObject.Find("MenuItem_part1_fix(Clone)") != null) Destroy(GameObject.Find("MenuItem_part1_fix(Clone)"));
        if (GameObject.Find("Canvas 1(Clone)") != null) Destroy(GameObject.Find("Canvas 1(Clone)"));
        if(GameObject.Find("Pertanyaan(Clone)") != null) Destroy(GameObject.Find("Pertanyaan(Clone)"));
        if (GameObject.Find("Menu(Clone)") != null) Destroy(GameObject.Find("Menu(Clone)"));


    }
    /*public void _nextButton()
    {
        if (Gambar.Count == 0) return;

        currentIndex++;


        if (currentIndex >= Gambar.Count) { 
        
            currentIndex = 0;
        };
        displayImage.sprite = Gambar[currentIndex];

    }


    public void _prevButton()
    {
        if (Gambar.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = Gambar.Count - 1; 
        }

        displayImage.sprite = Gambar[currentIndex];
    }*/
}
