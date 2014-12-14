// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHelper.cs" company="OBeautifulCode">
//   Copyright 2014 OBeautifulCode
// </copyright>
// <summary>
//   Provides various convenience methods for dealing with files.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Libs.IO
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Security.Cryptography;
    using System.Security.Permissions;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    using CuttingEdge.Conditions;

    using Ionic.Zip;

    using OBeautifulCode.Libs.Math;
    using OBeautifulCode.Libs.String;

    /// <summary>
    /// Provides various convenience methods for dealing with files.
    /// </summary>
    public class FileHelper
    {
        #region Fields (Private)

        /// <summary>
        /// Lock object for creating temporary resources.
        /// </summary>
        private static readonly object CreateTemporaryResourceLock = new object();

        /// <summary>
        /// Tokens that are restricted from being included in files.
        /// </summary>
        private static readonly HashSet<string> RestrictedFileNameTokens = new HashSet<string> { "CON", "PRN", "AUX", "CLOCK$", "NUL", "COM0", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT0", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        #region Alter Files

        /// <summary>
        /// Replaces the header line in a file.
        /// </summary>
        /// <param name="filePath">file containing header to replace.</param>
        /// <param name="newHeader">new header to paste into file's header line.</param>
        /// <remarks>
        /// If input file doesn't have a header, newHeader will be inserted as the first line.
        /// If newHeader is null, then the old header is simply deleted and the second line becomes the header.
        /// </remarks>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace or containsinvalid characters, or refers to a non-file device such as "con:", "com1:", etc.</exception>
        /// <exception cref="ArgumentNullException">newHeader is null.</exception>
        /// <exception cref="FileNotFoundException">File specified by filePath cannot be found.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing file specified by filePath could not be found or the filePath is a directory.</exception>
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions on filePath OR caller doesn't have permissions to create a temporary file OR caller doesn't have permission to write to filePath OR filePath is read-only</exception>
        /// <exception cref="PathTooLongException">filePath was too long.</exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the old header string.</exception>    
        /// <exception cref="NotSupportedException">filePath is in an invalid format</exception>
        public static void ReplaceHeader(string filePath, string newHeader)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();

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
                            byte[] newLineBytes = Environment.NewLine.ToBytes(reader.CurrentEncoding);
                            writer.Write(newLineBytes, 0, newLineBytes.Length);

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
        /// Merges one file into another.
        /// </summary>
        /// <param name="topFilePath">file containing contents that will be appended to.</param>
        /// <param name="bottomFilePath">path to the file whose contents will be placed at the bottom of the file located at topFilePath.</param>
        /// <param name="headerTreatment">determines whether the bottom file's header will be deleted or kept upon merge.</param>       
        /// <exception cref="ArgumentNullException">topFilePath or bottomFilePath is null.</exception>
        /// <exception cref="ArgumentException">topFilePath or bottomFilePath is whitespace or contains invalid characters, or referes to a non-file device such as "con:", "com1:" etc.</exception>
        /// <exception cref="FileNotFoundException">topFilePath or bottomFilePath cannot be found.</exception>
        /// <exception cref="IOException">An I/O error occurs with topFilePath or bottomFilePath, such as when these files are locked.</exception>
        /// <exception cref="IOException">I/O error writing to topFilePath</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission to access topFilePath or bottomFilePath</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission to write to topFilePath.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing topFilePath or bottomFilePath could not be found or the file path is a directory.</exception>
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions to topFilePath or bottomFilePath</exception>
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have write permission to topFilePath.</exception>
        /// <exception cref="UnauthorizedAccessException">topFilePath is read-only</exception>
        /// <exception cref="PathTooLongException">topFilePath or bottomFilePath was too long.</exception>
        /// <exception cref="NotSupportedException">topFilePath or bottomFilePath is in an invalid format</exception>
        /// <remarks>Calls the MergeFiles method with mergeMethod = MergeIntoTopFile and null for newFilePath</remarks>
        public static void MergeFiles(string topFilePath, string bottomFilePath, FileMergeHeaderTreatment headerTreatment)
        {
            MergeFiles(topFilePath, bottomFilePath, headerTreatment, FileMergeMethod.MergeIntoTopFile, null);
        }

        /// <summary>
        /// Merges two files.
        /// </summary>
        /// <param name="topFilePath">path to file whose contents will be place at the top of the merged file.</param>
        /// <param name="bottomFilePath">path to the file whose contents will be placed at the bottom of the merged file.</param>
        /// <param name="headerTreatment">determines whether the bottom file's header will be deleted or kept upon merge.</param>
        /// <param name="mergeMethod">Determines if the bottom file should be merged into the top file, or if both should be merged into a new file.</param>
        /// <param name="newFilePath">path to the new file to create IF mergeMethod is MergeIntoNewFile</param>
        /// <exception cref="ArgumentNullException">topFilePath or bottomFilePath is null.</exception>
        /// <exception cref="ArgumentException">topFilePath or bottomFilePath is whitespace or contains invalid characters, or referes to a non-file device such as "con:", "com1:" etc.</exception>
        /// <exception cref="ArgumentNullException">mergeMethod = MergeIntoNewFile and newFilePath is null</exception>
        /// <exception cref="ArgumentException">mergeMethod = MergeIntoNewFile and newFilePath is whitespace or contains invalid characters.</exception>
        /// <exception cref="FileNotFoundException">topFilePath or bottomFilePath cannot be found.</exception>
        /// <exception cref="IOException">An I/O error occurs with topFilePath or bottomFilePath or newFilePath, such as when these files are locked.</exception>
        /// <exception cref="IOException">I/O error writing to topFilePath or newFilePath depending on MergeMethod.</exception>        
        /// <exception cref="SecurityException">The caller does not have the required permission to access topFilePath or bottomFilePath</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission to write to topFilePath or newFilePath depending on the MergeMethod.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing topFilePath or bottomFilePath could not be found or the filePath is a directory.</exception>
        /// <exception cref="DirectoryNotFoundException">mergeMethod = MergeIntoNewFile and directory containing newFilePath could not be found, or newFilePath is a directory.</exception>
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions to topFilePath or bottomFilePath</exception>
        /// <exception cref="UnauthorizedAccessException">topFilePath is read-only</exception>
        /// <exception cref="UnauthorizedAccessException">newFilePath is readonly and mergeMethod is MergeIntoNewFile</exception>       
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have write permission to either newFilePath or topFilePath depending on MethodMethod</exception>
        /// <exception cref="PathTooLongException">topFilePath or bottomFilePath was too long.</exception>
        /// <exception cref="PathTooLongException">mergeMethod = MergeIntoNewFile and newFilePath was too long.</exception>
        /// <exception cref="NotSupportedException">topFilePath or bottomFilePath is in an invalid format</exception>
        /// <exception cref="NotSupportedException">mergeMethod = MergeIntoNewFile and newFilePath is in an invalid format</exception>
        /// <remarks>
        /// If the top file ends in a newline, then the bottom file is merged into the top without an additional newline.
        /// If, however, the top file doesn't end in a new line, then a newline is inserted at the end of the top file before merging in the bottom file.
        /// The bottom file always remains intact, except when headerTreatment is DeleteBottomFileHeader.  In that case, the first line (including the newline at the end of that line, if it exists) are removed before merging.
        /// </remarks>
        public static void MergeFiles(string topFilePath, string bottomFilePath, FileMergeHeaderTreatment headerTreatment, FileMergeMethod mergeMethod, string newFilePath)
        {
            Condition.Requires(topFilePath, "topFilePath").IsNotNullOrWhiteSpace();
            Condition.Requires(bottomFilePath, "bottomFilePath").IsNotNullOrWhiteSpace();
            if (mergeMethod == FileMergeMethod.MergeIntoNewFile)
            {
                Condition.Requires(newFilePath, "newFilePath").IsNotNullOrWhiteSpace();
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
                    } // bottom file StreamReader
                } // bottom file FileStream
            } // destination file writer            
        } // MergeFiles

        #endregion

        #region Clear Temporary Resources

        /// <summary>
        /// Deletes all Files in the Windows Temporary folder that were last accessed prior
        /// to a specified number of minutes looking back from now.
        /// </summary>
        /// <param name="minutesToKeep">
        /// Keeps files that were last accessed within this number of minutes.  Minutes are based on time - keeping
        /// 1440 minutes means keeping files that were last modified 24 hours prior to right now.
        /// </param>
        /// <exception cref="ArgumentException">minutesToKeep is &lt;=0</exception>
        /// <exception cref="UnauthorizedAccessException"> method can't access the directory</exception>
        /// <remarks>
        /// Assumption that its not possible to delete the Windows temp folder and as such it won't disappear somehow
        /// in this process
        /// </remarks>
        public static void ClearTemporaryFiles(int minutesToKeep)
        {
            ClearTemporaryFiles(Path.GetTempPath(), minutesToKeep);
        }

        /// <summary>
        /// Deletes all Files in a specified folder that were last accessed prior
        /// to a specified number of minutes looking back from now.
        /// </summary>
        /// <param name="temporaryFolder">The folder containing the files and folder to delete.</param>
        /// <param name="minutesToKeep">
        /// Keeps files that were last accessed within this number of minutes.  Minutes are based on time - keeping
        /// 1440 minutes means keeping files that were last modified 24 hours prior to right now.
        /// </param>        
        /// <exception cref="ArgumentNullException">temporaryFolder is null.</exception>
        /// <exception cref="ArgumentException">temporaryFolder is whitespace.</exception>
        /// <exception cref="ArgumentException">minutesToKeep is &lt;=0</exception>
        /// <exception cref="DirectoryNotFoundException">The directory doesn't exist or disappears during the process.</exception>
        /// <exception cref="UnauthorizedAccessException">method can't access the directory</exception>        
        public static void ClearTemporaryFiles(string temporaryFolder, int minutesToKeep)
        {
            // check arguments
            Condition.Requires(temporaryFolder, "temporaryFolder").IsNullOrWhiteSpace();
            Condition.Requires(minutesToKeep, "minutesToKeep").IsGreaterThan(0);

            if (!Directory.Exists(temporaryFolder))
            {
                throw new DirectoryNotFoundException("temporaryFolder doesn't exist '" + temporaryFolder + "'");
            }

            DateTime now = DateTime.Now;

            // delete files older than specified number of minutesToKeep
            // only exceptions that might be thrown are UnauthorizedAccessException or PathTooLongException (would this get past Directory.Exists?)
            string[] allFiles = Directory.GetFiles(temporaryFolder);
            foreach (string filePath in allFiles)
            {
                double minutesElapsed;
                try
                {
                    DateTime lastAccess = File.GetLastAccessTime(filePath);
                    minutesElapsed = MathHelper.Truncate((now - lastAccess).TotalMinutes);
                }
                catch (UnauthorizedAccessException)
                {
                    // if we get here then we can access the directory.  skip files we can't access
                    continue;
                }
                catch (PathTooLongException)
                {
                    // questionable if we can ever hit this.  If we get here the filePath is too long.  Folder path isn't otherwise Directory.GetFiles would have thrown.  ok to skip
                    continue;
                }

                if (minutesElapsed > minutesToKeep)
                {
                    try
                    {
                        // the only exception that can be thrown on this line is DirectoryNotFoundException if the
                        // temporaryFolder somehow disappears
                        DeleteFile(filePath);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // no problem, move on.
                    }
                    catch (IOException)
                    {
                        // no problem if we can't delete file, just move on
                    }
                } // file is old enough to delete?
            } // for each file to consider
        } // delete temp files

        #endregion

        #region Compression

        /// <summary>
        /// Zips a single file to disk.
        /// </summary>
        /// <remarks>
        /// The directory of the file being compressed is not preserved in the output zip file.
        /// </remarks>
        /// <param name="sourceFilePath">Path to file to compress.</param>
        /// <param name="zipFilePath">Path to write compressed file.</param>
        /// <exception cref="ArgumentNullException">sourceFilePath or zipFilePath is null.</exception>
        /// <exception cref="ArgumentException">sourceFilePath or zipFilePath is whitespace.</exception>
        /// <exception cref="FileNotFoundException">File at sourceFilePath was not found on disk or is an invalid file path.</exception>
        /// <exception cref="ArgumentException">zipFilePath is not a valid file path.</exception>
        /// <exception cref="InvalidOperationException">A file already exists at the zipFilePath.</exception>
        /// <exception cref="IOException">sourceFilePath is locked.</exception>
        public static void CompressFile(string sourceFilePath, string zipFilePath)
        {
            Condition.Requires(sourceFilePath, "sourceFilePath").IsNotNullOrWhiteSpace();
            Condition.Requires(zipFilePath, "zipFilePath").IsNotNullOrWhiteSpace();
            if (!File.Exists(sourceFilePath))
            {
                throw new FileNotFoundException("sourceFilePath points to a file that does not exist on disk: " + sourceFilePath);
            }

            if (!IsValidFilePath(zipFilePath))
            {
                throw new ArgumentException("zipFilePath is not a valid file path: " + zipFilePath);
            }

            if (File.Exists(zipFilePath))
            {
                throw new InvalidOperationException("A file already exists at the zipFilePath.");
            }

            try
            {
                using (var zip = new ZipFile(zipFilePath))
                {
                    zip.AddFile(sourceFilePath, @"\");
                    zip.Save();
                }
            }
            catch (Exception)
            {
                File.Delete(zipFilePath);
                throw;
            }
        }

        /// <summary>
        /// Decompresses a zip file.
        /// </summary>
        /// <param name="zipFilePath">Path to compressed file.</param>
        /// <param name="targetDirectory">Directory to decompress files to.</param>
        /// <exception cref="ArgumentNullException">zipFilePath or targetDirectory is is null.</exception>
        /// <exception cref="ArgumentException">zipFilePath or targetDirectory is whitespace.</exception>
        /// <exception cref="FileNotFoundException">zipFilePath points to a file that does not exist on disk.</exception>
        /// <exception cref="DirectoryNotFoundException">targetDirectory was not found on disk.</exception>
        /// <exception cref="IOException">The zip file is locked.</exception>
        /// <exception cref="ZipException">The zip file is malformed.</exception>
        /// <exception cref="IOException">A file in the zip file cannot be written to disk because the target file path is locked.</exception>
        /// <remarks>
        /// Overwrites existing files on disk with the same target path file.
        /// </remarks>
        public static void DecompressFile(string zipFilePath, string targetDirectory)
        {
            Condition.Requires(zipFilePath, "zipFilePath").IsNotNullOrWhiteSpace();
            Condition.Requires(targetDirectory, "targetDirectory").IsNotNullOrWhiteSpace();

            targetDirectory = targetDirectory.AppendMissing(@"\");

            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("zipFilePath points to a file that does not exist on disk: " + zipFilePath);
            }

            if (!Directory.Exists(targetDirectory))
            {
                throw new DirectoryNotFoundException("targetDirectory was not found on disk: " + targetDirectory);
            }

            try
            {
                using (ZipFile zipFile = ZipFile.Read(zipFilePath))
                {
                    zipFile.ExtractAll(targetDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            catch (IOException ex)
            {
                // hack: workaround bug in DotNetZip where IOException is thrown instead of BadReadException
                if (ex.Message == "An attempt was made to move the file pointer before the beginning of the file.\r\n")
                {
                    throw new BadReadException("Failure when extracting zip file", ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Compresses files to memory.
        /// </summary>
        /// <remarks>
        /// The directory structure of the files is not preserved in the compressed return byte array.
        /// </remarks>
        /// <param name="files">ICollection of files to zip</param>
        /// <returns>
        /// Byte array containing the compressed zip.
        /// </returns>
        /// <exception cref="ArgumentNullException">files is null.</exception>
        /// <exception cref="ArgumentNullException">files is empty.</exception>
        /// <exception cref="ArgumentException">A file name in files isn't a valid file name.</exception>
        /// <exception cref="ArgumentException">A filepath in files doesn't exist.</exception>
        /// <exception cref="IOException">A file is locked.</exception>
        public static byte[] CompressFilesToMemory(ICollection<FileToZip> files)
        {
            Condition.Requires(files, "files").IsNotEmpty();
            var uniqueFileNames = new HashSet<string>();

            using (var compressed = new MemoryStream())
            {
                using (var zip = new ZipFile())
                {
                    foreach (FileToZip file in files)
                    {
                        string fileName = file.Name.Trim();
                        string filePath = file.Path;
                        if (!IsValidFileName(fileName))
                        {
                            throw new ArgumentException("File name '" + fileName + "' isn't valid.");
                        }

                        if (!File.Exists(filePath))
                        {
                            throw new ArgumentException("File '" + filePath + "' couldn't be found on disk.");
                        }

                        if (!uniqueFileNames.Add(fileName.ToLower(CultureInfo.CurrentCulture)))
                        {
                            throw new ArgumentException("The same file name is specified for two files: " + fileName);
                        }

                        zip.AddFile(filePath, @"\").FileName = fileName;
                    }

                    zip.Save(compressed);
                } // using zip
                return compressed.ToArray();
            } // using MemoryStream
        }

        /// <summary>
        /// Decompress the contents of an in-memory ZipFile to a folder.
        /// </summary>
        /// <remarks>
        /// Any directories contained in the zip will be ignored.  Files names should be specified
        /// without a path, otherwise method will fail.
        /// </remarks>
        /// <param name="zip">the byte array representing a ZipFile object.</param>
        /// <param name="outputFolder">folder to write files in zip.</param>
        /// <exception cref="ArgumentNullException">zip is null.</exception>
        /// <exception cref="ArgumentNullException">outputFolder is null.</exception>
        /// <exception cref="ArgumentException">outputFolder is whitespace.</exception>
        /// <exception cref="DirectoryNotFoundException">outputFolder doesn't exist on disk.</exception>
        /// <exception cref="ArgumentException">zip file has directories</exception>
        /// <exception cref="ArgumentException">A file name in the zip file causes the resulting path to be invalid (i.e. file name contains directories)</exception>
        /// <exception cref="ZipException">The zip file is malformed.</exception>
        public static void DecompressFilesFromMemory(byte[] zip, string outputFolder)
        {
            Condition.Requires(zip, "zip").IsNotNull();
            Condition.Requires(outputFolder, "outputFolder").IsNotNullOrWhiteSpace();
            if (!Directory.Exists(outputFolder))
            {
                throw new DirectoryNotFoundException("outputFolder does not exist: " + outputFolder);
            }

            // open the zip file from the zip stream
            using (var stream = new MemoryStream(zip))
            {
                using (ZipFile zipFile = ZipFile.Read(stream))
                {
                    zipFile.ExtractAll(outputFolder);
                }
            }
        }

        #endregion

        #region Create Resources

        /// <summary>
        /// Creates a temporary file in the Windows temporary folder.
        /// </summary>
        /// <returns>the filePath to a newly created temporary file.</returns>
        /// <exception cref="UnauthorizedAccessException">User doesn't have the proper access permissions.</exception>
        /// <exception cref="IOException">There are no temporary file names available, even after old files have been cleared.</exception>
        public static string CreateTemporaryFile()
        {
            lock (CreateTemporaryResourceLock)
            {
                try
                {
                    return Path.GetTempFileName();
                }
                catch (IOException)
                {
                    // no point to try again, Windows will always try to find the next available file
                    // so IOException means that there's none available
                    ClearTemporaryFiles(15);
                    return Path.GetTempFileName();
                }
            }
        }

        /// <summary>
        /// Creates a temporary file in a specified folder.
        /// </summary>
        /// <param name="rootFolder">folder in which to create the temporary file.</param>
        /// <returns>
        /// Returns the path to the temporary file that was created.
        /// </returns>
        /// <exception cref="ArgumentNullException">rootFolder is null.</exception>
        /// <exception cref="ArgumentException">rootFolder is whitespace or contains illegal characters.</exception>
        /// <exception cref="DirectoryNotFoundException">The rootFolder doesn't exist or disappears during the process.</exception>
        /// <exception cref="UnauthorizedAccessException">method can't access rootFolder to clear out older temporary files, or when the system doesn't have access permission to write the zero-byte file to rootFolder.</exception>
        /// <exception cref="PathTooLongException">rootFolder is greater than 248 characters or if the temporary file would exceed the character limit.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission to create a zero-byte file in the rootFolder.</exception>
        /// <exception cref="IOException">Could't create a temporary file.</exception>
        public static string CreateTemporaryFile(string rootFolder)
        {
            lock (CreateTemporaryResourceLock)
            {
                Condition.Requires(rootFolder, "rootFolder").IsNotNullOrWhiteSpace();
                if (string.IsNullOrEmpty(rootFolder.Trim()))
                {
                    throw new ArgumentException("rootFolder contains only whitespace.");
                }

                int attempt = 0;
                do
                {
                    string tempfilePath = rootFolder.AppendMissing(@"\") + MathHelper.RandomNumber(int.MaxValue).ToString(CultureInfo.CurrentCulture) + ".tmp";

                    if (File.Exists(tempfilePath))
                    {
                        attempt++;
                        if (attempt == 25)
                        {
                            ClearTemporaryFiles(rootFolder, 3);
                        }
                    }
                    else
                    {
                        if (CreateZeroByteFile(tempfilePath))
                        {
                            return tempfilePath;
                        }

                        throw new IOException("Couldn't create zero byte file " + tempfilePath);
                    }
                }
                while (attempt < 50);
                throw new IOException("Couldn't create zero byte file");
            } // lock _createTemporaryResourceLock
        }

        /// <summary>
        /// Saves a stream to a file.
        /// </summary>
        /// <param name="stream">stream containing data to save to file</param>
        /// <param name="filePath">location to write file to disk.</param>
        /// <exception cref="ArgumentNullException">stream is null.</exception>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace or has illegal characters, or referes to a non-file device such as "con:"</exception>
        /// <exception cref="NotSupportedException">stream does not support reading.</exception>
        /// <exception cref="SecurityException">caller does not have the required permission</exception>
        /// <exception cref="DirectoryNotFoundException">directory specified in filePath not found</exception>
        /// <exception cref="UnauthorizedAccessException">filePath is an existing file that's read-only or the caller doesn't have write access to the file or folder represented by filePath.</exception>
        /// <exception cref="PathTooLongException">filepath has too many characters</exception>
        /// <exception cref="ObjectDisposedException">Methods called on stream after it was closed.</exception>
        /// <remarks>
        /// If filePath already exists, it will be overwritten.
        /// </remarks>
        public static void SaveStreamToFile(Stream stream, string filePath)
        {
            Condition.Requires(stream, "stream").IsNotNull();
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();

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
        }

        #endregion // create resources

        #region Delete Files

        /// <summary>
        /// Deletes all files in a given folder.
        /// </summary>
        /// <param name="folder">Folder containing files to delete.</param>
        /// <exception cref="ArgumentNullException">folder is null.</exception>
        /// <exception cref="ArgumentException">folder is whitespace or contains illegal characters.</exception>
        /// <exception cref="IOException">folder is a file name, or if a file couldn't be deleted.</exception>
        /// <exception cref="UnauthorizedAccessException">caller doesn't have permission to browse files in the folder or delete a file in the folder</exception>
        /// <exception cref="PathTooLongException">Folder's path is too long or file within folder path is too long.</exception>
        /// <exception cref="DirectoryNotFoundException">folder wasn't found.</exception>
        /// <remarks>
        /// If a file couldn't be deleted, the delete process stops.  So folder may contain multiple files that haven't
        /// been deleted yet.
        /// </remarks>
        public static void DeleteAllFiles(string folder)
        {
            Condition.Requires(folder, "folder").IsNotNullOrWhiteSpace();

            // DONT append backslash - we don't want user accidentally deleting all files in the current working directory
            string[] files = Directory.GetFiles(folder);

            foreach (string filePath in files)
            {
                DeleteFile(filePath);
            }
        }

        /// <summary>
        /// Deletes a file from disk.
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace or contains one or more invalid characters.</exception>
        /// <exception cref="PathTooLongException">The filePath exceeds the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing the file doesn't exist.</exception>
        /// <exception cref="NotSupportedException">filePath is in an invalid format.</exception>       
        /// <exception cref="IOException">file couldn't be deleted.</exception>
        /// <exception cref="UnauthorizedAccessException">filePath is a directory, caller doesn't have the required permissions</exception>
        /// <remarks>
        /// No exception is thrown if file doesn't exist to begin with.
        /// </remarks>
        public static void DeleteFile(string filePath)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();

            try
            {
                try
                {
                    // don't call File.Exists first - we're optimizing for the case where the file
                    // does exist
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                    if (File.Exists(filePath))
                    {
                        // file might have been opened in a way that permits deleting
                        // but delete only happens after file handle is released by the holder
                        WaitForUnlock(filePath, 10);
                    }
                    else
                    {
                        // don't want to go back to disk...just return immediately.
                        return;
                    }
                }
                catch (FileNotFoundException)
                {
                    // file wasn't found
                    return;
                }
                catch (IOException)
                {
                    // file is in use and locked in a way that doesn't permit deleting
                    // wait for unlock and try again
                    if (WaitForUnlock(filePath, 10))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            catch (IOException)
            {
                // somehow file got unlocked, but then locked up again right before we could delete it
            }

            if (File.Exists(filePath))
            {
                throw new IOException("Couldn't delete file " + filePath);
            }
        }

        /// <summary>
        /// Deletes a files from disk that match a specified search pattern in the specified folder, using a value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="folder">The directory containing files to delete.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path. The parameter cannot end in two periods ("..") or contain two periods ("..") followed by DirectorySeparatorChar or AltDirectorySeparatorChar, nor can it contain any of the characters in InvalidPathChars.</param>
        /// <param name="searchOption">One of the SearchOption values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
        /// <exception cref="ArgumentNullException">folder is null.</exception>
        /// <exception cref="ArgumentException">folder is whitespace or contains illegal characters.</exception>
        /// <exception cref="ArgumentNullException">searchPattern is null.</exception>
        /// <exception cref="ArgumentException">searchPattern is whitespace.</exception>
        /// <exception cref="IOException">folder is a file name, or if a file couldn't be deleted.</exception>
        /// <exception cref="UnauthorizedAccessException">caller doesn't have permission to browse files in the folder or delete a file in the folder</exception>
        /// <exception cref="PathTooLongException">folder path is too long or file within folder path is too long.</exception>
        /// <exception cref="DirectoryNotFoundException">folder wasn't found.</exception>
        /// <exception cref="ArgumentException">searchPattern does not contain a valid pattern.</exception>
        public static void DeleteFiles(string folder, string searchPattern, SearchOption searchOption)
        {
            Condition.Requires(folder, "folder").IsNotNullOrWhiteSpace();
            Condition.Requires(searchPattern, "searchPattern").IsNotNullOrWhiteSpace();

            string[] files = Directory.GetFiles(folder, searchPattern, searchOption);
            foreach (string filePath in files)
            {
                DeleteFile(filePath);
            }
        }

        #endregion

        #region Information Gathering

        /// <summary>
        /// Counts the number of non-blank lines in a file
        /// </summary>
        /// <param name="filePath">file to count lines</param>
        /// <returns>number of non-blank lines in the file</returns>
        /// <exception cref="ArgumentNullException">filePath is null</exception>
        /// <exception cref="ArgumentException">filePath is empty, not in legal form, has illegal characters in path, or points to a Win32 device.</exception>
        /// <exception cref="FileNotFoundException">file wasn't found on disk.</exception>
        /// <exception cref="DirectoryNotFoundException">filePath path is invalid.</exception>
        /// <exception cref="PathTooLongException">filepath was too long.</exception>
        /// <exception cref="NotSupportedException">The filePath's format is not supported</exception>
        /// <exception cref="IOException">There's an IO error accessing file.</exception>
        public static long CountNonblankLinesInFile(string filePath)
        {
            long count = 0;

            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        count++;
                    }
                } // while not end of file
            }

            return count;
        }

        /// <summary>
        /// Gets the MD5 hash of a file
        /// </summary>
        /// <param name="filePath">file to calculate MD5</param>
        /// <returns>the MD5 hash</returns>
        /// <exception cref="ArgumentNullException">filePath is null</exception>
        /// <exception cref="ArgumentException">filePath is whitespace, contains invalid characters, or refers to a non-file device such as "con:", "com1:", etc.</exception>
        /// <exception cref="FileNotFoundException">File specified by filePath cannot be found.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing file specified by filePath could not be found or the filePath is a directory.</exception>
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions on file.</exception>
        /// <exception cref="PathTooLongException">filePath was too long.</exception>
        /// <exception cref="NotSupportedException">path is in an invalid format</exception>
        public static string MD5(string filePath)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();

            var md5Provider = new MD5CryptoServiceProvider();
            byte[] hash;

            using (Stream reader = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                hash = md5Provider.ComputeHash(reader);
            }

            return ToHexString(hash);
        }

        /// <summary>
        /// Returns the first line in a file.
        /// </summary>
        /// <param name="filePath">File to read.</param>
        /// <returns>String containing first line in file.  If file has no lines, empty string is returned.</returns>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace, contains invalid characters, or refers to a non-file device such as "con:", "com1:", etc.</exception>
        /// <exception cref="FileNotFoundException">File specified by filePath cannot be found.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing file specified by filePath could not be found or the filePath is a directory.</exception>
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions on file.</exception>
        /// <exception cref="PathTooLongException">filePath was too long.</exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
        /// <exception cref="NotSupportedException">path is in an invalid format</exception>
        public static string ReadHeaderLine(string filePath)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();
            using (var filestream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(filestream))
                {
                    if (reader.EndOfStream)
                    {
                        return string.Empty;
                    }

                    return reader.ReadLine();
                }
            }
        }

        /// <summary>
        /// Returns the first line in a file after the header line.
        /// </summary>
        /// <param name="filePath">File to read.</param>
        /// <returns>String containing first non-header line in file.  If file has only a header line then empty string is returned.</returns>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace, contains invalid characters, or refers to a non-file device such as "con:", "com1:", etc.</exception>
        /// <exception cref="FileNotFoundException">File specified by filePath cannot be found.</exception>
        /// <exception cref="InvalidOperationException">There is no header line.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing file specified by filePath could not be found or the filePath is a directory.</exception>
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions on file.</exception>
        /// <exception cref="PathTooLongException">filePath was too long.</exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
        /// <exception cref="NotSupportedException">path is in an invalid format</exception>
        public static string ReadFirstNonHeaderLine(string filePath)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();
            using (var filestream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(filestream))
                {
                    if (reader.EndOfStream)
                    {
                        throw new InvalidOperationException("There is no header line.");
                    }

                    reader.ReadLine();
                    if (reader.EndOfStream)
                    {
                        return string.Empty;
                    }

                    return reader.ReadLine();
                }
            }
        }

        /// <summary>
        /// Returns the last line in a file.
        /// </summary>
        /// <param name="filePath">file to read.</param>
        /// <returns>String containing last line of file.  If file has no lines then an empty string is returned.</returns>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace, contains invalid characters, or refers to a non-file device such as "con:", "com1:", etc.</exception>
        /// <exception cref="FileNotFoundException">File specified by filePath cannot be found.</exception>
        /// <exception cref="IOException">An I/O error occurs, such as when the file is locked.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing file specified by filePath could not be found or the filePath is a directory.</exception>
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions on file.</exception>
        /// <exception cref="PathTooLongException">filePath was too long.</exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
        /// <exception cref="NotSupportedException">path is in an invalid format</exception>
        public static string ReadLastLine(string filePath)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();
            using (var wholeStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                string text = null;
                long position = wholeStream.Length;

                while (position >= 0)
                {
                    wholeStream.Seek(position, SeekOrigin.Begin);

                    // Can't close nor dispose partialStream, it will cause fs to dispose prematurely
                    var partialStream = new StreamReader(wholeStream);

                    text = partialStream.ReadToEnd();
                    int endOfLineIndex = text.IndexOf(Environment.NewLine, StringComparison.CurrentCulture);
                    if (endOfLineIndex >= 0)
                    {
                        return text.Substring(endOfLineIndex + Environment.NewLine.Length);
                    }

                    position--;
                }

                // we get here if file has no lines or one line
                return text;
            } // using wholeStream
        }  // ReadLastLine

        /// <summary>
        /// Returns the last line in a file that's not blank.
        /// </summary>
        /// <param name="filePath">file to read.</param>
        /// <returns>String containing last line of file that's not blank.  If file has no lines then an empty string is returned.</returns>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace, contains invalid characters, or refers to a non-file device such as "con:", "com1:", etc.</exception>
        /// <exception cref="FileNotFoundException">File specified by filePath cannot be found.</exception>
        /// <exception cref="IOException">An I/O error occurs, such as when the file is locked.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing file specified by filePath could not be found or the filePath is a directory.</exception>
        /// <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions on file.</exception>
        /// <exception cref="PathTooLongException">filePath was too long.</exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
        /// <exception cref="NotSupportedException">path is in an invalid format</exception>
        /// <exception cref="InvalidOperationException">filePath points to a zero-byte file.</exception>
        public static string ReadLastNonblankLine(string filePath)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();

            using (var wholeStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                string text = null;
                long position = wholeStream.Length;
                int newLinesFound = 0;
                while (position >= 0)
                {
                    wholeStream.Seek(position, SeekOrigin.Begin);

                    // Can't close nor dispose partialStream, it will cause fs to dispose prematurely
                    var partialStream = new StreamReader(wholeStream);

                    text = partialStream.ReadToEnd();
                    int endOfLineIndex = text.IndexOf(Environment.NewLine, StringComparison.CurrentCulture);
                    if (endOfLineIndex >= 0)
                    {
                        string lineFound = text.Substring(endOfLineIndex + Environment.NewLine.Length);
                        var regex = new Regex(Environment.NewLine);
                        lineFound = regex.Replace(lineFound, string.Empty, newLinesFound);

                        if (string.IsNullOrEmpty(lineFound))
                        {
                            newLinesFound++;
                        }
                        else
                        {
                            return lineFound.Replace(Environment.NewLine, string.Empty);
                        }
                    }

                    position--;
                }

                // we get here if file has no lines or one line
                if (text == null)
                {
                    return string.Empty;
                }

                return text.Replace(Environment.NewLine, string.Empty);
            } // using wholeStream
        }  // ReadLastLine

        /// <summary>
        /// Reads all non-blank lines in a file to a list of string.
        /// </summary>
        /// <param name="filePath">file to read.</param>
        /// <returns>List of string containing all non-blank lines, or an empty List if there are none.</returns>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace, contains invalid characters, or refers to a non-file device such as "con:", "com1:", etc.</exception>
        /// <exception cref="PathTooLongException">filePath has too many characters.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive), filePath is actually a directory</exception>
        /// <exception cref="FileNotFoundException">The file specified in filePath was not found</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file, such as when the file is locked.</exception>
        /// <exception cref="UnauthorizedAccessException">caller doesn't have the required permissions</exception>
        /// <exception cref="NotSupportedException">path is in an invalid format</exception>
        /// <exception cref="SecurityException">the caller doesn't have the required permissions.</exception>
        public static ReadOnlyCollection<string> ReadAllNonblankLines(string filePath)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();
            string[] lines = File.ReadAllLines(filePath);
            var nonblankLines = lines.Where(line => !string.IsNullOrEmpty(line)).ToList();
            return new ReadOnlyCollection<string>(nonblankLines);
        }

        #endregion

        #region Legal and Illegal Files

        /// <summary>
        /// Replaces all illegal characters in a filename them a space character.
        /// Do not pass a file path because backslash will be replaced by a space.
        /// </summary>
        /// <param name="fileName">filename to evaluate</param>
        /// <returns>The legal filename.</returns>
        /// <exception cref="ArgumentNullException">filename is null.</exception>
        /// <exception cref="ArgumentException">filename is whitespace.</exception>
        public static string MakeLegalFileName(string fileName)
        {
            Condition.Requires(fileName, "fileName").IsNotNullOrWhiteSpace();
            char[] illegalCharacters = Path.GetInvalidFileNameChars();
            return illegalCharacters.Aggregate(fileName, (current, illegal) => current.Replace(illegal, ' '));
        }

        /// <summary>
        /// Determines if a file name is valid.
        /// </summary>
        /// <param name="fileName">The file name to validate.</param>
        /// <remarks>
        /// A file name is invalid if it contains illegal characters or contains an OS restricted term such as PRN
        /// </remarks>
        /// <returns>
        /// Returns true if the given file name is valid, false if not.
        /// </returns>
        /// <exception cref="ArgumentNullException">filename is null.</exception>
        /// <exception cref="ArgumentException">filename is whitespace.</exception>
        public static bool IsValidFileName(string fileName)
        {
            Condition.Requires(fileName, "fileName").IsNotNullOrWhiteSpace();
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
        public static bool IsValidFilePath(string filePath)
        {
            if (IsValidPath(filePath) && (!string.IsNullOrEmpty(Path.GetFileName(filePath))))
            {
                return true;
            }

            return false;
        }

        #endregion // legal and illegal files

        #region Locking and Writeability

        /// <summary>
        /// Determines if file can be written to.
        /// </summary>
        /// <param name="filePath">filePath to check.</param>
        /// <returns>True if the file can be written to, false if not.</returns>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace or contains one or more invalid characters.</exception>
        /// <exception cref="PathTooLongException">The filePath exceeds the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing the file doesn't exist.</exception>       
        /// <exception cref="NotSupportedException">filePath is in an invalid format.</exception>
        public static bool CanWriteToFile(string filePath)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();
            try
            {
                using (new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                {
                    return true;
                }
            }
            catch (FileNotFoundException)
            {
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                // File is read-only OR filePath is a directory OR the caller does not have the required permissions
                return false;
            }
            catch (IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if a file is in use (there's a file handle to the file).
        /// </summary>
        /// <param name="filePath">The file to check.</param>
        /// <returns>
        /// Returns true if the file is in use, false if not.
        /// </returns>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace or contains one or more invalid characters.</exception>
        /// <exception cref="PathTooLongException">The filePath exceeds the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing the file doesn't exist.</exception>
        /// <exception cref="NotSupportedException">filePath is in an invalid format.</exception>
        public static bool IsFileInUse(string filePath)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();
            try
            {
                using (File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return false;
                }
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                // filePath is a directory OR the caller does not have the required permissions
                return true;
            }
            catch (IOException)
            {
                // get more specific here?  here's what message should look like "Message = "The process cannot access the file 'c:\test.xml' because it is being used by another process."
                // this is typically what's thrown if file is locked
                return true;
            }
        }

        /// <summary>
        /// Waits a specified period of time for a file to become unlocked.
        /// </summary>
        /// <param name="filePath">path to file</param>
        /// <param name="timeoutSeconds">maximum number of seconds to wait for file to become unlocked.</param>
        /// <returns>True if file is unlocked.  False if not.</returns>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace or contains one or more invalid characters.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when timeoutSeconds &lt; 1</exception>
        /// <exception cref="PathTooLongException">The filePath exceeds the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing the file doesn't exist.</exception>       
        /// <exception cref="NotSupportedException">filePath is in an invalid format.</exception>
        public static bool WaitForUnlock(string filePath, int timeoutSeconds)
        {
            // argument check
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();
            Condition.Requires(timeoutSeconds, "timeoutSeconds").IsGreaterOrEqual(1);

            int elapsedSeconds = 0;

            do
            {
                if (!IsFileInUse(filePath))
                {
                    return true;
                }

                Thread.Sleep(1000);
                elapsedSeconds++;
            }
            while (elapsedSeconds <= timeoutSeconds);

            return false;
        }

        /// <summary>
        /// Waits a specified period of time for a file to become writable.
        /// </summary>
        /// <param name="filePath">path to file</param>
        /// <param name="timeoutSeconds">maximum number of seconds to wait for file to become writable.</param>
        /// <returns>True if file is writeable.  False if not.</returns>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace or contains one or more invalid characters.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when timeoutSeconds &lt; 1</exception>
        /// <exception cref="PathTooLongException">The filePath exceeds the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing the file doesn't exist.</exception>
        /// <exception cref="NotSupportedException">filePath is in an invalid format.</exception>
        public static bool WaitUntilFileIsWritable(string filePath, int timeoutSeconds)
        {
            // argument check
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();
            Condition.Requires(timeoutSeconds, "timeoutSeconds").IsGreaterOrEqual(1);
            int elapsedSeconds = 0;

            do
            {
                if (CanWriteToFile(filePath))
                {
                    return true;
                }

                Thread.Sleep(1000);
                elapsedSeconds++;
            }
            while (elapsedSeconds <= timeoutSeconds);

            return false;
        }

        #endregion

        #region Zero byte files

        /// <summary>
        /// Creates a zero-byte file.
        /// </summary>
        /// <param name="filePath">filePath to create.</param>
        /// <returns>True if zero-byte file was created.  False if not.</returns>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace or contains one or more invalid characters.</exception>
        /// <exception cref="NotSupportedException">filePath refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment</exception>
        /// <exception cref="IOException">An I/O error occurs, such as specifying FileMode.CreateNew and the file specified by filePath already exists</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission</exception>
        /// <exception cref="DirectoryNotFoundException">The specified filePath is invalid, such as being on an unmapped drive</exception>
        /// <exception cref="UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified path, such as when filePath points to a directory that the caller doesn't have write permission to</exception>
        /// <exception cref="PathTooLongException">The specified filePath exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        public static bool CreateZeroByteFile(string filePath)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();
            using (new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
            }

            return File.Exists(filePath) && IsFileSizeZero(filePath);
        }

        /// <summary>
        /// Determines if a file is zero-bytes in size.
        /// </summary>
        /// <param name="filePath">path to file to evaluate.</param>
        /// <returns>True if file is zero-byte, false if not</returns>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace or contains one or more invalid characters.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">Access to filePath is denied.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="NotSupportedException">filePath contains a colon (:) in the middle of the string.</exception>
        /// <exception cref="FileNotFoundException">The filePath doesn't exist or the filePath is a directory</exception>
        /// <exception cref="IOException">Couldn't get state of file.</exception>
        public static bool IsFileSizeZero(string filePath)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();
            var info = new FileInfo(filePath);
            return info.Length == 0;
        }

        #endregion // zero byte files

        #endregion

        #region Internal Methods

        /// <summary>
        /// Deletes a file using DOS
        /// </summary>
        /// <param name="filePath">file to delete</param>
        /// <exception cref="ArgumentNullException">filePath is null.</exception>
        /// <exception cref="ArgumentException">filePath is whitespace.</exception>
        /// <remarks>
        /// Won't delete hidden or system files, even if the /A attribute is used with the del command (that is only applicable to wildcards)
        /// Won't delete files that are in use.
        /// Should be used as a last-ditch effort.
        /// </remarks>
        internal static void DeleteFileDos(string filePath)
        {
            Condition.Requires(filePath, "filePath").IsNotNullOrWhiteSpace();
            var cmd = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Arguments = "/c del \"" + filePath + "\" /f"
                }
            };
            cmd.Start();
            cmd.WaitForExit();
        }

        /// <summary>
        /// Determines if a path is restricted by the operating system.
        /// </summary>
        /// <param name="path">path to a file or folder.</param>
        /// <returns>True if path is restricted, False if not.</returns>
        /// <remarks>
        /// Files with certain tokens are restricted.  It doesn't matter what the extension is.
        /// The OS seems to only care about the beginning part of the file.  so MyFile.con.txt is legit, whereas con.txt isn't.
        /// OS also restricts folder names in the same way files are restricted (i.e. no folder named "con"  or even "con.directory")
        /// </remarks>
        /// <exception cref="ArgumentNullException">path is null.</exception>
        /// <exception cref="ArgumentException">path is whitespace.</exception>
        internal static bool IsOsRestrictedPath(string path)
        {
            Condition.Requires(path, "path").IsNotNullOrWhiteSpace();

            path = path.ToUpper(CultureInfo.CurrentCulture);

            // get all terms that are separated by backslash
            string[] directories = path.Split(new[] { '\\' });

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
        internal static bool IsValidPath(string path)
        {
            try
            {
                // initialize with fully restricted permissions
                var permission = new FileIOPermission(PermissionState.None)
                {
                    AllFiles = FileIOPermissionAccess.PathDiscovery
                };

                // Declares that the calling code can access the resource protected by a permission demand through the code that calls this method, even if callers higher in the stack have not been granted permission to access the resource
                permission.Assert();
                try
                {
                    // ReSharper disable ReturnValueOfPureMethodIsNotUsed
                    Path.GetFullPath(path);
                    // ReSharper restore ReturnValueOfPureMethodIsNotUsed
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

        /// <summary>
        /// Converts an array of bytes to a uppercase hexidecimal string
        /// </summary>
        /// <param name="value">byte array to convert.</param>
        /// <returns>lowercase hex string representing the input byte array</returns>
        /// <exception cref="ArgumentNullException">value is null or empty.</exception>
        internal static string ToHexString(IList<byte> value)
        {
            Condition.Requires(value, "value").IsNotNull();

            var output = new StringBuilder(value.Count * 2);
            foreach (byte t in value)
            {
                output.Append(t.ToString("X2", CultureInfo.CurrentCulture));
            }

            return output.ToString().ToUpper(CultureInfo.CurrentCulture);
        }

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods

        #endregion
    }
}