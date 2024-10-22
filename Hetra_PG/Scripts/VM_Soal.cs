using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class VM_Soal
{
    private List<M_Soal> currSoal = new List<M_Soal>();
    public List<M_Soal> CurrSoal { get => currSoal; set => currSoal = value; }    

    private int currIndex;
    public int CurrIndex { get => currIndex; set => currIndex = value; }

    private int banyakPilihanJawaban;
    public int BanyakPilihanJawaban { get => banyakPilihanJawaban; set => banyakPilihanJawaban = value; }   

    public VM_Soal(int _opsiJawaban)
    {
        // curr index
        currIndex = 0;

        // banyak pilihan jawaban
        banyakPilihanJawaban = _opsiJawaban;

        // ambil soal
        currSoal = ParsingSoalJawaban();
    }

    public List<M_Soal> ParsingSoalJawaban()
    {
        List<M_Soal> soalList = new List<M_Soal>();

        // 1. ambil soal dari bank soal
        string bankSoal = BankSoal.banksoaljawaban;

        // 2. parsing per soal
        string[] bagiPerSoal = bankSoal.Split('#');

        foreach (var soal in bagiPerSoal)
        {
            string[] bagiPerBintang = soal.Split('*');
            List<string> _pilihanJawabans = new List<string>();

            // 3. parsing per pilihan jawaban
            for (int i = 1; i < banyakPilihanJawaban + 1; i++)
                _pilihanJawabans.Add(bagiPerBintang[i]);

            // 4. bikin objek untuk 1 soal
            M_Soal _soal = new M_Soal()
            {
                soal = bagiPerBintang[0],
                kunci = int.Parse(bagiPerBintang[6]),
                pilihanJawabans = _pilihanJawabans,
                isEnd = false,
                idx = soalList.Count
            };

            // 5. lempar ke list
            soalList.Add(_soal);
        }

        // return
        return soalList;
    }

    public M_Soal PilihSoalAvailable()
    {
        // ambil soal yang tersedia (isEnd = false)
        currSoal = currSoal.Where(x => x.isEnd.Equals(false)).ToList();        
        currIndex = Random.Range(0, currSoal.Count);
        //
        return currSoal[currIndex];
    }

    public bool IsMenjawab(int _idPilih, int idKunci)
    {
        bool isbenar = false;

        // validasi jawaban benar-salah
        if (_idPilih == idKunci)
            isbenar = true;

        return isbenar;
    }

    public bool CekSoalSudahSemua()
    {
        return currSoal.Where(x => x.isEnd.Equals(false)).Count() == 0 ? false : true;
    }

    public void CekKetersediaanSoalDebugging()
    {
        // 
        Debug.Log("soal tersedia");
        foreach (var item in CurrSoal)
        {
            Debug.Log("soal: " + item.soal + ", tersedia: " + item.isEnd);
        }
        Debug.Log("-------");
    }

    
}