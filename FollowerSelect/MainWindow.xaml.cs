using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            string[] MissionsDef = new string[] { "限时战斗", "强力法术", "重击", "群体伤害", "爪牙围攻", "危险区域", "致命爪牙", "魔法减益", "野生怪物入侵" };
            List<int> Test = new List<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            List<int>[] Missions ={ new List<int>(new int[] { 1, 2, 3, 6, 7, 9 }),
                new List<int>(new int[] { 4, 4, 5, 6, 7, 8 }),
                new List<int>(new int[] { 1, 1, 3, 6, 7, 8 }),
                new List<int>(new int[] { 1, 2, 6, 7, 7, 9 }) };
            List<List<int[]>> Solutions = new List<List<int[]>>();
            List<int> CurrArrangement;
            List<TRe> Followers = new List<TRe>();

            //List<int> FollowAbbility = new List<int>();
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
                            if (!CurrArrangement.Contains(Solutions[0][index0][0]))
                                CurrArrangement.Add(Solutions[0][index0][0]);
                            if (!CurrArrangement.Contains(Solutions[0][index0][1]))
                                CurrArrangement.Add(Solutions[0][index0][1]);
                            if (!CurrArrangement.Contains(Solutions[0][index0][2]))
                                CurrArrangement.Add(Solutions[0][index0][2]);
                            if (!CurrArrangement.Contains(Solutions[1][index1][0]))
                                CurrArrangement.Add(Solutions[1][index1][0]);
                            if (!CurrArrangement.Contains(Solutions[1][index1][1]))
                                CurrArrangement.Add(Solutions[1][index1][1]);
                            if (!CurrArrangement.Contains(Solutions[1][index1][2]))
                                CurrArrangement.Add(Solutions[1][index1][2]);
                            if (!CurrArrangement.Contains(Solutions[2][index2][0]))
                                CurrArrangement.Add(Solutions[2][index2][0]);
                            if (!CurrArrangement.Contains(Solutions[2][index2][1]))
                                CurrArrangement.Add(Solutions[2][index2][1]);
                            if (!CurrArrangement.Contains(Solutions[2][index2][2]))
                                CurrArrangement.Add(Solutions[2][index2][2]);
                            if (!CurrArrangement.Contains(Solutions[3][index3][0]))
                                CurrArrangement.Add(Solutions[3][index3][0]);
                            if (!CurrArrangement.Contains(Solutions[3][index3][1]))
                                CurrArrangement.Add(Solutions[3][index3][1]);
                            if (!CurrArrangement.Contains(Solutions[3][index3][2]))
                                CurrArrangement.Add(Solutions[3][index3][2]);
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
            lsbRe.DataContext = Followers;
            txb.Text = Followers.Count.ToString();
        }


        //FindSolution function
        //Input:6个int的list，任务列表
        //Output:int[3]的list，分组列表
        private List<int[]> FindSolution(List<int> Mission)
        {
            List<int[]> Result = new List<int[]>();
            for (int i = 1; i < 6; i++)
            {
                if (Mission[i] == Mission[0])
                    i++;
                List<int> L4 = new List<int>(Mission);
                L4.Remove(Mission[0]);
                L4.Remove(Mission[i]);
                for (int j = 1; j < 4; j++)
                {
                    if (L4[j] == L4[0])
                        j++;
                    List<int> L2 = new List<int>(L4);
                    L2.Remove(L4[0]);
                    L2.Remove(L4[j]);
                    int[] OneSol = new int[3] { Mission[0] * 10 + Mission[i], L4[0] * 10 + L4[j], L2[0] * 10 + L2[1] };
                    Result.Add(OneSol);
                }
            }
            return Result;
        }

        private void lsbRe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TRe sel = lsbRe.SelectedItem as TRe;
            lsbMission1.SelectedIndex = sel.SolIndex[0];
            lsbMission2.SelectedIndex = sel.SolIndex[1];
            lsbMission3.SelectedIndex = sel.SolIndex[2];
            lsbMission4.SelectedIndex = sel.SolIndex[3];

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
        public override string ToString()
        {
            return FollowerCnts.ToString() + " "
                + SolIndex[0].ToString() + "-"
                + SolIndex[1].ToString() + "-"
                + SolIndex[2].ToString() + "-"
                + SolIndex[3].ToString();
        }
    };
}
