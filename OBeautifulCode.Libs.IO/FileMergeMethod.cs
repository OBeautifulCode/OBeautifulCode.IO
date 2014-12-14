// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileMergeMethod.cs" company="OBeautifulCode">
//   Copyright 2014 OBeautifulCode
// </copyright>
// <summary>
//   Determines how to store the output of merging two files.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Libs.IO
{
    /// <summary>
    /// Determines how to store the output of merging two files
    /// </summary>
    public enum FileMergeMethod
    {
        /// <summary>
        /// Merge the bottom file into the top file.
        /// </summary>
        MergeIntoTopFile,

        /// <summary>
        /// Merge two files into a new file.
        /// </summary>
        MergeIntoNewFile
    }
}
