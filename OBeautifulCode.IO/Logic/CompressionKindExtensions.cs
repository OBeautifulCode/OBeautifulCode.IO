// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompressionKindExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Compression;
    using static System.FormattableString;

    /// <summary>
    /// Extension methods on <see cref="CompressionKindExtensions"/>.
    /// </summary>
    public static class CompressionKindExtensions
    {
        /// <summary>
        /// Gets the media type for a specified compression kind.
        /// </summary>
        /// <param name="compressionKind">The compression kind.</param>
        /// <returns>
        /// Returns null if <paramref name="compressionKind"/> is <see cref="CompressionKind.None"/>,
        /// otherwise returns the corresponding media type.
        /// </returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        public static MediaType? ToMediaType(
            this CompressionKind compressionKind)
        {
            MediaType? result;

            if (compressionKind == CompressionKind.Invalid)
            {
                throw new ArgumentOutOfRangeException(Invariant($"{nameof(compressionKind)} is {CompressionKind.Invalid}."));
            }
            else if (compressionKind == CompressionKind.None)
            {
                result = null;
            }
            else if (compressionKind == CompressionKind.DotNetZip)
            {
                result = MediaType.ApplicationGzip;
            }
            else
            {
                throw new NotSupportedException(Invariant($"This {nameof(CompressionKind)} is not supported: {compressionKind}."));
            }

            return result;
        }
    }
}
