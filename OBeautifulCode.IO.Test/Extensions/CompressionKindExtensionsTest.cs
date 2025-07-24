// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompressionKindExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO.Test
{
    using System;
    using System.Linq;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Compression;
    using Xunit;

    public static class CompressionKindExtensionsTest
    {
        [Fact]
        public static void ToMediaType___Should_throw_ArgumentOutOfRangeException___When_parameter_compressionKind_is_Invalid()
        {
            // Arrange, Act
            var actual = Record.Exception(() => CompressionKind.Invalid.ToMediaType());

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentOutOfRangeException>();
            actual.Message.AsTest().Must().ContainString("compressionKind");
            actual.Message.AsTest().Must().ContainString("Invalid");
        }

        [Fact]
        public static void ToMediaType___Should_return_null___When_parameter_compressionKind_is_None()
        {
            // Arrange, Act
            var actual = CompressionKind.None.ToMediaType();

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ToMediaType___Should_return_MediaType_for_all_CompressionKind_values___When_called()
        {
            // Arrange
            var compressionKindAndExpected = new[]
            {
                new { CompressionKind = CompressionKind.DotNetZip, Expected = (MediaType?)MediaType.ApplicationGzip },
            };

            var expected = compressionKindAndExpected.Select(_ => _.Expected).ToList();

            // Act
            var actual = compressionKindAndExpected.Select(_ => _.CompressionKind.ToMediaType()).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }
    }
}
