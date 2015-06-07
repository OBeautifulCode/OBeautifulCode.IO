// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryHelperTest.cs" company="OBeautifulCode">
//   Copyright 2014 OBeautifulCode
// </copyright>
// <summary>
//   Tests the <see cref="DirectoryHelper"/> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO.Test
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using OBeautifulCode.Libs.String;

    using Xunit;

    /// <summary>
    /// Tests the <see cref="DirectoryHelper"/> class.
    /// </summary>
    /// <remarks>
    /// This class was ported from an older library that used a poor style of unit testing.
    /// It had a few monolithic test methods instead of many smaller, single purpose methods.
    /// Because of the volume of test code, I was only able to break-up a few of these monolithic tests.
    /// The rest remain as-is.
    /// </remarks>
    public class DirectoryHelperTest
    {
        #region Fields (Private)

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the ClearTemporaryFolders method
        /// </summary>
        [Fact]
        public static void ClearTemporaryFoldersTest()
        {
            // arguments and exceptions
            Assert.Throws<ArgumentException>(() => DirectoryHelper.ClearTemporaryFolders("  ", 5 * 24 * 60));
            Assert.Throws<ArgumentException>(() => DirectoryHelper.ClearTemporaryFolders("\r\n", 5 * 24 * 60));
            Assert.Throws<ArgumentException>(() => DirectoryHelper.ClearTemporaryFolders(string.Empty, 5 * 24 * 60));
            Assert.Throws<ArgumentNullException>(() => DirectoryHelper.ClearTemporaryFolders(null, 5 * 24 * 60));
            Assert.Throws<ArgumentOutOfRangeException>(() => DirectoryHelper.ClearTemporaryFolders(@"c:\test\", 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => DirectoryHelper.ClearTemporaryFolders(@"c:\test\", -4));
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryHelper.ClearTemporaryFolders(@"c:\directorythatdoesntexistClearTemporaryFoldersTest\", 5 * 24 * 60));
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryHelper.ClearTemporaryFolders(@"c:\test<\", 5 * 24 * 60));
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryHelper.ClearTemporaryFolders(@"c:\reallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldername", 4 * 24 * 60));
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryHelper.ClearTemporaryFolders("lpt:", 5 * 24 * 60));
            Assert.Throws<IOException>(() => DirectoryHelper.ClearTemporaryFolders(Directory.GetCurrentDirectory().AppendMissing(@"\") + @"..\", 100 * 24 * 60));

            // good way to test the follow?
            // <exception cref="UnauthorizedAccessException">Thrown when method can't access the directory</exception>
            // <exception cref="SecurityException">The caller does not have the required permissions.</exception

            // create a bunch of temporary folders, turn back the clock on them, then clear and ensure they get deleted
            var tempFolders = new List<string>();
            for (int x = 0; x < 10; x++)
            {
                string tempFolder = Path.GetTempFileName() + @".dir\";
                DirectoryHelper.DeleteFolder(tempFolder, true);
                Assert.True(Directory.Exists(tempFolder));

                Directory.SetCreationTime(tempFolder, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
                Directory.SetLastWriteTime(tempFolder, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
                Directory.SetLastAccessTime(tempFolder, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
                tempFolders.Add(tempFolder);
            }

            string unmodifiedTempFolder = Path.GetTempFileName() + @".dir\";
            DirectoryHelper.DeleteFolder(unmodifiedTempFolder, true);

            DirectoryHelper.ClearTemporaryFolders(Path.GetTempPath(), 101 * 24 * 60);  // keeping too many days
            foreach (string folder in tempFolders)
            {
                Assert.True(Directory.Exists(folder));
            }

            DirectoryHelper.ClearTemporaryFolders(Path.GetTempPath(), 100 * 24 * 60);  // keeping too many days
            foreach (string folder in tempFolders)
            {
                Assert.True(Directory.Exists(folder));
            }

            DirectoryHelper.ClearTemporaryFolders(Path.GetTempPath(), 99 * 24 * 60);  // folders are old enough to delete
            foreach (string folder in tempFolders)
            {
                Assert.False(Directory.Exists(folder));
            }

            Assert.True(Directory.Exists(unmodifiedTempFolder)); // this folder was created moments ago
            DirectoryHelper.DeleteFolder(unmodifiedTempFolder);

            // make a directory read-only, OS system files, and hidden    
            string tempFolderAttributes = Path.GetTempFileName() + @".dir\";
            DirectoryHelper.DeleteFolder(tempFolderAttributes, true);
            Assert.True(Directory.Exists(tempFolderAttributes));
            // ReSharper disable ObjectCreationAsStatement
            new DirectoryInfo(tempFolderAttributes) { Attributes = FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden };
            // ReSharper restore ObjectCreationAsStatement
            Directory.SetCreationTime(tempFolderAttributes, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
            Directory.SetLastWriteTime(tempFolderAttributes, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
            Directory.SetLastAccessTime(tempFolderAttributes, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
            Assert.DoesNotThrow(() => DirectoryHelper.ClearTemporaryFolders(Path.GetTempPath(), 99 * 24 * 60));  // folders are old enough to delete
            Assert.False(Directory.Exists(tempFolderAttributes));

            // make folder with file in use (if file is in use, won't the last access be something recent?)
            string tempFolderFileLock = Path.GetTempFileName() + @".dir\";
            DirectoryHelper.DeleteFolder(tempFolderFileLock, true);
            Assert.True(Directory.Exists(tempFolderFileLock));
            string tempFile = tempFolderFileLock + "1.txt";
            File.WriteAllText(tempFile, "whatever");
            Assert.True(File.Exists(tempFile));

            using (new StreamReader(tempFile))
            {
                Directory.SetCreationTime(tempFolderFileLock, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
                Directory.SetLastWriteTime(tempFolderFileLock, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
                Directory.SetLastAccessTime(tempFolderFileLock, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));

                Assert.DoesNotThrow(() => DirectoryHelper.ClearTemporaryFolders(Path.GetTempPath(), 99 * 24 * 60));
                Assert.True(Directory.Exists(tempFolderFileLock));
            }

            // reset in case last access was affected by streamreader
            Directory.SetCreationTime(tempFolderFileLock, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
            Directory.SetLastWriteTime(tempFolderFileLock, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
            Directory.SetLastAccessTime(tempFolderFileLock, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
            DirectoryHelper.ClearTemporaryFolders(Path.GetTempPath(), 99 * 24 * 60);
            Assert.False(Directory.Exists(tempFolderFileLock));
        }

        /// <summary>
        /// Tests the CreateTemporaryFolder method
        /// </summary>
        [Fact]
        public static void CreateTemporaryFolderTest()
        {
            // arguments & exceptions
            Assert.Throws<ArgumentException>(() => DirectoryHelper.CreateTemporaryFolder("   "));
            Assert.Throws<ArgumentException>(() => DirectoryHelper.CreateTemporaryFolder("\r\n"));
            Assert.Throws<ArgumentException>(() => DirectoryHelper.CreateTemporaryFolder(string.Empty));
            Assert.Throws<ArgumentNullException>(() => DirectoryHelper.CreateTemporaryFolder(null));
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryHelper.CreateTemporaryFolder(@"c:\directorythatdoesntexistCreateTemporaryFolderTest\"));
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryHelper.CreateTemporaryFolder(@"c:\test<\"));
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryHelper.CreateTemporaryFolder(@"lpt:"));
            Assert.Throws<DirectoryNotFoundException>(() => DirectoryHelper.CreateTemporaryFolder(@"c:\pathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolong"));
            Assert.Throws<IOException>(() => DirectoryHelper.CreateTemporaryFolder(Directory.GetCurrentDirectory()));

            // how can we replicate these?
            // <exception cref="SecurityException">The caller does not have the required permissions.</exception>       
            // <exception cref="UnauthorizedAccessException">The caller does not have the required permission to create a folder under rootFolder.</exception>

            // no good way to test whether old folders get cleared out
            // <exception cref="IOException">could't create a temporary folder</exception>

            // create a root temporary folder to put all the others in
            string tempFolderRoot = Path.GetTempFileName() + @".dir\";
            DirectoryHelper.DeleteFolder(tempFolderRoot, true);
            Assert.True(Directory.Exists(tempFolderRoot));
            // ReSharper disable ObjectCreationAsStatement
            new DirectoryInfo(tempFolderRoot) { Attributes = FileAttributes.ReadOnly };
            // ReSharper restore ObjectCreationAsStatement

            // create 10,000 temporary folders.  no IOException, no repeat folders
            var folders = new HashSet<string>();

            // ReSharper disable JoinDeclarationAndInitializer
            int totalFolders;
            // ReSharper restore JoinDeclarationAndInitializer
            totalFolders = 500;
            for (int x = 0; x < totalFolders; x++)
            {
                string folder = DirectoryHelper.CreateTemporaryFolder(tempFolderRoot);
                Assert.True(folder[folder.Length - 1] == '\\');
                Assert.True(Directory.Exists(folder));
                Assert.Equal(0, Directory.GetFiles(folder).Length);
                Assert.True(folders.Add(folder));
            }

            // now check that all created folders are in the specified tempFolderRoot
            var foldersInRoot = new HashSet<string>(Directory.GetDirectories(tempFolderRoot));

            // get directories doesn't append trailing backslash
            foldersInRoot = new HashSet<string>(foldersInRoot.Select(folder => folder.AppendMissing(@"\")));
            foreach (string createdFolder in folders)
            {
                Assert.True(foldersInRoot.Contains(createdFolder, StringComparer.CurrentCultureIgnoreCase));
            }

            // cleanup
            DirectoryHelper.DeleteFolder(tempFolderRoot);
        }

        /// <summary>
        /// Tests the DeleteFolder method (all overloads)
        /// </summary>
        [Fact]
        public static void DeleteFolderTest()
        {
            // test arguments & exceptions
            // ArgumentException - how do we test a relative path that can't be made absolute?
            // no good way to tests UnauthorizedAccessException & SecurityException
            Assert.Throws<ArgumentNullException>(() => DirectoryHelper.DeleteFolder(null));
            Assert.Throws<ArgumentException>(() => DirectoryHelper.DeleteFolder(string.Empty));
            Assert.Throws<ArgumentException>(() => DirectoryHelper.DeleteFolder("   "));
            Assert.Throws<ArgumentException>(() => DirectoryHelper.DeleteFolder(@"c:\abcd<"));
            Assert.Throws<IOException>(() => DirectoryHelper.DeleteFolder(Directory.GetCurrentDirectory().AppendMissing(@"\") + @"..\"));
            Assert.Throws<PathTooLongException>(() => DirectoryHelper.DeleteFolder(@"c:\TooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPathTooLongOfAPath"));
            Assert.Throws<NotSupportedException>(() => DirectoryHelper.DeleteFolder("lpt:"));

            // simple test - create a folder, delete it
            string windowsTempFolder = Path.GetTempPath().AppendMissing(@"\");
            string tempFolder = windowsTempFolder + "DeleteFolderTest";
            Directory.CreateDirectory(tempFolder);
            Assert.True(Directory.Exists(tempFolder));
            DirectoryHelper.DeleteFolder(tempFolder);
            Assert.False(Directory.Exists(tempFolder));

            // add a folder, file, subfolder, file
            // make folder read-only, then delete folder
            tempFolder = tempFolder + @"\";
            Directory.CreateDirectory(tempFolder);
            // ReSharper disable ObjectCreationAsStatement
            new DirectoryInfo(tempFolder) { Attributes = FileAttributes.ReadOnly };
            // ReSharper restore ObjectCreationAsStatement
            Assert.True(Directory.Exists(tempFolder));
            DirectoryHelper.DeleteFolder(tempFolder);
            Assert.False(Directory.Exists(tempFolder));

            // add a folder, file, subfolder, file
            // make read-only system, hidden files/folders
            // then delete folder (with recreate option)
            Directory.CreateDirectory(tempFolder);
            Assert.True(Directory.Exists(tempFolder));
            string tempFile = tempFolder + "test.txt";
            using (var writer = new StreamWriter(tempFile))
            {
                writer.WriteLine("wrote some text");
            }

            Assert.True(File.Exists(tempFile));
            string tempFolderEmbed = tempFolder + @"embed\";
            Directory.CreateDirectory(tempFolderEmbed);
            Assert.True(Directory.Exists(tempFolderEmbed));
            string tempFileEmbed = tempFolderEmbed + "embed.txt";
            using (var writer = new StreamWriter(tempFileEmbed))
            {
                writer.WriteLine("wrote some more text");
            }

            Assert.True(File.Exists(tempFileEmbed));

            // make files/directories read-only, OS system files, and hidden
            // delete and recreate
            File.SetAttributes(tempFile, FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden);
            File.SetAttributes(tempFileEmbed, FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden);

            // ReSharper disable ObjectCreationAsStatement
            new DirectoryInfo(tempFolder) { Attributes = FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden };
            new DirectoryInfo(tempFolderEmbed) { Attributes = FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden };
            // ReSharper restore ObjectCreationAsStatement            
            Assert.True(Directory.Exists(tempFolder));
            DirectoryHelper.DeleteFolder(tempFolder, true);
            Assert.True(Directory.Exists(tempFolder));
            Assert.False(Directory.Exists(tempFolderEmbed));
            Assert.False(File.Exists(tempFileEmbed));
            Assert.False(File.Exists(tempFile));

            // try to delete a folder where file in folder is being used
            // shouldn't be able to
            Directory.CreateDirectory(tempFolder);
            Assert.True(Directory.Exists(tempFolder));
            using (var writer = new StreamWriter(tempFile))
            {
                writer.WriteLine("wrote some text");
            }

            Assert.True(File.Exists(tempFile));

            using (new StreamReader(tempFile))
            {
                Assert.Throws<IOException>(() => DirectoryHelper.DeleteFolder(tempFolder));
                Assert.True(Directory.Exists(tempFolder));
            }

            DirectoryHelper.DeleteFolder(tempFolder);
            Assert.False(Directory.Exists(tempFolder));

            // delete folder that doesn't exist, then recreate it
            Assert.False(Directory.Exists(tempFolder));
            DirectoryHelper.DeleteFolder(tempFolder, true);
            Assert.True(Directory.Exists(tempFolder));
            Directory.Delete(tempFolder);

            // anyway to get folder to delete but not recreate (resulting in IOException)?
        }

        /// <summary>
        /// Tests the DeleteFolderDos method
        /// </summary>
        [Fact]
        public static void DeleteFolderDosTest()
        {
            // arguments
            Assert.Throws<ArgumentNullException>(() => DirectoryHelper.DeleteFolderDos(null));
            Assert.Throws<ArgumentException>(() => DirectoryHelper.DeleteFolderDos(string.Empty));

            // first create a dosdeletetest folder
            string windowsTempFolder = Path.GetTempPath().AppendMissing(@"\");
            string tempFolder = windowsTempFolder + "dosdeletetest";

            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            Assert.True(Directory.Exists(tempFolder));
            DirectoryHelper.DeleteFolderDos(tempFolder);
            Assert.False(Directory.Exists(tempFolder));

            // add a folder, file, subfolder, file
            // make read-only, system, hidden files/folders
            // then delete folder
            tempFolder = tempFolder + @"\";
            Directory.CreateDirectory(tempFolder);
            Assert.True(Directory.Exists(tempFolder));
            string tempFile = tempFolder + "test.txt";
            using (var writer = new StreamWriter(tempFile))
            {
                writer.WriteLine("wrote some text");
            }

            Assert.True(File.Exists(tempFile));
            string tempFolderEmbed = tempFolder + @"embed\";
            Directory.CreateDirectory(tempFolderEmbed);
            Assert.True(Directory.Exists(tempFolderEmbed));
            string tempFileEmbed = tempFolderEmbed + "embed.txt";
            using (var writer = new StreamWriter(tempFileEmbed))
            {
                writer.WriteLine("wrote some more text");
            }

            Assert.True(File.Exists(tempFileEmbed));

            // make files/directories read-only, OS system files, and hidden
            File.SetAttributes(tempFile, FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden);
            File.SetAttributes(tempFileEmbed, FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden);
            // ReSharper disable ObjectCreationAsStatement
            new DirectoryInfo(tempFolder) { Attributes = FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden };
            new DirectoryInfo(tempFolderEmbed) { Attributes = FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden };
            // ReSharper restore ObjectCreationAsStatement
            Assert.True(Directory.Exists(tempFolder));
            DirectoryHelper.DeleteFolderDos(tempFolder);
            Assert.False(Directory.Exists(tempFolder));
        }

        /// <summary>
        /// Tests the IsFolderInWorkingDirectory method
        /// </summary>
        [Fact]
        public static void IsFolderInWorkingDirectoryTest()
        {
            Assert.Throws<ArgumentException>(() => DirectoryHelper.IsFolderInWorkingDirectory(string.Empty));
            Assert.Throws<ArgumentNullException>(() => DirectoryHelper.IsFolderInWorkingDirectory(null));
            Assert.Throws<ArgumentException>(() => DirectoryHelper.IsFolderInWorkingDirectory(@"C:\test>\"));
            Assert.Throws<NotSupportedException>(() => DirectoryHelper.IsFolderInWorkingDirectory(@"lpt:"));
            Assert.Throws<PathTooLongException>(() => DirectoryHelper.IsFolderInWorkingDirectory(@"C:\pathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolong\"));

            Assert.DoesNotThrow(() => DirectoryHelper.IsFolderInWorkingDirectory(@"whatever"));
            Assert.DoesNotThrow(() => DirectoryHelper.IsFolderInWorkingDirectory(@"c:\directorythatdoesntexistIsFolderInWorkingDirectoryTest"));
            Assert.DoesNotThrow(() => DirectoryHelper.IsFolderInWorkingDirectory(@"c:\wierd\path\.\..\that\may\not\.\.\notexist\"));
            Assert.DoesNotThrow(() => DirectoryHelper.IsFolderInWorkingDirectory(@"c:\anotherwierdpath.\"));

            Assert.True(DirectoryHelper.IsFolderInWorkingDirectory(Path.GetPathRoot(Directory.GetCurrentDirectory())));
            Assert.False(DirectoryHelper.IsFolderInWorkingDirectory(@"c:\SomeOutsideDirectory"));
            Assert.True(DirectoryHelper.IsFolderInWorkingDirectory(Directory.GetCurrentDirectory()));
            Assert.True(DirectoryHelper.IsFolderInWorkingDirectory(Directory.GetCurrentDirectory().AppendMissing(@"\")));
            Assert.True(DirectoryHelper.IsFolderInWorkingDirectory(Directory.GetCurrentDirectory().AppendMissing(@"\").ToLower(CultureInfo.CurrentCulture)));
            Assert.True(DirectoryHelper.IsFolderInWorkingDirectory(Directory.GetCurrentDirectory().AppendMissing(@"\").ToUpper(CultureInfo.CurrentCulture)));
            Assert.True(DirectoryHelper.IsFolderInWorkingDirectory(Directory.GetCurrentDirectory().AppendMissing(@"\") + @"..\"));
            Assert.False(DirectoryHelper.IsFolderInWorkingDirectory(Directory.GetCurrentDirectory().AppendMissing(@"\") + @"..\SomeOtherDir"));

            // how to test a relative path that couldn't be made absolute?
            // <exception cref="ArgumentException">Thrown when temporaryFolder is null or empty, or a relative path couldn't be made absolute.</exception>

            // how can we reproduce this?
            // <exception cref="SecurityException">The caller does not have the required permissions.</exception>              
        }

        /// <summary>
        /// Tests the IsValidPath method
        /// </summary>
        [Fact]
        public static void IsValidDirectoryPathTest()
        {
            // bad filepaths
            Assert.False(DirectoryHelper.IsValidDirectoryPath(null));
            Assert.False(DirectoryHelper.IsValidDirectoryPath(string.Empty));
            Assert.False(DirectoryHelper.IsValidDirectoryPath("    "));
            Assert.False(DirectoryHelper.IsValidDirectoryPath(@"c:\test>.txt"));
            Assert.False(DirectoryHelper.IsValidDirectoryPath(@"c:\baddir>s\test.txt"));
            Assert.False(DirectoryHelper.IsValidDirectoryPath(@"c:\te:st.txt"));
            Assert.False(DirectoryHelper.IsValidDirectoryPath(@"c:\bad:irs\test.txt"));
            Assert.False(DirectoryHelper.IsValidDirectoryPath("c:\folderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolong\test.txt"));
            Assert.False(DirectoryHelper.IsValidDirectoryPath(Path.GetTempPath().AppendMissing(@"\") + "filethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolong.txt"));

            // good filepaths
            Assert.True(DirectoryHelper.IsValidDirectoryPath(Path.GetTempPath()));
            Assert.False(DirectoryHelper.IsValidDirectoryPath(Path.GetTempFileName()));

            // files/folders that don't exist should still be valid
            string tempFilepath = Path.GetTempFileName();
            Assert.False(File.Exists(tempFilepath + ".unknown"));
            Assert.False(DirectoryHelper.IsValidDirectoryPath(tempFilepath + ".unknown"));
            Assert.True(DirectoryHelper.IsValidDirectoryPath(tempFilepath + @".unknown\"));
            Assert.True(DirectoryHelper.IsValidDirectoryPath(@"c:\folderthatdoesntexistCheckValidFilePath\"));
            Assert.False(DirectoryHelper.IsValidDirectoryPath(@"c:\folderthatdoesntexistCheckValidFilePath\filethatdoesntexistCheckValidFilePath.txt"));
            Assert.False(DirectoryHelper.IsValidDirectoryPath(@"x:\drivethatdoesntexist"));
            Assert.True(DirectoryHelper.IsValidDirectoryPath(@"con:"));

            // restricted os files
            Assert.False(DirectoryHelper.IsValidDirectoryPath(@"c:\lpt1.folder\"));
            Assert.False(DirectoryHelper.IsValidDirectoryPath(@"c:\nul\"));
            Assert.False(DirectoryHelper.IsValidDirectoryPath(@"c:\folder\otherfolder\con\"));

            // how about directories with leading or training spaces?  are those just ignored by the OS? (i.e. they OK)
        }

        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods

        #endregion
    }
}
