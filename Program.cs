using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = DocFile(out string duongDanThuMuc);
            var b = XuLy(a);
            var c = GhiFile(b, duongDanThuMuc);
            Console.WriteLine(c);
            Console.ReadLine();
        }

        /// <summary>
        /// Tinh diem trung binh
        /// </summary>
        /// <param name="dsDiem"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static double TinhTrungBinh(double[] dsDiem)
        {
            if (dsDiem.Length != 3)
            {
                throw new Exception("Du lieu khong hop le");
            }

            double tong = 0;
            for (int i = 0; i < dsDiem.Length; i++)
            {
                if (dsDiem[i] < 0 || dsDiem[i] > 10)
                {
                    throw new Exception("Diem so phai >= 0 va <= 10");
                }
                tong += dsDiem[i];
            }

            return tong / (double)dsDiem.Length;
        }

        /// <summary>
        /// Doc file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static string[] DocFile(out string filePath) {
            bool valid = false;
            string[] noiDungFile = null;
            filePath = "";
            bool XacThucDuongDan(string duongDan)
            {
                if (duongDan == null || duongDan == "")
                {
                    Console.WriteLine("ERR::Duong dan bi rong");
                    return false;
                }
                if (!File.Exists(duongDan))
                {
                    Console.WriteLine("ERR::File khong ton tai Hoac duong dan khong hop le");
                    return false;
                }
                if ( Path.GetExtension(duongDan).ToLower() != ".txt")
                {
                    Console.WriteLine("ERR::File khong hop le");
                    return false;
                }
                return true;
            }

            while(!valid)
            {
                Console.Write("Nhap duong dan file >>> ");
                filePath = Console.ReadLine();
                valid = XacThucDuongDan(filePath);
                if (valid)
                {
                    noiDungFile = File.ReadAllLines(filePath);
                    if (noiDungFile.Length == 0)
                    {
                        Console.WriteLine("ERR::File rong!");
                        valid = false;
                    }
                }
            }
            filePath = Path.GetDirectoryName(filePath);
            return noiDungFile;
        }

        /// <summary>
        /// Ghi file
        /// </summary>
        /// <param name="noiDungFile"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static bool GhiFile(string[] noiDungFile, string filePath = "") {
            try
            {
                File.WriteAllLines(filePath + "\\output.txt", noiDungFile);
                Console.WriteLine("Done!");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Sap xep mang
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        static string[] SapXep(string[] ds) {
            string[] mang = ds;
            int doRongMang = mang.Length;

            if (doRongMang == 1) return ds;

            void _HoanVi(ref string a, ref string b)
            {
                string temp = a;
                a = b;
                b = temp;
            }

            for (int i = 1; i < doRongMang; i++)
            {
                int j = i - 1;
                while (j > 0)
                {
                    CHUANBI:
                    string[] tamI = mang[i].Split(',');
                    string[] tamJ = mang[j].Split(',');
                    bool validI = double.TryParse(tamI[tamI.Length - 1], out double diemTaiHangThuI);
                    bool validJ = double.TryParse(tamJ[tamJ.Length - 1], out double diemTaiHangThuJ);
                    
                    if (!validI)
                    {
                        _HoanVi(ref mang[i], ref mang[doRongMang - 1]);
                        doRongMang--;
                        goto CHUANBI;
                    }
                    if (!validJ)
                    {
                        _HoanVi(ref mang[j], ref mang[doRongMang - 1]);
                        doRongMang--;
                        goto CHUANBI;
                    }

                    if (validI && validJ && diemTaiHangThuI > diemTaiHangThuJ)
                    {
                        _HoanVi(ref mang[i], ref mang[j]);
                    }
                    else
                        break;
                    j--;
                }
            }
            return mang;
        }

        /// <summary>
        /// Xu ly noi dung file
        /// </summary>
        /// <param name="noiDungFile"></param>
        /// <returns></returns>
        static string[] XuLy(string[] noiDungFile)
        {
            string[] chuaSapXep = new string[noiDungFile.Length - 1];
            string[] ketqua = new string[noiDungFile.Length];

            // Tinh trung binh
            for (int i = 1; i < noiDungFile.Length; i++)
            {
                string[] line = noiDungFile[i].Split(',');
                double[] dsDiem = new double[] { 0d, 0d, 0d };
                string[] lineKetQua = new string[4] { line[0], line[1], line[2], "" };

                try
                {
                    for (int j = 3; j < line.Length; j++)
                    {
                        bool valid = double.TryParse(line[j], out double diem);
                        if (valid) dsDiem[j - 3] = diem;
                        else throw new Exception("Du lieu khong hop le");
                    }
                    lineKetQua[3] = TinhTrungBinh(dsDiem).ToString();
                }
                catch(Exception e)
                {
                    lineKetQua[3] = e.Message;
                }
                chuaSapXep[i - 1] = String.Join(",", lineKetQua);
            }

            // Sap xep
            string[] daSapXep = SapXep(chuaSapXep);

            // HEADER
            ketqua[0] = "name,id,birth,dtb";
            for(int i = 1; i < ketqua.Length; i++)
            {
                ketqua[i] = daSapXep[i - 1];
            }

            return ketqua;
        }
    }
}
