////////////////////////////////////////////////////////
/// File: ConfigFactory.cs
/// Author: Daniel Probert
/// Date: 27-07-2007
/// Version: 1.0
////////////////////////////////////////////////////////
namespace DanSharp.XmlViewer.Configuration
{
    /// <summary>
    /// FactoryClass that contains methods for serialising
    /// and de-serialising a Config object
    /// </summary>
    public class ConfigFactory : FactoryBase
    {
        #region Constructors
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfigFactory()
        {
               
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Builds a new Config object from the supplied xml string or file name
        /// </summary>
        /// <param name="inputString">XML string or file path</param>
        /// <returns>Hydrated Config object</returns>
        public Config Deserialise(string inputString)
        {
            return (Config)base.DeserialiseObject(inputString, typeof(Config));
        }

        /// <summary>
        /// Serialises the given Config and returns the serialised object
        /// </summary>
        /// <param name="manifest">Object to serialise</param>
        public string Serialise(Config manifest)
        {
            return base.SerialiseObject(manifest);
        }

        /// <summary>
        /// Serialises the given Config and writes it to a file
        /// </summary>
        /// <param name="manifest">Object to serialise</param>
        /// <param name="fileName">File to serialise the object to</param>
        public void SerialiseToFile(Config manifest, string fileName)
        {
            base.SerialiseObject(manifest, fileName);
        }

        #endregion
    }
}
