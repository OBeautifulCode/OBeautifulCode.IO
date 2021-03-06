﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileMergeMethod.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.IO.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO.Recipes
{
    /// <summary>
    /// Determines how to store the output of merging two files.
    /// </summary>
#if !OBeautifulCodeIOSolution
    [global::System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.IO.Recipes", "See package version number")]
    internal
#else
    public
#endif
    enum FileMergeMethod
    {
        /// <summary>
        /// Merge the bottom file into the top file.
        /// </summary>
        MergeIntoTopFile,

        /// <summary>
        /// Merge two files into a new file.
        /// </summary>
        MergeIntoNewFile,
    }
}
