using System.Xml;

namespace System.Xml
{
    /// <summary>
    /// Class that creates <see cref="XmlReader"/>s and <see cref="XmlWriter"/>s
    /// </summary>
    /// <remarks>
    /// Provides two default impelementations for general use.  These are <see cref="Default"/> and
    /// <see cref="DefaultAsync"/>.
    /// </remarks>
    public sealed class ReaderWriterGenerator
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ReaderWriterGenerator"/> with default settings from <see cref="Defaults"/>
        /// </summary>
        /// <remarks>
        /// This is a private method, as it uses the default reader and writer.  If you need only the default ones, or the
        /// default async ones, then use <see cref="Default"/> or <see cref="DefaultAsync"/>.
        /// </remarks>
        private ReaderWriterGenerator()
            : this(XmlWriterSettings, XmlReaderSettings)
        {
        }
        /// <summary>
        /// Constructs a new <see cref="ReaderWriterGenerator"/> with given settings
        /// </summary>
        /// <param name="writerSettings">Settings to use for creating <see cref="XmlWriter"/>s</param>
        /// <param name="readerSettings">Settings to use for creating <see cref="XmlReader"/>s</param>
        public ReaderWriterGenerator(XmlWriterSettings writerSettings, XmlReaderSettings readerSettings)
        {
            _writerSettings = writerSettings;
            _readerSettings = readerSettings;
        }
        /// <summary>
        /// Initializes the <see cref="ReaderWriterGenerator"/>
        /// </summary>
        static ReaderWriterGenerator()
        {
            Default = new ReaderWriterGenerator();
            DefaultAsync = new ReaderWriterGenerator(XmlWriterSettingsAsync, XmlReaderSettingsAsync);
        }
        #endregion
        #region Member Variables
        private XmlReaderSettings _readerSettings;
        private XmlWriterSettings _writerSettings;

        public static readonly ReaderWriterGenerator Default;
        public static readonly ReaderWriterGenerator DefaultAsync;
        #endregion
        #region Defaults
        /// <summary>
        /// Default settings for XmlWriter
        /// </summary>
        public static XmlWriterSettings XmlWriterSettings
        {
            get
            {
                return new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineOnAttributes = true,
                };
            }
        }

        /// <summary>
        /// Default settings for an XmlWriter in asynchronous mode
        /// </summary>
        public static XmlWriterSettings XmlWriterSettingsAsync
        {
            get
            {
                XmlWriterSettings settings = XmlWriterSettings;
                settings.Async = true;
                return settings;
            }
        }

        /// <summary>
        /// Default settings for an XmlReader
        /// </summary>
        public static XmlReaderSettings XmlReaderSettings
        {
            get
            {
                return new XmlReaderSettings()
                {
                    DtdProcessing = DtdProcessing.Ignore,
                };
            }
        }

        /// <summary>
        /// Default settings for an XmlReader in asynchronous mode
        /// </summary>
        public static XmlReaderSettings XmlReaderSettingsAsync
        {
            get
            {
                XmlReaderSettings settings = XmlReaderSettings;
                settings.Async = true;
                return settings;
            }
        }
        #endregion
        #region Methods
        public XmlReader GetReader(Stream stream)
        {
            return XmlReader.Create(stream, _readerSettings);
        }
        public XmlWriter GetWriter(Stream stream)
        {
            return XmlWriter.Create(stream, _writerSettings);
        }
        #endregion
    }
}
