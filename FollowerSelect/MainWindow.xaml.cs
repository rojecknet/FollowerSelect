﻿using Microsoft.Win32;
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
        public List<TMyFollower> myFollowers;
        //public List<TMyFollower> myEpicFollowers;
        //public List<TMyFollower> myOtherFollowers;
        public List<TOptedRe> OptedResult;
        public Dictionary<int, string> AbbilityDef = new Dictionary<int, string>();
        public bool isMyFollowerReaded = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                List<int> Test = new List<int>(new int[] { 1, 2, 3, 4, 5, 6 });
                List<int>[] Missions ={ new List<int>(new int[] { 1, 2, 3, 6, 7 }),
                new List<int>(new int[] { 4, 4, 5, 6, 8, 9 }),
                new List<int>(new int[] { 1, 1, 3, 5, 7, 8 }),
                new List<int>(new int[] { 1, 2, 6, 7, 7, 9 }) };
                List<List<TSolOne>> Solutions = new List<List<TSolOne>>();
                List<int> CurrArrangement;
                List<TRe> InitFollowers = new List<TRe>();
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
                                AbbilityArrangement.AbbilityPairSelect = CurrArrangement;
                                InitFollowers.Add(AbbilityArrangement);
                            }
                        }
                    }
                }
                //所需追随者数量排序
                //List<TRe> SortedFolloers = new List<TRe>();
                //SortedFolloers = InitFollowers.OrderBy(x => x.FollowerCnts).ToList();
                //优化结果，去除冗余结果
                for (int itrStart = 0; itrStart < InitFollowers.Count; itrStart++)
                {

                    for (int itrEnd = InitFollowers.Count - 1; itrEnd > itrStart; itrEnd--)
                    {
                        if (InitFollowers[itrEnd].Contains(InitFollowers[itrStart]))
                            InitFollowers.RemoveAt(itrEnd);
                    }
                }
                //lsbRe.DataContext = SortedFolloers;
                txb.Text = InitFollowers.Count.ToString();
                MessageBox.Show("任务分组完毕");
                if (isMyFollowerReaded)
                {
                    OptedResult = new List<TOptedRe>();
                    foreach (TRe result in InitFollowers)
                    {
                        TOptedRe optedArrangement = new TOptedRe();
                        optedArrangement.MissionArrangement = result;
                        foreach (int AbbPair in result.AbbilityPairSelect)
                        {
                            int isCountered = 0; //0无法全部对应，1可以全部对应，2可能可以全部对应

                            foreach (TMyFollower follower in myFollowers)
                            {
                                int cntr = follower.Counterable(AbbPair);
                                if (cntr == 1)
                                {
                                    isCountered = 1;
                                    break;
                                }
                                else if (cntr ==2)
                                {
                                    isCountered = 2;
                                    continue;
                                }
                                
                            }
                            if (isCountered == 0)
                                optedArrangement.CannotCounteredCnt++;
                            else if (isCountered == 1)
                                optedArrangement.CounteredCnt++;
                            else if (isCountered == 2)
                                optedArrangement.MayFullyCounteredCnt++;



                          /*foreach (TMyFollower follower in myEpicFollowers) //紫色随从对应
                            {
                                isCountered = follower.Counterable(AbbPair);
                                if (isCountered == 1)
                                    break;
                            }
                            if (isCountered == 1)
                            {
                                optedArrangement.CounteredCnt++;
                                continue;
                            }
                            foreach (TMyFollower follower in myOtherFollowers)
                            {
                                isCountered = follower.Counterable(AbbPair);
                                if (isCountered == 2)
                                    break;
                            }
                            if (isCountered == 2)
                            {
                                optedArrangement.MayFullyCounteredCnt++;
                                continue;
                            }
                            optedArrangement.CannotCounteredCnt++;*/
                        }
                        if (optedArrangement.CounteredCnt + optedArrangement.CannotCounteredCnt + optedArrangement.MayFullyCounteredCnt == optedArrangement.MissionArrangement.AbbilityPairSelect.Count)
                            OptedResult.Add(optedArrangement);
                        else
                        {
                            MessageBox.Show("技能错误");
                        }
                    }
                    //List<TOptedRe> SortedOptedResult;
                    if (rdb1.IsChecked.Value)  //总
                    {
                        OptedResult = OptedResult.OrderBy(x => x.MissionArrangement.FollowerCnts).ToList();
                    }
                    else if (rdb2.IsChecked.Value)  //已
                    {
                        OptedResult = OptedResult.OrderByDescending(x => x.CounteredCnt).ToList();
                    }
                    else if (rdb3.IsChecked.Value)  //可
                    {
                        OptedResult = OptedResult.OrderBy(x => x.MayFullyCounteredCnt).ToList();
                    }
                    else if (rdb4.IsChecked.Value)  //未
                    {
                        OptedResult = OptedResult.OrderBy(x => x.CannotCounteredCnt).ToList();
                    }

                    lsbOptedRe.DataContext = OptedResult;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //FindSolution function
        //Input:6个int的list，任务列表
        //Output:int[3]的list，分组列表
        private List<TSolOne> FindSolution(List<int> Mission)
        {
            List<TSolOne> Result = new List<TSolOne>();
            for (int i = 1; i < Mission.Count; i++)
            {
                //if (Mission[i] == Mission[0])
                //    i++;
                for (int addCnt = Mission.Count; addCnt < 6; addCnt++)
                {
                    Mission.Add(0);
                }
                List<int> L4 = new List<int>(Mission);
                L4.Remove(Mission[0]);
                L4.Remove(Mission[i]);
                for (int j = 1; j < Mission.Count - 2; j++)
                {
                    //if (L4[j] == L4[0])
                    //    j++;
                    List<int> L2 = new List<int>(L4);
                    L2.Remove(L4[0]);
                    L2.Remove(L4[j]);
                    int a =  Mission[0] * 10 + Mission[i];
                    if (a % 10 == 0)
                        a = a / 10;
                    int b = L4[0] * 10 + L4[j];
                    if (b % 10 == 0)
                        b = b / 10;
                    int c = L2[0] * 10 + L2[1];
                    if (c % 10 == 0)
                        c= c / 10;
                    TSolOne OneSol = new TSolOne(new int[3] { a, b, c });
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

        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable OpenCSV(string filePath)
        {
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
                        if (j >= aryLine.Count())
                            dr[j] = "";
                        else
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

        /*private void lsbRe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TRe sel = lsbRe.SelectedItem as TRe;
            lsbMission1.SelectedIndex = sel.SolIndex[0];
            lsbMission2.SelectedIndex = sel.SolIndex[1];
            lsbMission3.SelectedIndex = sel.SolIndex[2];
            lsbMission4.SelectedIndex = sel.SolIndex[3];

        }*/

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV文件|*.csv|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == true)
                {
                    DataTable dt = OpenCSV(openFileDialog.FileName);
                    dtgMy.DataContext = dt;
                    myFollowers = new List<TMyFollower>();
                    //myEpicFollowers = new List<TMyFollower>();
                    //myOtherFollowers = new List<TMyFollower>();
                    foreach (DataRow row in dt.Rows)
                    {

                        //MessageBox.Show(row.ItemArray[6].ToString());
                        //if (int.Parse(row.ItemArray[2].ToString()) == 4) //紫色追随者
                        //{
                            int abb1 = FindAbbility(row.ItemArray[6].ToString());
                            int abb2 = FindAbbility(row.ItemArray[7].ToString());
                            int abb = abb1 * 10 + abb2;
                            /* if(row.ItemArray[8].ToString()=="超级舞王"
                                 ||row.ItemArray[9].ToString()=="超级舞王"
                                 ||row.ItemArray[10].ToString()=="超级舞王")
                             {
                                 int abb3 = 6;
                                 //(abb1<abb2？abb1:abb2)
                             }*/
                            TMyFollower epicfollower = new TMyFollower(row.ItemArray[0].ToString(), int.Parse(row.ItemArray[3].ToString()), int.Parse(row.ItemArray[2].ToString()), abb);
                            myFollowers.Add(epicfollower);
                        //}
                        /*else if (int.Parse(row.ItemArray[2].ToString()) == 2 || int.Parse(row.ItemArray[2].ToString()) == 3)
                        {
                            int abb1 = FindAbbility(row.ItemArray[6].ToString());
                            TMyFollower otherfollower = new TMyFollower(row.ItemArray[0].ToString(), int.Parse(row.ItemArray[3].ToString()), int.Parse(row.ItemArray[2].ToString()), abb1);
                            myOtherFollowers.Add(otherfollower);
                        }*/
                    }
                    isMyFollowerReaded = true;
                    btn1.Content = "点我";
                    btn1.IsEnabled = true;

                }
                else
                {
                    MessageBox.Show("用户取消");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int FindAbbility(string abb)
        {
            foreach (KeyValuePair<int, string> abbDef in AbbilityDef)
            {
                if (abb.Equals(abbDef.Value))
                    return abbDef.Key;
            }
            //if (abb.Equals(""))
            //    return 0;
            MessageBox.Show("技能错误");
            return -1;
        }

        private string GetAbbName(int abb)
        {
            string AbbName="";
            if(abb>0 && abb<10)
            {
                AbbName = AbbilityDef[abb];
            }
            else if(abb>=10 && abb<100)
            {
                int first = abb / 10;
                int second = abb % 10;
                AbbName = AbbilityDef[first] + "+" + AbbilityDef[second];
            }
            else if(abb>=100 && abb<1000)
            {

            }

            return AbbName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AbbilityDef.Add(0, "");
            AbbilityDef.Add(1, "限时战斗");
            AbbilityDef.Add(2, "强力法术");
            AbbilityDef.Add(3, "重击");
            AbbilityDef.Add(4, "群体伤害");
            AbbilityDef.Add(5, "爪牙围攻");
            AbbilityDef.Add(6, "危险区域");
            AbbilityDef.Add(7, "致命爪牙");
            AbbilityDef.Add(8, "魔法减益");
            AbbilityDef.Add(9, "野生怪物入侵");
            DataTable MissionHighMual = new DataTable();
            MissionHighMual.Columns.Add("任务");
            MissionHighMual.Columns.Add("限时战斗");
            MissionHighMual.Columns.Add("强力法术");
            MissionHighMual.Columns.Add("重击");
            MissionHighMual.Columns.Add("群体伤害");
            MissionHighMual.Columns.Add("爪牙围攻");
            MissionHighMual.Columns.Add("危险区域");
            MissionHighMual.Columns.Add("致命爪牙");
            MissionHighMual.Columns.Add("魔法减益");
            MissionHighMual.Columns.Add("野生怪物入侵");

            MissionHighMual.Rows.Add(new string[] { "一", "1", "1", "1", "", "", "1", "1", "", "1" });
            MissionHighMual.Rows.Add(new string[] { "二", "", "", "", "2", "1", "1", "", "1", "1" });
            MissionHighMual.Rows.Add(new string[] { "三", "2", "", "1", "", "1", "", "1", "1", "" });
            MissionHighMual.Rows.Add(new string[] { "四", "1", "1", "", "", "", "1", "2", "", "1" });
            dtgMission.DataContext = MissionHighMual;
        }

        private void rdb1_Checked(object sender, RoutedEventArgs e)
        {
            if (OptedResult != null)
            {
                OptedResult = OptedResult.OrderBy(x => x.MissionArrangement.FollowerCnts).ToList();
                lsbOptedRe.DataContext = OptedResult;
            }

        }

        private void rdb2_Checked(object sender, RoutedEventArgs e)
        {
            if (OptedResult != null)
            {
                OptedResult = OptedResult.OrderByDescending(x => x.CounteredCnt).ToList();
                lsbOptedRe.DataContext = OptedResult;
            }
  }

        private void rdb3_Checked(object sender, RoutedEventArgs e)
        {
            if (OptedResult != null)
            { 
                OptedResult = OptedResult.OrderBy(x => x.MayFullyCounteredCnt).ToList();
                lsbOptedRe.DataContext = OptedResult;
            }
        }

        private void rdb4_Checked(object sender, RoutedEventArgs e)
        {
            if (OptedResult != null)
            { 
                OptedResult = OptedResult.OrderBy(x => x.CannotCounteredCnt).ToList();
                lsbOptedRe.DataContext = OptedResult;
            }
        }

        private void lsbOptedRe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TOptedRe selRe = lsbOptedRe.SelectedItem as TOptedRe;
            lsbMission1.SelectedIndex = selRe.MissionArrangement.SolIndex[0];
            lsbMission2.SelectedIndex = selRe.MissionArrangement.SolIndex[1];
            lsbMission3.SelectedIndex = selRe.MissionArrangement.SolIndex[2];
            lsbMission4.SelectedIndex = selRe.MissionArrangement.SolIndex[3];
            DataTable dtCountered = new DataTable();
            dtCountered.Columns.Add("技能组合");
            dtCountered.Columns.Add("应对追随者");
            DataTable dtMayCountered = new DataTable();
            dtMayCountered.Columns.Add("技能组合");
            dtMayCountered.Columns.Add("应对追随者");
            DataTable dtCannotCountered = new DataTable();
            dtCannotCountered.Columns.Add("技能组合");
            dtCannotCountered.Columns.Add("应对追随者");

            foreach(int abb in selRe.MissionArrangement.AbbilityPairSelect)
            {
                int iscountered = 0;//0无法全部对应，1可以全部对应，2可能可以全部对应
                string followersName = "";
                foreach(TMyFollower Follower in myFollowers)
                {
                    //iscountered = epicFollower.Counterable(abb);
                    if (Follower.Counterable(abb) == 1)
                    {
                        followersName += Follower.Name + "：";
                        iscountered = 1;
                    }
                }
                if(iscountered != 1)
                {
                    foreach (TMyFollower otherFollower in myFollowers)
                    {
                        if(otherFollower.Counterable(abb) == 2)
                        {
                            followersName += otherFollower.Name + "：";
                            iscountered = 2;
                        }
                    }
                }
                if(iscountered == 0)
                {
                    dtCannotCountered.Rows.Add(new string[2] { GetAbbName(abb), followersName });

                }
                else if (iscountered == 1)
                {
                    dtCountered.Rows.Add(new string[2] { GetAbbName(abb), followersName });
                }
                else if(iscountered == 2)
                {
                    dtMayCountered.Rows.Add(new string[2] { GetAbbName(abb), followersName });
                }
            }
            dtg1.DataContext = dtCountered;
            dtg2.DataContext = dtMayCountered;
            dtg3.DataContext = dtCannotCountered;
        
        
        }
    }
    public class TRe
    {
        public int FollowerCnts;
        public int[] SolIndex { get; set; }
        public List<int> AbbilityPairSelect { get; set; }
        public TRe()
        {
            SolIndex = new int[4];
            AbbilityPairSelect = new List<int>();
        }
        public bool Contains(TRe value)
        {
            //bool Result = false;
            foreach (int AbbilityPair in value.AbbilityPairSelect)
            {
                if (!this.AbbilityPairSelect.Contains(AbbilityPair))
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

    public class TOptedRe
    {
        public TRe MissionArrangement;
        public int CounteredCnt;
        public int MayFullyCounteredCnt;
        public int CannotCounteredCnt;
        public TOptedRe()
        {
            MissionArrangement = new TRe();
            CounteredCnt = 0;
            MayFullyCounteredCnt = 0;
            CannotCounteredCnt = 0;
        }
        public override string ToString()
        {
            return MissionArrangement.FollowerCnts.ToString()+" "+
                CounteredCnt.ToString() + " " +
                MayFullyCounteredCnt.ToString() + " " +
                CannotCounteredCnt.ToString() + "   "
                + (MissionArrangement.SolIndex[0]+1).ToString() + "-"
                + (MissionArrangement.SolIndex[1]+1).ToString() + "-"
                + (MissionArrangement.SolIndex[2]+1).ToString() + "-"
                + (MissionArrangement.SolIndex[3]+1).ToString();
        }

    }

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
        public int Level { set; get; }
        public int Quality { set; get; }   //2=绿 3=蓝 4=紫
        public int Abbility { set; get; }
        public TMyFollower(string name, int level, int quality, int abbility)
        {
            this.Name = name;
            this.Level = level;
            this.Quality = quality;
            if (abbility >= 10 && abbility < 100)  //小数字前置
            {
                int first = abbility / 10;
                int second = abbility % 10;
                if (first == 0)
                    this.Abbility = second;
                else if (second == 0)
                    this.Abbility = first;
                else if (first <= second)
                    this.Abbility = first * 10 + second;
                else
                    this.Abbility = second * 10 + first;
            }
            else if (abbility >= 100 && abbility < 1000)
            {
                int a = abbility / 100;
                int b = abbility / 10 % 10;
                int c = abbility % 10;

                if (a == 6 || b == 6 || c == 6)
                {
                    int first = Math.Min(Math.Min(a, b), c);
                    int second = a > b ? (a < c ? a : (b > c ? b : c)) : (b < c ? b : (a > c ? a : c));
                    int third = Math.Max(Math.Max(a, b), c);
                    this.Abbility = first * 100 + second * 10 + third;
                }
                else
                    throw new ArgumentOutOfRangeException(this.Name + "技能异常");
            }
            else if (abbility < 10 && abbility > 0)
                this.Abbility = abbility;
            else
                throw new ArgumentOutOfRangeException(this.Name + "技能异常");

        }
        public int Counterable(int AbbilityPair)
        {
            int Result = 0; //0无法全部对应，1可以全部对应，2可能可以全部对应
            if (this.Abbility < 10)  //单一技能追随者
            {
                if (AbbilityPair > 10)
                {
                    int first = AbbilityPair / 10;
                    int second = AbbilityPair % 10;
                    if (this.Abbility == first || this.Abbility == second)
                        Result = 2;
                }
                if (AbbilityPair == this.Abbility)
                        Result = 1;

            }
            else if (this.Abbility > 10 && this.Abbility < 100)
            {
                if (this.Abbility == AbbilityPair)
                    Result = 1;
                if(AbbilityPair < 10)
                {
                    int first = this.Abbility / 10;
                    int second = this.Abbility % 10;
                    if (first == AbbilityPair || second == AbbilityPair)
                        Result = 1;
                }
            }
            else if (this.Abbility > 100)
            {
                int first = Abbility / 100;
                int second = this.Abbility / 10 % 10;
                int third = this.Abbility % 10;
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
        public override string ToString()
        {
            return this.Name + " " + this.Quality.ToString() + " " + this.Abbility.ToString();
        }
    }

}


