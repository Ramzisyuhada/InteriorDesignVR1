using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    [SerializeField] GameObject _canvas;
    public void Close()
    {
        if (GameObject.Find("Music Menu(Clone)") != null) Destroy(GameObject.Find("Music Menu(Clone)"));
    }
    public void ShowCanvas()
    {

        if (GameObject.Find("Music Menu(Clone)") == null)
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
            GameObject.Find("Music Menu(Clone)").transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, targetPosition.z), 1f);

            GameObject.Find("Music Menu(Clone)").transform.DORotate(cameraRotation.eulerAngles, 1f);

            GameObject.Find("Music Menu(Clone)").transform.DORestart();

            DOTween.Play(GameObject.Find("Music Menu(Clone)"));
        }
        if (GameObject.Find("MenuItem_part1_fix(Clone)") != null) Destroy(GameObject.Find("MenuItem_part1_fix(Clone)"));
        if (GameObject.Find("Canvas 1(Clone)") != null) Destroy(GameObject.Find("Canvas 1(Clone)"));
        if (GameObject.Find("Pertanyaan(Clone)") != null) Destroy(GameObject.Find("Pertanyaan(Clone)"));

    }

}
