﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHelper.LegalAndIllegalFiles.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.IO source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO.Recipes
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Security.Permissions;

    using OBeautifulCode.Validation.Recipes;

#if !OBeautifulCodeIORecipesProject
    internal
#else
    public
#endif
    static partial class FileHelper
    {
        /// <summary>
        /// Determines if a file name is valid.
        /// </summary>
        /// <param name="fileName">The file name to validate.</param>
        /// <remarks>
        /// A file name is invalid if it contains illegal characters or contains an OS restricted term such as PRN.
        /// </remarks>
        /// <returns>
        /// Returns true if the given file name is valid, false if not.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> is whitespace.</exception>
        public static bool IsValidFileName(
            string fileName)
        {
            new { fileName }.Must().NotBeNullNorWhiteSpace();

            fileName = fileName.Trim(); // remove leading/lagging whitespace
            return (!Path.GetInvalidFileNameChars().Any(illegalChar => fileName.Contains(illegalChar))) && (!IsOsRestrictedPath(fileName));
        }

        /// <summary>
        /// Determines if a file path is in a valid format.
        /// </summary>
        /// <param name="filePath">The path to check.</param>
        /// <returns>
        /// Returns true if the path is a valid file path, false if not.
        /// </returns>
        public static bool IsValidFilePath(
            string filePath)
        {
            if (IsValidPath(filePath) && (!string.IsNullOrEmpty(Path.GetFileName(filePath))))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Replaces all illegal characters in a filename them a space character.
        /// Do not pass a file path because backslash will be replaced by a space.
        /// </summary>
        /// <param name="fileName">filename to evaluate.</param>
        /// <returns>The legal filename.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> is whitespace.</exception>
        public static string MakeLegalFileName(
            string fileName)
        {
            new { fileName }.Must().NotBeNullNorWhiteSpace();

            char[] illegalCharacters = Path.GetInvalidFileNameChars();
            return illegalCharacters.Aggregate(fileName, (current, illegal) => current.Replace(illegal, ' '));
        }

        /// <summary>
        /// Determines if a path is restricted by the operating system.
        /// </summary>
        /// <param name="path">path to a file or folder.</param>
        /// <returns>True if path is restricted, False if not.</returns>
        /// <remarks>
        /// Files with certain tokens are restricted.  It doesn't matter what the extension is.
        /// The OS seems to only care about the beginning part of the file.  so MyFile.con.txt is legit, whereas con.txt isn't.
        /// OS also restricts folder names in the same way files are restricted (i.e. no folder named "con"  or even "con.directory").
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> is whitespace.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os", Justification = "This is spelled correctly.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os", Justification = "This is cased as we would like it.")]
        public static bool IsOsRestrictedPath(
            string path)
        {
            new { path }.Must().NotBeNullNorWhiteSpace();

            path = path.ToUpper(CultureInfo.CurrentCulture);

            // get all terms that are separated by slash
            string[] directories = path.Split('\\', '/');

            // ensure each term isn't restricted by checking the first substring when splitting on the period character
            // this will work for both files and folders
            return directories
                .Select(directory => directory.Split(".".ToCharArray(), 2))
                .Select(dotSeparated => dotSeparated[0])
                .Any(toCheck => RestrictedFileNameTokens.Contains(toCheck));
        }

        /// <summary>
        /// Determines if a path (folder or file) is in a valid format and isn't restricted by the OS.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>
        /// Returns true if the path is valid.  False if not.
        /// </returns>
        public static bool IsValidPath(
            string path)
        {
            try
            {
                // initialize with fully restricted permissions
                var permission = new FileIOPermission(PermissionState.None)
                {
                    AllFiles = FileIOPermissionAccess.PathDiscovery,
                };

                // Declares that the calling code can access the resource protected by a permission demand through the code that calls this method, even if callers higher in the stack have not been granted permission to access the resource
                permission.Assert();
                try
                {
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    Path.GetFullPath(path);
                }
                finally
                {
                    CodeAccessPermission.RevertAssert();
                }

                return !IsOsRestrictedPath(path);
            }
            catch (SecurityException)
            {
                // The calling code does not have SecurityPermissionFlag.Assertion
            }
            catch (ArgumentNullException)
            {
                // path is a null reference
            }
            catch (ArgumentException)
            {
                // path is a zero-length string, contains only whitespace, invalid characters, or the system cannot retrieve the absolute path
            }
            catch (NotSupportedException)
            {
                // path contains a colon (":") that is not part of a volume identifier
            }
            catch (PathTooLongException)
            {
                // path contains too many characters
            }

            return false;
        }
    }
}