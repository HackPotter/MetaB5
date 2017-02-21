using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetReferenceGenerator
{
    [MenuItem("Metablast/Utility/Generate Asset Reference File")]
    public static void OpenWindow()
    {
        //StreamWriter streamWriter = new StreamWriter(File.Create("Assets/Scripts/AssetReferenceList.cs"));
        //GenerateFile(streamWriter);
        //Debug.Log("Finished!");
    }

    private static void GenerateFile(StreamWriter writer)
    {
        // Creates a URI of the final path
        Uri root = new Uri(Application.dataPath + "\\");
        
        List<string> allFiles = new List<string>();


        foreach (string directory in Directory.GetDirectories(Application.dataPath, "*Resources*", SearchOption.AllDirectories))
        {
            foreach (string filename in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                // Relativizes filenames
                Uri relativeFilename = root.MakeRelativeUri(new Uri(filename));
                string filenameNoExt = Path.GetFileNameWithoutExtension(relativeFilename.ToString());
                string containingDirectory = Path.GetDirectoryName(relativeFilename.ToString());

                if (IsBlackListed(relativeFilename.ToString()))
                {
                    continue;
                }

                allFiles.Add(containingDirectory + "/" + filenameNoExt);
            }
        }

        Tree tree = new Tree("Assets");

        for (int i = 0; i < allFiles.Count; i++)
        {
            string[] split = allFiles[i].Split('/');
            tree.AddLeaf(split);
        }

        PrintTreeRec(tree.root, "", writer);
        writer.Close();
    }

    private static bool IsBlackListed(string filename)
    {
        if (filename.EndsWith(".cs"))
        {
            return true;
        }
        else if (filename.EndsWith(".jar"))
        {
            return true;
        }
        else if (filename.EndsWith(".meta"))
        {
            return true;
        }
        return false;
    }

    private static void PrintTreeRec(Node node, string prepend, StreamWriter streamWriter)
    {
        if (node.Children.Count == 0)
        {
            streamWriter.WriteLine(prepend + "public const string " + node.Name + " = " + "\"" + node.AbsoluteName + "\";");
            streamWriter.WriteLine("");
            return;
        }

        streamWriter.WriteLine(prepend + "public static class " + node.Name);
        streamWriter.WriteLine(prepend + "{");
        foreach (Node child in node.Children)
        {
            PrintTreeRec(child, "\t" + prepend, streamWriter);
        }
        streamWriter.WriteLine(prepend + "}");
    }

    private class Tree
    {
        public Node root;

        public Tree(string rootName)
        {
            root = new Node(rootName);
        }

        public void AddLeaf(string[] path)
        {
            Node currentNode = root;
            foreach (string s in path)
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                string cleanedName = s.Replace("-", "__");
                cleanedName = cleanedName.Replace(".", "_");
                cleanedName = cleanedName.Replace("%20", "");
                cleanedName = cleanedName.Replace(" ", "");

                Node n = currentNode.ChildWithName(cleanedName);
                if (n == null)
                {
                    Node newNode = new Node(cleanedName);
                    newNode._parent = currentNode;
                    currentNode.Children.Add(newNode);
                    currentNode = newNode;
                }
                else
                {
                    currentNode = n;
                }
            }
        }
    }

    private class Node
    {
        private string _name;
        public Node _parent;
        private List<Node> _children = new List<Node>();

        public string Name
        {
            get { return _name; }
        }

        public string AbsoluteName
        {
            get
            {
                if (_parent == null)
                {
                    return "";
                }
                if (_parent.AbsoluteName == "" || _parent.AbsoluteName == "Resources")
                {
                    return _name;
                }
                return _parent.AbsoluteName + "/" + _name;
            }
        }

        public List<Node> Children
        {
            get { return _children; }
        }

        public Node ChildWithName(string nodeName)
        {
            foreach (Node n in Children)
            {
                if (n._name == nodeName)
                {
                    return n;
                }
            }
            return null;
        }

        public Node(string name)
        {
            _name = name;
        }
    }
}
