// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaTypeExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Mime;
    using OBeautifulCode.CodeAnalysis.Recipes;

    /// <summary>
    /// Extension methods on <see cref="MediaTypeExtensions"/>.
    /// </summary>
    public static class MediaTypeExtensions
    {
        private static readonly IReadOnlyDictionary<MediaType, string> MediaTypeToMimeTypeNameMap =
            new Dictionary<MediaType, string>
            {
                { MediaType.ApplicationOctet, MediaTypeNames.Application.Octet },
                { MediaType.ApplicationAndroidPackageArchive, "application/vnd.android.package-archive" },
                { MediaType.ApplicationEpub, "application/epub+zip" },
                { MediaType.ApplicationGoogleDocs, "application/vnd.google-apps.document" },
                { MediaType.ApplicationGoogleDrawing, "application/vnd.google-apps.drawing" },
                { MediaType.ApplicationGoogleSlides, "application/vnd.google-apps.presentation" },
                { MediaType.ApplicationGoogleSheets, "application/vnd.google-apps.spreadsheet" },
                { MediaType.ApplicationGzip, "application/gzip" },
                { MediaType.ApplicationJson, "application/json" },
                { MediaType.ApplicationMicrosoftOfficeExcel, "application/vnd.ms-excel" },
                { MediaType.ApplicationMicrosoftOfficeExcelBinary, "application/vnd.ms-excel.sheet.binary.macroenabled.12" },
                { MediaType.ApplicationMicrosoftOfficeExcelMacroEnabled, "application/vnd.ms-excel.sheet.macroenabled.12" },
                { MediaType.ApplicationMicrosoftOfficeOoxmlExcel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { MediaType.ApplicationMicrosoftOfficeOoxmlExcelTemplate, "application/vnd.openxmlformats-officedocument.spreadsheetml.template" },
                { MediaType.ApplicationMicrosoftOfficePowerPoint, "application/vnd.ms-powerpoint" },
                { MediaType.ApplicationMicrosoftOfficeOoxmlPowerPoint, "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
                { MediaType.ApplicationMicrosoftOfficeWord, "application/msword" },
                { MediaType.ApplicationMicrosoftOfficeOoxmlWord, "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { MediaType.ApplicationMicrosoftOfficeOoxmlWordTemplate, "application/vnd.openxmlformats-officedocument.wordprocessingml.template" },
                { MediaType.ApplicationOpenDocumentGraphics, "application/vnd.oasis.opendocument.graphics" },
                { MediaType.ApplicationOpenDocumentImage, "application/vnd.oasis.opendocument.image" },
                { MediaType.ApplicationOpenDocumentPresentation, "application/vnd.oasis.opendocument.presentation" },
                { MediaType.ApplicationOpenDocumentSpreadsheet, "application/vnd.oasis.opendocument.spreadsheet" },
                { MediaType.ApplicationOpenDocumentText, "application/vnd.oasis.opendocument.text" },
                { MediaType.ApplicationPdf, MediaTypeNames.Application.Pdf },
                { MediaType.ApplicationPerl, "application/x-perl" },
                { MediaType.ApplicationPostscript, "application/postscript" },
                { MediaType.ApplicationRtf, MediaTypeNames.Application.Rtf },
                { MediaType.ApplicationShellScript, "application/x-shellscript" },
                { MediaType.ApplicationShockwaveFlash, "application/x-shockwave-flash" },
                { MediaType.ApplicationSoap, MediaTypeNames.Application.Soap },
                { MediaType.ApplicationSql, "application/sql" },
                { MediaType.ApplicationTar, "application/x-tar" },
                { MediaType.ApplicationTex, "application/x-tex" },
                { MediaType.ApplicationZip, MediaTypeNames.Application.Zip },
                { MediaType.AudioMpeg4, "audio/m4a" },
                { MediaType.AudioMpeg, "audio/mpeg" },
                { MediaType.AudioOgg, "audio/ogg" },
                { MediaType.AudioWav, "audio/wav" },
                { MediaType.ImageBmp, "image/bmp" },
                { MediaType.ImageGif, MediaTypeNames.Image.Gif },
                { MediaType.ImageJpeg, MediaTypeNames.Image.Jpeg },
                { MediaType.ImagePhotoshop, "image/vnd.adobe.photoshop" },
                { MediaType.ImagePng, "image/png" },
                { MediaType.ImageSvg, "image/svg+xml" },
                { MediaType.ImageTiff, MediaTypeNames.Image.Tiff },
                { MediaType.MessageEmail, "message/rfc822" },
                { MediaType.MultipartRelated, "multipart/related" },
                { MediaType.TextC, "text/x-c" },
                { MediaType.TextCSharp, "text/x-csharp" },
                { MediaType.TextCss, "text/css" },
                { MediaType.TextCsv, "text/csv" },
                { MediaType.TextFortran, "text/x-fortran" },
                { MediaType.TextHtml, MediaTypeNames.Text.Html },
                { MediaType.TextJava, "text/x-java-source" },
                { MediaType.TextJavaScript, "text/javascript" },
                { MediaType.TextMarkdown, "text/markdown" },
                { MediaType.TextPlain, MediaTypeNames.Text.Plain },
                { MediaType.TextPython, "text/x-python" },
                { MediaType.TextRichText, MediaTypeNames.Text.RichText },
                { MediaType.TextTsv, "text/tab-separated-values" },
                { MediaType.TextVcard, "text/vcard" },
                { MediaType.TextXml, MediaTypeNames.Text.Xml },
                { MediaType.TextYaml, "text/yaml" },
                { MediaType.VideoFlash, "video/x-flv" },
                { MediaType.VideoMkv, "video/x-matroska" },
                { MediaType.VideoMpeg, "video/mpeg" },
                { MediaType.VideoMpeg4, "video/mp4" },
                { MediaType.VideoOgg, "video/ogg" },
                { MediaType.VideoQuickTime, "video/quicktime" },
                { MediaType.VideoWebm, "video/webm" },
                { MediaType.VideoWmv, "video/x-ms-wmv" },
            };

        /// <summary>
        /// Gets the MIME type name for the specified media type.
        /// </summary>
        /// <param name="mediaType">The media type.</param>
        /// <returns>
        /// The MIME type name for the specified media type.
        /// </returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        public static string ToMimeTypeName(
            this MediaType mediaType)
        {
            // Note that a unit test guarantees that all MediaType values are in the dictionary.
            var result = MediaTypeToMimeTypeNameMap[mediaType];

            return result;
        }
    }
}
