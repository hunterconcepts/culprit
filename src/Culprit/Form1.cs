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

namespace Culprit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void scanButton_Click(object sender, EventArgs e)
        {
            folderTreeView.Nodes.Clear();

            var folder = folderTextBox.Text;
            ScanDirectory(folder);

        }

        private void ScanDirectory(string folder, TreeNode parent = null)
        {
            var target = new DirectoryInfo(folder);

            foreach (var f in target.GetDirectories())
            {
                try
                {
                    var size = DirectorySize(f);
                    var fileSize = GetFileSize(size);

                    var node = new TreeNode(f.Name + " (" + GetFileSize(size) + ")");

                    if (fileSize.Contains("GB"))
                    {
                        node.BackColor = Color.Red;
                    }

                    if (parent == null)
                    {
                        folderTreeView.Nodes.Add(node);
                    }
                    else
                    {
                        parent.Nodes.Add(node);
                    }

                    if (f.GetDirectories().Any())
                    {
                        ScanDirectory(f.FullName, node);
                    }

                }
                catch (Exception ex)
                {
                    folderTreeView.Nodes.Add(f.Name + " (" + ex.Message + ")");
                }
            }
        }

        public string GetFileSize(double len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public static long DirectorySize(DirectoryInfo directory)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = directory.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = directory.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirectorySize(di);
            }
            return size;
        }
    }
}
