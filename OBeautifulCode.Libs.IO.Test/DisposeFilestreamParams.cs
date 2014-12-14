// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisposeFilestreamParams.cs" company="OBeautifulCode">
//   Copyright 2014 OBeautifulCode
// </copyright>
// <summary>
//   A wrapper for a filestream for testing purposes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Libs.IO.Test
{
    using System.IO;

    /// <summary>
    /// A wrapper for a file stream for testing purposes.
    /// </summary>
    public class DisposeFilestreamParams
    {
        #region Fields (Private)
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposeFilestreamParams"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="waitSeconds">The number of seconds to wait.</param>
        public DisposeFilestreamParams(FileStream fileStream, int waitSeconds)
        {
            this.FileStream = fileStream;
            this.WaitSeconds = waitSeconds;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file stream.
        /// </summary>
        public FileStream FileStream { get; private set; }

        /// <summary>
        /// Gets the number of seconds to wait.
        /// </summary>
        public int WaitSeconds { get; private set; }

        #endregion

        #region Public Methods

        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods

        #endregion
    }
}
