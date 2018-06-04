// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisposeFilestreamParams.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO.Recipes.Test
{
    using System.IO;

    /// <summary>
    /// A wrapper for a file stream for testing purposes.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Params", Justification = "This is spelled correctly.")]
    public class DisposeFileStreamParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisposeFileStreamParams"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="waitSeconds">The number of seconds to wait.</param>
        public DisposeFileStreamParams(
            FileStream fileStream,
            int waitSeconds)
        {
            this.FileStream = fileStream;
            this.WaitSeconds = waitSeconds;
        }

        /// <summary>
        /// Gets the file stream.
        /// </summary>
        public FileStream FileStream { get; private set; }

        /// <summary>
        /// Gets the number of seconds to wait.
        /// </summary>
        public int WaitSeconds { get; private set; }
    }
}
