////////////////////////////////////////////////////////
/// File: FactoryBase.cs
/// Author: Daniel Probert
/// Date: 27-07-2007
/// Version: 1.0
////////////////////////////////////////////////////////
namespace DanSharp.XmlViewer.Configuration
{
    #region Using Statements

    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    /// Base class for factories.
    /// Provides a generic mechanism for serialising/deserialising objects
    /// </summary>
    public class FactoryBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public FactoryBase()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serialises the given object to a string
        /// </summary>
        /// <param name="objectToSerialise">Object to be serialised</param>
        /// <returns>Serialised object</returns>
        public virtual string SerialiseObject(object objectToSerialise)
        {
            return SerialiseObject(objectToSerialise, objectToSerialise.GetType());
        }

        /// <summary>
        /// Serialises the given object to a string
        /// </summary>
        /// <param name="objectToSerialise">Object to be serialised</param>
        /// <param name="type">Type of object to serialise</param>
        /// <returns>Serialised object</returns>
        public virtual string SerialiseObject(object objectToSerialise, Type type)
        {
            // Create stream to hold bytes
            MemoryStream ms = new MemoryStream();

            // Wrap a writer round this stream
            XmlTextWriter xmlWriter = new XmlTextWriter(ms, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;

            // Create a serializer for the type to be serialized
            XmlSerializer saveXML = new XmlSerializer(type);

            // Serialize to the stream
            saveXML.Serialize(xmlWriter, objectToSerialise);

            // Reset the stream
            ms.Seek(0,System.IO.SeekOrigin.Begin);

            // Create a reader to read from the stream
            StreamReader reader = new StreamReader(ms);

            // Read from the stream
            string serialisedObject = reader.ReadToEnd();

            // Clean up
            xmlWriter.Close();
            reader.Close(); 

            return serialisedObject;
        }

        /// <summary>
        /// Serialises the given object and writes it to a file
        /// </summary>
        /// <param name="objectToSerialise">Object to be serialised</param>
        /// <param name="fileName">File to serialise the object to</param>
        public virtual void SerialiseObject(object objectToSerialise, string fileName)
        {
            SerialiseObject(objectToSerialise, fileName, objectToSerialise.GetType());
        }

        /// <summary>
        /// Serialises the given object and writes it to a file
        /// </summary>
        /// <param name="objectToSerialise">Object to be serialised</param>
        /// <param name="fileName">File to serialise the object to</param>
        /// <param name="type">Type of object to serialise</param>
        public virtual void SerialiseObject(object objectToSerialise, string fileName, Type type)
        {
            // Open a writer for the file
            XmlTextWriter xmlTextWriter = new XmlTextWriter(fileName, Encoding.UTF8);

            // Create a serializer for the type to be serialized
            XmlSerializer serialiser = new XmlSerializer(type);

            // Serialize to the file
            serialiser.Serialize(xmlTextWriter, objectToSerialise);

            // Clean up
            xmlTextWriter.Close();
        }

        /// <summary>
        /// Deserialises an object from the given XML or FilePath
        /// </summary>
        /// <param name="inputString">Either a file name or an xml string</param>
        /// <param name="objectType">Type of object to deserialise</param>
        /// <returns>Hydrated ApplicationManifest object</returns>
        public object DeserialiseObject(string inputString, Type objectType)
        {
            // Check that we have the correct parameters
            if ((inputString == null) || (inputString.Length == 0))
            {
                throw new ArgumentNullException("inputString", "A string to deserialise must be supplied");
            }

            if (objectType == null)
            {
                throw new ArgumentNullException("objectType", "The type of object to deserialise must be supplied");
            }

            // Check if we have an xml string, or a file name
            if (inputString.Trim().StartsWith("<"))
            {
                return DeserialiseObjectFromString(inputString, objectType);
            }
            else
            {
                // If it's not XML, then we assume this is a file name
                return DeserialiseObjectFromFile(inputString, objectType);     
            }
        }

        /// <summary>
        /// Deserialises an object from the given FilePath
        /// </summary>
        /// <param name="filePath">File that contains XML to deserialise</param>
        /// <param name="objectType">Type of object to deserialise</param>
        /// <returns>Hydrated object</returns>
        private object DeserialiseObjectFromFile(string filePath, Type objectType)
        {
            object deserialisedObject;

            // Create a reader to read from the file
            XmlTextReader xmlReader = new XmlTextReader(filePath);

            // Create a serializer for the type to be deserialized
            XmlSerializer serialiser = new XmlSerializer(objectType);

            // Deserialize from reader
            deserialisedObject = serialiser.Deserialize(xmlReader);

            // Clean up
            xmlReader.Close();

            return deserialisedObject;
        }

        /// <summary>
        /// Deserialises an object from the given XML string
        /// </summary>
        /// <param name="xml">Serialised object</param>
        /// <param name="objectType">Type of object to deserialise</param>
        /// <returns>Hydrated object</returns>
        public object DeserialiseObjectFromString(string xml, Type objectType)
        {
            object deserialisedObject;

            // Wrap a reader around the string
            StringReader sr = new StringReader(xml);
            XmlTextReader xmlReader = new XmlTextReader(sr);

            // Create a serializer for the type to be deserialized
            XmlSerializer serialiser = new XmlSerializer(objectType);

            // Deserialize from reader
            deserialisedObject = serialiser.Deserialize(xmlReader);

            // Clean up
            xmlReader.Close();

            return deserialisedObject;
        }
        #endregion
    }
}
