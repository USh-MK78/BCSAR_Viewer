using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCSARLibrary;

namespace BCSAR_Viewer
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

        public int BankDataCount { get; private set; }

        public BCSAR BCSAR { get; set; }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
            //ファイルを開く
            OpenFileDialog Open_BCSAR = new OpenFileDialog()
            {
                Title = "Open(BCSAR)",
                InitialDirectory = @"C:\Users\User\Desktop",
                Filter = "bcsar file|*.bcsar"
            };

            if (Open_BCSAR.ShowDialog() != DialogResult.OK) return;

            System.IO.FileStream fs = new FileStream(Open_BCSAR.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            BCSAR bcsar = new BCSAR();
            bcsar.Read_BCSAR(br);

            br.Close();
            fs.Close();

            BCSAR = bcsar;

            //foreach (var item in bcsar.PartitionOffsetInfoList[0].PartitionData.Partition_STRG.StringOffsetTables)
            //{
            //    listBox1.Items.Add(item.Name);
            //}


            #region TreeView
            treeView1.HideSelection = false;

            List<TreeNode> SectionNodeList = new List<TreeNode>();
            for (int i = 0; i < bcsar.PartitionOffsetInfoList.Count; i++)
            {
                List<TreeNode> EntryNameList = new List<TreeNode>();

                if (bcsar.PartitionOffsetInfoList[i].ReferenceID == BCSARHelper.Enum.PartitionReferenceID.STRG)
                {
                    var StringTableNode = bcsar.PartitionOffsetInfoList[i].PartitionData.Partition_STRG.StringTableList.Select(x => new TreeNode(x.Name)).ToArray();
                    var LookUpTableNode = bcsar.PartitionOffsetInfoList[i].PartitionData.Partition_STRG.LookupTableList.Select(x => new TreeNode(x.ID.ToString())).ToArray();

                    TreeNode StringTableTreeNode = new TreeNode("StringTable", StringTableNode);
                    TreeNode LookUpTableTreeNode = new TreeNode("LookupTable", LookUpTableNode);
                    EntryNameList.Add(StringTableTreeNode);
                    EntryNameList.Add(LookUpTableTreeNode);

                    //TreeNode treeNode = new TreeNode("STRG");
                    //treeNode.Nodes.Add(new TreeNode("StringTable", StringTableNode));
                    //treeNode.Nodes.Add(new TreeNode("LookupTable", LookUpTableNode));
                    //EntryNameList.Add(treeNode);
                }
                else if (bcsar.PartitionOffsetInfoList[i].ReferenceID == BCSARHelper.Enum.PartitionReferenceID.INFO)
                {
                    TreeNode treeNode = new TreeNode("TableDictionary");
                    foreach (var dictionary in bcsar.PartitionOffsetInfoList[i].PartitionData.Partition_INFO.INFOTableDictionary)
                    {
                        if (dictionary.Value.INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.AudioTable)
                        {
                            List<TreeNode> AudioTableNodeList = new List<TreeNode>();
                            for (int AudioDataCount = 0; AudioDataCount < dictionary.Value.Audio_Table.AudioDataList.Count; AudioDataCount++)
                            {
                                AudioTableNodeList.Add(new TreeNode(dictionary.Value.Audio_Table.AudioDataList[AudioDataCount].ToString() + "_" + AudioDataCount));
                            }

                            treeNode.Nodes.Add(new TreeNode("AudioTable", AudioTableNodeList.ToArray()));
                        }
                        else if (dictionary.Value.INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.SetTable)
                        {
                            List<TreeNode> SetTableNodeList = new List<TreeNode>();
                            for (int SetDataCount = 0; SetDataCount < dictionary.Value.Set_Table.SetDataList.Count; SetDataCount++)
                            {
                                SetTableNodeList.Add(new TreeNode(dictionary.Value.Set_Table.SetDataList[SetDataCount].ToString() + "_" + SetDataCount));
                            }

                            treeNode.Nodes.Add(new TreeNode("SetTable", SetTableNodeList.ToArray()));
                        }
                        else if (dictionary.Value.INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.BankTable)
                        {
                            List<TreeNode> BankTableNodeList = new List<TreeNode>();
                            for (int BankDataCount = 0; BankDataCount < dictionary.Value.Bank_Table.BankDataList.Count; BankDataCount++)
                            {
                                BankTableNodeList.Add(new TreeNode(dictionary.Value.Bank_Table.BankDataList[BankDataCount].ToString() + "_" + BankDataCount));
                            }

                            treeNode.Nodes.Add(new TreeNode("BankTable", BankTableNodeList.ToArray()));
                        }
                        else if (dictionary.Value.INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.WAVArchiveTable)
                        {
                            List<TreeNode> WAVArchiveTableNodeList = new List<TreeNode>();
                            for (int WAVArchiveDataCount = 0; WAVArchiveDataCount < dictionary.Value.WAVArchive_Table.WAVArchiveDataList.Count; WAVArchiveDataCount++)
                            {
                                WAVArchiveTableNodeList.Add(new TreeNode(dictionary.Value.WAVArchive_Table.WAVArchiveDataList[WAVArchiveDataCount].ToString() + "_" + WAVArchiveDataCount));
                            }

                            treeNode.Nodes.Add(new TreeNode("WAVArchiveTable", WAVArchiveTableNodeList.ToArray()));
                        }
                        else if (dictionary.Value.INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.GroupTable)
                        {
                            List<TreeNode> GroupTableNodeList = new List<TreeNode>();
                            for (int GroupDataCount = 0; GroupDataCount < dictionary.Value.Group_Table.GroupDataList.Count; GroupDataCount++)
                            {
                                GroupTableNodeList.Add(new TreeNode(dictionary.Value.Group_Table.GroupDataList[GroupDataCount].ToString() + "_" + GroupDataCount));
                            }

                            treeNode.Nodes.Add(new TreeNode("GroupTable", GroupTableNodeList.ToArray()));
                        }
                        else if (dictionary.Value.INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.PlayerTable)
                        {
                            List<TreeNode> PlayerTableNodeList = new List<TreeNode>();
                            for (int PlayerDataCount = 0; PlayerDataCount < dictionary.Value.Player_Table.PlayerDataList.Count; PlayerDataCount++)
                            {
                                PlayerTableNodeList.Add(new TreeNode(dictionary.Value.Player_Table.PlayerDataList[PlayerDataCount].ToString() + "_" + PlayerDataCount));
                            }

                            treeNode.Nodes.Add(new TreeNode("PlayerTable", PlayerTableNodeList.ToArray()));
                        }
                        else if (dictionary.Value.INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.FILETable)
                        {
                            List<TreeNode> FILETableNodeList = new List<TreeNode>();
                            for (int FILEDataCount = 0; FILEDataCount < dictionary.Value.FILE_Table.FILEDataList.Count; FILEDataCount++)
                            {
                                FILETableNodeList.Add(new TreeNode(dictionary.Value.FILE_Table.FILEDataList[FILEDataCount].ToString() + "_" + FILEDataCount));
                            }

                            treeNode.Nodes.Add(new TreeNode("FILETable", FILETableNodeList.ToArray()));
                        }
                        else if (dictionary.Value.INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.FileInfoTable)
                        {
                            treeNode.Nodes.Add(new TreeNode("FileInfoTable"));
                        }
                    }

                    EntryNameList.Add(treeNode);
                }
                else if (bcsar.PartitionOffsetInfoList[i].ReferenceID == BCSARHelper.Enum.PartitionReferenceID.FILE)
                {

                }


                TreeNode PartitionNameNode = new TreeNode(bcsar.PartitionOffsetInfoList[i].ReferenceID.ToString(), EntryNameList.ToArray());
                SectionNodeList.Add(PartitionNameNode);
            }

            TreeNode RootNode = new TreeNode("BCSAR_Root", SectionNodeList.ToArray());
            treeView1.Nodes.Add(RootNode);
            treeView1.TopNode.Expand();
            #endregion
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                treeView1.PathSeparator = ",";
                string[] Set = treeView1.SelectedNode.FullPath.Split(',');
                if (Set[0] == "BCSAR_Root")
                {
                    if (Set[1] == "STRG")
                    {
                        var h = BCSAR.PartitionOffsetInfoList.Find(x => x.ReferenceID == BCSARHelper.Enum.PartitionReferenceID.STRG).PartitionData.Partition_STRG;
                        if (Set.Length >= 3)
                        {
                            if (Set[2] == "StringTable")
                            {
                                if (Set[3] != null) propertyGrid1.SelectedObject = h.StringTableList.Find(x => x.Name == Set[3]);
                            }
                            else if (Set[2] == "LookupTable")
                            {
                                if (Set[3] != null) propertyGrid1.SelectedObject = h.LookupTableList.Find(x => x.ID == short.Parse(Set[3]));
                                //propertyGrid1.SelectedObject = h.LookupTableList.Find( x=> x.ID == Set[])
                            }
                        }
                        else
                        {
                            propertyGrid1.SelectedObject = h;
                        }

                        //var h = BCSAR.PartitionOffsetInfoList.Find(x => x.ReferenceID == BCSARHelper.Enum.PartitionReferenceID.STRG).PartitionData.Partition_STRG;
                        
                    }
                    else if (Set[1] == "INFO")
                    {
                        //if (Set[2] == "TableDictionary")
                        //{
                        //    if (Set[3] == "AudioTable")
                        //    {

                        //    }
                        //}

                        var h = BCSAR.PartitionOffsetInfoList.Find(x => x.ReferenceID == BCSARHelper.Enum.PartitionReferenceID.INFO).PartitionData.Partition_INFO;
                        propertyGrid1.SelectedObject = h;
                    }
                    else if (Set[1] == "FILE")
                    {
                        var h = BCSAR.PartitionOffsetInfoList.Find(x => x.ReferenceID == BCSARHelper.Enum.PartitionReferenceID.FILE).PartitionData.Partition_FILE;
                        propertyGrid1.SelectedObject = h;
                    }
                }


                //Set[]
            }
        }
    }
}
