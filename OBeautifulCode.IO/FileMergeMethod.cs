﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileMergeMethod.cs" company="OBeautifulCode">
//   Copyright 2015 OBeautifulCode
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO
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