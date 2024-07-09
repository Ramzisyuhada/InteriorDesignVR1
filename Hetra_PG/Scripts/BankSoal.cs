using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankSoal
{
    // format penulisannya -(string)--> 
    // (soal)*(pilihanjawaban1)*(pilihanjawaban2)*(pilihanjawaban3)*.....*(pilihanjawaban-n)*(index jawaban benar)
    // pisahkan tiap soal dengan #
    // soal terakhir, gk usah menggunakan tanda # di akhirnya

    // contoh bank soal dengan 5 pilihan jawaban
    public static string banksoaljawaban =
        "Faktor yang perlu diperhatikan dalam memilih dan menentukan lokasi gedung kantor adalah*Warna cat bangunan*Jenis atap*Kepemilikan sebelumnya*Akses transportasi*Model jendela*3#" +
        "Mengapa faktor keamanan penting dalam memilih lokasi kantor?*Meningkatkan estetika*Menarik lebih banyak pelanggan*Melindungi aset dan personil*Mengurangi biaya transportasi*Memudahkan dekorasi*2#" +
        "Bagaimana pengaruh lingkungan kerja terhadap aktivitas kantor?*Mengurangi kecemasan kerja*Mempengaruhi produktivitas*Meningkatkan biaya*Menurunkan kecepatan kerja*Membuat kantor lebih indah*1#" +
        "Apa manfaat lokasi kantor dekat dengan pasar tenaga kerja?*Harga sewa bangunan menjadi lebih murah*Tenaga kerja tidak menuntut gaji tinggi*Mudah dikunjungi oleh masyarakat*Biaya transportasi lebih tinggi*Mendapat banyak pegawai berkualitas*1#" +
        "Bagaimana cara menjamin fleksibilitas dalam susunan gedung kantor?*Memberikan ruang untuk perluasan*Meningkatkan keamanan*Meminimalkan penggunaan mebel*Menggunakan lebih banyak kaca*Memilih lokasi di pusat kota*0#" +
        "Faktor apa yang penting dalam pengorganisasian ruang kantor*Pilihan warna cat*Tipe lantai*Aliran kerja yang efektif*Lokasi restoran terdekat*Akses ke pusat kebugaran*2#" +
        "Apa yang dihasilkan dari tata ruang kantor yang baik?*Meningkatkan produktivitas*Menurunkan biaya*Meningkatkan kebisingan*Membatasi interaksi*Mengurangi cahaya alami*0#" +
        "Bagaimana asas jarak terpendek mempengaruhi tata ruang kantor? *Meningkatkan privasi*Mempercepat penyelesaian pekerjaan*Mengurangi jumlah mebel*Mengurangi biaya pemanasan*Meningkatkan keamanan*1#" +
        "Apa tujuan dari menyediakan ruang yang cukup antara meja kerja? *Meningkatkan kebisingan*Mengurangi biaya*Meningkatkan kenyamanan dan efisiensi*Menarik lebih banyak karyawan*Memperkecil ukuran kantor*2#" +
        "Mengapa penting mempertimbangkan penerangan yang memadai dalam tata ruang kantor? *Menjaga kesehatan mata*Mengurangi biaya pemanasan*Meningkatkan keamanan*Menyediakan lingkungan yang lebih estetis*Mempengaruhi harga sewa*0#" +
        "Bagaimana warna mempengaruhi kenyamanan dan produktivitas di kantor?*Warna tidak berpengaruh*Hanya warna terang yang efektif*Warna dapat mempengaruhi mood dan fokus*Semua warna memiliki efek yang sama*Warna hanya untuk estetika*2#" +
        "Apa keuntungan dari kantor dengan ruang terbuka?*Memudahkan komunikasi dan kolaborasi*Mengurangi biaya cahaya*Meningkatkan privasi*Mempermudah alokasi ruang*Membatasi interaksi sosial*0#" +
        "Apa tujuan dari penggunaan peralatan dan perabot ergonomis?*Menyediakan ruang lebih*Meningkatkan kenyamanan dan mencegah cedera*Menyederhanakan desain*Mengurangi biaya*Meningkatkan kecepatan kerja*1#" +
        "Mengapa penting memiliki pintu akses terpisah untuk staf dan pengunjung?*Mengurangi biaya*Memperbaiki aliran dan keamanan*Mengurangi kebisingan*Memperbaiki estetika*Menyediakan lebih banyak ruang*1#" +
        "Bagaimana fleksibilitas tata ruang mempengaruhi kantor?*Meningkatkan biaya*Membatasi jumlah staf*Memudahkan adaptasi terhadap perubahan*Menurunkan produktivitas*Mengurangi kebutuhan ruang*2#" +
        "Mengapa pencahayaan yang tepat penting untuk efisiensi kantor?*Mencegah kelelahan mata dan meningkatkan konsentrasi*Menghemat energi*Memperbaiki suasana hati*Mengurangi risiko kecelakaan*Memperindah ruangan*0#" +
        "Apa konsekuensi dari tidak memperhatikan keamanan dalam manajemen gedung kantor?*Risiko kehilangan aset dan data*Kenaikan harga sewa*Pengurangan ruang kantor*Meningkatnya biaya transportasi*Kebutuhan akan lebih banyak dekorasi*0#" +
        "Apa fungsi dari ruang kerja yang dapat diubah-ubah?*Memperbaiki tampilan estetika*Menyediakan adaptabilitas untuk berbagai kebutuhan*Mengurangi jumlah mebel*Mengurangi kebisingan*Meningkatkan privasi*1#" +
        "Bagaimana aspek keamanan mempengaruhi pilihan lokasi kantor?*Tidak berpengaruh*Lokasi yang aman mengurangi biaya keamanan tambahan*Mempengaruhi desain interior*Menyediakan lebih banyak ruang parkir*Mempermudah pengaturan perabot*1#" +
        "Mengapa perlu mempertimbangkan jarak dengan unit organisasi lain?*Untuk keindahan kantor*Untuk mengurangi kebisingan*Untuk memperlancar komunikasi*Untuk mengurangi biaya sewa*Untuk memperluas area*2";
}