// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaTypeExtensionsTest.cs" company="OBeautifulCode">
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

    public static class MediaTypeExtensionsTest
    {
        [Fact]
        public static void ToMimeTypeName___Should_return_mime_type_names_for_all_MediaType_values___When_called()
        {
            // Arrange
            var mediaTypes = EnumExtensions.GetAllPossibleEnumValues<MediaType>().ToList();

            // Act
            var actual = mediaTypes.Select(_ => _.ToMimeTypeName()).ToList();

            // Assert
            actual.AsTest().Must().Each().NotBeNullNorWhiteSpace();
        }
    }
}
