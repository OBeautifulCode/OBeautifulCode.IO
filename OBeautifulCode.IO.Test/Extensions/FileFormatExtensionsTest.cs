// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileFormatExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO.Test
{
    using System;
    using System.Linq;
    using OBeautifulCode.Assertion.Recipes;
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
    }
}
