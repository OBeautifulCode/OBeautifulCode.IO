// --------------------------------------------------------------------------------------------------------------------
// <copyright file="File.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Represents a file.
    /// </summary>
    public partial class File : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        /// <param name="bytes">The bytes of the file.</param>
        /// <param name="fileName">OPTIONAL name of the file.  DEFAULT is to an unspecified name.</param>
        /// <param name="fileFormat">OPTIONAL format of the file.  DEFAULT is unspecified.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public File(
            byte[] bytes,
            string fileName = null,
            FileFormat fileFormat = FileFormat.Unspecified)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            this.Bytes = bytes;
            this.FileName = fileName;
            this.FileFormat = fileFormat;
        }

        /// <summary>
        /// Gets the bytes of the file.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = ObcSuppressBecause.CA1819_PropertiesShouldNotReturnArrays_DataPayloadsAreCommonlyRepresentedAsByteArrays)]
        public byte[] Bytes { get; private set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the format of the file.
        /// </summary>
        public FileFormat FileFormat { get; private set; }
    }
}