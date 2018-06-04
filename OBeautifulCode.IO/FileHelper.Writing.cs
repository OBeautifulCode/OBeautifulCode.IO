﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHelper.Writing.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.IO source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO.Recipes
{
    using System;
    using System.IO;
    using System.Security;
    using System.Text;

    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Validation.Recipes;

    /// <summary>
    /// Provides various convenience methods for dealing with files.
    /// </summary>
#if !OBeautifulCodeIORecipesProject
    internal
#else
    public
#endif
    static partial class FileHelper
    {
        /// <summary>
        /// Merges two files.
        /// </summary>
        /// <param name="topFilePath">path to file whose contents will be place at the top of the merged file.</param>
        /// <param name="bottomFilePath">path to the file whose contents will be placed at the bottom of the merged file.</param>
        /// <param name="headerTreatment">determines whether the bottom file's header will be deleted or kept upon merge.</param>
        /// <param name="mergeMethod">Determines if the bottom file should be merged into the top file, or if both should be merged into a new file.</param>
        /// <param name="newFilePath">path to the new file to create IF <paramref name="mergeMethod"/> is <see cref="FileMergeMethod.MergeIntoNewFile"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="topFilePath"/> or <paramref name="bottomFilePath"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="topFilePath"/> or <paramref name="bottomFilePath"/> is whitespace or contains invalid characters, or referes to a non-file device such as "con:", "com1:" etc.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="mergeMethod"/> = <see cref="FileMergeMethod.MergeIntoNewFile"/> and <paramref name="newFilePath"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="mergeMethod"/> = <see cref="FileMergeMethod.MergeIntoNewFile"/> and <paramref name="newFilePath"/> is whitespace or contains invalid characters.</exception>
        /// <exception cref="FileNotFoundException"><paramref name="topFilePath"/> or <paramref name="bottomFilePath"/> cannot be found.</exception>
        /// <exception cref="IOException">An I/O error occurs with <paramref name="topFilePath"/> or <paramref name="bottomFilePath"/> or <paramref name="newFilePath"/>, such as when these files are locked.</exception>
        /// <exception cref="IOException">I/O error writing to <paramref name="topFilePath"/> or <paramref name="newFilePath"/> depending on <paramref name="mergeMethod"/>.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission to access <paramref name="topFilePath"/> or <paramref name="bottomFilePath"/>.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission to write to <paramref name="topFilePath"/> or <paramref name="newFilePath"/> depending on the MergeMethod.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing <paramref name="topFilePath"/> or <paramref name="bottomFilePath"/> could not be found or the filePath is a directory.</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="mergeMethod"/> = <see cref="FileMergeMethod.MergeIntoNewFile"/> and directory containing <paramref name="newFilePath"/> could not be found, or <paramref name="newFilePath"/> is a directory.</exception>
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions to <paramref name="topFilePath"/> or <paramref name="bottomFilePath"/>.</exception>
        /// <exception cref="UnauthorizedAccessException"><paramref name="topFilePath"/> is read-only.</exception>
        /// <exception cref="UnauthorizedAccessException"><paramref name="newFilePath"/> is readonly and <paramref name="mergeMethod"/> is <see cref="FileMergeMethod.MergeIntoNewFile"/>.</exception>
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have write permission to either <paramref name="newFilePath"/> or <paramref name="topFilePath"/> depending on MethodMethod.</exception>
        /// <exception cref="IOException"><paramref name="topFilePath"/> or <paramref name="bottomFilePath"/> was too long.</exception>
        /// <exception cref="IOException"><paramref name="mergeMethod"/> = <see cref="FileMergeMethod.MergeIntoNewFile"/> and <paramref name="newFilePath"/> was too long.</exception>
        /// <exception cref="NotSupportedException"><paramref name="topFilePath"/> or <paramref name="bottomFilePath"/> is in an invalid format.</exception>
        /// <exception cref="NotSupportedException"><paramref name="mergeMethod"/> = <see cref="FileMergeMethod.MergeIntoNewFile"/> and <paramref name="newFilePath"/> is in an invalid format.</exception>
        /// <remarks>
        /// If the top file ends in a newline, then the bottom file is merged into the top without an additional newline.
        /// If, however, the top file doesn't end in a new line, then a newline is inserted at the end of the top file before merging in the bottom file.
        /// The bottom file always remains intact, except when <paramref name="headerTreatment"/> is DeleteBottomFileHeader.  In that case, the first line (including the newline at the end of that line, if it exists) are removed before merging.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "It's redundant but not harmful.")]
        public static void MergeFiles(
            string topFilePath, 
            string bottomFilePath, 
            FileMergeHeaderTreatment headerTreatment, 
            FileMergeMethod mergeMethod, 
            string newFilePath)
        {
            new { topFilePath }.Must().NotBeNullNorWhiteSpace();
            new { bottomFilePath }.Must().NotBeNullNorWhiteSpace();

            if (mergeMethod == FileMergeMethod.MergeIntoNewFile)
            {
                new { newFilePath }.Must().NotBeNullNorWhiteSpace();
            }

            // is the last character in filepathTop a newline?
            bool lastLineOfTopFileIsBlank = string.IsNullOrEmpty(ReadLastLine(topFilePath));
            if (mergeMethod == FileMergeMethod.MergeIntoNewFile)
            {
                File.Copy(topFilePath, newFilePath, true);
            }
            else
            {
                newFilePath = topFilePath;
            }

            // do the encoding types match?
            Encoding encoding;
            using (var readerTop = new StreamReader(topFilePath))
            {
                using (var readerBottomStream = new FileStream(bottomFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var readerBottom = new StreamReader(readerBottomStream))
                    {
                        encoding = readerTop.CurrentEncoding;
                        if (readerTop.CurrentEncoding.GetType() != readerBottom.CurrentEncoding.GetType())
                        {
                            throw new NotSupportedException("Cannot merge files with different encodings.");
                        }
                    }
                }
            }

            using (var writer = new FileStream(newFilePath, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                using (var readerStream = new FileStream(bottomFilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (var reader = new StreamReader(readerStream))
                    {
                        long readerPosition = 0;
                        if (headerTreatment == FileMergeHeaderTreatment.DeleteBottomFileHeader)
                        {
                            string header = reader.ReadLine();
                            readerPosition = reader.CurrentEncoding.GetByteCount(header + Environment.NewLine);
                        }

                        if (!reader.EndOfStream)
                        {
                            if (!lastLineOfTopFileIsBlank)
                            {
                                byte[] newlineBytes = Environment.NewLine.ToBytes(encoding);
                                writer.Write(newlineBytes, 0, newlineBytes.Length);
                            }

                            readerStream.Seek(readerPosition, SeekOrigin.Begin);
                            readerStream.CopyTo(writer);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Replaces the header line in a file.
        /// </summary>
        /// <param name="filePath">file containing header to replace.</param>
        /// <param name="newHeader">new header to paste into file's header line.</param>
        /// <remarks>
        /// If input file doesn't have a header, <paramref name="newHeader"/> will be inserted as the first line.
        /// If <paramref name="newHeader"/> is null, then the old header is simply deleted and the second line becomes the header.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="filePath"/> is whitespace or containsinvalid characters, or refers to a non-file device such as "con:", "com1:", etc.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newHeader"/> is null.</exception>
        /// <exception cref="FileNotFoundException">File specified by <paramref name="filePath"/> cannot be found.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing file specified by <paramref name="filePath"/> could not be found or the <paramref name="filePath"/> is a directory.</exception>
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions on <paramref name="filePath"/> OR caller doesn't have permissions to create a temporary file OR caller doesn't have permission to write to filePath OR filePath is read-only.</exception>
        /// <exception cref="PathTooLongException"><paramref name="filePath"/> was too long.</exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the old header string.</exception>
        /// <exception cref="NotSupportedException"><paramref name="filePath"/> is in an invalid format.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "It's redundant but not harmful.")]
        public static void ReplaceHeader(
            string filePath, 
            string newHeader)
        {
            new { filePath }.Must().NotBeNullNorWhiteSpace();

            string tempFile = CreateTemporaryFile();

            using (var readerStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(readerStream))
                {
                    // use FileStream instead of StreamWriter because its easier to manage a class with base Stream
                    // maintains proper stream position after writing header
                    using (var writer = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // write header line to writer
                        if (newHeader != null)
                        {
                            byte[] newHeaderBytes = newHeader.ToBytes(reader.CurrentEncoding);
                            writer.Write(newHeaderBytes, 0, newHeaderBytes.Length);
                        }

                        // first line in reader to discard
                        string oldHeader = reader.ReadLine();

                        // the one-liner case
                        if (!reader.EndOfStream)
                        {
                            // multi-line file, write a newline
                            if (newHeader != null)
                            {
                                byte[] newLineBytes = Environment.NewLine.ToBytes(reader.CurrentEncoding);
                                writer.Write(newLineBytes, 0, newLineBytes.Length);
                            }

                            // need to seek because BaseStream doesn't keep-up with reader.
                            reader.BaseStream.Seek(reader.CurrentEncoding.GetByteCount(oldHeader + Environment.NewLine), SeekOrigin.Begin);
                            reader.BaseStream.CopyTo(writer);
                        }
                    } // using writer
                } // using reader
            }

            File.Copy(tempFile, filePath, true);
        }

        /// <summary>
        /// Saves a stream to a file.
        /// </summary>
        /// <param name="stream">stream containing data to save to file.</param>
        /// <param name="filePath">location to write file to disk.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="filePath"/> is whitespace or has illegal characters, or referes to a non-file device such as "con:".</exception>
        /// <exception cref="NotSupportedException"><paramref name="stream"/> does not support reading.</exception>
        /// <exception cref="SecurityException">caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">directory specified in <paramref name="filePath"/> not found.</exception>
        /// <exception cref="UnauthorizedAccessException"><paramref name="filePath"/> is an existing file that's read-only or the caller doesn't have write access to the file or folder represented by filePath.</exception>
        /// <exception cref="PathTooLongException"><paramref name="filePath"/> has too many characters.</exception>
        /// <exception cref="ObjectDisposedException">Methods called on <paramref name="stream"/> after it was closed.</exception>
        /// <remarks>
        /// If <paramref name="filePath"/> already exists, it will be overwritten.
        /// </remarks>
        /// <returns>
        /// Returns the inputted stream.
        /// </returns>
        public static Stream SaveStreamToFile(
            this Stream stream, 
            string filePath)
        {
            new { stream }.Must().NotBeNull();
            new { filePath }.Must().NotBeNullNorWhiteSpace();

            try
            {
                stream.Seek(0, SeekOrigin.Begin);
            }
            catch (IOException)
            {
            }
            catch (NotSupportedException)
            {
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }

            return stream;
        }

        /// <summary>
        /// Creates a zero-byte file.
        /// </summary>
        /// <param name="filePath">filePath to create.</param>
        /// <returns>True if zero-byte file was created.  False if not.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="filePath"/> is whitespace or contains one or more invalid characters.</exception>
        /// <exception cref="NotSupportedException"><paramref name="filePath"/> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
        /// <exception cref="IOException">An I/O error occurs, such as specifying FileMode.CreateNew and the file specified by <paramref name="filePath"/> already exists.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified <paramref name="filePath"/> is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified path, such as when <paramref name="filePath"/> points to a directory that the caller doesn't have write permission to.</exception>
        /// <exception cref="PathTooLongException">The specified <paramref name="filePath"/> exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        public static bool CreateZeroByteFile(
            string filePath)
        {
            new { filePath }.Must().NotBeNullNorWhiteSpace();

            using (new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
            }

            return File.Exists(filePath) && IsFileSizeZero(filePath);
        }
    }
}