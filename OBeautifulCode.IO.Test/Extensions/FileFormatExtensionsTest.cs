// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileFormatExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Enum.Recipes;
    using Xunit;

    public static class FileFormatExtensionsTest
    {
        [Fact]
        public static void GetTypicalFileExtensions___Should_throw_ArgumentOutOfRangeException___When_parameter_fileFormat_is_Unspecified()
        {
            // Arrange, Act
            var actual = Record.Exception(() => FileFormat.Unspecified.GetTypicalFileExtensions());

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentOutOfRangeException>();
            actual.Message.AsTest().Must().ContainString("fileFormat");
            actual.Message.AsTest().Must().ContainString("Unspecified");
        }

        [Fact]
        public static void GetTypicalFileExtensions___Should_return_extensions_for_all_FileFormat_values___When_called()
        {
            // Arrange
            var fileFormats = EnumExtensions.GetAllPossibleEnumValues<FileFormat>()
                .Where(_ => _ != FileFormat.Unspecified)
                .Where(_ => _ != FileFormat.DockerFile) // Docker files don't have an extension
                .ToList();

            // Act
            var actual = fileFormats.Select(_ => _.GetTypicalFileExtensions()).ToList();

            // Assert
            foreach (var fileFormatExtensions in actual)
            {
                fileFormatExtensions.AsTest().Must().Each().StartWith(".");
            }
        }

        [Fact]
        public static void ToMediaType___Should_return_MediaType_for_all_FileFormat_values___When_called()
        {
            // Arrange
            var filesFormats = EnumExtensions.GetAllPossibleEnumValues<FileFormat>().ToList();

            // Act
            var mediaTypes = filesFormats.Select(_ => _.ToMediaType()).ToList();

            // Assert
            // need some kind of assertion so that compiler doesn't complain about unused calls.
            mediaTypes.AsArg().Must().HaveCount(filesFormats.Count);
        }

        [Fact]
        public static void GetPossibleFileFormat___Should_throw_ArgumentNullException___When_parameter_fileNameWithExtension_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => FileFormatExtensions.GetPossibleFileFormat(null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("fileNameWithExtension");
        }

        [Fact]
        public static void GetPossibleFileFormat___Should_throw_ArgumentException___When_parameter_fileNameWithExtension_is_white_space()
        {
            // Arrange, Act
            var actual = Record.Exception(() => " \r ".GetPossibleFileFormat());

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentException>();
            actual.Message.AsTest().Must().ContainString("fileNameWithExtension");
            actual.Message.AsTest().Must().ContainString("white space");
        }

        [Fact]
        public static void GetPossibleFileFormat___Should_return_FileFormat_Unspecified___When_fileNameWithExtension_has_no_extension()
        {
            // Arrange
            var fileNameWithExtension = "some file name";

            // Act
            var actual = fileNameWithExtension.GetPossibleFileFormat();

            // Assert
            actual.AsTest().Must().BeEqualTo(FileFormat.Unspecified);
        }

        [Fact]
        public static void GetPossibleFileFormat___Should_return_FileFormat_Unspecified___When_extension_is_not_recognized()
        {
            // Arrange
            var fileNameWithExtension = "some-file-name.xyzabcd123";

            // Act
            var actual = fileNameWithExtension.GetPossibleFileFormat();

            // Assert
            actual.AsTest().Must().BeEqualTo(FileFormat.Unspecified);
        }

        [Fact]
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "For testing purposes.")]
        public static void GetPossibleFileFormat___Should_return_FileFormat___When_called()
        {
            // Arrange
            var fileFormats = EnumExtensions.GetAllPossibleEnumValues<FileFormat>()
                .Where(_ => _ != FileFormat.Unspecified)
                .Where(_ => _ != FileFormat.DockerFile) // Docker files don't have an extension
                .Where(_ => _ != FileFormat.Matlab) // MatLab shares an extension with Objective-C
                .ToList();

            // Act, Assert
            foreach (var expected in fileFormats)
            {
                var extensions = expected.GetTypicalFileExtensions();

                foreach (var extension in extensions)
                {
                    var fileName1 = ("some-file-name" + extension).ToLowerInvariant();
                    var fileName2 = fileName1.ToUpperInvariant();

                    var actual1 = fileName1.GetPossibleFileFormat();
                    var actual2 = fileName2.GetPossibleFileFormat();

                    actual1.AsTest().Must().BeEqualTo(expected);
                    actual2.AsTest().Must().BeEqualTo(expected);
                }
            }
        }
    }
}
