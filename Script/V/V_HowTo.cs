using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class V_HowTo : MonoBehaviour
{

    //Data Gambar dan index ;
    [SerializeField] M_HowTO data;


    // UI Gambar ;
    [SerializeField] Image image;


    private static VM_HowTo howto;

    private static int index =  0 ;

    void Start()
    {

        howto = new VM_HowTo(data);

        if (data.list.Count > 0)
        {
            image.sprite = data.list[index].sprite;
        }
    }

    /*void Update()
    {

    }*/


    public void Close()
    {

        Destroy(transform.parent.gameObject);
    }
    public void _next()
    {
        index ++;
        var values = howto.Next(index);
        image.sprite = values.Value.Item1;
        index = values.Value.Item2;

    }


    public void _prev()
    {
        index --;
        var values = howto.Prev(index);
        image.sprite = values.Value.Item1;
        index = values.Value.Item2;
    }
}
