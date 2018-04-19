////////////////////////////////////////////////////////
/// File: ViewerNode.cs
/// Author: Daniel Probert
/// Date: 27-07-2008
/// Version: 1.0
////////////////////////////////////////////////////////
namespace DanSharp.XmlViewer
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    #endregion

    /// <summary>
    /// Represents an Xml node being displayed in the viewer
    /// </summary>
    public class ViewerNode
    {
        #region Private Instance Fields

        /// <summary>
        /// Stores the list of child nodes under this node
        /// </summary>
        private List<ViewerNode> _childNodes = new List<ViewerNode>();

        /// <summary>
        /// Stores the list of attributes for this node
        /// </summary>
        private List<ViewerNode> _attributes = new List<ViewerNode>();

        /// <summary>
        /// Stores the parent of this node
        /// </summary>
        private ViewerNode _parent = null;

        /// <summary>
        /// Stores the name of this node
        /// </summary>
        private string _name = null;

        /// <summary>
        /// Stores the local name of this node
        /// </summary>
        private string _localName = null;

        /// <summary>
        /// Stores the namespace for this node
        /// </summary>
        private string _namespace = null;

        /// <summary>
        /// Stores the number of normal attributes
        /// </summary>
        private int _normalAttributeCount = 0;

        /// <summary>
        /// Stores the value of this node
        /// </summary>
        private string _value = null;

        /// <summary>
        /// Stores the type of the node
        /// </summary>
        private NodeType _type = NodeType.Unknown;

        /// <summary>
        /// Stores the attribute type of the node
        /// </summary>
        private AttributeType _attrType = AttributeType.None;

        /// <summary>
        ///  Stores the XmlNode that this node is based on
        /// </summary>
        private XmlNode _originalNode = null;

        /// <summary>
        /// Stores the XPath statement for this node
        /// </summary>
        private string _xpath = null;

        /// <summary>
        ///  Stores the name of the node type
        /// </summary>
        private string _typeName = null;

        /// <summary>
        ///  Stores the node path
        /// </summary>
        private string _nodePath = null;

        /// <summary>
        /// Stores the node path without any occurrence indicators
        /// </summary>
        private string _nonRecurringNodePath = null;

        /// <summary>
        ///  Stores the index of this node in a repeating list
        /// </summary>
        private int _occurrenceIndex = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of a ViewerNode from the given root XmlNode
        /// </summary>
        /// <param name="rootNode">Root XmlNode to create this instance from</param>
        public ViewerNode(XmlNode rootNode)
            : this(rootNode, null)
        {
        }

        /// <summary>
        /// Creates an instance of a ViewerNode from the given XmlNode
        /// </summary>
        /// <param name="originalNode">Node to create ViewerNode from</param>
        /// <param name="parent">Parent node of this node</param>
        public ViewerNode(XmlNode originalNode, ViewerNode parent) : this(originalNode, parent, -1)
        {
        }

        /// <summary>
        /// Creates an instance of a ViewerNode from the given occurrence of an XmlNode in a list
        /// </summary>
        /// <param name="originalNode">Node to create ViewerNode from</param>
        /// <param name="parent">Parent node of this node</param>
        /// <param name="occurrenceIndex">Occurrence in the list (1 based)</param>
        public ViewerNode(XmlNode originalNode, ViewerNode parent, int occurrenceIndex)
        {
            _originalNode = originalNode;
            _parent = parent;
            _occurrenceIndex = occurrenceIndex;
            Build();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Builds the ViewerNode
        /// </summary>
        private void Build()
        {
            _name = _originalNode.Name;
            _localName = _originalNode.LocalName;
            _namespace = _originalNode.NamespaceURI;
            _value = _originalNode.Value;

            // Check the type of node
            if (_originalNode is XmlElement)
            {
                // Node is an element
                _type = NodeType.Element;
            }
            else if (_originalNode is XmlAttribute)
            {
                // Node is an attribute
                _type = NodeType.Attribute;

                // Check if this is a Type attribute
                if (_originalNode.Name.EndsWith(":type"))
                {
                    _attrType = AttributeType.Type;
                    // Get the actual type name
                    if (!string.IsNullOrEmpty(_originalNode.Value))
                    {
                        string value = _originalNode.Value;
                        string typeName = null;
                        if (value.IndexOf(":") > -1)
                        {
                            typeName = value.Substring(value.IndexOf(":") + 1);
                        }
                        else
                        {
                            typeName = value;
                        }
                        // Set the type name of the parent node
                        if (_parent != null)
                        {
                            _parent.TypeName = typeName;
                        }
                    }
                }

                // Check if this is an xmlns attribute
                if (_originalNode.Name.StartsWith("xmlns"))
                {
                    _attrType = AttributeType.Xmlns;
                }

                // If this is a "normal" attribute
                // (i.e. atribute type is "None")
                // then increment the count of
                // normal attributes for the parent
                if (_attrType == AttributeType.None)
                {
                    _parent.NormalAttributeCount++;
                }
            }

            // Add attributes
            if ((_originalNode.Attributes != null) && (_originalNode.Attributes.Count > 0))
            {
                for (int attrIndex = 0; attrIndex < _originalNode.Attributes.Count; attrIndex++)
                {
                    _attributes.Add(new ViewerNode(_originalNode.Attributes[attrIndex], this));
                }
            }

            // Add child nodes
            if ((_originalNode.ChildNodes != null) && (_originalNode.ChildNodes.Count > 0))
            {
                for (int childIndex = 0; childIndex < _originalNode.ChildNodes.Count; childIndex++)
                {
                    // Check if this is a text node
                    if (_originalNode.ChildNodes[childIndex].Name == "#text")
                    {
                        _value = _originalNode.ChildNodes[childIndex].Value;
                    }
                    else
                    {
                        // Check if this child node repeats, and if so, get the index of this instance
                        int occurrenceIndex = GetChildNodeRepeatingIndex(_originalNode.ChildNodes[childIndex], _originalNode.ChildNodes);
                        _childNodes.Add(new ViewerNode(_originalNode.ChildNodes[childIndex], this, occurrenceIndex));
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the given node repeats in the given list and, if so, gets
        /// the index of this occurrence
        /// </summary>
        /// <param name="node">Node to check</param>
        /// <param name="list">List of nodes</param>
        private int GetChildNodeRepeatingIndex(XmlNode node, XmlNodeList list)
        {
            int nodeCount = 0;
            int nodePosition = 0;
            bool foundNode = false;

            // Loop through the nodes in the list
            for (int listIndex = 0; listIndex < list.Count; listIndex++)
            {
                // Check if this is a matching node
                if (string.Compare(list[listIndex].LocalName, node.LocalName, true) == 0)
                {
                    nodeCount += 1;
                    // Check if this is the same node
                    if ((list[listIndex] != node) && (!foundNode))
                    {
                        nodePosition += 1;
                    }
                    else
                    {
                        foundNode = true;
                    }

                    // Check if we should exit the loop
                    if ((nodeCount > 1) && (foundNode))
                    {
                        break;
                    }
                }
            }

            // Check if we found the node
            if (nodeCount > 1)
            {
                return nodePosition;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Builds the XPath for this node
        /// </summary>
        private void BuildXPath()
        {
            StringBuilder builder = new StringBuilder();
            if (_parent != null)
            {
                builder.Append(_parent.XPath);
            }
            builder.Append(NodePath);
            _xpath = builder.ToString();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the occurrence index for this node (i.e. where it appears in a list of nodes with the same name/namespace)
        /// </summary>
        public int OccurrenceIndex
        {
            get
            {
                return _occurrenceIndex;
            }
        }

        /// <summary>
        /// Gets or sets the count of normal attributes (i.e. not xmlns or other special attributes)
        /// </summary>
        public int NormalAttributeCount
        {
            get
            {
                return _normalAttributeCount;
            }
            set
            {
                _normalAttributeCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the type for this node
        /// </summary>
        public string TypeName
        {
            get
            {
                return _typeName;
            }
            set
            {
                _typeName = value;
            }
        }

        /// <summary>
        /// Gets the type of attribute
        /// </summary>
        public AttributeType AttributeType
        {
            get
            {
                return _attrType;
            }
        }

        /// <summary>
        ///  Gets the type of node
        /// </summary>
        public NodeType NodeType
        {
            get
            {
                return _type;
            }
        }

        /// <summary>
        /// Gets the local name of the node
        /// </summary>
        public string LocalName
        {
            get
            {
                return _localName;
            }
        }

        /// <summary>
        /// Gets the name of the node
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets the namespace for the node
        /// </summary>
        public string Namespace
        {
            get
            {
                return _namespace;
            }
        }

        /// <summary>
        /// Gets the value for the node
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// Gets the xpath for this node
        /// </summary>
        public string XPath
        {
            get
            {
                if (_xpath == null)
                {
                    BuildXPath();
                }
                return _xpath;
            }
        }

        /// <summary>
        /// Gets the list of child nodes
        /// </summary>
        public List<ViewerNode> ChildNodes
        {
            get
            {
                return _childNodes;
            }
        }

        /// <summary>
        /// Gets the list of attributes for this node
        /// </summary>
        public List<ViewerNode> Attributes
        {
            get
            {
                return _attributes;
            }
        }

        /// <summary>
        /// Gets the path to the node
        /// </summary>
        public string NodePath
        {
            get
            {
                if (_nodePath == null)
                {
                    StringBuilder value = new StringBuilder();
                    value.Append(NonRecurringNodePath);

                    if (_occurrenceIndex > -1)
                    {
                        value.Append("[");
                        value.Append(_occurrenceIndex + 1);
                        value.Append("]");
                    }

                    _nodePath = value.ToString();
                }
                return _nodePath;

            }
        }

        /// <summary>
        /// Gets the path to the node, stripping out any occurrence information
        /// </summary>
        public string NonRecurringNodePath
        {
            get
            {
                if (_nonRecurringNodePath == null)
                {
                    StringBuilder value = new StringBuilder();
                    value.Append("/");

                    if (_type == NodeType.Attribute)
                    {
                        value.Append("@");
                    }

                    value.Append("*");
                    value.AppendFormat("[local-name()='{0}' and namespace-uri()='{1}']", _localName, _namespace);
                    _nonRecurringNodePath = value.ToString();
                }
                return _nonRecurringNodePath;
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a count of the number of times the given node
        /// repeats in the list of children nodes
        /// </summary>
        /// <param name="childNode">ChildNode t get count of</param>
        /// <returns>Coutn of the number of times nodes with the same Name/Namespace appear</returns>
        public int GetCountOfRepeatingChildNodes(ViewerNode childNode)
        {
            // Sanity check parameters
            if (childNode == null)
            {
                throw new ArgumentNullException("A valid ViewerNode must be supplied");
            }

            // Check if this is a repeating node
            if (childNode.OccurrenceIndex == -1)
            {
                return 0;
            }

            // Get a count of child nodes with the same name
            int nodeCount = 0;
            for (int nodeIndex = 0; nodeIndex < _childNodes.Count; nodeIndex++)
            {
                if ((string.Compare(_childNodes[nodeIndex].LocalName, childNode.LocalName, true) == 0)
                    && (string.Compare(_childNodes[nodeIndex].Namespace, childNode.Namespace, true) == 0))
                {
                    nodeCount++;
                }
            }

            return nodeCount;
        }

        /// <summary>
        /// Gets a detail string representation of this object (used for displaying the object instance in the viewer)
        /// </summary>
        /// <returns>Detail string representation</returns>
        public string ToDetailsString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_type.ToString());
            builder.Append(": ");
            builder.Append(_localName);
            builder.Append(Environment.NewLine);
            if (!string.IsNullOrEmpty(_value))
            {
                builder.AppendLine(string.Format("Value: {0}", _value));
            }

            if (!string.IsNullOrEmpty(_namespace))
            {
                builder.AppendLine(string.Format("Namespace: {0}", _namespace));
            }

            if (_normalAttributeCount > 0)
            {
                builder.AppendLine(string.Format("Attribute Count: {0}", _normalAttributeCount));
            }

            if (_childNodes.Count > 0)
            {
                builder.AppendLine(string.Format("Child Node Count: {0}", _childNodes.Count));
            }

            return builder.ToString();
        }

        #endregion

        #region Public Overriden Members

        /// <summary>
        /// Gets a string representation of this object
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            string returnValue = null;

            if (_type == NodeType.Attribute)
            {
                returnValue = "@" + _localName;
            }
            else if (_type == NodeType.Element)
            {
                if (string.IsNullOrEmpty(_typeName))
                {
                    returnValue = _localName;
                }
                else
                {
                    returnValue = _localName + " [" + _typeName + "]";
                }
            }
            else
            {
                returnValue = _name;
            }

            if ((_childNodes.Count == 0) && (!string.IsNullOrEmpty(_value)))
            {
                returnValue += ": " + _value;
            }

            return returnValue;
        }

        #endregion
    }

    #region NodeType Enum

    /// <summary>
    /// Enum of node types
    /// </summary>
    public enum NodeType
    {
        Element,
        Attribute,
        Unknown
    }

    #endregion

    #region AttributeType Enum

    /// <summary>
    /// Enum of attribute types
    /// </summary>
    public enum AttributeType
    {
        None,
        Xmlns,
        Type
    }

    #endregion
}

