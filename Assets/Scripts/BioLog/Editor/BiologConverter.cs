//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BiologConverter
//{
//    private XMLReader parser = new XMLReader();
//    private XMLNode node;

//    public List<LogEntry> entries = new List<LogEntry>();

//    public void load()
//    {
//        string pathToFile = "Data/Logbook";

//        TextAsset biologText = (TextAsset)Resources.Load(pathToFile, typeof(TextAsset));
//        Asserter.NotNull(biologText);

//        node = parser.read(biologText.text);
//        entries = new List<LogEntry>();

//        foreach (XMLNode entry in node.children)
//        {
//            walk(entry.tagName, entry);
//        }
//        sortLog();
//    }

//    private void sortLog()
//    {
//        bool sorted = false;
//        LogEntry curEntry;
//        LogEntry nextEntry;
//        LogEntry tempEntry;

//        while (!sorted)
//        {
//            sorted = true;
//            for (int i = 0; i < (entries.Count - 1); i++)
//            {
//                curEntry = entries[i];
//                nextEntry = entries[(i + 1)];
//                if (System.Convert.ToChar(curEntry.getName().Substring(0, 1)) > System.Convert.ToChar(nextEntry.getName().Substring(0, 1)))
//                {
//                    tempEntry = curEntry;
//                    entries[i] = nextEntry;
//                    entries[i + 1] = tempEntry;
//                    sorted = false;
//                }
//            }
//        }
//    }

//    private void walk(string curGroup, XMLNode curNode)
//    {
//        LogEntry newEntry = new LogEntry(curNode.tagName);
//        newEntry.setType(curGroup);

//        foreach (DictionaryEntry p in curNode.attributes)
//        {
//            bool temp = false;
//            if ("true".Equals(p.Value))
//            {
//                temp = true;
//            }
//            switch (p.Key as string)
//            {
//                case "user-defined":
//                    newEntry.userDefined = temp;
//                    break;
//                case "show-information":
//                    newEntry.show = temp;
//                    break;
//                case "show-video":
//                    newEntry.showVideo = temp;
//                    break;
//            }
//        }

//        foreach (XMLNode detail in curNode.children)
//        {
//            switch (detail.tagName)
//            {
//                case "Description":
//                    if (detail.value != null)
//                        newEntry.setDescription(detail.value);
//                    break;
//                case "Videos":
//                    foreach (XMLNode videoType in detail.children)
//                    {
//                        switch (videoType.tagName)
//                        {
//                            case "Atom":
//                                foreach (XMLNode specs in videoType.children)
//                                {
//                                    if (specs.tagName == "Path")
//                                        newEntry.setAtomPath(specs.value);
//                                    else if (specs.tagName == "Type")
//                                        newEntry.setAtomType(specs.value);
//                                }
//                                break;
//                            case "Mesh":
//                                foreach (XMLNode specs in videoType.children)
//                                {
//                                    if (specs.tagName == "Path")
//                                        newEntry.setMeshPath(specs.value);
//                                    else if (specs.tagName == "Type")
//                                        newEntry.setMeshType(specs.value);
//                                }
//                                break;
//                            case "Ribbon":
//                                foreach (XMLNode specs in videoType.children)
//                                {
//                                    if (specs.tagName == "Path")
//                                        newEntry.setRibbonPath(specs.value);
//                                    else if (specs.tagName == "Type")
//                                        newEntry.setRibbonType(specs.value);
//                                }
//                                break;
//                            case "Single":
//                                foreach (XMLNode specs in videoType.children)
//                                {
//                                    if (specs.tagName == "Path")
//                                        newEntry.setSinglePath(specs.value);
//                                    else if (specs.tagName == "Type")
//                                        newEntry.setSingleType(specs.value);
//                                }
//                                break;
//                        }
//                    }
//                    break;
//                case "Children":
//                    foreach (XMLNode child in detail.children)
//                    {
//                        walk(curGroup, child);
//                    }
//                    break;
//            }
//        }

//        entries.Add(newEntry);
//    }

//    public class LogEntry
//    {
//        private string name = "";
//        private string formattedName = "";
//        private string scrambledName = "";
//        private string description;
//        public bool userDefined;
//        public bool show;
//        public bool showVideo;

//        private string atomPath;
//        private string atomType;
//        private string meshPath;
//        private string meshType;
//        private string ribbonPath;
//        private string ribbonType;

//        private Texture2D meshTexture;
//        private Texture2D atomTexture;
//        private Texture2D ribbonTexture;
//        private MovieTexture meshMovieTexture;
//        private MovieTexture atomMovieTexture;
//        private MovieTexture ribbonMovieTexture;

//        private string singlePath;
//        private string singleType;
//        private Texture2D singleTexture;

//        private string type;
//        public bool selected;

//        public LogEntry()
//        {
//            this.name = "";
//            this.selected = false;
//            this.description = "";
//            this.atomPath = "";
//            this.atomType = "";
//            this.meshPath = "";
//            this.meshType = "";
//            this.ribbonPath = "";
//            this.ribbonType = "";
//            this.singlePath = "";
//            this.singleType = "";
//            this.show = true;
//            this.userDefined = true;
//            this.showVideo = true;
//            formatName();
//            this.scrambledName = scrambleText(this.formattedName);
//        }

//        public LogEntry(string name)
//        {
//            this.name = name;
//            this.selected = false;
//            this.description = "";
//            this.atomPath = "";
//            this.atomType = "";
//            this.meshPath = "";
//            this.meshType = "";
//            this.ribbonPath = "";
//            this.ribbonType = "";
//            this.singlePath = "";
//            this.singleType = "";
//            this.show = true;
//            this.userDefined = true;
//            this.showVideo = true;
//            formatName();
//            this.scrambledName = scrambleText(this.formattedName);
//        }

//        public void setName(string newName)
//        {
//            this.name = newName;
//            formatName();
//        }

//        public void setDescription(string newDesc)
//        {
//            this.description = newDesc;
//        }

//        public void setType(string newType)
//        {
//            this.type = newType;
//        }

//        public void setMeshPath(string Path)
//        {
//            this.meshPath = PathWithoutExtension(Path);
//        }

//        public void setAtomPath(string Path)
//        {
//            this.atomPath = PathWithoutExtension(Path);
//        }

//        public void setRibbonPath(string Path)
//        {
//            this.ribbonPath = PathWithoutExtension(Path);
//        }

//        public void setMeshType(string Type)
//        {
//            this.meshType = Type;
//        }

//        public void setAtomType(string Type)
//        {
//            this.atomType = Type;
//        }

//        public void setRibbonType(string Type)
//        {
//            this.ribbonType = Type;
//        }

//        public void setSinglePath(string Path)
//        {
//            this.singlePath = PathWithoutExtension(Path);
//        }

//        public void setSingleType(string type)
//        {
//            this.singleType = type;
//        }

//        private string PathWithoutExtension(string path)
//        {
//            int index = path.LastIndexOf('.');
//            if (index < 0)
//            {
//                //Debug.Log("Has no extension: " + path);
//                return path;
//            }
//            return path.Substring(0, index);
//        }

//        public void grabMedia()
//        {
//            string pathToFile;
//            string url;
//            WWW www;

//            if (!this.showVideo)
//            {
//                if (this.singleType == "image")
//                {
//                    this.singleTexture = ResourcesExt.Load<Texture2D>("Data/LogMedia/images/" + this.singlePath);
//                }
//            }
//            else
//            {
//                if (this.atomType == "image")
//                {
//                    this.atomTexture = ResourcesExt.Load<Texture2D>("Data/LogMedia/images/" + this.atomPath);
//                }
//                else if (this.atomType == "movie")
//                {
//                    this.atomMovieTexture = ResourcesExt.Load<MovieTexture>("Data/LogMedia/movies/" + this.atomPath);
//                }

//                if (this.meshType == "image")
//                {
//                    this.meshTexture = ResourcesExt.Load<Texture2D>("Data/LogMedia/images/" + this.meshPath);
//                }
//                else if (this.meshType == "movie")
//                {
//                    this.meshMovieTexture = ResourcesExt.Load<MovieTexture>("Data/LogMedia/movies/" + this.meshPath);
//                }

//                if (this.ribbonType == "image")
//                {
//                    this.ribbonTexture = ResourcesExt.Load<Texture2D>("Data/LogMedia/images/" + this.ribbonPath);
//                }
//                else if (this.ribbonType == "movie")
//                {
//                    this.ribbonMovieTexture = ResourcesExt.Load<MovieTexture>("Data/LogMedia/movies/" + this.ribbonPath);
//                }
//            }
//        }

//        private Texture2D loadTexture(string internalPath)
//        {
//            if (string.IsNullOrEmpty(internalPath))
//            {
//                return null;
//            }
//            return ResourcesExt.Load<Texture2D>("Data/LogMedia/images/" + internalPath);
//        }

//        private MovieTexture loadMovie(string internalPath)
//        {
//            if (string.IsNullOrEmpty(internalPath))
//            {
//                return null;
//            }
//            return ResourcesExt.Load<MovieTexture>("Data/LogMedia/movies/" + internalPath);
//        }

//        public Texture2D getSingleTexture()
//        {
//            //		return this.singleTexture;
//            return loadTexture(singlePath);
//        }

//        public Texture2D getAtomTexture()
//        {
//            //		return this.atomTexture;
//            return loadTexture(atomPath);
//        }

//        public Texture2D getMeshTexture()
//        {
//            //		return this.meshTexture;
//            return loadTexture(meshPath);
//        }

//        public Texture2D getRibbonTexture()
//        {
//            //		return this.ribbonTexture;
//            return loadTexture(ribbonPath);
//        }

//        public MovieTexture getSingleMovieTexture()
//        {
//            return this.singleMovieTexture;
//        }

//        public string getName()
//        {
//            return this.name;
//        }

//        public string getType()
//        {
//            return this.type;
//        }

//        public string getDescription()
//        {
//            return this.description;
//        }

//        public string getMeshPath()
//        {
//            return this.meshPath;
//        }

//        public string getAtomPath()
//        {
//            return this.atomPath;
//        }

//        public string getRibbonPath()
//        {
//            return this.ribbonPath;
//        }

//        public string getSinglePath()
//        {
//            return this.singlePath;
//        }

//        public string getSingleType()
//        {
//            return this.singleType;
//        }

//        public string getAtomType()
//        {
//            return this.atomType;
//        }

//        public string getMeshType()
//        {
//            return this.meshType;
//        }

//        public string getRibbonType()
//        {
//            return this.ribbonType;
//        }

//        public string getFormattedName()
//        {
//            return this.formattedName;
//        }

//        public string getScrambledName()
//        {
//            return this.scrambledName;
//        }

//        private void formatName()
//        {
//            formattedName = name.Replace("_", " ");
//        }

//        private string scrambleText(string strText)
//        {
//            if (strText.Length > 0)
//            {
//                char[] input = strText.ToCharArray();
//                int length = input.Length;
//                string returnString = "";
//                for (int i = 0; i < length; i++)
//                {
//                    int randomChar = UnityEngine.Random.Range(0, input.Length);
//                    returnString += input[randomChar];
//                    List<char> arrInput = new List<char>(input);
//                    arrInput.RemoveAt(randomChar);
//                    input = arrInput.ToArray();
//                }
//                return returnString;
//            }
//            else
//            {
//                return strText;
//            }
//        }
//    }
//}

