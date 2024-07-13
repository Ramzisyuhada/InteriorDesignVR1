using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VM_HowTo 
{
    public M_HowTO m_HowTOs ;

    public VM_HowTo(M_HowTO howTOs)
    {
        m_HowTOs = howTOs;
    }


    

    public (Sprite,int)? Next(int index)
    {

        if(m_HowTOs.list.Count == 0) return null;

        Debug.Log(index);
        if(index >= m_HowTOs.list.Count)
        {
            index = 0 ;
        }

        return (m_HowTOs.list[index].sprite,index);
    }

    public (Sprite, int)? Prev(int index)
    {

        if (m_HowTOs.list.Count == 0) return null;
        Debug.Log(index);
    

        if (index < 0)
        {
            index = m_HowTOs.list.Count - 1;
        }

        return (m_HowTOs.list[index].sprite, index);
    }

}
