// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagNames.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO
{
    /// <summary>
    /// Tag name constants.
    /// </summary>
    public static class TagNames
    {
        /// <summary>
        /// The tag name for <see cref="IO.MediaType"/>.
        /// </summary>
        /// <remarks>
        /// Note that this is purposefully not content-type so that it doesn't conflict the content-type
        /// header in API calls.
        /// </remarks>
        public const string MediaType = "media-type";

        /// <summary>
        /// The tag name for a <see cref="IO.MalwareScanResult"/>.
        /// </summary>
        public const string MalwareScanResult = "malware-scan-result";
    }
}
