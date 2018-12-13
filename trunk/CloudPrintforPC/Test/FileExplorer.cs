using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Microsoft.VisualBasic.FileIO;
using CloudPrintforPC;

namespace FileExplorer_TreeView_Cui
{
    /* Class  :FileExplorer
     * Author : Chandana Subasinghe
     * Date   : 10/03/2006
     * Discription : This class use to create the tree view and load 
     *               directories and files in to the tree
     *          
     */
    class FileExplorer
    {
        ImageList mImageList;
        public FileExplorer()
        {
            mImageList = new ImageList();
            this.mImageList.ImageSize = new System.Drawing.Size(24, 24);
            this.mImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
        }

        /* Method :CreateTree
         * Author : Chandana Subasinghe
         * Date   : 10/03/2006
         * Discription : This is use to creat and build the tree
         *          
         */

        public bool CreateTree(TreeView treeView)
        {
            bool returnValue = false;

            this.InitImageList(treeView);

            try
            {
                // Create Desktop
                TreeNodeFile desktop = new TreeNodeFile();
                desktop.Text = "Desktop";
                desktop.Tag = "Desktop";
                desktop.Nodes.Add("");
                this.AddIcon(desktop);
                treeView.Nodes.Add(desktop);
                // Get driveInfo
            
                foreach (DriveInfo drv in DriveInfo.GetDrives())
                {
                    
                    TreeNodeFile fChild = new TreeNodeFile();
                    if (drv.DriveType == DriveType.CDRom)
                    {
                        fChild.ImageIndex = 1;
                        fChild.SelectedImageIndex = 1;
                        fChild.Text = drv.Name;
                        fChild.mPath = drv.Name;
                        this.AddIcon(fChild);
                    }
                    else if (drv.DriveType == DriveType.Fixed)
                    {
                        fChild.ImageIndex = 0;
                        fChild.SelectedImageIndex = 0;
                        fChild.Text = drv.Name;
                        fChild.mPath = drv.Name;
                        this.AddIcon(fChild);
                    }
                   
                    fChild.Nodes.Add("");
                    treeView.Nodes.Add(fChild);
                    returnValue = true;
                   
                }

            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;
            
        }
        public bool CreateTreeCui(TreeView treeView)
        {
            bool returnValue = false;

            this.InitImageList(treeView);

            try
            {
                // Create Desktop
                TreeNodeFile desktop = new TreeNodeFile();
                desktop.Text = "Desktop";
                desktop.Tag = "Desktop";
                desktop.Nodes.Add("");
                treeView.Nodes.Add(desktop);
                this.AddIcon(desktop);               
                desktop.ImageKey = this.GetKey(desktop);
                desktop.SelectedImageKey = this.GetKey(desktop);
                // Get driveInfo

                foreach (DriveInfo drv in DriveInfo.GetDrives())
                {

                    TreeNodeFile fChild = new TreeNodeFile();
                    if (drv.DriveType == DriveType.CDRom)
                    {
                        fChild.Text = drv.Name;
                        fChild.mPath = drv.Name;
                        fChild.Nodes.Add("");
                        treeView.Nodes.Add(fChild);
                       
                        this.AddIcon(fChild);                       
                        fChild.ImageKey = this.GetKey(fChild);
                        fChild.SelectedImageKey = this.GetKey(fChild);
                    }
                    else if (drv.DriveType == DriveType.Fixed)
                    {
                        fChild.Text = drv.Name;
                        fChild.mPath = drv.Name;
                        fChild.Nodes.Add("");
                        treeView.Nodes.Add(fChild);
                        this.AddIcon(fChild);
                        fChild.ImageKey = this.GetKey(fChild);
                        fChild.SelectedImageKey = this.GetKey(fChild);           
                    }

                  
                   
                    returnValue = true;

                }

            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;

        }
        /* Method :EnumerateDirectory
         * Author : Chandana Subasinghe
         * Date   : 10/03/2006
         * Discription : This is use to Enumerate directories and files
         *          
         */
        public TreeNodeFile EnumerateDirectoryCui(TreeNodeFile parentNode)
        {

            try
            {
                DirectoryInfo rootDir= new DirectoryInfo(parentNode.mPath);
                
                
                parentNode.Nodes[0].Remove();
                foreach (DirectoryInfo dir in rootDir.GetDirectories())
                {

                    TreeNodeFile node = new TreeNodeFile();
                    node.Text = dir.Name;
                    node.mPath = dir.FullName;
                    node.Nodes.Add("");
                    parentNode.Nodes.Add(node);
                    this.AddIcon(node);
                    node.ImageKey = this.GetKey(node);
                    node.SelectedImageKey = this.GetKey(node);
                }
                //Fill files
                foreach (FileInfo file in rootDir.GetFiles())
                {
                    TreeNodeFile node = new TreeNodeFile();
                    node.Text = file.Name;
                    node.mPath = file.FullName;
                    //node.ImageIndex = 2;
                    //node.SelectedImageIndex = 2;
                    parentNode.Nodes.Add(node);
                    this.AddIcon(node);
                    node.ImageKey = this.GetKey(node);
                    node.SelectedImageKey = this.GetKey(node);
                }



            }

            catch (Exception ex)
            {
                //TODO : 
            }

            return parentNode;
        }
        public TreeNodeFile EnumerateDirectory(TreeNodeFile parentNode)
        {
          
            try
            {
                DirectoryInfo rootDir;

                // To fill Desktop
                Char [] arr={'\\'};
                string [] nameList=parentNode.FullPath.Split(arr);
                string path = "";

                if (nameList.GetValue(0).ToString() == "Desktop")
                {
                    path = SpecialDirectories.Desktop+"\\";

                    for (int i = 1; i < nameList.Length; i++)
                    {
                        path = path + nameList[i] + "\\";
                    }

                    rootDir = new DirectoryInfo(path);
                }
             // for other Directories
                else
                {
                   
                    rootDir = new DirectoryInfo(parentNode.FullPath + "\\");
                }
                
                parentNode.Nodes[0].Remove();
                foreach (DirectoryInfo dir in rootDir.GetDirectories())
                {
                    
                    TreeNodeFile node = new TreeNodeFile();
                    node.Text = dir.Name;
                    node.Nodes.Add("");
                    parentNode.Nodes.Add(node);
                    this.AddIcon(node);
                }
                //Fill files
                foreach (FileInfo file in rootDir.GetFiles())
                {
                    TreeNodeFile node = new TreeNodeFile();
                    node.Text = file.Name;
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                    parentNode.Nodes.Add(node);
                    this.AddIcon(node);
                }



            }

            catch (Exception ex)
            {
                //TODO : 
            }
           
            return parentNode;
        }
        public DirectoryInfo GetTreeNodeFileDirectory(TreeNodeFile parentNode)
        {
            DirectoryInfo rootDir=null;
            try
            {
                // To fill Desktop
                Char[] arr = { '\\' };
                string[] nameList = parentNode.FullPath.Split(arr);
                string path = "";

                if (nameList.GetValue(0).ToString() == "Desktop")
                {
                    path = SpecialDirectories.Desktop + "\\";

                    for (int i = 1; i < nameList.Length; i++)
                    {
                        path = path + nameList[i] + "\\";
                    }

                   return  rootDir = new DirectoryInfo(path);
                }
                // for other Directories
                else
                {

                  return  rootDir = new DirectoryInfo(parentNode.FullPath + "\\");
                }           



            }catch (Exception ex){
                //TODO : 
            }

            return null;
        
        }
        public static string GetTreeNodeFileDirectoryPath(TreeNodeFile parentNode)
        {

            if (parentNode.mPath != null) 
                return null;
            try
            {
                // To fill Desktop
                Char[] arr = { '\\' };
                string[] nameList = parentNode.FullPath.Split(arr);
                string path = "";

                if (nameList.GetValue(0).ToString() == "Desktop")
                {
                    path = SpecialDirectories.Desktop + "\\";

                    for (int i = 1; i < nameList.Length; i++)
                    {
                        path = path + nameList[i] + "\\";
                    }
                    parentNode.mPath = path;
                    return path;
                }
                // for other Directories
                else
                {
#if false
                    path = parentNode.FullPath;
                    return path;
#else
                    path = parentNode.mPath;
                    return path;
#endif

                }



            }
            catch (Exception ex)
            {
                //TODO : 
                
            }

            return null;

        }
        public void InitImageList(TreeView treeView)
        {
            treeView.ImageList = this.mImageList;
            this.mImageList.Images.Add("default", new Bitmap(16, 16));
        }
        public void AddIcon(TreeNodeFile node) {
            GetTreeNodeFileDirectoryPath(node);
            GetSysicon.GetSysIcon(node.mPath, this.mImageList);

        }
        public String GetKey(TreeNodeFile node)
        {

            if (node.mPath != null)
            {
                return node.mPath;
            }
            else {

                String fileStr = GetTreeNodeFileDirectoryPath(node);
                return GetSysicon.GetSysKey(fileStr);
            }       
  
        }
        public ImageList GetImageList() 
        {
          return this.mImageList;
        }
    }
}
