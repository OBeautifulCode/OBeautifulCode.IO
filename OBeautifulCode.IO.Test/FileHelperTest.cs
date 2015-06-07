// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHelperTest.cs" company="OBeautifulCode">
//   Copyright 2014 OBeautifulCode
// </copyright>
// <summary>
//   Tests the <see cref="FileHelper"/> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    using Ionic.Zip;

    using OBeautifulCode.IO;
    using OBeautifulCode.Math;
    using OBeautifulCode.Reflection;
    using OBeautifulCode.String;

    using Xunit;

    /// <summary>
    /// Tests the <see cref="FileHelper"/> class.
    /// </summary>
    /// <remarks>
    /// This class was ported from an older library that used a poor style of unit testing.
    /// It had a few monolithic test methods instead of many smaller, single purpose methods.
    /// Because of the volume of test code, I was only able to break-up a few of these monolithic tests.
    /// The rest remain as-is.
    /// </remarks>
    public class FileHelperTest
    {
        #region Fields (Private)

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        // ReSharper disable InconsistentNaming
        #region Alter and Write to Files

        /// <summary>
        /// Tests the MergeFiles method
        /// </summary>
        [Fact]
        public static void MergeFilesTest()
        {
            // arguments and exceptions
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles(string.Empty, Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), string.Empty, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<ArgumentNullException>(() => FileHelper.MergeFiles(null, Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<ArgumentNullException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), null, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles(@"c:\test<\file.txt", Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), @"c:\test<\file.txt", FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles("con:", Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), "con:", FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles("con:", Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), "con:", FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles("     ", Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), "     ", FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<ArgumentNullException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, null));
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, string.Empty));
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, "    "));
            Assert.Throws<ArgumentException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, @"c:\test<\file.txt"));
            Assert.Throws<FileNotFoundException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), Path.GetTempFileName() + ".notfound", FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<FileNotFoundException>(() => FileHelper.MergeFiles(Path.GetTempFileName() + ".notfound", Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<NotSupportedException>(() => FileHelper.MergeFiles(@"c:\pip:es.txt", Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<NotSupportedException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), @"c:\pip:es.txt", FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<NotSupportedException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, @"c:\pip:es.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.MergeFiles(Path.GetTempPath().AppendMissing(@"\") + @"directorythatdoesntexistMergeFilesTest\test.txt", Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), Path.GetTempPath().AppendMissing(@"\") + @"directorythatdoesntexistMergeFilesTest\test.txt", FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), Path.GetTempPath(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.MergeFiles(Path.GetTempPath(), Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, Path.GetTempPath().AppendMissing(@"\") + @"directorythatdoesntexistMergeFilesTest\test.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, Path.GetTempPath()));
            Assert.Throws<PathTooLongException>(() => FileHelper.MergeFiles(@"c:\paththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolong\test.txt", Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<PathTooLongException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), @"c:\paththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolongpaththatstoolong\test.txt", FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
            Assert.Throws<PathTooLongException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, @"c:\pathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolong\test.txt"));

            string newFile = Path.GetTempFileName();
            string topFile = Path.GetTempFileName();
            string bottomFile = Path.GetTempFileName();
            File.SetAttributes(newFile, FileAttributes.ReadOnly);
            File.SetAttributes(topFile, FileAttributes.ReadOnly);
            File.SetAttributes(bottomFile, FileAttributes.ReadOnly);
            Assert.Throws<UnauthorizedAccessException>(() => FileHelper.MergeFiles(topFile, Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, newFile));
            Assert.Throws<UnauthorizedAccessException>(() => FileHelper.MergeFiles(topFile, Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile));
            Assert.Throws<UnauthorizedAccessException>(() => FileHelper.MergeFiles(topFile, Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, Path.GetTempFileName()));
            Assert.DoesNotThrow(() => FileHelper.MergeFiles(Path.GetTempFileName(), bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, Path.GetTempFileName()));
            Assert.DoesNotThrow(() => FileHelper.MergeFiles(Path.GetTempFileName(), bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, Path.GetTempFileName()));

            newFile = Path.GetTempFileName();
            topFile = Path.GetTempFileName();
            bottomFile = Path.GetTempFileName();

            using (new FileStream(newFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                using (new FileStream(topFile, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (new FileStream(bottomFile, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        Assert.Throws<IOException>(() => FileHelper.MergeFiles(topFile, Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
                        Assert.Throws<IOException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null));
                        Assert.Throws<IOException>(() => FileHelper.MergeFiles(Path.GetTempFileName(), Path.GetTempFileName(), FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile));
                    }
                }
            }

            // how do we test these?
            // <exception cref="SecurityException">The caller does not have the required permission to access topFilepath or bottomFilepath</exception>
            // <exception cref="SecurityException">The caller does not have the required permission to write to topFilepath or newFilepath depending on the MergeMethod.</exception>
            // <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions to topFilepath or bottomFilepath</exception>
            // <exception cref="UnauthorizedAccessException">Caller doesn't have write permission to either newFilepath or topFilepath depending on MethodMethod</exception>            
            // <exception cref="IOException">I/O error writing to topFilepath or newFilepath depending on MergeMethod.</exception>

            // zero-byte files
            topFile = Path.GetTempFileName();
            bottomFile = Path.GetTempFileName();
            newFile = Path.GetTempFileName();
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal(string.Empty, File.ReadAllText(newFile));
            newFile = Path.GetTempFileName();
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal(string.Empty, File.ReadAllText(newFile));
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal(string.Empty, File.ReadAllText(topFile));
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal(string.Empty, File.ReadAllText(topFile));

            // top file is zero-byte, second file has 1-line of text
            topFile = Path.GetTempFileName();
            bottomFile = Path.GetTempFileName();
            newFile = Path.GetTempFileName();
            File.WriteAllText(bottomFile, "one line in bottom file");
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("one line in bottom file", File.ReadAllText(newFile));
            newFile = Path.GetTempFileName();
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal(string.Empty, File.ReadAllText(newFile));
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("one line in bottom file", File.ReadAllText(topFile));
            topFile = Path.GetTempFileName();
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal(string.Empty, File.ReadAllText(topFile));

            // top file has one line, second file is zero-byte
            topFile = Path.GetTempFileName();
            bottomFile = Path.GetTempFileName();
            newFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "one line in top file");
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("one line in top file", File.ReadAllText(newFile));
            newFile = Path.GetTempFileName();
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("one line in top file", File.ReadAllText(newFile));
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("one line in top file", File.ReadAllText(topFile));
            topFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "one line in top file");
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("one line in top file", File.ReadAllText(topFile));

            // both files have one line
            topFile = Path.GetTempFileName();
            bottomFile = Path.GetTempFileName();
            newFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "one line in top file");
            File.WriteAllText(bottomFile, "one line in bottom file");
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("one line in top file" + Environment.NewLine + "one line in bottom file", File.ReadAllText(newFile));
            newFile = Path.GetTempFileName();
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("one line in top file", File.ReadAllText(newFile));
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("one line in top file" + Environment.NewLine + "one line in bottom file", File.ReadAllText(topFile));
            topFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "one line in top file");
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("one line in top file", File.ReadAllText(topFile));

            // both files have one line, top file ends with a new line
            topFile = Path.GetTempFileName();
            bottomFile = Path.GetTempFileName();
            newFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "one line in top file" + Environment.NewLine);
            File.WriteAllText(bottomFile, "one line in bottom file");
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("one line in top file" + Environment.NewLine + "one line in bottom file", File.ReadAllText(newFile));
            newFile = Path.GetTempFileName();
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("one line in top file" + Environment.NewLine, File.ReadAllText(newFile));
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("one line in top file" + Environment.NewLine + "one line in bottom file", File.ReadAllText(topFile));
            topFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "one line in top file" + Environment.NewLine);
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("one line in top file" + Environment.NewLine, File.ReadAllText(topFile));

            // both files have one line, bottom file ends with a new line
            topFile = Path.GetTempFileName();
            bottomFile = Path.GetTempFileName();
            newFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "one line in top file");
            File.WriteAllText(bottomFile, "one line in bottom file" + Environment.NewLine);
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("one line in top file" + Environment.NewLine + "one line in bottom file" + Environment.NewLine, File.ReadAllText(newFile));
            newFile = Path.GetTempFileName();
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("one line in top file", File.ReadAllText(newFile));
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("one line in top file" + Environment.NewLine + "one line in bottom file" + Environment.NewLine, File.ReadAllText(topFile));
            topFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "one line in top file");
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("one line in top file", File.ReadAllText(topFile));

            // two lines each
            topFile = Path.GetTempFileName();
            bottomFile = Path.GetTempFileName();
            newFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "first line in top file" + Environment.NewLine + "second line in top file");
            File.WriteAllText(bottomFile, "first line in bottom file" + Environment.NewLine + "second line in bottom file");
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine + "first line in bottom file" + Environment.NewLine + "second line in bottom file", File.ReadAllText(newFile));
            newFile = Path.GetTempFileName();
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine + "second line in bottom file", File.ReadAllText(newFile));
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine + "first line in bottom file" + Environment.NewLine + "second line in bottom file", File.ReadAllText(topFile));
            topFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "first line in top file" + Environment.NewLine + "second line in top file");
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine + "second line in bottom file", File.ReadAllText(topFile));

            // two lines each with lagging newlines
            topFile = Path.GetTempFileName();
            bottomFile = Path.GetTempFileName();
            newFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine);
            File.WriteAllText(bottomFile, "first line in bottom file" + Environment.NewLine + "second line in bottom file" + Environment.NewLine);
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine + "first line in bottom file" + Environment.NewLine + "second line in bottom file" + Environment.NewLine, File.ReadAllText(newFile));
            newFile = Path.GetTempFileName();
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine + "second line in bottom file" + Environment.NewLine, File.ReadAllText(newFile));
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine + "first line in bottom file" + Environment.NewLine + "second line in bottom file" + Environment.NewLine, File.ReadAllText(topFile));
            topFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine);
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoTopFile, null);
            Assert.Equal("first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine + "second line in bottom file" + Environment.NewLine, File.ReadAllText(topFile));

            // bottom file has one line with leading newline
            topFile = Path.GetTempFileName();
            bottomFile = Path.GetTempFileName();
            newFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "first line in top file" + Environment.NewLine + "second line in top file");
            File.WriteAllText(bottomFile, Environment.NewLine + "first line in bottom file");
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine + Environment.NewLine + "first line in bottom file", File.ReadAllText(newFile));
            newFile = Path.GetTempFileName();
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine + "first line in bottom file", File.ReadAllText(newFile));

            // bottom file has one line with leading newline, top file has lagging newline
            topFile = Path.GetTempFileName();
            bottomFile = Path.GetTempFileName();
            newFile = Path.GetTempFileName();
            File.WriteAllText(topFile, "first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine);
            File.WriteAllText(bottomFile, Environment.NewLine + "first line in bottom file");
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine + Environment.NewLine + "first line in bottom file", File.ReadAllText(newFile));
            newFile = Path.GetTempFileName();
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("first line in top file" + Environment.NewLine + "second line in top file" + Environment.NewLine + "first line in bottom file", File.ReadAllText(newFile));

            // big files.  both have headers, both end in new lines
            var topFileRandomText = new StringBuilder();
            var bottomFileRandomText = new StringBuilder();
            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    topFileRandomText.Append(MathHelper.RandomNumber(int.MaxValue));
                    topFileRandomText.Append(",");
                    bottomFileRandomText.Append(MathHelper.RandomNumber(int.MinValue, int.MaxValue));
                    bottomFileRandomText.Append(",");
                }

                topFileRandomText.Append(Environment.NewLine);
                bottomFileRandomText.Append(Environment.NewLine);
            }

            topFile = Path.GetTempFileName() + ".big";
            bottomFile = Path.GetTempFileName() + ".big";
            newFile = Path.GetTempFileName() + ".big";
            File.WriteAllText(topFile, "header" + Environment.NewLine + topFileRandomText);
            File.WriteAllText(bottomFile, "header" + Environment.NewLine + bottomFileRandomText);
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.DeleteBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("header" + Environment.NewLine + topFileRandomText + bottomFileRandomText, File.ReadAllText(newFile));
            newFile = Path.GetTempFileName() + ".big";
            FileHelper.MergeFiles(topFile, bottomFile, FileMergeHeaderTreatment.KeepBottomFileHeader, FileMergeMethod.MergeIntoNewFile, newFile);
            Assert.Equal("header" + Environment.NewLine + topFileRandomText + "header" + Environment.NewLine + bottomFileRandomText, File.ReadAllText(newFile));
        }

        /// <summary>
        /// Tests the ReplaceHeader method
        /// </summary>
        [Fact]
        public static void ReplaceHeaderTest()
        {
            // arguments & exceptions
            Assert.Throws<ArgumentException>(() => FileHelper.ReplaceHeader(string.Empty, "header"));
            Assert.Throws<ArgumentNullException>(() => FileHelper.ReplaceHeader(null, "header"));
            Assert.Throws<ArgumentException>(() => FileHelper.ReplaceHeader("     ", "header"));
            Assert.Throws<ArgumentException>(() => FileHelper.ReplaceHeader(@"c:\test<\file.txt", "header"));
            Assert.Throws<ArgumentException>(() => FileHelper.ReplaceHeader("con:", "header"));
            Assert.Throws<FileNotFoundException>(() => FileHelper.ReplaceHeader(Path.GetTempPath().AppendMissing(@"\") + "filethatdoesntexistReplaceHeaderTest.txt", "new header"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ReplaceHeader(@"c:\directorythatdoesntexistReplaceHeaderTest\test.txt", "new header"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ReplaceHeader(Path.GetTempPath().AppendMissing(@"\"), "new header"));
            Assert.Throws<PathTooLongException>(() => FileHelper.ReplaceHeader(@"c:\thisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpath\file.txt", "header"));
            Assert.Throws<NotSupportedException>(() => FileHelper.ReplaceHeader(@"c:\pip:es.txt", "new header"));

            string tempFile = Path.GetTempFileName();
            File.SetAttributes(tempFile, FileAttributes.ReadOnly);
            Assert.Throws<UnauthorizedAccessException>(() => FileHelper.ReplaceHeader(tempFile, "new header"));

            // best way to test these?
            // <exception cref="IOException">An I/O error occurs.</exception>
            // <exception cref="OutOfMemoryException">Thrown when there is insufficient memory to allocate a buffer for the old header string.</exception>    
            // <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions on filepath OR caller doesn't have permissions to create a temporary file OR caller doesn't have permission to write to filepath</exception>
            // <exception cref="SecurityException">The caller does not have the required permission.</exception>

            // zero-byte file, has new header
            tempFile = Path.GetTempFileName();
            FileHelper.ReplaceHeader(tempFile, "new header");
            Assert.Equal("new header", File.ReadAllText(tempFile));

            // zero-byte file, new header is blank
            tempFile = Path.GetTempFileName();
            FileHelper.ReplaceHeader(tempFile, string.Empty);
            Assert.Equal(string.Empty, File.ReadAllText(tempFile));

            // zero-byte file, new header is null
            tempFile = Path.GetTempFileName();
            FileHelper.ReplaceHeader(tempFile, null);
            Assert.Equal(string.Empty, File.ReadAllText(tempFile));

            // one line, has header
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "this is a line");
            FileHelper.ReplaceHeader(tempFile, "new header");
            Assert.Equal("new header", File.ReadAllText(tempFile));

            // one line, header is blank
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "this is a line");
            FileHelper.ReplaceHeader(tempFile, string.Empty);
            Assert.Equal(string.Empty, File.ReadAllText(tempFile));

            // one line, header is null
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "this is a line");
            FileHelper.ReplaceHeader(tempFile, null);
            Assert.Equal(string.Empty, File.ReadAllText(tempFile));

            // no header, second line, has new header
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + "second line");
            FileHelper.ReplaceHeader(tempFile, "new header");
            Assert.Equal("new header" + Environment.NewLine + "second line", File.ReadAllText(tempFile));

            // no header, second line, new header is empty
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + "second line");
            FileHelper.ReplaceHeader(tempFile, string.Empty);
            Assert.Equal(Environment.NewLine + "second line", File.ReadAllText(tempFile));

            // no header, second line, new header is null
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + "second line");
            FileHelper.ReplaceHeader(tempFile, null);
            Assert.Equal("second line", File.ReadAllText(tempFile));

            // two liner, new header is null
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "first line" + Environment.NewLine + "second line");
            FileHelper.ReplaceHeader(tempFile, null);
            Assert.Equal("second line", File.ReadAllText(tempFile));

            // two liner, new header is empty
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "first line" + Environment.NewLine + "second line");
            FileHelper.ReplaceHeader(tempFile, string.Empty);
            Assert.Equal(Environment.NewLine + "second line", File.ReadAllText(tempFile));

            // two liner, new header is non-blank
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "first line" + Environment.NewLine + "second line");
            FileHelper.ReplaceHeader(tempFile, "a");
            Assert.Equal("a" + Environment.NewLine + "second line", File.ReadAllText(tempFile));

            // new header is non-blank, two liner
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "first line" + Environment.NewLine + "second line");
            FileHelper.ReplaceHeader(tempFile, "this header is longer than first line");
            Assert.Equal("this header is longer than first line" + Environment.NewLine + "second line", File.ReadAllText(tempFile));
        }

        /// <summary>
        /// Test SaveStreamToFile method
        /// </summary>
        [Fact]
        public static void SaveStreamToFileTest()
        {
            string streamFile = Path.GetTempFileName();
            string saveFile = Path.GetTempFileName() + ".save";
            const string SaveFileText = "this is some text that I've written";
            File.WriteAllText(streamFile, SaveFileText);
            FileStream stream;
            using (stream = new FileStream(streamFile, FileMode.Open, FileAccess.Write))
            {
                // arguments & exceptions
                Assert.Throws<ArgumentNullException>(() => FileHelper.SaveStreamToFile(null, "c:\test.txt"));
                Assert.Throws<ArgumentException>(() => stream.SaveStreamToFile(string.Empty));
                Assert.Throws<ArgumentNullException>(() => stream.SaveStreamToFile(null));
                Assert.Throws<ArgumentException>(() => stream.SaveStreamToFile("     "));
                Assert.Throws<ArgumentException>(() => stream.SaveStreamToFile(saveFile + ".<.txt"));
                Assert.Throws<PathTooLongException>(() => stream.SaveStreamToFile(Path.GetTempPath().AppendMissing(@"\") + "verylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailenameverylongfailename.txt"));
                Assert.Throws<ArgumentException>(() => stream.SaveStreamToFile("con:"));
                Assert.Throws<NotSupportedException>(() => stream.SaveStreamToFile(saveFile));
                Assert.Throws<DirectoryNotFoundException>(() => stream.SaveStreamToFile(@"c:\directorythatdoesntexistSaveStreamToFileTest\test.txt"));
            }

            Assert.Throws<ObjectDisposedException>(() => stream.SaveStreamToFile(saveFile));

            // can't overwrite a read-only file
            string tempFolder = DirectoryHelper.CreateTemporaryFolder();
            string tempFile = FileHelper.CreateTemporaryFile(tempFolder);
            File.SetAttributes(tempFile, FileAttributes.ReadOnly);
            using (stream = new FileStream(streamFile, FileMode.Open, FileAccess.Read))
            {
                Assert.Throws<UnauthorizedAccessException>(() => stream.SaveStreamToFile(tempFile));
            }

            // save to file - should work fine
            using (stream = new FileStream(streamFile, FileMode.Open, FileAccess.Read))
            {
                stream.SaveStreamToFile(saveFile);
            }

            using (var reader = new StreamReader(saveFile))
            {
                string text = reader.ReadLine();
                Assert.Equal(SaveFileText, text);
                Assert.True(reader.EndOfStream);
            }
        }

        #endregion

        #region Temporary Resources

        /// <summary>
        /// Tests the CreateTemporaryFile method
        /// </summary>
        [Fact]
        public static void ClearTemporaryFilesWindowsTempFolderTest()
        {
            // arguments and exceptions
            Assert.Throws<ArgumentOutOfRangeException>(() => FileHelper.ClearTemporaryFiles(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => FileHelper.ClearTemporaryFiles(-4));

            // good way to test the follow?
            // <exception cref="UnauthorizedAccessException">Thrown when method can't access the directory</exception>

            // create a bunch of temporary files, turn back the clock on them, then clear and ensure they get deleted
            var tempFiles = new List<string>();
            string unmodifiedTempFilepath = Path.GetTempFileName();
            for (int x = 0; x < 10; x++)
            {
                string tempFile = Path.GetTempFileName();
                File.SetCreationTime(tempFile, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
                File.SetLastWriteTime(tempFile, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
                File.SetLastAccessTime(tempFile, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
                tempFiles.Add(tempFile);
            }

            FileHelper.ClearTemporaryFiles(101 * 24 * 60);  // keeping too many days
            foreach (string filePath in tempFiles)
            {
                Assert.True(File.Exists(filePath));
            }

            FileHelper.ClearTemporaryFiles(100 * 24 * 60);  // keeping too many days
            foreach (string filePath in tempFiles)
            {
                Assert.True(File.Exists(filePath));
            }

            FileHelper.ClearTemporaryFiles(99 * 24 * 60);  // files are old enough to delete
            foreach (string filePath in tempFiles)
            {
                Assert.False(File.Exists(filePath));
            }

            Assert.True(File.Exists(unmodifiedTempFilepath)); // this file was created moments ago

            // lock a file that should be deleted - no exceptions are thrown
            string tempFilepath = Path.GetTempFileName();
            File.SetCreationTime(tempFilepath, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
            File.SetLastWriteTime(tempFilepath, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
            File.SetLastAccessTime(tempFilepath, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
            using (new FileStream(tempFilepath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Assert.DoesNotThrow(() => FileHelper.ClearTemporaryFiles(98 * 24 * 60));
            }

            Assert.True(File.Exists(tempFilepath));
        }

        /// <summary>
        /// Tests the CreateTemporaryFile method
        /// </summary>
        [Fact]
        public static void ClearTemporaryFilesSpecifiedTempFolderTest()
        {
            // create a temp folder
            string tempFolder = Path.GetTempFileName() + @".dir\";
            DirectoryHelper.DeleteFolder(tempFolder, true);

            // arguments and exceptions
            Assert.Throws<ArgumentException>(() => FileHelper.ClearTemporaryFiles(string.Empty, 5 * 24 * 60));
            Assert.Throws<ArgumentException>(() => FileHelper.ClearTemporaryFiles("   ", 5 * 24 * 60));
            Assert.Throws<ArgumentNullException>(() => FileHelper.ClearTemporaryFiles(null, 5 * 24 * 60));
            Assert.Throws<ArgumentOutOfRangeException>(() => FileHelper.ClearTemporaryFiles(@"c:\test\", 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => FileHelper.ClearTemporaryFiles(@"c:\test\", -4));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ClearTemporaryFiles(@"c:\directorythatdoesntexistClearTemporaryFilesSpecifiedTempFolderTest\", 5 * 24 * 60));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ClearTemporaryFiles(@"c:\test<\", 5 * 24 * 60));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ClearTemporaryFiles(@"c:\reallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldernamereallylongfoldername", 4 * 24 * 60));

            // good way to test the follow?
            // <exception cref="UnauthorizedAccessException">Thrown when method can't access the directory</exception>

            // create a bunch of temporary files, turn back the clock on them, then clear and ensure they get deleted
            var tempFiles = new List<string>();
            for (int x = 0; x < 10; x++)
            {
                string tempFile = tempFolder + x.ToString(CultureInfo.CurrentCulture) + ".txt";
                File.WriteAllText(tempFile, "whatever");
                File.SetCreationTime(tempFile, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
                File.SetLastWriteTime(tempFile, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
                File.SetLastAccessTime(tempFile, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
                tempFiles.Add(tempFile);
            }

            string unmodifiedTempFilepath = tempFolder + "11.txt";
            File.WriteAllText(unmodifiedTempFilepath, "whatever");
            Assert.True(File.Exists(unmodifiedTempFilepath)); // this file was created moments ago

            FileHelper.ClearTemporaryFiles(tempFolder, 101 * 24 * 60);  // keeping too many days
            foreach (string filePath in tempFiles)
            {
                Assert.True(File.Exists(filePath));
            }

            FileHelper.ClearTemporaryFiles(tempFolder, 100 * 24 * 60);  // keeping too many days
            foreach (string filePath in tempFiles)
            {
                Assert.True(File.Exists(filePath));
            }

            FileHelper.ClearTemporaryFiles(tempFolder, 99 * 24 * 60);  // files are old enough to delete
            foreach (string filePath in tempFiles)
            {
                Assert.False(File.Exists(filePath));
            }

            Assert.True(File.Exists(unmodifiedTempFilepath)); // this file was created moments ago

            // lock a file that should be deleted - no exceptions are thrown
            string tempFilepath = tempFolder + "12.txt";
            File.WriteAllText(tempFilepath, "whatever");
            Assert.True(File.Exists(tempFilepath));
            File.SetCreationTime(tempFilepath, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
            File.SetLastWriteTime(tempFilepath, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
            File.SetLastAccessTime(tempFilepath, DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0)));
            using (new FileStream(tempFilepath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Assert.DoesNotThrow(() => FileHelper.ClearTemporaryFiles(tempFolder, 98 * 24 * 60));
            }

            Assert.True(File.Exists(tempFilepath));

            // cleanup
            DirectoryHelper.DeleteFolder(tempFolder);
        }

        /// <summary>
        /// Tests the CreateTemporaryFile method
        /// </summary>
        [Fact]
        public static void CreateTemporaryFileTest()
        {
            string tempFilepath = FileHelper.CreateTemporaryFile();

            // temp file is in the windows temp folder
            Assert.True(Directory.GetFiles(Path.GetTempPath()).Contains(tempFilepath, StringComparer.CurrentCultureIgnoreCase));
            Assert.True(FileHelper.IsFileSizeZero(tempFilepath));

            // no great way to test when temp folder is full - would have to completely populate folder
            // <exception cref="IOException">Thrown when there are no temporary file names available, even after old files have been cleared.</exception>
        }

        /// <summary>
        /// Tests the CreateTemporaryFile overloaded method
        /// </summary>
        [Fact]
        public static void CreateTemporaryFileTest2()
        {
            // arguments & exceptions
            Assert.Throws<ArgumentException>(() => FileHelper.CreateTemporaryFile(string.Empty));
            Assert.Throws<ArgumentNullException>(() => FileHelper.CreateTemporaryFile(null));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateTemporaryFile(@"c:\test<\"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateTemporaryFile(@"     "));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.CreateTemporaryFile(@"c:\directorythatdoesntexistCreateTemporaryFileTest2\"));
            Assert.Throws<PathTooLongException>(() => FileHelper.CreateTemporaryFile(@"c:\pathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolongpathtoolong"));

            // create a temp folder, make read-only and try to create temp file
            string tempFolder = Path.GetTempFileName() + @".dir\";
            DirectoryHelper.DeleteFolder(tempFolder, true);
            // ReSharper disable ObjectCreationAsStatement
            new DirectoryInfo(tempFolder) { Attributes = FileAttributes.ReadOnly };
            // ReSharper restore ObjectCreationAsStatement
            Assert.DoesNotThrow(() => FileHelper.CreateTemporaryFile(tempFolder));

            // how do we reproduce these?
            // <exception cref="SecurityException">The caller does not have the required permission to create a zero-byte file in the rootFolder.</exception>
            // <exception cref="UnauthorizedAccessException">Thrown when method can't access rootFolder to clear out older temporary files, or when the system doesn't have access permission to write the zero-byte file to rootFolder.</exception>

            // no great way to test when temp folder is full - would have to completely populate folder
            // <exception cref="IOException">Could't create a temporary file.</exception>

            // create 100,000 temporary files.  no IOException, no repeat files
            var filenames = new HashSet<string>();
            const int TotalFiles = 500;

            for (int x = 0; x < TotalFiles; x++)
            {
                string filepath = FileHelper.CreateTemporaryFile(tempFolder);
                Assert.True(File.Exists(filepath));
                Assert.True(FileHelper.IsFileSizeZero(filepath));
                Assert.True(filenames.Add(filepath));
            }

            // now check that all of the created files are in the specified folder
            var filesInFolder = new HashSet<string>(Directory.GetFiles(tempFolder));
            foreach (string createdFile in filenames)
            {
                Assert.True(filesInFolder.Contains(createdFile, StringComparer.CurrentCultureIgnoreCase));
            }

            // cleanup
            DirectoryHelper.DeleteFolder(tempFolder);
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_RootDirectoryIsInvalid_ThrowsArgumentException()
        {
            // Act, Assert            
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(@"c:\baddir>s\"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(@"c:\badd:irs"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(@"c:\folderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolong\"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(@"c:\badd:irs"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(@"con:"));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_RootDirectoryDoesNotExist_ThrowsDirectoryNotFoundException()
        {
            // Arrange, Act
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.CreateFileNamedByTimestamp(@"c:\doesnotexist\"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.CreateFileNamedByTimestamp(@"z:\"));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_RootDirectoryIsNullOrWhitespace_ReturnsPathToFileInWorkingDirectory()
        {
            // Arrange
            string workingDirectory = Directory.GetCurrentDirectory();

            // Act
            // ReSharper disable RedundantArgumentDefaultValue
            string tempFilePath1 = FileHelper.CreateFileNamedByTimestamp(null);
            Thread.Sleep(1000);
            // ReSharper restore RedundantArgumentDefaultValue
            string tempFilePath2 = FileHelper.CreateFileNamedByTimestamp(string.Empty);
            Thread.Sleep(1000);
            string tempFilePath3 = FileHelper.CreateFileNamedByTimestamp("    ");
            Thread.Sleep(1000);
            string tempFilePath4 = FileHelper.CreateFileNamedByTimestamp("  \r\n  ");

            // Assert
            Assert.Equal(workingDirectory, Path.GetDirectoryName(tempFilePath1));
            Assert.Equal(workingDirectory, Path.GetDirectoryName(tempFilePath2));
            Assert.Equal(workingDirectory, Path.GetDirectoryName(tempFilePath3));
            Assert.Equal(workingDirectory, Path.GetDirectoryName(tempFilePath4));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_RootDirectoryIsNullOrWhitespace_CreatesFile()
        {
            // Arrange, Act
            // ReSharper disable once RedundantArgumentDefaultValue
            string tempFilePath1 = FileHelper.CreateFileNamedByTimestamp(null);
            // ReSharper restore once RedundantArgumentDefaultValue
            Thread.Sleep(1000);
            string tempFilePath2 = FileHelper.CreateFileNamedByTimestamp(string.Empty);
            Thread.Sleep(1000);
            string tempFilePath3 = FileHelper.CreateFileNamedByTimestamp("    ");
            Thread.Sleep(1000);
            string tempFilePath4 = FileHelper.CreateFileNamedByTimestamp("  \r\n  ");

            // Assert
            Assert.True(File.Exists(tempFilePath1));
            Assert.True(File.Exists(tempFilePath2));
            Assert.True(File.Exists(tempFilePath3));
            Assert.True(File.Exists(tempFilePath4));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_RootDirectoryIsPathRelativeToWorkingDirectory_CreatesFileInSpecifiedDirectory()
        {
            // Arrange
            string workingDirectory = Directory.GetCurrentDirectory();
            string tempDirectory = Path.Combine(workingDirectory, "DeleteMe\\");
            Directory.CreateDirectory(tempDirectory);

            // Act
            string tempFile = FileHelper.CreateFileNamedByTimestamp(tempDirectory);

            // Assert
            Assert.True(File.Exists(tempFile));
            Assert.Equal(tempDirectory.AppendMissing(@"\"), Path.GetDirectoryName(tempFile).AppendMissing(@"\"));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_RootDirectoryExists_CreatesFileInRootDirectory()
        {
            // Arrange
            string tempDirectory = DirectoryHelper.CreateTemporaryFolder();

            // Act
            string tempFile = FileHelper.CreateFileNamedByTimestamp(tempDirectory);

            // Assert
            Assert.True(File.Exists(tempFile));
            Assert.Equal(tempDirectory.AppendMissing(@"\"), Path.GetDirectoryName(tempFile).AppendMissing(@"\"));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_CalledTwiceWithOneSecondPause_CreatesFilesWithDifferentNames()
        {
            // Arrange
            string tempDirectory = DirectoryHelper.CreateTemporaryFolder();

            // Act
            string tempFile1 = FileHelper.CreateFileNamedByTimestamp(tempDirectory);
            Thread.Sleep(1000);
            string tempFile2 = FileHelper.CreateFileNamedByTimestamp(tempDirectory);

            // Assert
            Assert.NotEqual(tempFile1, tempFile2);
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_WithInvalidPrefix_ThrowsArgumentException()
        {
            // Act, Assert            
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, @"sadf:9234"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, @"abc?def"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, @"<test>"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, @"c:\folderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolong\"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, @"c:\badd:irs"));            
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_PrefixSpecified_CreatesFileWithSpecifiedPrefix()
        {
            // Arrange
            string tempDirectory = DirectoryHelper.CreateTemporaryFolder();
            const string Prefix1 = "pre";
            const string Prefix2 = "pre ";

            // Act
            string tempFile1 = FileHelper.CreateFileNamedByTimestamp(tempDirectory, Prefix1);
            Thread.Sleep(1000);
            string tempFile2 = FileHelper.CreateFileNamedByTimestamp(tempDirectory, Prefix2);

            // Assert
            // ReSharper disable PossibleNullReferenceException
            Assert.True(Path.GetFileName(tempFile1).StartsWith(Prefix1));            
            Assert.True(Path.GetFileName(tempFile2).StartsWith(Prefix2));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_WithInvalidSuffix_ThrowsArgumentException()
        {
            // Act, Assert            
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, null, @"sadf:9234"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, null, @"abc?def"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, null, @"<test>"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, null, @"c:\folderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolong\"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, null, @"c:\badd:irs"));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_SuffixSpecified_CreatesFileWithSpecifiedSuffix()
        {
            // Arrange
            string tempDirectory = DirectoryHelper.CreateTemporaryFolder();
            const string Suffix1 = "suffix";
            const string Suffix2 = " suffix";

            // Act
            string tempFile1 = FileHelper.CreateFileNamedByTimestamp(tempDirectory, null, Suffix1);
            Thread.Sleep(1000);
            string tempFile2 = FileHelper.CreateFileNamedByTimestamp(tempDirectory, null, Suffix2);

            // Assert
            // ReSharper disable PossibleNullReferenceException
            Assert.True(Path.GetFileName(tempFile1).EndsWith(Suffix1 + ".tmp"));
            Assert.True(Path.GetFileName(tempFile2).EndsWith(Suffix2 + ".tmp"));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_WithInvalidExtension_ThrowsArgumentException()
        {
            // Act, Assert            
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, null, null, @"sadf:9234"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, null, null, @"abc?def"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, null, null, @"<test>"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, null, null, @"c:\folderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolong\"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateFileNamedByTimestamp(null, null, null, @"c:\badd:irs"));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_ExtensionNotSpecified_CreatesFileWithTmpExtension()
        {
            // Arrange
            string tempDirectory = DirectoryHelper.CreateTemporaryFolder();
            
            // Act
            // ReSharper disable RedundantArgumentDefaultValue
            string tempFile1 = FileHelper.CreateFileNamedByTimestamp(tempDirectory, null, null);            
            Thread.Sleep(1000);
            string tempFile2 = FileHelper.CreateFileNamedByTimestamp(tempDirectory, null, null);
            // ReSharper restore RedundantArgumentDefaultValue

            // Assert
            // ReSharper disable PossibleNullReferenceException
            Assert.True(Path.GetFileName(tempFile1).EndsWith(".tmp"));
            Assert.True(Path.GetFileName(tempFile2).EndsWith(".tmp"));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_NonDefaultExtensionSpecified_CreatesFileWithSpecifiedExtension()
        {
            // Arrange
            string tempDirectory = DirectoryHelper.CreateTemporaryFolder();
            const string Ext1 = "test";
            const string Ext2 = " test";

            // Act
            string tempFile1 = FileHelper.CreateFileNamedByTimestamp(tempDirectory, null, null, Ext1);
            Thread.Sleep(1000);
            string tempFile2 = FileHelper.CreateFileNamedByTimestamp(tempDirectory, null, null, Ext2);

            // Assert
            // ReSharper disable PossibleNullReferenceException
            Assert.True(Path.GetFileName(tempFile1).EndsWith("." + Ext1));
            Assert.True(Path.GetFileName(tempFile2).EndsWith("." + Ext2));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CreateFileNamedByTimestamp_ExtensionIsNullOrWhitespace_NoExtensionApplied()
        {
            // Arrange
            string tempDirectory = DirectoryHelper.CreateTemporaryFolder();
            const string Suffix = "ENDING";
            
            // Act
            string tempFile1 = FileHelper.CreateFileNamedByTimestamp(tempDirectory, null, Suffix, null);
            Thread.Sleep(1000);
            string tempFile2 = FileHelper.CreateFileNamedByTimestamp(tempDirectory, null, Suffix, string.Empty);
            Thread.Sleep(1000);
            string tempFile3 = FileHelper.CreateFileNamedByTimestamp(tempDirectory, null, Suffix, "   ");
            Thread.Sleep(1000);
            string tempFile4 = FileHelper.CreateFileNamedByTimestamp(tempDirectory, null, Suffix, "   \r\n  ");

            // Assert
            // ReSharper disable PossibleNullReferenceException
            Assert.True(Path.GetFileName(tempFile1).EndsWith(Suffix));
            Assert.True(Path.GetFileName(tempFile2).EndsWith(Suffix));
            Assert.True(Path.GetFileName(tempFile3).EndsWith(Suffix));
            Assert.True(Path.GetFileName(tempFile4).EndsWith(Suffix));
            // ReSharper restore PossibleNullReferenceException
        }

        #endregion

        #region Compression

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFile_ParameterSourceFilePathIsNullOrWhiteSpace_ThrowsArgumentException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => FileHelper.CompressFile(null, zipFilePath));
            Assert.Throws<ArgumentException>(() => FileHelper.CompressFile(string.Empty, zipFilePath));
            Assert.Throws<ArgumentException>(() => FileHelper.CompressFile("        ", zipFilePath));
            Assert.Throws<ArgumentException>(() => FileHelper.CompressFile("    \r\n    ", zipFilePath));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFile_ParameterZipFilePathIsNullOrWhiteSpace_ThrowsArgumentException()
        {
            // Arrange
            string sourceFilePath = FileHelper.CreateTemporaryFile();

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => FileHelper.CompressFile(sourceFilePath, null));
            Assert.Throws<ArgumentException>(() => FileHelper.CompressFile(sourceFilePath, string.Empty));
            Assert.Throws<ArgumentException>(() => FileHelper.CompressFile(sourceFilePath, "      "));
            Assert.Throws<ArgumentException>(() => FileHelper.CompressFile(sourceFilePath, "    \r\n     "));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFile_SourceFileDoesNotExistOnDisk_ThrowsFileNotFoundException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            string sourceFilePath = FileHelper.CreateTemporaryFile() + ".doesnotexist.txt";
            File.Delete(sourceFilePath);

            // Act / Assert
            Assert.Throws<FileNotFoundException>(() => FileHelper.CompressFile(sourceFilePath, zipFilePath));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFile_ParameterSourceFileIsAnInvalidFilePath_ThrowsFileNotFoundException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            string sourceFilePath = Path.GetTempPath().AppendMissing(@"\") + @"****\/\/\/???com://";

            // Act / Assert
            Assert.Throws<FileNotFoundException>(() => FileHelper.CompressFile(sourceFilePath, zipFilePath));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFile_ParameterZipFileIsAnInvalidFilePath_ThrowsArgumentException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + @"****\/\/\/???com://" + ".zip";
            string sourceFilePath = FileHelper.CreateTemporaryFile();

            // Act / Assert
            Assert.Throws<ArgumentException>(() => FileHelper.CompressFile(sourceFilePath, zipFilePath));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFile_SourceFileIsLocked_ThrowsIOExceptionAndZipFileNotWritten()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            string sourceFilePath = FileHelper.CreateTemporaryFile();
            var sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Write, FileShare.None);

            // Act / Assert
            Assert.Throws<IOException>(() => FileHelper.CompressFile(sourceFilePath, zipFilePath));
            Assert.False(File.Exists(zipFilePath));

            // Cleanup
            sourceStream.Dispose();
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFile_ZipFileIsLocked_ThrowsInvalidOperationException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            string sourceFilePath = FileHelper.CreateTemporaryFile();
            var zipStream = new FileStream(zipFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => FileHelper.CompressFile(sourceFilePath, zipFilePath));

            // Cleanup
            zipStream.Dispose();
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFile_ZipFileAlreadyExists_ThrowsInvalidOperationException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            File.WriteAllText(zipFilePath, "asdf");
            string sourceFilePath = FileHelper.CreateTemporaryFile();
            File.WriteAllText(sourceFilePath, "thisisthesourcefile");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => FileHelper.CompressFile(sourceFilePath, zipFilePath));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFile_ValidSourceFile_CanBeDecompressedAndContainsOriginalSourceFileNameAndContents()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            string sourceFilePath = FileHelper.CreateTemporaryFile();
            File.WriteAllText(sourceFilePath, "thisisthesourcefile");
            string uncompressFolder = DirectoryHelper.CreateTemporaryFolder();
            string expectedUncompressedFilePath = uncompressFolder.AppendMissing(@"\") + Path.GetFileName(sourceFilePath);

            // Act
            FileHelper.CompressFile(sourceFilePath, zipFilePath);
            FileHelper.DecompressFile(zipFilePath, uncompressFolder);

            // Assert
            Assert.True(File.Exists(expectedUncompressedFilePath));
            Assert.Equal("thisisthesourcefile", File.ReadAllText(expectedUncompressedFilePath));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFilesToMemory_ParameterFilesIsNull_ThrowsArgumentNullException()
        {
            // Act / Assert            
            Assert.Throws<ArgumentNullException>(() => FileHelper.CompressFilesToMemory(null));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFilesToMemory_ParameterFilesIsEmpty_ThrowsArgumentException()
        {
            // Act / Assert            
            Assert.Throws<ArgumentException>(() => FileHelper.CompressFilesToMemory(new List<FileToZip>()));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFilesToMemory_ParameterFilesContainsInvalidFileName_ThrowsArgumentException()
        {
            // Arrange
            string filepath1 = FileHelper.CreateTemporaryFile();
            string filepath2 = FileHelper.CreateTemporaryFile();
            string filepath3 = FileHelper.CreateTemporaryFile();
            var filesToCompress1 = new[] { new FileToZip("1", filepath1), new FileToZip("abcde*", filepath2), new FileToZip("3", filepath3) };
            var filesToCompress2 = new[] { new FileToZip("1", filepath1), new FileToZip("abc\\de.out", filepath2), new FileToZip("3", filepath3) };

            // Act / Assert
            Assert.Throws<ArgumentException>(() => FileHelper.CompressFilesToMemory(filesToCompress1));
            Assert.Throws<ArgumentException>(() => FileHelper.CompressFilesToMemory(filesToCompress2));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFilesToMemory_ParameterFilesContainsFileMissingFromDisk_ThrowsArgumentException()
        {
            // Arrange
            string filepath1 = FileHelper.CreateTemporaryFile();
            string filepath2 = FileHelper.CreateTemporaryFile();
            string filepath3 = FileHelper.CreateTemporaryFile();
            var filesToCompress = new[] { new FileToZip("1", filepath1), new FileToZip("2", filepath2 + ".doesntexist"), new FileToZip("3", filepath3) };

            // Act / Assert
            Assert.Throws<ArgumentException>(() => FileHelper.CompressFilesToMemory(filesToCompress));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFilesToMemory_SameFileNameSpecifiedTwice_ThrowsArgumentException()
        {
            // Arrange
            string filepath1 = FileHelper.CreateTemporaryFile();
            string filepath2 = FileHelper.CreateTemporaryFile();
            string filepath3 = FileHelper.CreateTemporaryFile();
            var filesToCompress = new[] { new FileToZip("1.txt", filepath1), new FileToZip("2", filepath2), new FileToZip(" 1.txt", filepath3) };

            // Act / Assert
            Assert.Throws<ArgumentException>(() => FileHelper.CompressFilesToMemory(filesToCompress));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFilesToMemory_AFileToCompressIsLocked_ThrowsIOException()
        {
            // Arrange
            string filepath1 = FileHelper.CreateTemporaryFile();
            string filepath2 = FileHelper.CreateTemporaryFile();
            string filepath3 = FileHelper.CreateTemporaryFile();
            var filesToCompress = new[] { new FileToZip("1", filepath1), new FileToZip("2", filepath2), new FileToZip("3", filepath3) };

            // Act / Assert
            using (new FileStream(filepath2, FileMode.Open, FileAccess.Write, FileShare.None))
            {
                Assert.Throws<IOException>(() => FileHelper.CompressFilesToMemory(filesToCompress));
            }
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CompressFilesToMemory_ReturnsByteArrayThatCanBeDecompressed()
        {
            // Arrange

            // write random text into files
            string filepath1 = FileHelper.CreateTemporaryFile();
            string filepath2 = FileHelper.CreateTemporaryFile();
            string filepath3 = FileHelper.CreateTemporaryFile();
            File.WriteAllText(filepath1, RandomString(10000, false));
            File.WriteAllText(filepath2, RandomString(10000, false));
            File.WriteAllText(filepath3, RandomString(10000, false));

            var filesToCompress = new[] { new FileToZip("1", filepath1), new FileToZip("2", filepath2), new FileToZip("3", filepath3) };
            string decompressFolder = DirectoryHelper.CreateTemporaryFolder();

            // Act
            byte[] compressed = FileHelper.CompressFilesToMemory(filesToCompress);

            // Assert
            FileHelper.DecompressFilesFromMemory(compressed, decompressFolder);
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Equal(File.ReadAllText(filepath1), File.ReadAllText(Path.Combine(decompressFolder, "1")));
            Assert.Equal(File.ReadAllText(filepath2), File.ReadAllText(Path.Combine(decompressFolder, "2")));
            Assert.Equal(File.ReadAllText(filepath3), File.ReadAllText(Path.Combine(decompressFolder, "3")));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFile_ParameterZipFilePathIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            string tempFolder = Path.GetTempPath();

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => FileHelper.DecompressFile(null, tempFolder));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFile_ParameterZipFilePathIsWhiteSpace_ThrowsArgumentException()
        {
            // Arrange
            string tempFolder = Path.GetTempPath();

            // Act / Assert            
            Assert.Throws<ArgumentException>(() => FileHelper.DecompressFile(string.Empty, tempFolder));
            Assert.Throws<ArgumentException>(() => FileHelper.DecompressFile("     ", tempFolder));
            Assert.Throws<ArgumentException>(() => FileHelper.DecompressFile("  \r\n       ", tempFolder));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFile_ParameterTargetDirectoryIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            AssemblyHelper.OpenEmbeddedResourceStream("SingleFile.zip", true).SaveStreamToFile(zipFilePath);

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => FileHelper.DecompressFile(zipFilePath, null));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFile_ParameterTargetDirectoryIsWhiteSpace_ThrowsArgumentException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            AssemblyHelper.OpenEmbeddedResourceStream("SingleFile.zip", true).SaveStreamToFile(zipFilePath);

            // Act / Assert            
            Assert.Throws<ArgumentException>(() => FileHelper.DecompressFile(zipFilePath, string.Empty));
            Assert.Throws<ArgumentException>(() => FileHelper.DecompressFile(zipFilePath, "     "));
            Assert.Throws<ArgumentException>(() => FileHelper.DecompressFile(zipFilePath, "   \r\n  "));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFile_ZipFileDoesNotExistOnDisk_ThrowsFileNotFoundException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";

            // Act / Assert
            Assert.Throws<FileNotFoundException>(() => FileHelper.DecompressFile(zipFilePath, Path.GetTempPath()));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFile_TargetDirectoryDoesNotExist_ThrowsDirectoryNotFoundException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            AssemblyHelper.OpenEmbeddedResourceStream("SingleFile.zip", true).SaveStreamToFile(zipFilePath);
            string nonExistingFolder1 = Path.GetTempPath().AppendMissing(@"\") + "DecompressFileFolderDoesNotExist";
            string nonExistingFolder2 = Path.GetTempPath().AppendMissing(@"\") + @"DecompressFileFolderDoesNotExist\";

            // Act / Assert
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.DecompressFile(zipFilePath, nonExistingFolder1));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.DecompressFile(zipFilePath, nonExistingFolder2));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFile_ZipFileIsLocked_ThrowsIOException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            AssemblyHelper.OpenEmbeddedResourceStream("SingleFile.zip", true).SaveStreamToFile(zipFilePath);
            var zipFileStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read, FileShare.None);

            // Act / Assert
            Assert.Throws<IOException>(() => FileHelper.DecompressFile(zipFilePath, Path.GetTempPath()));

            // Cleanup
            zipFileStream.Dispose();
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFile_ZipFileIsMalformed_ThrowsZipException()
        {
            // Arrange
            string malformedZipFilePath1 = FileHelper.CreateTemporaryFile() + ".zip";
            File.WriteAllText(malformedZipFilePath1, "asdfasdfasdf");

            string malformedZipFilePath2 = FileHelper.CreateTemporaryFile() + ".zip";
            AssemblyHelper.OpenEmbeddedResourceStream("Malformed.zip", true).SaveStreamToFile(malformedZipFilePath2);

            // Act / Assert
            Assert.Throws<ZipException>(() => FileHelper.DecompressFile(malformedZipFilePath1, Path.GetTempPath()));
            Assert.Throws<ZipException>(() => FileHelper.DecompressFile(malformedZipFilePath2, Path.GetTempPath()));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFile_SingleFileInZip_DecompressesFileMaintainingFileNameAndContents()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            AssemblyHelper.OpenEmbeddedResourceStream("SingleFile.zip", true).SaveStreamToFile(zipFilePath);
            string decompressFolder = DirectoryHelper.CreateTemporaryFolder();
            string expectedFilePath = decompressFolder.AppendMissing(@"\") + "FirstFile.txt";

            // Act
            FileHelper.DecompressFile(zipFilePath, decompressFolder);

            // Assert
            Assert.Single(Directory.EnumerateFiles(decompressFolder));
            Assert.True(File.Exists(expectedFilePath));
            Assert.Equal("this is the first file", File.ReadAllText(expectedFilePath));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFile_MultipleFilesInZipWithEmbeddedFolderStructure_DecompressesFilesMaintainingFileNamesAndContent()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            AssemblyHelper.OpenEmbeddedResourceStream("MultiFile.zip", true).SaveStreamToFile(zipFilePath);
            string decompressFolder = DirectoryHelper.CreateTemporaryFolder();
            string expectedFilePath1 = decompressFolder.AppendMissing(@"\") + "FirstFile.txt";
            string expectedFilePath2 = decompressFolder.AppendMissing(@"\") + "Second.File.txt";
            string expectedFilePath3 = decompressFolder.AppendMissing(@"\") + "ThirdFile.out";
            string expectedFilePath4 = decompressFolder.AppendMissing(@"\") + @"TestFolder\FourthFile.txt";

            // Act
            FileHelper.DecompressFile(zipFilePath, decompressFolder);

            // Assert
            Assert.Equal(4, Directory.EnumerateFiles(decompressFolder, "*.*", SearchOption.AllDirectories).Count());
            Assert.True(File.Exists(expectedFilePath1));
            Assert.True(File.Exists(expectedFilePath2));
            Assert.True(File.Exists(expectedFilePath3));
            Assert.True(File.Exists(expectedFilePath4));
            Assert.Equal("this is the first file", File.ReadAllText(expectedFilePath1));
            Assert.Equal("this is the second file", File.ReadAllText(expectedFilePath2));
            Assert.Equal("this is the third file", File.ReadAllText(expectedFilePath3));
            Assert.Equal("this is the fourth file.", File.ReadAllText(expectedFilePath4));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFile_TargetFilePathIsLocked_ThrowsIOException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            AssemblyHelper.OpenEmbeddedResourceStream("SingleFile.zip", true).SaveStreamToFile(zipFilePath);
            string decompressFolder = DirectoryHelper.CreateTemporaryFolder();
            string expectedFilePath = decompressFolder.AppendMissing(@"\") + "FirstFile.txt";
            var expectedFileStream = new FileStream(expectedFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);

            // Act / Assert
            Assert.Throws<IOException>(() => FileHelper.DecompressFile(zipFilePath, decompressFolder));

            // Cleanup
            expectedFileStream.Dispose();
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFile_OverwritesExistingFileWithSameTargetFilePath()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            AssemblyHelper.OpenEmbeddedResourceStream("SingleFile.zip", true).SaveStreamToFile(zipFilePath);
            string decompressFolder = DirectoryHelper.CreateTemporaryFolder();
            string expectedFilePath = decompressFolder.AppendMissing(@"\") + "FirstFile.txt";
            File.WriteAllText(expectedFilePath, "asdfasdf");
            string expectedFilePath2 = decompressFolder.AppendMissing(@"\") + "TestFile.txt";
            File.WriteAllText(expectedFilePath2, "testfile");

            // Act
            FileHelper.DecompressFile(zipFilePath, decompressFolder);

            // Assert
            Assert.Equal(2, Directory.EnumerateFiles(decompressFolder).Count());
            Assert.True(File.Exists(expectedFilePath));
            Assert.Equal("this is the first file", File.ReadAllText(expectedFilePath));
            Assert.True(File.Exists(expectedFilePath2));
            Assert.Equal("testfile", File.ReadAllText(expectedFilePath2));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFilesFromMemory_ParameterZipIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            string decompressFolder = DirectoryHelper.CreateTemporaryFolder();

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => FileHelper.DecompressFilesFromMemory(null, decompressFolder));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFilesFromMemory_ParameterOutputFolderIsNullOrWhiteSpace_ThrowsArgumentException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            AssemblyHelper.OpenEmbeddedResourceStream("SingleFile.zip", true).SaveStreamToFile(zipFilePath);
            byte[] zip;
            using (var compressed = new MemoryStream())
            {
                using (var zipFile = ZipFile.Read(zipFilePath))
                {
                    zipFile.Save(compressed);
                }

                zip = compressed.ToArray();
            } // using compressed memory stream

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => FileHelper.DecompressFilesFromMemory(zip, null));
            Assert.Throws<ArgumentException>(() => FileHelper.DecompressFilesFromMemory(zip, string.Empty));
            Assert.Throws<ArgumentException>(() => FileHelper.DecompressFilesFromMemory(zip, "      "));
            Assert.Throws<ArgumentException>(() => FileHelper.DecompressFilesFromMemory(zip, "\r\n"));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFilesFromMemory_ParameterOutputFolderDoesNotExist_ThrowsDirectoryNotFoundException()
        {
            // Arrange
            string zipFilePath = FileHelper.CreateTemporaryFile() + ".zip";
            AssemblyHelper.OpenEmbeddedResourceStream("SingleFile.zip", true).SaveStreamToFile(zipFilePath);
            byte[] zip;
            using (var compressed = new MemoryStream())
            {
                using (var zipFile = ZipFile.Read(zipFilePath))
                {
                    zipFile.Save(compressed);
                }

                zip = compressed.ToArray();
            }

            string decompressFolder = DirectoryHelper.CreateTemporaryFolder();

            // Act / Assert
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.DecompressFilesFromMemory(zip, Path.Combine(decompressFolder, "missing")));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void DecompressFilesFromMemory_ZipIsMalformed_ThrowsZipException()
        {
            // Arrange
            string decompressFolder = DirectoryHelper.CreateTemporaryFolder();

            // Act / Assert
            Assert.Throws<ZipException>(() => FileHelper.DecompressFilesFromMemory(new byte[] { 23, 43, 23 }, decompressFolder));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact(Skip = "Tested in CompressFilesToMemory_ReturnsByteArrayThatCanBeDecompressed")]
        public static void DecompressFilesFromMemory_DecompressesFilesToDisk()
        {
        }

        #endregion

        #region Delete Files

        /// <summary>
        /// Tests the DeleteFile method.
        /// </summary>
        [Fact]
        public static void DeleteFileTest()
        {
            // arguments & exceptions
            Assert.Throws<ArgumentNullException>(() => FileHelper.DeleteFile(null));
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFile(string.Empty));
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFile("      "));
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFile("c:\test*.txt"));
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFile("c:\te:st.txt"));
            Assert.Throws<UnauthorizedAccessException>(() => FileHelper.DeleteFile(Path.GetTempPath().AppendMissing(@"\")));

            // path too long or directory not found, does not throw
            Assert.DoesNotThrow(() => FileHelper.DeleteFile(@"c:\ThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolongThisisapaththatstoolong.txt"));
            Assert.DoesNotThrow(() => FileHelper.DeleteFile(@"c:\thisfolderdoesntexist\file.txt"));

            // how to replicate other causes of UnauthorizedAccessException?

            // create a temp file and then delete it
            string tempFile = Path.GetTempFileName();
            Assert.True(File.Exists(tempFile));
            FileHelper.DeleteFile(tempFile);
            Assert.False(File.Exists(tempFile));

            // create a temp file, make it read-only, system, hidden.  then delete
            tempFile = Path.GetTempFileName();
            Assert.True(File.Exists(tempFile));
            File.SetAttributes(tempFile, FileAttributes.System | FileAttributes.Hidden | FileAttributes.ReadOnly);
            FileHelper.DeleteFile(tempFile);
            Assert.False(File.Exists(tempFile));

            // create a temp file, open a filestream with delete share permissions, 
            // try delete, ensure deleted after filestream is closed
            tempFile = Path.GetTempFileName();
            Assert.True(File.Exists(tempFile));
            var stream = new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite);
            var disposeParams = new DisposeFilestreamParams(stream, 5);
            var th = new Thread(DisposeFilestream);
            th.Start(disposeParams);
            FileHelper.DeleteFile(tempFile);
            Assert.False(File.Exists(tempFile));

            // create temp file, open a filestream without delete share permissions
            // try delete, ensure delete after filestream is closed
            tempFile = Path.GetTempFileName();
            Assert.True(File.Exists(tempFile));
            stream = new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.None);
            disposeParams = new DisposeFilestreamParams(stream, 5);
            th = new Thread(DisposeFilestream);
            th.Start(disposeParams);
            FileHelper.DeleteFile(tempFile);
            Assert.False(File.Exists(tempFile));

            // create a temp file - mark read-only, system, hidden.  try delete
            tempFile = Path.GetTempFileName();
            File.SetAttributes(tempFile, FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden);
            Assert.True(File.Exists(tempFile));
            FileHelper.DeleteFile(tempFile);
            Assert.False(File.Exists(tempFile));

            // create a temp file - lock it, try to delete.  shouldn't be able to
            tempFile = Path.GetTempFileName();
            Assert.True(File.Exists(tempFile));
            using (new StreamReader(tempFile))
            {
                Assert.Throws<IOException>(() => FileHelper.DeleteFile(tempFile));
            }

            FileHelper.DeleteFile(tempFile);
            Assert.False(File.Exists(tempFile));

            // delete a file that doesn't exist - no problem
            tempFile = Path.GetTempFileName();
            File.Delete(tempFile);
            Assert.False(File.Exists(tempFile));
            FileHelper.DeleteFile(tempFile);
            Assert.False(File.Exists(tempFile));
        }

        /// <summary>
        /// Tests the DeleteAllFiles method
        /// </summary>
        [Fact]
        public static void DeleteAllFilesTest()
        {
            // arguments and exceptions
            // how to replicate UnauthorizedAccessException?
            Assert.Throws<ArgumentNullException>(() => FileHelper.DeleteFiles(null));
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFiles(string.Empty));
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFiles("    "));
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFiles("c:\thisisille>gal"));
            string tempFile = Path.GetTempFileName();
            Assert.Throws<IOException>(() => FileHelper.DeleteFiles(tempFile));
            Assert.Throws<PathTooLongException>(() => FileHelper.DeleteFiles(@"c:\ThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLong"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.DeleteFiles(@"c:\thisdoesntexist"));

            // create a folder with some files
            string windowsTempFolder = Path.GetTempPath().AppendMissing(@"\");
            string tempFolder = windowsTempFolder + @"DeleteAllFilesTest\";
            if (Directory.Exists(tempFolder))
            {
                Directory.Delete(tempFolder, true);
            }

            Directory.CreateDirectory(tempFolder);
            Assert.True(Directory.Exists(tempFolder));
            string tempFile1 = Path.GetTempFileName();
            string tempFile2 = Path.GetTempFileName();
            string tempFile3 = Path.GetTempFileName();
            File.Copy(tempFile1, tempFolder + Path.GetFileNameWithoutExtension(tempFile1));
            File.Copy(tempFile2, tempFolder + Path.GetFileNameWithoutExtension(tempFile2));
            File.Copy(tempFile3, tempFolder + Path.GetFileNameWithoutExtension(tempFile3));
            tempFile1 = tempFolder + Path.GetFileNameWithoutExtension(tempFile1);
            tempFile2 = tempFolder + Path.GetFileNameWithoutExtension(tempFile2);
            tempFile3 = tempFolder + Path.GetFileNameWithoutExtension(tempFile3);
            Assert.True(File.Exists(tempFile1));
            Assert.True(File.Exists(tempFile2));
            Assert.True(File.Exists(tempFile3));
            FileHelper.DeleteFiles(tempFolder);
            Assert.True(Directory.Exists(tempFolder));
            Assert.False(File.Exists(tempFile1));
            Assert.False(File.Exists(tempFile2));
            Assert.False(File.Exists(tempFile3));

            // delete all files in an empty folder - shouldn't cause a problem
            Assert.DoesNotThrow(() => FileHelper.DeleteFiles(tempFolder));
            Directory.Delete(tempFolder);

            // all other cases are covered in DeleteFile - read-only and locked files
        }

        /// <summary>
        /// Tests the DeleteFiles method with searchPattern
        /// </summary>
        [Fact]
        public static void DeleteFilesTest()
        {
            // arguments and exceptions
            Assert.Throws<ArgumentNullException>(() => FileHelper.DeleteFiles(null, "*.*", SearchOption.TopDirectoryOnly));
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFiles(string.Empty, "*.*", SearchOption.TopDirectoryOnly));
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFiles("    ", "*.*", SearchOption.TopDirectoryOnly));
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFiles("c:\thisisille>gal", "*.*", SearchOption.TopDirectoryOnly));

            // how to replicate UnauthorizedAccessException?
            string tempFile = Path.GetTempFileName();
            Assert.Throws<IOException>(() => FileHelper.DeleteFiles(tempFile, "*.*", SearchOption.TopDirectoryOnly));
            Assert.Throws<PathTooLongException>(() => FileHelper.DeleteFiles(@"c:\ThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLongThisIsReallyLong", "*.*", SearchOption.TopDirectoryOnly));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.DeleteFiles(@"c:\thisdoesntexist", "*.*", SearchOption.TopDirectoryOnly));

            // create a temp folder 
            string windowsTempFolder = Path.GetTempPath().AppendMissing(@"\");
            string tempFolder = windowsTempFolder + @"DeleteFilesTest\";
            if (Directory.Exists(tempFolder))
            {
                Directory.Delete(tempFolder, true);
            }

            Directory.CreateDirectory(tempFolder);
            string subFolder1 = tempFolder + @"Sub1\";
            string subFolder2 = tempFolder + @"Sub2\";
            Directory.CreateDirectory(subFolder1);
            Directory.CreateDirectory(subFolder2);

            // null or empty searchPattern
            Assert.Throws<ArgumentNullException>(() => FileHelper.DeleteFiles(tempFolder, null, SearchOption.TopDirectoryOnly));
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFiles(tempFolder, string.Empty, SearchOption.TopDirectoryOnly));

            // invalid searchPattern
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFiles(tempFolder, "*-|||^^?", SearchOption.TopDirectoryOnly));

            // populate folder with some files
            Assert.True(Directory.Exists(tempFolder));
            string tempFile1 = tempFolder + "info.txt";
            string tempFile2 = tempFolder + "financial.csv";
            string tempFile3 = tempFolder + "systemSave.temp";
            string tempFile4 = subFolder1 + "nice.txt";
            string tempFile5 = subFolder1 + "bat.csv";
            string tempFile6 = subFolder2 + "wave.txt";
            string tempFile7 = subFolder2 + "bat.temp";
            File.WriteAllText(tempFile1, "whatever");
            File.WriteAllText(tempFile2, "whatever");
            File.WriteAllText(tempFile3, "whatever");
            File.WriteAllText(tempFile4, "whatever");
            File.WriteAllText(tempFile5, "whatever");
            File.WriteAllText(tempFile6, "whatever");
            File.WriteAllText(tempFile7, "whatever");

            Assert.True(File.Exists(tempFile1));
            Assert.True(File.Exists(tempFile2));
            Assert.True(File.Exists(tempFile3));
            Assert.True(File.Exists(tempFile4));
            Assert.True(File.Exists(tempFile5));
            Assert.True(File.Exists(tempFile6));
            Assert.True(File.Exists(tempFile7));

            FileHelper.DeleteFiles(tempFolder, "*.txt", SearchOption.TopDirectoryOnly);
            Assert.False(File.Exists(tempFile1));
            Assert.True(File.Exists(tempFile2));
            Assert.True(File.Exists(tempFile3));
            Assert.True(File.Exists(tempFile4));
            Assert.True(File.Exists(tempFile5));
            Assert.True(File.Exists(tempFile6));
            Assert.True(File.Exists(tempFile7));

            FileHelper.DeleteFiles(tempFolder, "*.csv", SearchOption.AllDirectories);
            Assert.False(File.Exists(tempFile1));
            Assert.False(File.Exists(tempFile2));
            Assert.True(File.Exists(tempFile3));
            Assert.True(File.Exists(tempFile4));
            Assert.False(File.Exists(tempFile5));
            Assert.True(File.Exists(tempFile6));
            Assert.True(File.Exists(tempFile7));

            FileHelper.DeleteFiles(tempFolder, "*.txt", SearchOption.AllDirectories);
            Assert.False(File.Exists(tempFile1));
            Assert.False(File.Exists(tempFile2));
            Assert.True(File.Exists(tempFile3));
            Assert.False(File.Exists(tempFile4));
            Assert.False(File.Exists(tempFile5));
            Assert.False(File.Exists(tempFile6));
            Assert.True(File.Exists(tempFile7));

            // delete all files in an empty folder - shouldn't cause a problem
            Assert.DoesNotThrow(() => FileHelper.DeleteFiles(subFolder1, "*.*", SearchOption.TopDirectoryOnly));

            // delete everything
            Assert.DoesNotThrow(() => FileHelper.DeleteFiles(tempFolder, "*.*", SearchOption.AllDirectories));
            Assert.False(File.Exists(tempFile1));
            Assert.False(File.Exists(tempFile2));
            Assert.False(File.Exists(tempFile3));
            Assert.False(File.Exists(tempFile4));
            Assert.False(File.Exists(tempFile5));
            Assert.False(File.Exists(tempFile6));
            Assert.False(File.Exists(tempFile7));

            // cleanup
            DirectoryHelper.DeleteFolder(tempFolder);

            // all other cases are covered in DeleteFile - read-only and locked files
        }

        /// <summary>
        /// Tests the DeleteFileDos method.
        /// </summary>
        [Fact]
        public static void DeleteFileDosTest()
        {
            // argument validation
            Assert.Throws<ArgumentNullException>(() => FileHelper.DeleteFileDos(null));
            Assert.Throws<ArgumentException>(() => FileHelper.DeleteFileDos(string.Empty));

            string windowsTempFolder = Path.GetTempPath().AppendMissing(@"\");
            string tempFile = windowsTempFolder + @"\" + "DeleteFileDosTest.txt";

            // create a file and delete it
            if (!File.Exists(tempFile))
            {
                using (var writer = new StreamWriter(tempFile))
                {
                    writer.WriteLine("This is a bunch of text");
                }
            }

            Assert.True(File.Exists(tempFile));
            FileHelper.DeleteFileDos(tempFile);
            Assert.False(File.Exists(tempFile));

            // create a file
            // make read-only and system (not hidden or system because that will fail)
            // then try to delete it
            using (var writer = new StreamWriter(tempFile))
            {
                writer.WriteLine("This is a bunch of text");
            }

            File.SetAttributes(tempFile, FileAttributes.ReadOnly);
            Assert.True(File.Exists(tempFile));
            FileHelper.DeleteFileDos(tempFile);
            Assert.False(File.Exists(tempFile));
        }

        #endregion

        #region Extract Data

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_FilePathIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => FileHelper.CountLines(null));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_FilePathIsWhitespace_ThrowsArgumentException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => FileHelper.CountLines(string.Empty));
            Assert.Throws<ArgumentException>(() => FileHelper.CountLines("    "));
            Assert.Throws<ArgumentException>(() => FileHelper.CountLines("  \r\n  "));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_FilePathIsInvalid_ThrowsArgumentException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => FileHelper.CountLines(@"c:\test<\file.txt"));
            Assert.Throws<ArgumentException>(() => FileHelper.CountLines("con:"));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_FileDoesNotExist_ThrowsFileNotFoundException()
        {
            // Arrange, Act, Assert
            Assert.Throws<FileNotFoundException>(() => FileHelper.CountLines(Path.GetTempPath().AppendMissing(@"\") + "filethatdoesntexistReadAllNonBlankLines.txt"));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_DirectoryDoesNotExist_ThrowsDirectoryNotFoundException()
        {
            // Arrange, Act, Assert
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.CountLines(@"c:\directorythatdoesntexistReadLastLineTest\test.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.CountLines(Path.GetTempPath().AppendMissing(@"\")));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_FilePathIsTooLong_ThrowsPathTooLongException()
        {
            // Arrange, Act, Assert
            Assert.Throws<PathTooLongException>(() => FileHelper.CountLines(@"c:\thisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpath\file.txt"));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_FilePathFormatIsNotSupported_ThrowsNotSupportedException()
        {
            // Arrange, Act, Assert
            Assert.Throws<NotSupportedException>(() => FileHelper.CountLines(@"c:\pip:es.txt"));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_ZeroByteFile_Returns0()
        {
            // Arrange, 
            string tempFile = Path.GetTempFileName();

            // Act, 
            long lines = FileHelper.CountLines(tempFile);

            // Assert
            Assert.Equal(0, lines);
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_OneChunkOfTextWithNoNewLines_Returns1()
        {
            // Arrange, 
            string tempFile1 = Path.GetTempFileName();
            File.WriteAllText(tempFile1, "a");

            string tempFile2 = Path.GetTempFileName();
            File.WriteAllText(tempFile2, "This is the first line");

            // Act, 
            long lines1 = FileHelper.CountLines(tempFile1);
            long lines2 = FileHelper.CountLines(tempFile2);

            // Assert
            Assert.Equal(1, lines1);
            Assert.Equal(1, lines2);
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_OneChunkOfTextEndingInNewline_Returns1()
        {
            // Arrange, 
            string tempFile2 = Path.GetTempFileName();
            File.WriteAllText(tempFile2, "This is the first line" + Environment.NewLine);

            // Act, 
            long lines2 = FileHelper.CountLines(tempFile2);

            // Assert
            Assert.Equal(1, lines2);
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_NewLineFollowedByOneChunkOfText_Returns2()
        {
            // Arrange, 
            string tempFile1 = Path.GetTempFileName();
            File.WriteAllText(tempFile1, Environment.NewLine + "a");
            
            // Act, 
            long lines1 = FileHelper.CountLines(tempFile1);
            
            // Assert
            Assert.Equal(2, lines1);            
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_NewLineFollowedByOneChunkOfTextFollowedByNewline_Returns2()
        {
            // Arrange, 
            string tempFile1 = Path.GetTempFileName();
            File.WriteAllText(tempFile1, Environment.NewLine + "a" + Environment.NewLine);

            // Act, 
            long lines1 = FileHelper.CountLines(tempFile1);

            // Assert
            Assert.Equal(2, lines1);
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void CountLines_MultipleLinesSomeAreNewLine_ReturnsOneLinePerTextNewlinePairOrTextNotEndingInNewline()
        {
            // Arrange, 
            string tempFile1 = Path.GetTempFileName();
            File.WriteAllText(tempFile1, Environment.NewLine + Environment.NewLine + Environment.NewLine);

            string tempFile2 = Path.GetTempFileName();
            File.WriteAllText(tempFile2, "This is the first line" + Environment.NewLine + "this is the second line" + Environment.NewLine);

            string tempFile3 = Path.GetTempFileName();
            File.WriteAllText(tempFile3, "This is the first line" + Environment.NewLine + "this is the second line" + Environment.NewLine + "this is the third line");

            // Act, 
            long lines1 = FileHelper.CountLines(tempFile1);
            long lines2 = FileHelper.CountLines(tempFile2);
            long lines3 = FileHelper.CountLines(tempFile3);

            // Assert
            Assert.Equal(3, lines1);
            Assert.Equal(2, lines2);
            Assert.Equal(3, lines3);
        }
        
        /// <summary>
        /// Tests the CountNonBlankLinesInFile methods.
        /// </summary>
        [Fact]
        public static void CountNonblankLinesTest()
        {
            Assert.Throws<ArgumentException>(() => FileHelper.CountNonblankLines(string.Empty));
            Assert.Throws<ArgumentNullException>(() => FileHelper.CountNonblankLines(null));
            Assert.Throws<ArgumentException>(() => FileHelper.CountNonblankLines("   "));
            Assert.Throws<ArgumentException>(() => FileHelper.CountNonblankLines(@"c:\test<\file.txt"));
            Assert.Throws<ArgumentException>(() => FileHelper.CountNonblankLines("con:"));
            Assert.Throws<FileNotFoundException>(() => FileHelper.CountNonblankLines(Path.GetTempPath().AppendMissing(@"\") + "filethatdoesntexistReadAllNonBlankLines.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.CountNonblankLines(@"c:\directorythatdoesntexistReadLastLineTest\test.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.CountNonblankLines(Path.GetTempPath().AppendMissing(@"\")));
            Assert.Throws<PathTooLongException>(() => FileHelper.CountNonblankLines(@"c:\thisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpath\file.txt"));
            Assert.Throws<NotSupportedException>(() => FileHelper.CountNonblankLines(@"c:\pip:es.txt"));

            // how about: UnauthorizedAccess, Security, and OutOfMemory exceptions?  

            // zero-byte file
            string tempFile = Path.GetTempFileName();
            Assert.Equal(0, FileHelper.CountNonblankLines(tempFile));

            // one character, two characters, three characters
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "a");
            Assert.Equal(1, FileHelper.CountNonblankLines(tempFile));

            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "ab");
            Assert.Equal(1, FileHelper.CountNonblankLines(tempFile));

            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "abc");
            Assert.Equal(1, FileHelper.CountNonblankLines(tempFile));

            // one line
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line");
            Assert.Equal(1, FileHelper.CountNonblankLines(tempFile));

            // one line with newline
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine);
            Assert.Equal(1, FileHelper.CountNonblankLines(tempFile));

            // two lines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + "This is the second line");
            Assert.Equal(2, FileHelper.CountNonblankLines(tempFile));

            // two lines and a new line
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + "This is the second line" + Environment.NewLine);
            Assert.Equal(2, FileHelper.CountNonblankLines(tempFile));

            // three lines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + "This is the second line" + Environment.NewLine + "This is the third line");
            Assert.Equal(3, FileHelper.CountNonblankLines(tempFile));

            // one line of text, two lagging newlines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + Environment.NewLine);
            Assert.Equal(1, FileHelper.CountNonblankLines(tempFile));

            // two lines of text, two lagging newlines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + "This is the second line" + Environment.NewLine + Environment.NewLine);
            Assert.Equal(2, FileHelper.CountNonblankLines(tempFile));

            // newline, text, newline
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + "text" + Environment.NewLine);
            Assert.Equal(1, FileHelper.CountNonblankLines(tempFile));

            // two new lines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + Environment.NewLine);
            Assert.Equal(0, FileHelper.CountNonblankLines(tempFile));

            // three new lines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + Environment.NewLine + Environment.NewLine);
            Assert.Equal(0, FileHelper.CountNonblankLines(tempFile));

            // lock a file
            using (new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Assert.Throws<IOException>(() => FileHelper.CountNonblankLines(tempFile));
            }
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void Md5Test()
        {
            Assert.Throws<ArgumentException>(() => FileHelper.Md5(string.Empty));
            Assert.Throws<ArgumentNullException>(() => FileHelper.Md5(null));
            Assert.Throws<ArgumentException>(() => FileHelper.Md5("   "));
            Assert.Throws<ArgumentException>(() => FileHelper.Md5(@"c:\test<\file.txt"));
            Assert.Throws<ArgumentException>(() => FileHelper.Md5("con:"));
            Assert.Throws<FileNotFoundException>(() => FileHelper.Md5(Path.GetTempPath().AppendMissing(@"\") + "filethatdoesntexistMD5Test.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.Md5(@"c:\directorythatdoesntexistMD5Test\test.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.Md5(Path.GetTempPath().AppendMissing(@"\")));
            Assert.Throws<PathTooLongException>(() => FileHelper.Md5(@"c:\thisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpath\file.txt"));
            Assert.Throws<NotSupportedException>(() => FileHelper.Md5(@"c:\pip:es.txt"));

            // how do we reproduce these?
            // <exception cref="IOException">An I/O error occurs.</exception>
            // <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions on file.</exception>
            // <exception cref="SecurityException">The caller does not have the required permission.</exception>

            // test some known MD5s
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "this is a file where we know the md5");

            // should be no problem with read-only
            File.SetAttributes(tempFile, FileAttributes.ReadOnly);
            Assert.Equal("EB7B8C6A6BBFAC133490B59A457C4D0C", FileHelper.Md5(tempFile));
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "this is another file where we know the md5");
            Assert.Equal("79F3D53EA695B4A44870FE7383A591A3", FileHelper.Md5(tempFile));

            // lock a file
            using (new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Assert.Throws<IOException>(() => FileHelper.Md5(tempFile));
            }
        }

        /// <summary>
        /// Tests the ToHexString method.
        /// </summary>
        [Fact]
        public static void ToHexStringTest()
        {
            Assert.Throws<ArgumentNullException>(() => FileHelper.ToHexString(null));

            byte[] input = { };

            // corner case - empty array return empty string?
            Assert.Equal(string.Empty, FileHelper.ToHexString(input));

            var input2 = new List<byte> { 123, 101, 2, 255, 0 };
            Assert.Equal("7B6502FF00", FileHelper.ToHexString(input2));
        }

        /// <summary>
        /// Tests the ReadAllNonBlankLines method.
        /// </summary>
        [Fact]
        public static void ReadAllNonblankLinesTest()
        {
            // arguments & exceptions
            Assert.Throws<ArgumentException>(() => FileHelper.ReadAllNonblankLines(string.Empty));
            Assert.Throws<ArgumentNullException>(() => FileHelper.ReadAllNonblankLines(null));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadAllNonblankLines("   "));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadAllNonblankLines(@"c:\test<\file.txt"));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadAllNonblankLines("con:"));
            Assert.Throws<NotSupportedException>(() => FileHelper.ReadAllNonblankLines(@"c:\pip:es.txt"));
            Assert.Throws<FileNotFoundException>(() => FileHelper.ReadAllNonblankLines(Path.GetTempPath().AppendMissing(@"\") + "filethatdoesntexistReadAllNonBlankLinesTest.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ReadAllNonblankLines(@"c:\directorythatdoesntexistReadAllNonBlankLinesTest\test.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ReadAllNonblankLines(Path.GetTempPath().AppendMissing(@"\")));
            Assert.Throws<PathTooLongException>(() => FileHelper.ReadAllNonblankLines(@"c:\thisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpath\file.txt"));

            string tempFile = Path.GetTempFileName();
            File.SetAttributes(tempFile, FileAttributes.ReadOnly);
            Assert.DoesNotThrow(() => FileHelper.ReadAllNonblankLines(tempFile));

            tempFile = Path.GetTempFileName();
            using (new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Assert.Throws<IOException>(() => FileHelper.ReadAllNonblankLines(tempFile));
            }

            // what's the best way to reproduce these?
            // <exception cref="UnauthorizedAccessException">caller doesn't have the required permissions</exception>
            // <exception cref="SecurityException">the caller doesn't have the required permissions.</exception>

            // zero-byte file
            tempFile = Path.GetTempFileName();
            Assert.Empty(FileHelper.ReadAllNonblankLines(tempFile));

            // one line
            tempFile = Path.GetTempFileName();
            // ReSharper disable JoinDeclarationAndInitializer
            ReadOnlyCollection<string> results;
            // ReSharper restore JoinDeclarationAndInitializer
            File.WriteAllText(tempFile, "this is a line");
            results = FileHelper.ReadAllNonblankLines(tempFile);
            Assert.Equal(1, results.Count);
            Assert.Equal("this is a line", results[0]);

            // one line with newline
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine);
            results = FileHelper.ReadAllNonblankLines(tempFile);
            Assert.Equal(1, results.Count);
            Assert.Equal("This is the first line", results[0]);

            // newline then one line
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + "This is the first line");
            results = FileHelper.ReadAllNonblankLines(tempFile);
            Assert.Equal(1, results.Count);
            Assert.Equal("This is the first line", results[0]);

            // two lines and a new line
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + "This is the second line" + Environment.NewLine);
            results = FileHelper.ReadAllNonblankLines(tempFile);
            Assert.Equal(2, results.Count);
            Assert.Equal("This is the first line", results[0]);
            Assert.Equal("This is the second line", results[1]);

            // file with lots of newlines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + Environment.NewLine + "This is the first line" + Environment.NewLine + "This is the second line" + Environment.NewLine + "This is the third line" + Environment.NewLine + Environment.NewLine);
            results = FileHelper.ReadAllNonblankLines(tempFile);
            Assert.Equal(3, results.Count);
            Assert.Equal("This is the first line", results[0]);
            Assert.Equal("This is the second line", results[1]);
            Assert.Equal("This is the third line", results[2]);
        }

        /// <summary>
        /// Tests the ReadFirstNonHeaderLine method.
        /// </summary>
        [Fact]
        public static void ReadFirstNonHeaderLineTest()
        {
            // arguments and exceptions
            Assert.Throws<ArgumentException>(() => FileHelper.ReadFirstNonHeaderLine(string.Empty));
            Assert.Throws<ArgumentNullException>(() => FileHelper.ReadFirstNonHeaderLine(null));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadFirstNonHeaderLine("   "));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadFirstNonHeaderLine(@"c:\test<\file.txt"));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadFirstNonHeaderLine("con:"));
            Assert.Throws<FileNotFoundException>(() => FileHelper.ReadFirstNonHeaderLine(Path.GetTempPath().AppendMissing(@"\") + "filethatdoesntexistReadFirstNonHeaderLineTest.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ReadFirstNonHeaderLine(@"c:\directorythatdoesntexistReadFirstNonHeaderLineTest\test.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ReadFirstNonHeaderLine(Path.GetTempPath().AppendMissing(@"\")));
            Assert.Throws<PathTooLongException>(() => FileHelper.ReadFirstNonHeaderLine(@"c:\thisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpath\file.txt"));
            Assert.Throws<NotSupportedException>(() => FileHelper.ReadFirstNonHeaderLine(@"c:\pip:es.txt"));

            // how do we reproduce these?
            // <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions on file.</exception>
            // <exception cref="SecurityException">The caller does not have the required permission.</exception>
            // <exception cref="OutOfMemoryException">Thrown when there is insufficient memory to allocate a buffer for the returned string.</exception>

            // zero-byte file
            string tempFile = Path.GetTempFileName();
            Assert.Throws<InvalidOperationException>(() => FileHelper.ReadFirstNonHeaderLine(tempFile));

            // one character
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "a");
            Assert.Equal(string.Empty, FileHelper.ReadFirstNonHeaderLine(tempFile));

            // one line
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line");
            Assert.Equal(string.Empty, FileHelper.ReadFirstNonHeaderLine(tempFile));

            // one line with newline
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine);
            Assert.Equal(string.Empty, FileHelper.ReadFirstNonHeaderLine(tempFile));

            // two lines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + "This is the second line");
            Assert.Equal("This is the second line", FileHelper.ReadFirstNonHeaderLine(tempFile));

            // one leading newline, one line of text
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + "This is the first line");
            Assert.Equal("This is the first line", FileHelper.ReadFirstNonHeaderLine(tempFile));

            // header, newline, third line
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + Environment.NewLine + "This is the third line");
            Assert.Equal(string.Empty, FileHelper.ReadFirstNonHeaderLine(tempFile));

            // three lines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + "This is the second line" + Environment.NewLine + "This is the third line");
            Assert.Equal("This is the second line", FileHelper.ReadFirstNonHeaderLine(tempFile));

            // lock a file
            using (new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Assert.Throws<IOException>(() => FileHelper.ReadFirstNonHeaderLine(tempFile));
            }
        }

        /// <summary>
        /// Tests the ReadHeaderLine method.
        /// </summary>
        [Fact]
        public static void ReadHeaderLineTest()
        {
            // arguments and exceptions
            Assert.Throws<ArgumentException>(() => FileHelper.ReadHeaderLine(string.Empty));
            Assert.Throws<ArgumentNullException>(() => FileHelper.ReadHeaderLine(null));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadHeaderLine("   "));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadHeaderLine(@"c:\test<\file.txt"));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadHeaderLine("con:"));
            Assert.Throws<FileNotFoundException>(() => FileHelper.ReadHeaderLine(Path.GetTempPath().AppendMissing(@"\") + "filethatdoesntexistReadHeaderLineTest.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ReadHeaderLine(@"c:\directorythatdoesntexistReadHeaderLineTest\test.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ReadHeaderLine(Path.GetTempPath().AppendMissing(@"\")));
            Assert.Throws<PathTooLongException>(() => FileHelper.ReadHeaderLine(@"c:\thisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpath\file.txt"));
            Assert.Throws<NotSupportedException>(() => FileHelper.ReadHeaderLine(@"c:\pip:es.txt"));

            // how do we reproduce these?
            // <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions on file.</exception>
            // <exception cref="SecurityException">The caller does not have the required permission.</exception>
            // <exception cref="OutOfMemoryException">Thrown when there is insufficient memory to allocate a buffer for the returned string.</exception>

            // zero-byte file
            string tempFile = Path.GetTempFileName();
            Assert.Equal(string.Empty, FileHelper.ReadHeaderLine(tempFile));

            // one character
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "a");
            Assert.Equal("a", FileHelper.ReadHeaderLine(tempFile));

            // one line
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line");
            Assert.Equal("This is the first line", FileHelper.ReadHeaderLine(tempFile));

            // one line with newline
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine);
            Assert.Equal("This is the first line", FileHelper.ReadHeaderLine(tempFile));

            // two lines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + "This is the second line");
            Assert.Equal("This is the first line", FileHelper.ReadHeaderLine(tempFile));

            // one leading newline, one line of text
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + "This is the first line");
            Assert.Equal(string.Empty, FileHelper.ReadHeaderLine(tempFile));

            // lock a file
            using (new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Assert.Throws<IOException>(() => FileHelper.ReadHeaderLine(tempFile));
            }
        }

        /// <summary>
        /// Tests the ReadLastLine and ReadLastLineNonblank methods.
        /// </summary>
        [Fact]
        public static void ReadLastLineTest()
        {
            Assert.Throws<ArgumentException>(() => FileHelper.ReadLastLine(string.Empty));
            Assert.Throws<ArgumentNullException>(() => FileHelper.ReadLastLine(null));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadLastLine("   "));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadLastLine(@"c:\test<\file.txt"));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadLastLine("con:"));
            Assert.Throws<FileNotFoundException>(() => FileHelper.ReadLastLine(Path.GetTempPath().AppendMissing(@"\") + "filethatdoesntexistReadLastLineTest.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ReadLastLine(@"c:\directorythatdoesntexistReadLastLineTest\test.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ReadLastLine(Path.GetTempPath().AppendMissing(@"\")));
            Assert.Throws<PathTooLongException>(() => FileHelper.ReadLastLine(@"c:\thisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpath\file.txt"));
            Assert.Throws<NotSupportedException>(() => FileHelper.ReadLastLine(@"c:\pip:es.txt"));

            Assert.Throws<ArgumentException>(() => FileHelper.ReadLastNonblankLine(string.Empty));
            Assert.Throws<ArgumentNullException>(() => FileHelper.ReadLastNonblankLine(null));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadLastNonblankLine("   "));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadLastNonblankLine(@"c:\test<\file.txt"));
            Assert.Throws<ArgumentException>(() => FileHelper.ReadLastNonblankLine("con:"));
            Assert.Throws<FileNotFoundException>(() => FileHelper.ReadLastNonblankLine(Path.GetTempPath().AppendMissing(@"\") + "filethatdoesntexistReadLastNonBlankLine.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ReadLastNonblankLine(@"c:\directorythatdoesntexistReadLastNonBlankLine\test.txt"));
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.ReadLastNonblankLine(Path.GetTempPath().AppendMissing(@"\")));
            Assert.Throws<PathTooLongException>(() => FileHelper.ReadLastNonblankLine(@"c:\thisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpaththisisaverlongpath\file.txt"));
            Assert.Throws<NotSupportedException>(() => FileHelper.ReadLastNonblankLine(@"c:\pip:es.txt"));

            // how do we reproduce these?
            // <exception cref="UnauthorizedAccessException">Caller doesn't have read permissions on file.</exception>
            // <exception cref="SecurityException">The caller does not have the required permission.</exception>
            // <exception cref="OutOfMemoryException">Thrown when there is insufficient memory to allocate a buffer for the returned string.</exception>

            // zero-byte file
            string tempFile = Path.GetTempFileName();
            Assert.Equal(string.Empty, FileHelper.ReadLastLine(tempFile));
            Assert.Equal(string.Empty, FileHelper.ReadLastNonblankLine(tempFile));

            // one character, two characters, three characters
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "a");
            Assert.Equal("a", FileHelper.ReadLastLine(tempFile));
            Assert.Equal("a", FileHelper.ReadLastNonblankLine(tempFile));
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "ab");
            Assert.Equal("ab", FileHelper.ReadLastLine(tempFile));
            Assert.Equal("ab", FileHelper.ReadLastNonblankLine(tempFile));
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "abc");
            Assert.Equal("abc", FileHelper.ReadLastLine(tempFile));
            Assert.Equal("abc", FileHelper.ReadLastNonblankLine(tempFile));

            // one line
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line");
            Assert.Equal("This is the first line", FileHelper.ReadLastLine(tempFile));
            Assert.Equal("This is the first line", FileHelper.ReadLastNonblankLine(tempFile));

            // one line with newline
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine);
            Assert.Equal(string.Empty, FileHelper.ReadLastLine(tempFile));
            Assert.Equal("This is the first line", FileHelper.ReadLastNonblankLine(tempFile));

            // two lines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + "This is the second line");
            Assert.Equal("This is the second line", FileHelper.ReadLastLine(tempFile));
            Assert.Equal("This is the second line", FileHelper.ReadLastNonblankLine(tempFile));

            // two lines and a new line
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + "This is the second line" + Environment.NewLine);
            Assert.Equal(string.Empty, FileHelper.ReadLastLine(tempFile));
            Assert.Equal("This is the second line", FileHelper.ReadLastNonblankLine(tempFile));

            // three lines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + "This is the second line" + Environment.NewLine + "This is the third line");
            Assert.Equal("This is the third line", FileHelper.ReadLastLine(tempFile));
            Assert.Equal("This is the third line", FileHelper.ReadLastNonblankLine(tempFile));

            // one line of text, two lagging newlines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + Environment.NewLine);
            Assert.Equal(string.Empty, FileHelper.ReadLastLine(tempFile));
            Assert.Equal("This is the first line", FileHelper.ReadLastNonblankLine(tempFile));

            // two lines of text, two lagging newlines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "This is the first line" + Environment.NewLine + "This is the second line" + Environment.NewLine + Environment.NewLine);
            Assert.Equal(string.Empty, FileHelper.ReadLastLine(tempFile));
            Assert.Equal("This is the second line", FileHelper.ReadLastNonblankLine(tempFile));

            // newline, text, newline
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + "text" + Environment.NewLine);
            Assert.Equal(string.Empty, FileHelper.ReadLastLine(tempFile));
            Assert.Equal("text", FileHelper.ReadLastNonblankLine(tempFile));

            // two new lines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + Environment.NewLine);
            Assert.Equal(string.Empty, FileHelper.ReadLastLine(tempFile));
            Assert.Equal(string.Empty, FileHelper.ReadLastNonblankLine(tempFile));

            // three new lines
            tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, Environment.NewLine + Environment.NewLine + Environment.NewLine);
            Assert.Equal(string.Empty, FileHelper.ReadLastLine(tempFile));
            Assert.Equal(string.Empty, FileHelper.ReadLastNonblankLine(tempFile));

            // lock a file
            using (new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Assert.Throws<IOException>(() => FileHelper.ReadLastLine(tempFile));
                Assert.Throws<IOException>(() => FileHelper.ReadLastNonblankLine(tempFile));
            }
        }

        #endregion

        #region Legal and Illegal Files

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void IsValidFileName_ParameterFileNameIsNull_ThrowsNullArgumentException()
        {
            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => FileHelper.IsValidFileName(null));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void IsValidFileName_ParameterFileNameIsWhiteSpace_ThrowsArgumentException()
        {
            // Act / Assert            
            Assert.Throws<ArgumentException>(() => FileHelper.IsValidFileName(string.Empty));
            Assert.Throws<ArgumentException>(() => FileHelper.IsValidFileName("    "));
            Assert.Throws<ArgumentException>(() => FileHelper.IsValidFileName("  \r\n   "));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void IsValidFileName_InvalidFileName_ReturnsFalse()
        {
            // Act / Assert
            Assert.False(FileHelper.IsValidFileName("testing\""));
            Assert.False(FileHelper.IsValidFileName("my\\test"));
            Assert.False(FileHelper.IsValidFileName("my*file.txt"));
            Assert.False(FileHelper.IsValidFileName("PRN.txt"));
            Assert.False(FileHelper.IsValidFileName("CON.txt"));
        }

        /// <summary>
        /// Test method.
        /// </summary>
        [Fact]
        public static void IsValidFileName_ValidFileName_ReturnsTrue()
        {
            // Act / Assert
            Assert.True(FileHelper.IsValidFileName("test"));
            Assert.True(FileHelper.IsValidFileName("test.txt"));
            Assert.True(FileHelper.IsValidFileName(" test.txt"));
            Assert.True(FileHelper.IsValidFileName("test.txt "));
            Assert.True(FileHelper.IsValidFileName(" test.txt "));
            Assert.True(FileHelper.IsValidFileName("test.txt .rstudio.R"));
        }

        /// <summary>
        /// Tests the IsValidPath method
        /// </summary>
        [Fact]
        public static void IsValidFilePathTest()
        {
            // bad filepaths
            Assert.False(FileHelper.IsValidFilePath(null));
            Assert.False(FileHelper.IsValidFilePath(string.Empty));
            Assert.False(FileHelper.IsValidFilePath("    "));
            Assert.False(FileHelper.IsValidFilePath(@"c:\test>.txt"));
            Assert.False(FileHelper.IsValidFilePath(@"c:\baddir>s\test.txt"));
            Assert.False(FileHelper.IsValidFilePath(@"c:\te:st.txt"));
            Assert.False(FileHelper.IsValidFilePath(@"c:\badd:irs\test.txt"));
            Assert.False(FileHelper.IsValidFilePath("c:\folderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolongfolderthatstoolong\test.txt"));
            Assert.False(FileHelper.IsValidFilePath(Path.GetTempPath().AppendMissing(@"\") + "filethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolongfilethatstoolong.txt"));

            // good filepaths
            Assert.False(FileHelper.IsValidFilePath(Path.GetTempPath()));
            Assert.True(FileHelper.IsValidFilePath(Path.GetTempFileName()));
            Assert.True(FileHelper.IsValidFilePath("test.txt"));
            Assert.True(FileHelper.IsValidFilePath(" test.txt"));
            Assert.True(FileHelper.IsValidFilePath("test.txt "));

            // files/folders that don't exist should still be valid
            string tempFilepath = Path.GetTempFileName();
            Assert.False(File.Exists(tempFilepath + ".unknown"));

            Assert.True(FileHelper.IsValidFilePath(tempFilepath + ".unknown"));

            Assert.False(FileHelper.IsValidFilePath(tempFilepath + @".unknown\"));

            Assert.False(FileHelper.IsValidFilePath(@"c:\folderthatdoesntexistCheckValidFilePath\"));

            Assert.True(FileHelper.IsValidFilePath(@"c:\folderthatdoesntexistCheckValidFilePath\filethatdoesntexistCheckValidFilePath.txt"));

            Assert.True(FileHelper.IsValidFilePath(@"x:\drivethatdoesntexist"));

            Assert.False(FileHelper.IsValidFilePath(@"con:"));

            Assert.True(FileHelper.IsValidFilePath(@"con:suraj.txt"));

            Assert.True(FileHelper.IsValidFilePath(@"c:on\suraj.txt"));

            // restricted os files
            Assert.False(FileHelper.IsValidFilePath("lpt1.txt"));
            Assert.False(FileHelper.IsValidFilePath(@"c:\folder\con.txt"));
            Assert.False(FileHelper.IsValidFilePath(@"c:\folder\lpt0.txt"));
            Assert.False(FileHelper.IsValidFilePath(@"c:\folder\lpt1.txt.xlsb"));

            // how about directories with leading or training spaces?  are those just ignored by the OS? (i.e. they OK)
        }

        /// <summary>
        /// Tests the IsOSRestrictedPath method
        /// </summary>
        [Fact]
        public static void IsOsRestrictedPathTest()
        {
            Assert.True(FileHelper.IsOsRestrictedPath("con.whatever.i.want.txt"));
            Assert.False(FileHelper.IsOsRestrictedPath("whtever.con.i.want.txt"));
            Assert.True(FileHelper.IsOsRestrictedPath("nUL"));
            
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:\mydir\my.long.dir\lpt1.aux"));
            Assert.False(FileHelper.IsOsRestrictedPath(@"c:\mydir\my.long.dir\whatever.aux"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:\mydir\prn.long.dir\"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:\mydir\prn.long.dir"));
            Assert.True(FileHelper.IsOsRestrictedPath("nUL"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:\NUl"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:\NUl\whatever.txt"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:\con\whatever.txt"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:\LPT1.0\whatever.txt"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:\good\great\lPT2.whatever\"));
            Assert.False(FileHelper.IsOsRestrictedPath(@"c:\good\great\whatever.lpt2\"));
            Assert.False(FileHelper.IsOsRestrictedPath(@"c:\"));
            Assert.False(FileHelper.IsOsRestrictedPath(@"\"));
            
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:/mydir/my.long.dir/lpt1.aux"));
            Assert.False(FileHelper.IsOsRestrictedPath(@"c:/mydir/my.long.dir/whatever.aux"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:/mydir/prn.long.dir/"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:/mydir/prn.long.dir"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:/NUl"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:/NUl/whatever.txt"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:/con/whatever.txt"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:/LPT1.0/whatever.txt"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:/good/great/lPT2.whatever/"));
            Assert.False(FileHelper.IsOsRestrictedPath(@"c:/good/great/whatever.lpt2/"));
            Assert.False(FileHelper.IsOsRestrictedPath(@"c:/"));
            Assert.False(FileHelper.IsOsRestrictedPath(@"/"));

            Assert.True(FileHelper.IsOsRestrictedPath(@"c:/mydir\my.long.dir/lpt1.aux"));
            Assert.False(FileHelper.IsOsRestrictedPath(@"c:/mydir\my.long.dir\whatever.aux"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:\mydir/prn.long.dir\"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:/mydir\prn.long.dir"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:\NUl/whatever.txt"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:\con/whatever.txt"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:/LPT1.0\whatever.txt"));
            Assert.True(FileHelper.IsOsRestrictedPath(@"c:/good\great/lPT2.whatever/"));
            
            Assert.Throws<ArgumentException>(() => FileHelper.IsOsRestrictedPath(string.Empty));
            Assert.Throws<ArgumentNullException>(() => FileHelper.IsOsRestrictedPath(null));

            // IsOsRestrictedPath doesn't check for illegal characters.  Use IsValidFilePath and IsValidDirectoryPath to check
            // for illegal characters and OS restricted terms
            // Assert.Throws<ArgumentException>( () => FileHelper.IsOSRestrictedPath( @"c:\someillegal<chars\con.txt" ) );
            // Assert.Throws<ArgumentException>( () => FileHelper.IsOSRestrictedPath( @"c:\someillegalchars\con.<txt" ) );            
        }

        /// <summary>
        /// Tests the MakeLegalFilename method
        /// </summary>
        [Fact]
        public static void MakeLegalFileNameTest()
        {
            string badname = @"a\b/c:d*e?f""g<h>i|.test.txt";
            string goodname = FileHelper.MakeLegalFileName(badname);
            Assert.Equal("a b c d e f g h i .test.txt", goodname);

            badname = "this/isaTest.txt";
            goodname = FileHelper.MakeLegalFileName(badname);
            Assert.Equal("this isaTest.txt", goodname);

            Assert.Throws<ArgumentNullException>(() => FileHelper.MakeLegalFileName(null));
            Assert.Throws<ArgumentException>(() => FileHelper.MakeLegalFileName(string.Empty));
            Assert.Throws<ArgumentException>(() => FileHelper.MakeLegalFileName("    "));
        }

        #endregion

        #region Locking and Writeability

        /// <summary>
        /// Tests the CanWriteToFile method
        /// </summary>
        [Fact]
        public static void CanWriteToFileTest()
        {
            // argument and exception tests
            Assert.Throws<ArgumentNullException>(() => FileHelper.CanWriteToFile(null));
            Assert.Throws<ArgumentException>(() => FileHelper.CanWriteToFile(string.Empty));
            Assert.Throws<ArgumentException>(() => FileHelper.CanWriteToFile("    "));
            Assert.Throws<ArgumentException>(() => FileHelper.CanWriteToFile(@"c:\pip>es.txt"));
            Assert.Throws<NotSupportedException>(() => FileHelper.CanWriteToFile(@"c:\pip:es.txt"));

            // PathTooLong and DirectoryNotFound ignored
            Assert.False(FileHelper.CanWriteToFile(@"c:\thisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileename.txt"));
            Assert.False(FileHelper.CanWriteToFile(@"c:\DoesntExistDirectory\test.txt"));

            // create a temp file, we should be able to write to it
            string tempFile = Path.GetTempFileName();
            Assert.True(File.Exists(tempFile));
            Assert.True(FileHelper.CanWriteToFile(tempFile));

            // open streamreader to the file, shouldn't be able to write to it because, by default, Streamreader calls
            // new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read) 
            using (new StreamReader(tempFile))
            {
                Assert.False(FileHelper.CanWriteToFile(tempFile));
            }

            using (new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                Assert.True(FileHelper.CanWriteToFile(tempFile));
            }

            // open a filestream without write permissions
            using (new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Assert.False(FileHelper.CanWriteToFile(tempFile));
            }

            // make file readonly, system, hidden - shouldn't be able to write to it
            File.SetAttributes(tempFile, FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden);
            Assert.False(FileHelper.CanWriteToFile(tempFile));

            // cleanup
            FileHelper.DeleteFile(tempFile);

            // specify a directory - can't write to a directory
            string folder = Path.GetTempPath().AppendMissing(@"\") + "CanWriteToFile";
            Directory.CreateDirectory(folder);
            Assert.False(FileHelper.CanWriteToFile(folder));
            Directory.Delete(folder);

            // specify a file that doesn't exist
            tempFile = Path.GetTempFileName();
            File.Delete(tempFile);
            Assert.False(File.Exists(tempFile));
            Assert.True(FileHelper.CanWriteToFile(tempFile));
        }

        /// <summary>
        /// Tests the IsFileInUse method
        /// </summary>
        [Fact]
        public static void IsFileInUseTest()
        {
            // argument and exception tests
            Assert.Throws<ArgumentNullException>(() => FileHelper.IsFileInUse(null));
            Assert.Throws<ArgumentException>(() => FileHelper.IsFileInUse(string.Empty));
            Assert.Throws<ArgumentException>(() => FileHelper.IsFileInUse("    "));
            Assert.Throws<ArgumentException>(() => FileHelper.IsFileInUse(@"c:\pip>es.txt"));
            Assert.Throws<NotSupportedException>(() => FileHelper.IsFileInUse(@"c:\pip:es.txt"));

            // if the path is too long or directory is not found then interpret as file is not in use
            Assert.False(FileHelper.IsFileInUse(@"c:\thisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileename.txt"));
            Assert.False(FileHelper.IsFileInUse(@"c:\DoesntExistDirectory\test.txt"));

            // create a temp file, it shouldn't be locked
            string tempFile = Path.GetTempFileName();
            Assert.True(File.Exists(tempFile));
            Assert.False(FileHelper.IsFileInUse(tempFile));

            // open streamreader to the file, should be locked now
            using (new StreamReader(tempFile))
            {
                Assert.True(FileHelper.IsFileInUse(tempFile));
            }

            using (new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                Assert.True(FileHelper.IsFileInUse(tempFile));
            }

            Assert.False(FileHelper.IsFileInUse(tempFile));

            // make file readonly, system, hidden - shouldn't be locked
            File.SetAttributes(tempFile, FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden);
            Assert.False(FileHelper.IsFileInUse(tempFile));

            // cleanup
            FileHelper.DeleteFile(tempFile);

            // specify a directory - should be locked
            string folder = Path.GetTempPath().AppendMissing(@"\") + "FileInUse";
            Directory.CreateDirectory(folder);
            Assert.True(FileHelper.IsFileInUse(folder));
            Directory.Delete(folder);

            // specify a file that doesn't exist
            tempFile = Path.GetTempFileName();
            File.Delete(tempFile);
            Assert.False(File.Exists(tempFile));
            Assert.False(FileHelper.IsFileInUse(tempFile));
        }

        /// <summary>
        /// Tests the WaitForUnlock method
        /// </summary>
        [Fact]
        public static void WaitForUnlockTest()
        {
            // arguments & exceptions
            Assert.Throws<ArgumentNullException>(() => FileHelper.WaitForUnlock(null, 5));
            Assert.Throws<ArgumentException>(() => FileHelper.WaitForUnlock(string.Empty, 5));
            Assert.Throws<ArgumentException>(() => FileHelper.WaitForUnlock("    ", 5));
            Assert.Throws<ArgumentException>(() => FileHelper.WaitForUnlock(@"c:\pip>es.txt", 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => FileHelper.WaitForUnlock(@"c:\pipes.txt", 0));
            Assert.Throws<NotSupportedException>(() => FileHelper.WaitForUnlock(@"c:\pip:es.txt", 5));

            // path too long or directory not found, returns true
            Assert.True(FileHelper.WaitForUnlock(@"c:\thisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileename.txt", 5));
            Assert.True(FileHelper.WaitForUnlock(@"c:\DoesntExistDirectory\test.txt", 5));

            // create file - should be unlocked
            string tempFile = Path.GetTempFileName();
            Assert.True(File.Exists(tempFile));
            Assert.True(FileHelper.WaitForUnlock(tempFile, 2));

            // open streamreader to the file, should be locked now
            // ReSharper disable JoinDeclarationAndInitializer
            FileStream stream;
            // ReSharper restore JoinDeclarationAndInitializer
            using (new StreamReader(tempFile))
            {
                Assert.False(FileHelper.WaitForUnlock(tempFile, 2));
            }

            using (new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                Assert.False(FileHelper.WaitForUnlock(tempFile, 2));
            }

            Assert.True(FileHelper.WaitForUnlock(tempFile, 2));

            // make file readonly, system, hidden - shouldn't be locked
            File.SetAttributes(tempFile, FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden);
            Assert.True(FileHelper.WaitForUnlock(tempFile, 2));

            // cleanup
            FileHelper.DeleteFile(tempFile);

            // specify a directory - should be locked
            string folder = Path.GetTempPath().AppendMissing(@"\") + "WaitForUnlock";
            Directory.CreateDirectory(folder);
            Assert.False(FileHelper.WaitForUnlock(folder, 2));
            Directory.Delete(folder);

            // specify a file that doesn't exist
            tempFile = Path.GetTempFileName();
            File.Delete(tempFile);
            Assert.False(File.Exists(tempFile));
            Assert.True(FileHelper.WaitForUnlock(tempFile, 2));

            // create a temp file, open a filestream creating a lock
            // wait for unlock to see what happens
            tempFile = Path.GetTempFileName();
            Assert.True(File.Exists(tempFile));
            stream = new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var disposeParams = new DisposeFilestreamParams(stream, 6);
            var th = new Thread(DisposeFilestream);
            th.Start(disposeParams);
            Assert.False(FileHelper.WaitForUnlock(tempFile, 1));
            Assert.True(FileHelper.WaitForUnlock(tempFile, 10));
            File.Delete(tempFile);
        }

        /// <summary>
        /// Tests the WaitUntilFileIsWritable method
        /// </summary>
        [Fact]
        public static void WaitUntilFileIsWritableTest()
        {
            // argument and exception tests
            Assert.Throws<ArgumentNullException>(() => FileHelper.WaitUntilFileIsWritable(null, 2));
            Assert.Throws<ArgumentException>(() => FileHelper.WaitUntilFileIsWritable(string.Empty, 2));
            Assert.Throws<ArgumentException>(() => FileHelper.WaitUntilFileIsWritable("    ", 2));
            Assert.Throws<ArgumentException>(() => FileHelper.WaitUntilFileIsWritable(@"c:\pip>es.txt", 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => FileHelper.WaitUntilFileIsWritable(@"c:\pipes.txt", 0));
            Assert.Throws<NotSupportedException>(() => FileHelper.WaitUntilFileIsWritable(@"c:\pip:es.txt", 2));

            // path too long or directory not found - returns false
            Assert.False(FileHelper.WaitUntilFileIsWritable(@"c:\thisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileenamethisisaverlongfileename.txt", 2));
            Assert.False(FileHelper.WaitUntilFileIsWritable(@"c:\DoesntExistDirectory\test.txt", 2));

            // create a temp file, we should be able to write to it
            string tempFile = Path.GetTempFileName();
            Assert.True(File.Exists(tempFile));
            Assert.True(FileHelper.WaitUntilFileIsWritable(tempFile, 2));

            // open streamreader to the file, shouldn't be able to write to it because, by default, Streamreader calls
            // new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read) 
            // ReSharper disable JoinDeclarationAndInitializer
            FileStream stream;
            // ReSharper restore JoinDeclarationAndInitializer
            using (new StreamReader(tempFile))
            {
                Assert.False(FileHelper.WaitUntilFileIsWritable(tempFile, 3));
            }

            using (new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                Assert.True(FileHelper.WaitUntilFileIsWritable(tempFile, 3));
            }

            // open a filestream without write permissions
            using (new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Assert.False(FileHelper.WaitUntilFileIsWritable(tempFile, 3));
            }

            // make file readonly, system, hidden - shouldn't be able to write to it
            File.SetAttributes(tempFile, FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Hidden);
            Assert.False(FileHelper.WaitUntilFileIsWritable(tempFile, 3));

            // cleanup
            FileHelper.DeleteFile(tempFile);

            // specify a directory - can't write to a directory
            string folder = Path.GetTempPath().AppendMissing(@"\") + "WaitUntilFileIsWritable";
            Directory.CreateDirectory(folder);
            Assert.False(FileHelper.WaitUntilFileIsWritable(folder, 3));
            Directory.Delete(folder);

            // specify a file that doesn't exist
            tempFile = Path.GetTempFileName();
            File.Delete(tempFile);
            Assert.False(File.Exists(tempFile));
            Assert.True(FileHelper.WaitUntilFileIsWritable(tempFile, 3));

            // create a temp file, open a filestream with read-write permission
            // wait for unlock to see what happens
            tempFile = Path.GetTempFileName();
            Assert.True(File.Exists(tempFile));
            stream = new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var disposeParams = new DisposeFilestreamParams(stream, 20);
            var th = new Thread(DisposeFilestream);
            th.Start(disposeParams);
            Assert.True(FileHelper.WaitUntilFileIsWritable(tempFile, 5));  // this should immediately unlock
            stream.Dispose();
            th.Abort();

            // now with read permissions
            stream = new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            disposeParams = new DisposeFilestreamParams(stream, 10);
            th = new Thread(DisposeFilestream);
            th.Start(disposeParams);
            Assert.False(FileHelper.WaitUntilFileIsWritable(tempFile, 3));  // this should immediately unlock
            Assert.True(FileHelper.WaitUntilFileIsWritable(tempFile, 15));  // this should immediately unlock           
            File.Delete(tempFile);
        }

        #endregion

        #region Zero Byte Files

        /// <summary>
        /// Tests the CreateZeroByteFile method
        /// </summary>
        [Fact]
        public static void CreateZeroByteFileTest()
        {
            // arguments and exceptions
            Assert.Throws<ArgumentException>(() => FileHelper.CreateZeroByteFile(string.Empty));
            Assert.Throws<ArgumentNullException>(() => FileHelper.CreateZeroByteFile(null));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateZeroByteFile(@"c:\test\sbc>.txt"));
            Assert.Throws<ArgumentException>(() => FileHelper.CreateZeroByteFile("     "));
            Assert.Throws<NotSupportedException>(() => FileHelper.CreateZeroByteFile(@"lpt:"));
            Assert.Throws<NotSupportedException>(() => FileHelper.CreateZeroByteFile(@"c:\tes:t.txt"));
            Assert.Throws<PathTooLongException>(() => FileHelper.CreateZeroByteFile(@"c:\abcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefg.txt"));

            // file that already exists
            string tempFilepath = Path.GetTempFileName();
            Assert.Throws<IOException>(() => FileHelper.CreateZeroByteFile(tempFilepath));

            // directory doesn't exist
            tempFilepath = @"c:\directorythatdoesntexistCreateZeroByteFileTest\test.txt";
            Assert.Throws<DirectoryNotFoundException>(() => FileHelper.CreateZeroByteFile(tempFilepath));

            // create a zero byte file
            tempFilepath = Path.GetTempFileName() + ".new";
            Assert.False(File.Exists(tempFilepath));
            Assert.True(FileHelper.CreateZeroByteFile(tempFilepath));
            Assert.True(File.Exists(tempFilepath));
            var fileInfo = new FileInfo(tempFilepath);
            Assert.Equal(0, fileInfo.Length);

            // create a zero byte file in a read-only folder - no problem
            string tempFolder = Path.GetTempPath().AppendMissing(@"\") + @"CreateZeroByteTest\";
            DirectoryHelper.DeleteFolder(tempFolder, true);
            Assert.True(Directory.Exists(tempFolder));
            // ReSharper disable ObjectCreationAsStatement
            new DirectoryInfo(tempFolder) { Attributes = FileAttributes.ReadOnly };
            // ReSharper restore ObjectCreationAsStatement
            tempFilepath = tempFolder + "Test.txt";
            Assert.True(FileHelper.CreateZeroByteFile(tempFilepath));
            Assert.True(File.Exists(tempFilepath));
            fileInfo = new FileInfo(tempFilepath);
            Assert.Equal(0, fileInfo.Length);
            DirectoryHelper.DeleteFolder(tempFolder);

            // no great way to test these:
            // <exception cref="SecurityException">The caller does not have the required permission</exception>
            // <exception cref="UnauthorizedAccessException">Thrown when method can't access rootFolder to clear out older temporary files, or when the system doesn't have permission to write the zero-byte file to rootFolder.</exception>            
        }

        /// <summary>
        /// Tests the IsFileSizeZero method
        /// </summary>
        [Fact]
        public static void IsFileSizeZeroTest()
        {
            // arguments and exceptions
            Assert.Throws<ArgumentException>(() => FileHelper.IsFileSizeZero(string.Empty));
            Assert.Throws<ArgumentNullException>(() => FileHelper.IsFileSizeZero(null));
            Assert.Throws<ArgumentException>(() => FileHelper.IsFileSizeZero(@"c:\test\sbc>.txt"));
            Assert.Throws<ArgumentException>(() => FileHelper.IsFileSizeZero("     "));
            Assert.Throws<NotSupportedException>(() => FileHelper.IsFileSizeZero(@"c:\test\:sbc.txt"));
            Assert.Throws<PathTooLongException>(() => FileHelper.IsFileSizeZero(@"c:\abcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefg.txt"));
            Assert.Throws<FileNotFoundException>(() => FileHelper.IsFileSizeZero(@"r:\test.txt"));
            Assert.Throws<FileNotFoundException>(() => FileHelper.IsFileSizeZero(Path.GetTempPath().AppendMissing(@"\")));

            // can't test these:
            // <exception cref="SecurityException">The caller does not have the required permission.</exception>
            // <exception cref="UnauthorizedAccessException">Access to filePath is denied.</exception>
            // <exception cref="IOException">Couldn't get state of file.</exception>

            // basic test - temp file is zero-byte
            string tempFilepath = Path.GetTempFileName();
            Assert.True(FileHelper.IsFileSizeZero(tempFilepath));

            // open for writing
            File.WriteAllText(tempFilepath, "this is a line of text");

            // now file is not zero-byte
            Assert.False(FileHelper.IsFileSizeZero(tempFilepath));

            // no problem with read-only
            File.SetAttributes(tempFilepath, FileAttributes.ReadOnly);
            Assert.False(FileHelper.IsFileSizeZero(tempFilepath));

            // file that doesn't exist
            FileHelper.DeleteFile(tempFilepath);
            Assert.False(File.Exists(tempFilepath));
            Assert.Throws<FileNotFoundException>(() => FileHelper.IsFileSizeZero(tempFilepath));
        }

        // ReSharper restore InconsistentNaming
        #endregion

        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// disposes a file stream after a wait period
        /// </summary>
        /// <param name="data">The file stream wrapper.</param>
        private static void DisposeFilestream(object data)
        {
            var disposeParams = data as DisposeFilestreamParams;
            if (disposeParams != null)
            {
                Thread.Sleep(disposeParams.WaitSeconds * 1000);
                disposeParams.FileStream.Dispose();
            }
        }

        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
        private static string RandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor((26 * random.NextDouble()) + 65)));
                builder.Append(ch);
            }

            if (lowerCase)
            {
                return builder.ToString().ToLower(CultureInfo.CurrentCulture);
            }

            return builder.ToString();
        }

        #endregion
    }
}
