// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileMergeHeaderTreatment.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO
{
    /// <summary>
    /// Determines what to do with the header line of the bottom file when merging two files.
    /// </summary>
    public enum FileMergeHeaderTreatment
    {
        /// <summary>
        /// Delete the header of the bottom file
        /// </summary>
        DeleteBottomFileHeader,

        /// <summary>
        /// keep the header of the bottom file (i.e. take the file completely as-is)
        /// </summary>
        KeepBottomFileHeader
    }
}
