﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryHelper.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.IO.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO.Recipes
{
    using global::System;
    using global::System.Diagnostics;
    using global::System.Globalization;
    using global::System.IO;
    using global::System.Security;

    using OBeautifulCode.Math.Recipes;
    using OBeautifulCode.String.Recipes;

    using static global::System.FormattableString;

    /// <summary>
    /// Provides various convenience methods for dealing with directories.
    /// </summary>
#if !OBeautifulCodeIOSolution
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [global::System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.IO.Recipes", "See package version number")]
    internal
#else
    public
#endif
    static class DirectoryHelper
    {
        /// <summary>
        /// Lock object for creating temporary resources.
        /// </summary>
        private static readonly object CreateTemporaryResourceLock = new object();

        /// <summary>
        /// Deletes all subfolders of a specified folder that were last accessed to prior
        /// to a specified number of minutes looking back from now.
        /// </summary>
        /// <param name="rootFolder">The folder containing the temporary folders to delete.</param>
        /// <param name="minutesToKeep">
        /// Keeps subfolders that were last accessed to within this number of minutes.  Minutes are based on time - keeping
        /// 1440 minutes means keeping subfolders last accessed to 24 hours prior to right now.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="rootFolder"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="rootFolder"/> is whitespace or a relative path couldn't be made absolute.</exception>
        /// <exception cref="ArgumentOutOfRangeException">minutesToKeep is &lt;=0.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory doesn't exist or disappears during the process.</exception>
        /// <exception cref="UnauthorizedAccessException">method can't access the directory.</exception>
        /// <exception cref="IOException"><paramref name="rootFolder"/> is in the application's current working directory.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permissions.</exception>
        public static void ClearTemporaryFolders(
            string rootFolder,
            int minutesToKeep)
        {
            if (rootFolder == null)
            {
                throw new ArgumentNullException(nameof(rootFolder));
            }

            if (string.IsNullOrWhiteSpace(rootFolder))
            {
                throw new ArgumentException(Invariant($"'{nameof(rootFolder)}' is white space"));
            }

            if (minutesToKeep <= 0)
            {
                throw new ArgumentOutOfRangeException(Invariant($"'{nameof(minutesToKeep)}' <= '{0}'"), (Exception)null);
            }

            if (!Directory.Exists(rootFolder))
            {
                throw new DirectoryNotFoundException("root folder doesn't exist '" + rootFolder + "'");
            }

            // check if the directory is the application's current working directory
            if (IsFolderInWorkingDirectory(rootFolder))
            {
                throw new IOException("root folder is the application's current working directory.");
            }

            // now delete folders
            DateTime now = DateTime.Now;
            string[] allSubfolders = Directory.GetDirectories(rootFolder);
            foreach (string subfolder in allSubfolders)
            {
                int minutesElapsed;
                try
                {
                    DateTime lastAccess = Directory.GetLastAccessTime(subfolder);
                    minutesElapsed = MathHelper.Truncate((now - lastAccess).TotalMinutes);
                }
                catch (UnauthorizedAccessException)
                {
                    // don't have access to this folder. no problem, skip
                    continue;
                }
                catch (PathTooLongException)
                {
                    // questionable about whether we can get here.  path is too long.  skip
                    continue;
                }

                if (minutesElapsed > minutesToKeep)
                {
                    try
                    {
                        DeleteFolder(subfolder);
                    }
                    catch (IOException)
                    {
                        // folder cannot be deleted (we already checked for application's working directory reason for this exception)
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // ReSharper disable once RedundantJumpStatement
                        continue;
                    }
                    catch (SecurityException)
                    {
                        // ReSharper disable once RedundantJumpStatement
                        continue;
                    }

                    // ReSharper restore RedundantJumpStatement
                } // enough time has passed?
            } // for each subfolder
        }

        /// <summary>
        /// Creates a temporary folder in the windows temporary folder.
        /// </summary>
        /// <returns>The path to the new temporary folder.</returns>
        /// <remarks>path to new temporary folder will have trailing backslash.</remarks>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission to create a folder in the windows temporary folder.</exception>
        /// <exception cref="IOException">Could't create a temporary folder.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permissions.</exception>
        public static string CreateTemporaryFolder()
        {
            lock (CreateTemporaryResourceLock)
            {
                return CreateTemporaryFolder(Path.GetTempPath());
            }
        }

        /// <summary>
        /// Creates a temporary folder in a given root folder.
        /// </summary>
        /// <param name="rootFolder">folder in which to create a temporary folder.</param>
        /// <returns>The path to the new temporary folder.</returns>
        /// <remarks>path to new temporary folder will have trailing backslash.</remarks>
        /// <exception cref="ArgumentException"><paramref name="rootFolder"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="rootFolder"/> is whitespace or is a relative path that cannot be made absolute.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission to create a folder under rootFolder or cannot access rootFolder.</exception>
        /// <exception cref="PathTooLongException">temporary folder would exceed the character limit.</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="rootFolder"/> is invalid, such as being on an unmapped drive, doesn't exist, or disappears during the process.</exception>
        /// <exception cref="IOException">could't create a temporary folder or rootFolder was in the application's current working directory.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permissions.</exception>
        public static string CreateTemporaryFolder(
            string rootFolder)
        {
            lock (CreateTemporaryResourceLock)
            {
                if (rootFolder == null)
                {
                    throw new ArgumentNullException(nameof(rootFolder));
                }

                if (string.IsNullOrWhiteSpace(rootFolder))
                {
                    throw new ArgumentException(Invariant($"'{nameof(rootFolder)}' is white space"));
                }

                if (!Directory.Exists(rootFolder))
                {
                    throw new DirectoryNotFoundException("root folder doesn't exist '" + rootFolder + "'");
                }

                // check if the directory is the application's current working directory
                if (IsFolderInWorkingDirectory(rootFolder))
                {
                    throw new IOException("root folder is the application's current working directory.");
                }

                rootFolder = rootFolder.AppendMissing(@"\");

                int attempt = 0;
                do
                {
                    string tempFolder = rootFolder + ThreadSafeRandom.Next(0, int.MaxValue).ToString(CultureInfo.CurrentCulture).AppendMissing(@"\");
                    if (Directory.Exists(tempFolder))
                    {
                        attempt++;
                        if (attempt == 25)
                        {
                            ClearTemporaryFolders(rootFolder, 3);
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(tempFolder);
                        return tempFolder;
                    } // directory exists?
                }
                while (attempt < 50);

                throw new IOException("Couldn't create temporary folder.");
            } // lock _createTemporaryResourceLock
        }

        /// <summary>
        /// Deletes a folder and all contents of that folder.
        /// </summary>
        /// <param name="folder">The folder to delete.</param>
        /// <param name="recreate">Determines whether to recreate the deleted folder.</param>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="folder"/> is whitespace or contains illegal characters, or has a relative path that couldn't be made absolute.</exception>
        /// <exception cref="IOException">The directory is the application's current working directory OR the directory couldn't be deleted OR the directory couldn't be recreated.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have permission to recreate the folder when recreate = true.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permissions.</exception>
        /// <exception cref="NotSupportedException">folder contains a colon (":") that is not part of a volume identifier (for example, "lpt:").</exception>
        public static void DeleteFolder(
            string folder, 
            bool recreate = false)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            if (string.IsNullOrWhiteSpace(folder))
            {
                throw new ArgumentException(Invariant($"'{nameof(folder)}' is white space"));
            }
            folder = folder.AppendMissing(@"\");

            // check if the directory is the application's current working directory
            if (IsFolderInWorkingDirectory(folder))
            {
                throw new IOException("The directory is the application's current working directory.");
            }

            try
            {
                Directory.Delete(folder, true);
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (UnauthorizedAccessException)
            {
                // this is not just about permissions.  if files within the folders are read-only then we can get here
                DeleteFolderDos(folder);
            }
            catch (IOException)
            {
                // the directory set by path is readonly or resource in the path is locked
                DeleteFolderDos(folder);
            }

            if (Directory.Exists(folder))
            {
                throw new IOException("Couldn't delete folder '" + folder + "'");
            }

            if (recreate)
            {
                Directory.CreateDirectory(folder);
            }
        }

        /// <summary>
        /// Determines if a directory path is in a valid format (expects trailing backslash).
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>
        /// Returns true if the path is a valid directory path, false if not.
        /// </returns>
        public static bool IsValidDirectoryPath(
            string path)
        {
            if (FileHelper.IsValidPath(path) && string.IsNullOrEmpty(Path.GetFileName(path)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes a folder, including all subfolders and files, using a Windows command-line command.
        /// </summary>
        /// <param name="folder">folder to delete.</param>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="folder"/> is whitespace.</exception>
        /// <remarks>
        /// The purpose of this function is to skirt attributes like read-only.  It doesn't deal locked folders.
        /// If there's an existing file handle on a file in the folder (or any subfolders), then all paths to the file will remain intact and won't be deleted.  Other subfolders will be deleted.
        /// </remarks>
        public static void DeleteFolderDos(
            string folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            if (string.IsNullOrWhiteSpace(folder))
            {
                throw new ArgumentException(Invariant($"'{nameof(folder)}' is white space"));
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = "/c rmdir /S /Q \"" + folder + "\"",
            };

            using (var cmd = new Process())
            {
                cmd.StartInfo = startInfo;
                cmd.Start();
                cmd.WaitForExit();
            }
        }

        /// <summary>
        /// Determines if a folder is in the application's current working directory.
        /// </summary>
        /// <param name="folder">The name of the folder (not the path).</param>
        /// <returns>
        /// Returns true if the folder is in the working directory, false if not.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="folder"/> is whitespace, has invalid characters, or is a relative path that couldn't be made absolute.</exception>
        /// <exception cref="PathTooLongException"><paramref name="folder"/> has too many characters.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permissions.</exception>
        /// <exception cref="NotSupportedException"><paramref name="folder"/> contains a colon (":") that is not part of a volume identifier (for example, "lpt:").</exception>
        public static bool IsFolderInWorkingDirectory(
            string folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            if (string.IsNullOrWhiteSpace(folder))
            {
                throw new ArgumentException(Invariant($"'{nameof(folder)}' is white space"));
            }

            var fullpath = Path.GetFullPath(folder).AppendMissing(@"\");
            var currentDirectory = Directory.GetCurrentDirectory().AppendMissing(@"\");
            if (currentDirectory.ToLower(CultureInfo.CurrentCulture).Contains(fullpath.ToLower(CultureInfo.CurrentCulture)))
            {
                return true;
            }

            return false;
        }
    }
}
