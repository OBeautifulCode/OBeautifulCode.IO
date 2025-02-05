// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaType.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO
{
    using System.CodeDom.Compiler;

    /// <summary>
    /// Specifies the type of media.
    /// </summary>
    [GeneratedCode("Ignore all CA warnings", "Ignore all CA warnings")]
    public enum MediaType
    {
        /// <summary>
        /// The application data is not interpreted (default).
        /// </summary>
        ApplicationOctet,

        /// <summary>
        /// The application data is Android Package Kit format.
        /// </summary>
        ApplicationAndroidPackageArchive,

        /// <summary>
        /// The application data is in EPUB format.
        /// </summary>
        ApplicationEpub,

        /// <summary>
        /// The application data is in Google Docs format.
        /// </summary>
        ApplicationGoogleDocs,

        /// <summary>
        /// The application data is in Google Drawing format.
        /// </summary>
        ApplicationGoogleDrawing,

        /// <summary>
        /// The application data is in Google Slides format.
        /// </summary>
        ApplicationGoogleSlides,

        /// <summary>
        /// The application data is in Google Sheets format.
        /// </summary>
        ApplicationGoogleSheets,

        /// <summary>
        /// The application data is compressed in GZip format.
        /// </summary>
        ApplicationGzip,

        /// <summary>
        /// The application data is in JavaScript Object Notation (JSON) format.
        /// </summary>
        ApplicationJson,

        /// <summary>
        /// The application data is in Microsoft Office PowerPoint (ppt) format.
        /// </summary>
        ApplicationMicrosoftOfficePowerPoint,

        /// <summary>
        /// The application data is in Microsoft Office OOXML PowerPoint (pptx) format.
        /// </summary>
        ApplicationMicrosoftOfficeOoxmlPowerPoint,

        /// <summary>
        /// The application data is in Microsoft Office Excel (xls) format.
        /// </summary>
        ApplicationMicrosoftOfficeExcel,

        /// <summary>
        /// The application data is in Microsoft Office Excel Binary (xlsb) format.
        /// </summary>
        ApplicationMicrosoftOfficeExcelBinary,

        /// <summary>
        /// The application data is in Microsoft Office Excel Macro Enabled (xlsm) format.
        /// </summary>
        ApplicationMicrosoftOfficeExcelMacroEnabled,

        /// <summary>
        /// The application data is in Microsoft Office OOXML Excel (xlsx) format.
        /// </summary>
        ApplicationMicrosoftOfficeOoxmlExcel,

        /// <summary>
        /// The application data is in Microsoft Office OOXML Excel Template (xltx) format.
        /// </summary>
        ApplicationMicrosoftOfficeOoxmlExcelTemplate,

        /// <summary>
        /// The application data is in Microsoft Office Word (doc) format.
        /// </summary>
        ApplicationMicrosoftOfficeWord,

        /// <summary>
        /// The application data is in Microsoft Office OOXML Word (docx) format.
        /// </summary>
        ApplicationMicrosoftOfficeOoxmlWord,

        /// <summary>
        /// The application data is in Microsoft Office OOXML Word Template (dotx) format.
        /// </summary>
        ApplicationMicrosoftOfficeOoxmlWordTemplate,

        /// <summary>
        /// The application data is in OASIS OpenDocument Graphics format.
        /// </summary>
        ApplicationOpenDocumentGraphics,

        /// <summary>
        /// The application data is in OASIS OpenDocument Image format.
        /// </summary>
        ApplicationOpenDocumentImage,

        /// <summary>
        /// The application data is in OASIS OpenDocument Presentation format.
        /// </summary>
        ApplicationOpenDocumentPresentation,

        /// <summary>
        /// The application data is in OASIS OpenDocument Spreadsheet format.
        /// </summary>
        ApplicationOpenDocumentSpreadsheet,

        /// <summary>
        /// The application data is in OASIS OpenDocument Text format.
        /// </summary>
        ApplicationOpenDocumentText,

        /// <summary>
        /// The application data is in Portable Document Format (PDF).
        /// </summary>
        ApplicationPdf,

        /// <summary>
        /// The application is in PERL format.
        /// </summary>
        ApplicationPerl,

        /// <summary>
        /// The application data is in Encapsulated PostScript (EPS) format.
        /// </summary>
        ApplicationPostscript,

        /// <summary>
        /// The application data is in Rich Text Format (RTF).
        /// </summary>
        ApplicationRtf,

        /// <summary>
        /// The application data is in Bourne shell script format.
        /// </summary>
        ApplicationShellScript,

        /// <summary>
        /// The application data is in Shockwave Flash (SWF) format.
        /// </summary>
        ApplicationShockwaveFlash,

        /// <summary>
        /// The application data is a SOAP document.
        /// </summary>
        ApplicationSoap,

        /// <summary>
        /// The application data is in SQL format.
        /// </summary>
        ApplicationSql,

        /// <summary>
        /// The application data is in TAR format.
        /// </summary>
        ApplicationTar,

        /// <summary>
        /// The application data is in TeX format.
        /// </summary>
        ApplicationTex,

        /// <summary>
        /// The application data is compressed in ZIP format.
        /// </summary>
        ApplicationZip,

        /// <summary>
        /// The audio data is in MPEG-4 audio-only format.
        /// </summary>
        AudioMpeg4,

        /// <summary>
        /// The audio data is in MP3 format.
        /// </summary>
        AudioMpeg,

        /// <summary>
        /// The audio data is in OGG format.
        /// </summary>
        AudioOgg,

        /// <summary>
        /// The audio data is in Waveform Audio Format (WAV).
        /// </summary>
        AudioWav,

        /// <summary>
        /// The image data is in bitmap image format (BMP).
        /// </summary>
        ImageBmp,

        /// <summary>
        /// The image data is in Graphics Interchange Format (GIF).
        /// </summary>
        ImageGif,

        /// <summary>
        /// The image data is in Joint Photographic Experts Group (JPEG) format.
        /// </summary>
        ImageJpeg,

        /// <summary>
        /// The image data is in Photoshop format.
        /// </summary>
        ImagePhotoshop,

        /// <summary>
        /// The image data in Portable Network Graphics (PNG) format.
        /// </summary>
        ImagePng,

        /// <summary>
        /// The image is in Scalable Vector Graphics (SVG) format.
        /// </summary>
        ImageSvg,

        /// <summary>
        /// The image data is in Tagged Image File Format (TIFF).
        /// </summary>
        ImageTiff,

        /// <summary>
        /// The message data is an email (.eml).
        /// </summary>
        MessageEmail,

        /// <summary>
        /// Bundles several independent pieces of content into one message.
        /// </summary>
        MultipartRelated,

        /// <summary>
        /// The text data is in C/C++ format (.c, .cc, .cpp, .cxx, .dic, .h, .hh)
        /// </summary>
        TextC,

        /// <summary>
        /// The text data is in C-Sharp C# format (CS).
        /// </summary>
        TextCSharp,

        /// <summary>
        /// The text data is in Cascading Style Sheets (CSS) format.
        /// </summary>
        TextCss,

        /// <summary>
        /// The text data is in comma-separated values (CSV) format.
        /// </summary>
        TextCsv,

        /// <summary>
        /// The text data is in Fortran format.
        /// </summary>
        TextFortran,

        /// <summary>
        /// The text data is in HTML format.
        /// </summary>
        TextHtml,

        /// <summary>
        /// The text data is in Java format.
        /// </summary>
        TextJava,

        /// <summary>
        /// The text data is in JavaScript format.
        /// </summary>
        TextJavaScript,

        /// <summary>
        /// The text data is in Markdown format.
        /// </summary>
        TextMarkdown,

        /// <summary>
        /// The text data is in plain text format.
        /// </summary>
        TextPlain,

        /// <summary>
        /// The text data is in Python format.
        /// </summary>
        TextPython,

        /// <summary>
        /// The text data is in Rich Text Format (RTF).
        /// </summary>
        TextRichText,

        /// <summary>
        /// The text is in tab-separated values (TSV) format.
        /// </summary>
        TextTsv,

        /// <summary>
        /// The text data is in vCard (also known as VCF) format.
        /// </summary>
        TextVcard,

        /// <summary>
        /// The text data is in XML format.
        /// </summary>
        TextXml,

        /// <summary>
        /// The text data is in YAML Ain't Markup Language (YAML) format.
        /// </summary>
        TextYaml,

        /// <summary>
        /// The video data is in Flash format.
        /// </summary>
        VideoFlash,

        /// <summary>
        /// The video data is in MPEG format.
        /// </summary>
        VideoMpeg,

        /// <summary>
        /// The video data is in MPEG-4 (mp4) format.
        /// </summary>
        VideoMpeg4,

        /// <summary>
        /// /The video is in Matroska (mkv) format.
        /// </summary>
        VideoMkv,

        /// <summary>
        /// The video data is in OGG format.
        /// </summary>
        VideoOgg,

        /// <summary>
        /// The video data is in QuickTime format (mov) format.
        /// </summary>
        VideoQuickTime,

        /// <summary>
        /// The video data is in WebM format.
        /// </summary>
        VideoWebm,

        /// <summary>
        /// The video data is in Windows Media Video (WMV) format.
        /// </summary>
        VideoWmv,
    }
}
