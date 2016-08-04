// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileToZip.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO
{
    /// <summary>
    /// Represents a file that needs to be compressed
    /// </summary>
    public class FileToZip
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileToZip"/> class.
        /// </summary>
        /// <param name="name">The name of the file in the zip.</param>
        /// <param name="path">The path to the file on disk.</param>
        public FileToZip(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        /// <summary>
        /// Gets the name of the file in the zip.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the path to the file on disk.
        /// </summary>
        public string Path { get; private set; }
    }
}
