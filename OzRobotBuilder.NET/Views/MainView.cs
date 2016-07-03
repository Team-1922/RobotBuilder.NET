﻿using CommonLib;
using CommonLib.Model.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OzRobotBuilder.NET.Views
{
    public partial class MainView : Form
    {
        //credit: http://stackoverflow.com/questions/18769634/creating-tree-view-dynamically-according-to-json-text-in-winforms
        public static TreeNode Json2Tree(string text)
        {
            return Json2Tree(JObject.Parse(text));
        }
        public static TreeNode Json2Tree(JObject obj)
        {
            //create the parent node
            TreeNode parent = new TreeNode();
            //loop through the obj. all token should be pair<key, value>
            foreach (var token in obj)
            {
                //change the display Content of the parent
                parent.Text = token.Key.ToString();
                //create the child node
                TreeNode child = new TreeNode();
                child.Text = token.Key.ToString();
                //check if the value is of type obj recall the method
                if (token.Value.Type.ToString() == "Object")
                {
                    // child.Text = token.Key.ToString();
                    //create a new JObject using the the Token.value
                    JObject o = (JObject)token.Value;
                    //recall the method
                    child = Json2Tree(o);
                    //add the child to the parentNode
                    parent.Nodes.Add(child);
                }
                //if type is of array
                else if (token.Value.Type.ToString() == "Array")
                {
                    int ix = -1;
                    //  child.Text = token.Key.ToString();
                    //loop though the array
                    foreach (var itm in token.Value)
                    {
                        //check if value is an Array of objects
                        if (itm.Type.ToString() == "Object")
                        {
                            TreeNode objTN = new TreeNode();
                            //child.Text = token.Key.ToString();
                            //call back the method
                            ix++;

                            JObject o = (JObject)itm;
                            objTN = Json2Tree(o);
                            objTN.Text = token.Key.ToString() + "[" + ix + "]";
                            child.Nodes.Add(objTN);
                            //parent.Nodes.Add(child);
                        }
                        //regular array string, int, etc
                        else if (itm.Type.ToString() == "Array")
                        {
                            ix++;
                            TreeNode dataArray = new TreeNode();
                            foreach (var data in itm)
                            {
                                dataArray.Text = token.Key.ToString() + "[" + ix + "]";
                                dataArray.Nodes.Add(data.ToString());
                            }
                            child.Nodes.Add(dataArray);
                        }

                        else
                        {
                            child.Nodes.Add(itm.ToString());
                        }
                    }
                    parent.Nodes.Add(child);
                }
                else
                {
                    //if token.Value is not nested
                    // child.Text = token.Key.ToString();
                    //change the value into N/A if value == null or an empty string 
                    if (token.Value.ToString() == "")
                        child.Nodes.Add("N/A");
                    else
                        child.Nodes.Add(token.Value.ToString());
                    parent.Nodes.Add(child);
                }
            }
            return parent;

        }

        public MainView()
        {
            InitializeComponent();
        }

        private void MainView_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Updates the Tree View with the newly opened Document
        /// </summary>
        private void RecreateTreeView()
        {
            treeView1.Nodes.Clear();

            //get the tree from the document
            var doc = Program.DocManager.OpenDoc as RobotDocument;
            if (null == doc)
                return;//we have an issue

            var tree = doc.GetTree();

            RecreateTreeViewRecursive(tree, treeView1.Nodes.Add(tree.Key));

            //finally refresh the grid view (this usually just clears it)
            RefreshGridView(tree);

            //expand the tree view
            treeView1.ExpandAll();
        }

        internal void RecreateTreeViewRecursive(DataTreeNode node, TreeNode viewNode)
        {
            if (null == node)
                return;
            
            foreach(var child in node)
            {
                RecreateTreeViewRecursive(child, viewNode.Nodes.Add(child.Key));
            }
        }

        [Obsolete]
        public object GetActiveTreeVieObject()
        {
            //validate the document type
            if (!(Program.DocManager.OpenDoc is CommonLib.Model.Documents.RobotDocument))
                return null;

            //get the active tree view node
            var node = treeView1.SelectedNode;

            //TODO: eventually will these nodes have some data associated with them?
            if (node.FullPath == "Subsystems" || node.FullPath == "Commands" || node.FullPath == "OperatorInterface")
                return null;

            //get the top most node to figure out where to get the data from
            var topParentNode = node;
            var tmpParent = topParentNode.Parent;
            while(tmpParent != null)
            {
                topParentNode = tmpParent;
                tmpParent = tmpParent.Parent;
            }

            //i wish this was a bit more generic
            if(topParentNode.Name == "Subsystem")
            {
                foreach(var subsystem in (Program.DocManager.OpenDoc as RobotDocument).Subsystems)
                {
                    if (subsystem.Name == topParentNode.Name)
                        return subsystem;
                }
            }
            /*if (topParentNode.Name == "Commands")
            {
                foreach (var command in (Program.DocManager.OpenDoc as RobotDocument).Commands)
                {
                    if (command.Name == topParentNode.Name)
                        return command;
                }
            }*/
            /*if (topParentNode.Name == "OperatorInterface")
            {
                foreach (var subsystem in (Program.DocManager.OpenDoc as RobotDocument).Subsystems)
                {
                    if (subsystem.Name == topParentNode.Name)
                        return subsystem;
                }
            }*/



            return null;
        }
        public DataTreeNode GetActiveTreeViewNode()
        {
            var path = treeView1.SelectedNode.FullPath;
            var openDoc = Program.DocManager.OpenDoc as RobotDocument;
            var tree = openDoc.GetTree();
            return tree.GetNodeFromPath(path);
        }


        /// <summary>
        /// This updates the grid view with the selected tree view item
        /// </summary>
        public void RefreshGridView()
        {
            RefreshGridView(GetActiveTreeViewNode());
        }
        public void RefreshGridView(DataTreeNode activeNode)
        {
            //this might not be right
            dataGridView1.Rows.Clear();

            //add all of the children to the grid view but DO NOT recurse
            foreach(var node in activeNode)
            {
                dataGridView1.Rows.Add(node.Key, node.Data);
            }

            Invalidate();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Program.Controller.UpdateKey(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value as string, dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string);
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //get the "name" part
            var name = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

            //is the "name" a subnode?
            var activeNode = GetActiveTreeViewNode();
            var child = activeNode.GetChild(name);
            if (null == child)
                return;//do nothing, we can't go deeper

            //find the child node
            var firstChild = treeView1.SelectedNode.FirstNode;
            while (null != firstChild)
            {
                if(firstChild.Text == name)
                {
                    treeView1.SelectedNode = firstChild;
                }
                firstChild = firstChild.NextNode;
            }

            RefreshGridView();
        }

        private void FileOpen(object sender, EventArgs e)
        {
            //pop open dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
            openFileDialog.Filter = "Robot Files (*.robot.json)|*.robot.json";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Program.DocManager.OpenDocument(openFileDialog.FileName);
            }


            //finally refresh the grid view and recreate the tree view
            RecreateTreeView();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //refresh the selection
            RefreshGridView();
        }
    }
}
