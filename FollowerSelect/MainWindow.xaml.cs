using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace FollowerSelect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<TMyFollower> myEpicFollowers;
        public List<TMyFollower> myOtherFollowers;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<int> Test = new List<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            List<int>[] Missions ={ new List<int>(new int[] { 1, 2, 3, 6, 7, 9 }),
                new List<int>(new int[] { 4, 4, 5, 6, 8, 9 }),
                new List<int>(new int[] { 1, 1, 3, 5, 7, 8 }),
                new List<int>(new int[] { 1, 2, 6, 7, 7, 9 }) };
            List<List<TSolOne>> Solutions = new List<List<TSolOne>>();
            List<int> CurrArrangement;
            List<TRe> Followers = new List<TRe>();
            for (int missionCount = 0; missionCount < 4; missionCount++)
            {
                ListBox lsbNow = this.FindName("lsbMission" + (missionCount + 1).ToString()) as ListBox;
                Solutions.Add(FindSolution(Missions[missionCount]));
                lsbNow.DataContext = Solutions[missionCount];
            }

            for (int index0 = 0; index0 < Solutions[0].Count; index0++)
            {
                for (int index1 = 0; index1 < Solutions[1].Count; index1++)
                {

                    for (int index2 = 0; index2 < Solutions[2].Count; index2++)
                    {
                        for (int index3 = 0; index3 < Solutions[3].Count; index3++)
                        {
                            CurrArrangement = new List<int>();
                            if (!CurrArrangement.Contains(Solutions[0][index0].SolPair[0]))
                                CurrArrangement.Add(Solutions[0][index0].SolPair[0]);
                            if (!CurrArrangement.Contains(Solutions[0][index0].SolPair[1]))
                                CurrArrangement.Add(Solutions[0][index0].SolPair[1]);
                            if (!CurrArrangement.Contains(Solutions[0][index0].SolPair[2]))
                                CurrArrangement.Add(Solutions[0][index0].SolPair[2]);
                            if (!CurrArrangement.Contains(Solutions[1][index1].SolPair[0]))
                                CurrArrangement.Add(Solutions[1][index1].SolPair[0]);
                            if (!CurrArrangement.Contains(Solutions[1][index1].SolPair[1]))
                                CurrArrangement.Add(Solutions[1][index1].SolPair[1]);
                            if (!CurrArrangement.Contains(Solutions[1][index1].SolPair[2]))
                                CurrArrangement.Add(Solutions[1][index1].SolPair[2]);
                            if (!CurrArrangement.Contains(Solutions[2][index2].SolPair[0]))
                                CurrArrangement.Add(Solutions[2][index2].SolPair[0]);
                            if (!CurrArrangement.Contains(Solutions[2][index2].SolPair[1]))
                                CurrArrangement.Add(Solutions[2][index2].SolPair[1]);
                            if (!CurrArrangement.Contains(Solutions[2][index2].SolPair[2]))
                                CurrArrangement.Add(Solutions[2][index2].SolPair[2]);
                            if (!CurrArrangement.Contains(Solutions[3][index3].SolPair[0]))
                                CurrArrangement.Add(Solutions[3][index3].SolPair[0]);
                            if (!CurrArrangement.Contains(Solutions[3][index3].SolPair[1]))
                                CurrArrangement.Add(Solutions[3][index3].SolPair[1]);
                            if (!CurrArrangement.Contains(Solutions[3][index3].SolPair[2]))
                                CurrArrangement.Add(Solutions[3][index3].SolPair[2]);
                            TRe AbbilityArrangement = new TRe();
                            AbbilityArrangement.FollowerCnts = CurrArrangement.Count;
                            AbbilityArrangement.SolIndex[0] = index0;
                            AbbilityArrangement.SolIndex[1] = index1;
                            AbbilityArrangement.SolIndex[2] = index2;
                            AbbilityArrangement.SolIndex[3] = index3;
                            AbbilityArrangement.FollowerSelect = CurrArrangement;
                            Followers.Add(AbbilityArrangement);
                        }
                    }
                }
            }
            //所需追随者数量排序
            List<TRe> SortedFolloers = new List<TRe>();
            SortedFolloers = Followers.OrderBy(x => x.FollowerCnts).ToList();
            //优化结果，去除冗余结果
            for (int itrStart = 0; itrStart < SortedFolloers.Count; itrStart++)
            {

                for (int itrEnd = SortedFolloers.Count - 1; itrEnd > itrStart; itrEnd--)
                {
                    if (SortedFolloers[itrEnd].Contains(SortedFolloers[itrStart]))
                        SortedFolloers.RemoveAt(itrEnd);
                }
            }
            lsbRe.DataContext = SortedFolloers;
            txb.Text = SortedFolloers.Count.ToString();
        }


        //FindSolution function
        //Input:6个int的list，任务列表
        //Output:int[3]的list，分组列表
        private List<TSolOne> FindSolution(List<int> Mission)
        {
            List<TSolOne> Result = new List<TSolOne>();
            for (int i = 1; i < 6; i++)
            {
                //if (Mission[i] == Mission[0])
                //    i++;
                List<int> L4 = new List<int>(Mission);
                L4.Remove(Mission[0]);
                L4.Remove(Mission[i]);
                for (int j = 1; j < 4; j++)
                {
                    //if (L4[j] == L4[0])
                    //    j++;
                    List<int> L2 = new List<int>(L4);
                    L2.Remove(L4[0]);
                    L2.Remove(L4[j]);
                    TSolOne OneSol = new TSolOne(new int[3] { Mission[0] * 10 + Mission[i], L4[0] * 10 + L4[j], L2[0] * 10 + L2[1] });
                    Result.Add(OneSol);
                }
            }
            //去除冗余结果
            for (int i = 0; i < Result.Count; i++)
            {
                for (int j = Result.Count - 1; j > i; j--)
                {
                    if (Result[i].Equals(Result[j]))
                        Result.RemoveAt(j);
                }
            }


            return Result;
        }

        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable OpenCSV(string filePath)
        {
            //Encoding encoding = Encoding.GetEncoding(filePath);//  GetType(filePath); //Encoding.ASCII;//
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
            string strLine = "";
            string[] aryLine = null;
            string[] tableHead = null;
            int columnCount = 0;
            bool IsFirst = true;
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }
            sr.Close();
            fs.Close();
            return dt;
        }

        private void lsbRe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TRe sel = lsbRe.SelectedItem as TRe;
            lsbMission1.SelectedIndex = sel.SolIndex[0];
            lsbMission2.SelectedIndex = sel.SolIndex[1];
            lsbMission3.SelectedIndex = sel.SolIndex[2];
            lsbMission4.SelectedIndex = sel.SolIndex[3];

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV文件|*.csv|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == true)
            {
                DataTable dt = OpenCSV(openFileDialog.FileName);
                dtgMy.DataContext = dt;
                myEpicFollowers = new List<TMyFollower>();
                foreach(DataRow row in dt.Rows)
                {
                    
                    //MessageBox.Show(row.ItemArray[6].ToString());
                    if (int.Parse(row.ItemArray[2].ToString())==4) //紫色追随者
                    {
                        int abb1 = FindAbbility(row.ItemArray[6].ToString());
                        int abb2 = FindAbbility(row.ItemArray[7].ToString());
                        int abb = abb1 * 10 + abb2;
                        if(row.ItemArray[8].ToString()=="超级舞王"
                            ||row.ItemArray[9].ToString()=="超级舞王"
                            ||row.ItemArray[10].ToString()=="超级舞王")
                        {
                            int abb3 = 6;
                            //(abb1<abb2？abb1:abb2)
                        }
                    }

                   // TMyFollower follower = new TMyFollower(row.Cells[0].ToString(), )
                    
                }
            }
            else
            {
                MessageBox.Show("用户取消");
            }
        }

        private int FindAbbility(string abb)
        {
            int result = 0;
            if (abb.Equals("限时战斗"))
                result = 1;
            else if (abb.Equals("强力法术"))
                result = 2;
            else if (abb.Equals("重击"))
                result = 3;
            else if (abb.Equals("群体伤害"))
                result = 4;
            else if (abb.Equals("爪牙围攻"))
                result = 5;
            else if (abb.Equals("危险区域"))
                result = 6;
            else if (abb.Equals("致命爪牙"))
                result = 7;
            else if (abb.Equals("魔法减益"))
                result = 8;
            else if (abb.Equals("野生怪物入侵"))
                result = 9;
            else if (abb.Equals(""))
                result = 0;
            else
            {
                MessageBox.Show("技能错误");
                return -1;
            }
            return result;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    public class TRe
    {
        public int FollowerCnts;
        public int[] SolIndex { get; set; }
        public List<int> FollowerSelect { get; set; }
        public TRe()
        {
            SolIndex = new int[4];
            FollowerSelect = new List<int>();
        }
        public bool Contains(TRe value)
        {
            //bool Result = false;
            foreach (int AbbilityPair in value.FollowerSelect)
            {
                if (!this.FollowerSelect.Contains(AbbilityPair))
                    return false;
            }
            return true;
        }
        public override string ToString()
        {
            return FollowerCnts.ToString() + " "
                + SolIndex[0].ToString() + "-"
                + SolIndex[1].ToString() + "-"
                + SolIndex[2].ToString() + "-"
                + SolIndex[3].ToString();
        }
    };

    public class TSolOne
    {
        public int[] SolPair { set; get; }
        public TSolOne()
        {
            SolPair = new int[3];
        }
        public TSolOne(int[] Array)
        {
            SolPair = new int[3];
            SolPair[0] = Array[0];
            SolPair[1] = Array[1];
            SolPair[2] = Array[2];
        }
        public bool Equals(TSolOne SolOne)
        {
            if (SolOne.SolPair.Contains(this.SolPair[0]) &&
                SolOne.SolPair.Contains(this.SolPair[1]) &&
                SolOne.SolPair.Contains(this.SolPair[2]))
                return true;
            else
                return false;
        }
    };

    public class TMyFollower
    {

        public string Name { set; get; }
        public int Quality { set; get; }   //2=绿 3=蓝 4=紫
        public int Abbility { set; get; }
        public TMyFollower(string name, int quality, int abbility)
        {
            this.Name = name;
            this.Quality = quality;
            if (abbility >= 10 && abbility < 100)  //小数字前置
            {
                int first = abbility / 10;
                int second = abbility % 10;
                if (first == 0 || second == 0)
                    throw new ArgumentOutOfRangeException(this.Name + "技能异常");
                else if (first <= second)
                    this.Abbility = first * 10 + second;
                else
                    this.Abbility = second * 10 + first;
            }
            else if (abbility >= 100 && abbility < 1000)
            {
                int first = abbility / 100;
                int second = abbility / 10 % 10;
                int third = abbility % 10;

                if (first == 6||second == 6||third == 6)
                {
                    this.Abbility = Math.Min(Math.Min(first, second),third)*100+

                }
                else
                    throw new ArgumentOutOfRangeException(this.Name + "技能异常");
            }
            else if (abbility < 10 && abbility > 0)
                this.Abbility = abbility;
            else
                throw new ArgumentOutOfRangeException(this.Name + "技能异常");

        }
        public int Counters(int AbbilityPair)
        {
            int Result = 0; //0无法全部对应，1可以全部对应，2可能可以全部对应
            if (Abbility < 10)  //单一技能追随者
            {
                int first = AbbilityPair / 10;
                int second = AbbilityPair % 10;
                if (Abbility == first || Abbility == second)
                    Result = 2;
            }
            else if (Abbility > 10 && Abbility < 100)
            {
                if (Abbility == AbbilityPair)
                    Result = 1;
            }
            else if (Abbility > 100)
            {
                int first = Abbility / 100;
                int second = Abbility / 10 % 10;
                int third = Abbility % 10;
                int A = first * 10 + second;
                int B = first * 10 + third;
                int C = second * 10 + third;
                if (A == AbbilityPair || B == AbbilityPair || C == AbbilityPair)
                {
                    Result = 1;
                }
            }
            return Result;
        }
    }

}
