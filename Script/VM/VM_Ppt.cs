using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VM_Ppt 
{

    private M_PPT M_PPT;
   public VM_Ppt(M_PPT pPT)
    {
        M_PPT = pPT;
    }

    public (Material, int)? Next(int index)
    {

        if (M_PPT.Material.Count == 0) return null;

        Debug.Log(index);
        if (index >= M_PPT.Material.Count)
        {
            index = 0;
        }

        return (M_PPT.Material[index].materials, index);
    }

    public (Material, int)? Prev(int index)
    {

        if (M_PPT.Material.Count == 0) return null;
        Debug.Log(index);


        if (index < 0)
        {
            index = M_PPT.Material.Count - 1;
        }

        return (M_PPT.Material[index].materials, index);
    }
}
