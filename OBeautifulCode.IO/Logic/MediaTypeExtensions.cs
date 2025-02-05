// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaTypeExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Mime;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Extension methods on <see cref="MediaTypeExtensions"/>.
    /// </summary>
    public static class MediaTypeExtensions
    {
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
            switch (mediaType)
            {
                case MediaType.ApplicationOctet:
                    return MediaTypeNames.Application.Octet;
                case MediaType.ApplicationAndroidPackageArchive:
                    return "application/vnd.android.package-archive";
                case MediaType.ApplicationEpub:
                    return "application/epub+zip";
                case MediaType.ApplicationGoogleDocs:
                    return "application/vnd.google-apps.document";
                case MediaType.ApplicationGoogleDrawing:
                    return "application/vnd.google-apps.drawing";
                case MediaType.ApplicationGoogleSlides:
                    return "application/vnd.google-apps.presentation";
                case MediaType.ApplicationGoogleSheets:
                    return "application/vnd.google-apps.spreadsheet";
                case MediaType.ApplicationGzip:
                    return "application/gzip";
                case MediaType.ApplicationJson:
                    return "application/json";
                case MediaType.ApplicationMicrosoftOfficeExcel:
                    return "application/vnd.ms-excel";
                case MediaType.ApplicationMicrosoftOfficeExcelBinary:
                    return "application/vnd.ms-excel.sheet.binary.macroenabled.12";
                case MediaType.ApplicationMicrosoftOfficeExcelMacroEnabled:
                    return "application/vnd.ms-excel.sheet.macroenabled.12";
                case MediaType.ApplicationMicrosoftOfficeOoxmlExcel:
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case MediaType.ApplicationMicrosoftOfficeOoxmlExcelTemplate:
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                case MediaType.ApplicationMicrosoftOfficePowerPoint:
                    return "application/vnd.ms-powerpoint";
                case MediaType.ApplicationMicrosoftOfficeOoxmlPowerPoint:
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case MediaType.ApplicationMicrosoftOfficeWord:
                    return "application/msword";
                case MediaType.ApplicationMicrosoftOfficeOoxmlWord:
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case MediaType.ApplicationMicrosoftOfficeOoxmlWordTemplate:
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                case MediaType.ApplicationOpenDocumentGraphics:
                    return "application/vnd.oasis.opendocument.graphics";
                case MediaType.ApplicationOpenDocumentImage:
                    return "application/vnd.oasis.opendocument.image";
                case MediaType.ApplicationOpenDocumentPresentation:
                    return "application/vnd.oasis.opendocument.presentation";
                case MediaType.ApplicationOpenDocumentSpreadsheet:
                    return "application/vnd.oasis.opendocument.spreadsheet";
                case MediaType.ApplicationOpenDocumentText:
                    return "application/vnd.oasis.opendocument.text";
                case MediaType.ApplicationPdf:
                    return MediaTypeNames.Application.Pdf;
                case MediaType.ApplicationPerl:
                    return "application/x-perl";
                case MediaType.ApplicationPostscript:
                    return "application/postscript";
                case MediaType.ApplicationRtf:
                    return MediaTypeNames.Application.Rtf;
                case MediaType.ApplicationShellScript:
                    return "application/x-shellscript";
                case MediaType.ApplicationShockwaveFlash:
                    return "application/x-shockwave-flash";
                case MediaType.ApplicationSoap:
                    return MediaTypeNames.Application.Soap;
                case MediaType.ApplicationSql:
                    return "application/sql";
                case MediaType.ApplicationTar:
                    return "application/x-tar";
                case MediaType.ApplicationTex:
                    return "application/x-tex";
                case MediaType.ApplicationZip:
                    return MediaTypeNames.Application.Zip;
                case MediaType.AudioMpeg4:
                    return "audio/m4a";
                case MediaType.AudioMpeg:
                    return "audio/mpeg";
                case MediaType.AudioOgg:
                    return "audio/ogg";
                case MediaType.AudioWav:
                    return "audio/wav";
                case MediaType.ImageBmp:
                    return "image/bmp";
                case MediaType.ImageGif:
                    return MediaTypeNames.Image.Gif;
                case MediaType.ImageJpeg:
                    return MediaTypeNames.Image.Jpeg;
                case MediaType.ImagePhotoshop:
                    return "image/vnd.adobe.photoshop";
                case MediaType.ImagePng:
                    return "image/png";
                case MediaType.ImageSvg:
                    return "image/svg+xml";
                case MediaType.ImageTiff:
                    return MediaTypeNames.Image.Tiff;
                case MediaType.MessageEmail:
                    return "message/rfc822";
                case MediaType.MultipartRelated:
                    return "multipart/related";
                case MediaType.TextC:
                    return "text/x-c";
                case MediaType.TextCSharp:
                    return "text/x-csharp";
                case MediaType.TextCss:
                    return "text/css";
                case MediaType.TextCsv:
                    return "text/csv";
                case MediaType.TextFortran:
                    return "text/x-fortran";
                case MediaType.TextHtml:
                    return MediaTypeNames.Text.Html;
                case MediaType.TextJava:
                    return "text/x-java-source";
                case MediaType.TextJavaScript:
                    return "text/javascript";
                case MediaType.TextMarkdown:
                    return "text/markdown";
                case MediaType.TextPlain:
                    return MediaTypeNames.Text.Plain;
                case MediaType.TextPython:
                    return "text/x-python";
                case MediaType.TextRichText:
                    return MediaTypeNames.Text.RichText;
                case MediaType.TextTsv:
                    return "text/tab-separated-values";
                case MediaType.TextVcard:
                    return "text/vcard";
                case MediaType.TextXml:
                    return MediaTypeNames.Text.Xml;
                case MediaType.TextYaml:
                    return "text/yaml";
                case MediaType.VideoFlash:
                    return "video/x-flv";
                case MediaType.VideoMkv:
                    return "video/x-matroska";
                case MediaType.VideoMpeg:
                    return "video/mpeg";
                case MediaType.VideoMpeg4:
                    return "video/mp4";
                case MediaType.VideoOgg:
                    return "video/ogg";
                case MediaType.VideoQuickTime:
                    return "video/quicktime";
                case MediaType.VideoWebm:
                    return "video/webm";
                case MediaType.VideoWmv:
                    return "video/x-ms-wmv";
                default:
                    throw new NotSupportedException(Invariant($"This {nameof(MediaType)} is not supported: {mediaType}."));
            }
        }
    }
}
