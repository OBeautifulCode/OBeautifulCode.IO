// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileFormatExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.IO
{
    using System;
    using System.Collections.Generic;
    using static System.FormattableString;

    /// <summary>
    /// Extension methods on <see cref="FileFormat"/>.
    /// </summary>
    public static class FileFormatExtensions
    {
        private static readonly IReadOnlyDictionary<FileFormat, IReadOnlyCollection<string>>
            FileFormatToFileExtensionsMap =
                new Dictionary<FileFormat, IReadOnlyCollection<string>>
                {
                    { FileFormat.Text, new[] { ".txt" } },
                    { FileFormat.Ai, new[] { ".ai" } },
                    { FileFormat.Apk, new[] { ".apk" } },
                    { FileFormat.AppleScript, new[] { ".applescript" } },
                    { FileFormat.Binary, new[] { ".bin" } },
                    { FileFormat.Bmp, new[] { ".bmp" } },
                    { FileFormat.BoxNote, new[] { ".boxnote" } },
                    { FileFormat.C, new[] { ".c" } },
                    { FileFormat.CSharp, new[] { ".cs" } },
                    { FileFormat.Cpp, new[] { ".cpp", ".cc", ".cxx" } },
                    { FileFormat.Css, new[] { ".css" } },
                    { FileFormat.Csv, new[] { ".csv" } },
                    { FileFormat.Clojure, new[] { ".clj" } },
                    { FileFormat.CoffeeScript, new[] { ".coffee" } },
                    { FileFormat.Cfm, new[] { ".cfm" } },
                    { FileFormat.D, new[] { ".d" } },
                    { FileFormat.Dart, new[] { ".dart" } },
                    { FileFormat.Diff, new[] { ".diff", ".patch" } },
                    { FileFormat.Doc, new[] { ".doc" } },
                    { FileFormat.Docx, new[] { ".docx" } },
                    { FileFormat.DockerFile, new[] { string.Empty } },
                    { FileFormat.Dotx, new[] { ".dotx" } },
                    { FileFormat.Email, new[] { ".eml", ".msg" } },
                    { FileFormat.Eps, new[] { ".eps" } },
                    { FileFormat.Epub, new[] { ".epub" } },
                    { FileFormat.Erlang, new[] { ".erl" } },
                    { FileFormat.Fla, new[] { ".fla" } },
                    { FileFormat.Flv, new[] { ".flv" } },
                    { FileFormat.FSharp, new[] { ".fs", ".fsi", ".fsx" } },
                    { FileFormat.Fortran, new[] { ".f", ".for", ".f90", ".f77" } },
                    { FileFormat.GDoc, new[] { ".gdoc" } },
                    { FileFormat.GDraw, new[] { ".gdraw" } },
                    { FileFormat.Gif, new[] { ".gif" } },
                    { FileFormat.Go, new[] { ".go" } },
                    { FileFormat.GPres, new[] { ".gslides" } },
                    { FileFormat.Groovy, new[] { ".groovy" } },
                    { FileFormat.GSheet, new[] { ".gsheet" } },
                    { FileFormat.GZip, new[] { ".gz" } },
                    { FileFormat.Html, new[] { ".html", ".htm" } },
                    { FileFormat.Handlebars, new[] { ".hbs", ".handlebars" } },
                    { FileFormat.Haskell, new[] { ".hs" } },
                    { FileFormat.Haxe, new[] { ".hx" } },
                    { FileFormat.Indd, new[] { ".indd" } },
                    { FileFormat.Java, new[] { ".java" } },
                    { FileFormat.JavaScript, new[] { ".js" } },
                    { FileFormat.Json, new[] { ".json" } },
                    { FileFormat.Jpg, new[] { ".jpg", ".jpeg" } },
                    { FileFormat.Keynote, new[] { ".key" } },
                    { FileFormat.Kotlin, new[] { ".kt", ".kts" } },
                    { FileFormat.Latex, new[] { ".tex" } },
                    { FileFormat.Lisp, new[] { ".lisp", ".lsp" } },
                    { FileFormat.Lua, new[] { ".lua" } },
                    { FileFormat.M4a, new[] { ".m4a" } },
                    { FileFormat.Markdown, new[] { ".md", ".markdown" } },
                    { FileFormat.Matlab, new[] { ".m" } },
                    { FileFormat.MHtml, new[] { ".mhtml", ".mht" } },
                    { FileFormat.Mkv, new[] { ".mkv" } },
                    { FileFormat.Mov, new[] { ".mov" } },
                    { FileFormat.Mp3, new[] { ".mp3" } },
                    { FileFormat.Mp4, new[] { ".mp4" } },
                    { FileFormat.Mpg, new[] { ".mpg", ".mpeg" } },
                    { FileFormat.Numbers, new[] { ".numbers" } },
                    { FileFormat.Nzb, new[] { ".nzb" } },
                    { FileFormat.ObjC, new[] { ".m", ".mm" } },
                    { FileFormat.OCaml, new[] { ".ml", ".mli" } },
                    { FileFormat.Odg, new[] { ".odg" } },
                    { FileFormat.Odi, new[] { ".odi" } },
                    { FileFormat.Odp, new[] { ".odp" } },
                    { FileFormat.Ods, new[] { ".ods" } },
                    { FileFormat.Odt, new[] { ".odt" } },
                    { FileFormat.Ogg, new[] { ".ogg", ".oga" } },
                    { FileFormat.Ogv, new[] { ".ogv" } },
                    { FileFormat.Pages, new[] { ".pages" } },
                    { FileFormat.Pascal, new[] { ".pas" } },
                    { FileFormat.Pdf, new[] { ".pdf" } },
                    { FileFormat.Perl, new[] { ".pl", ".pm" } },
                    { FileFormat.Php, new[] { ".php" } },
                    { FileFormat.Pig, new[] { ".pig" } },
                    { FileFormat.Png, new[] { ".png" } },
                    { FileFormat.Post, new[] { ".post" } },
                    { FileFormat.PowerShell, new[] { ".ps1" } },
                    { FileFormat.Ppt, new[] { ".ppt" } },
                    { FileFormat.Pptx, new[] { ".pptx" } },
                    { FileFormat.Psd, new[] { ".psd" } },
                    { FileFormat.Puppet, new[] { ".pp" } },
                    { FileFormat.Python, new[] { ".py" } },
                    { FileFormat.Qtz, new[] { ".qtz" } },
                    { FileFormat.R, new[] { ".r" } },
                    { FileFormat.Rtf, new[] { ".rtf" } },
                    { FileFormat.Ruby, new[] { ".rb" } },
                    { FileFormat.Rust, new[] { ".rs" } },
                    { FileFormat.Sql, new[] { ".sql" } },
                    { FileFormat.Sass, new[] { ".scss", ".sass" } },
                    { FileFormat.Scala, new[] { ".scala" } },
                    { FileFormat.Scheme, new[] { ".scm" } },
                    { FileFormat.Sketch, new[] { ".sketch" } },
                    { FileFormat.Shell, new[] { ".sh" } },
                    { FileFormat.Smalltalk, new[] { ".st" } },
                    { FileFormat.Svg, new[] { ".svg" } },
                    { FileFormat.Swf, new[] { ".swf" } },
                    { FileFormat.Swift, new[] { ".swift" } },
                    { FileFormat.Tar, new[] { ".tar" } },
                    { FileFormat.Tiff, new[] { ".tiff", ".tif" } },
                    { FileFormat.Tsv, new[] { ".tsv" } },
                    { FileFormat.Vb, new[] { ".vb" } },
                    { FileFormat.VbScript, new[] { ".vbs" } },
                    { FileFormat.VCard, new[] { ".vcf" } },
                    { FileFormat.Velocity, new[] { ".vm" } },
                    { FileFormat.Verilog, new[] { ".v" } },
                    { FileFormat.Wav, new[] { ".wav" } },
                    { FileFormat.WebM, new[] { ".webm" } },
                    { FileFormat.Wmv, new[] { ".wmv" } },
                    { FileFormat.Xls, new[] { ".xls" } },
                    { FileFormat.Xlsx, new[] { ".xlsx" } },
                    { FileFormat.Xlsb, new[] { ".xlsb" } },
                    { FileFormat.Xlsm, new[] { ".xlsm" } },
                    { FileFormat.Xltx, new[] { ".xltx" } },
                    { FileFormat.Xml, new[] { ".xml" } },
                    { FileFormat.Yaml, new[] { ".yaml", ".yml" } },
                    { FileFormat.Zip, new[] { ".zip" } },
                };

        private static readonly IReadOnlyDictionary<FileFormat, MediaType>
            FileFormatToMediaTypeMap = new Dictionary<FileFormat, MediaType>
            {
                { FileFormat.Unspecified, MediaType.ApplicationOctet },
                { FileFormat.Text, MediaType.TextPlain },
                { FileFormat.Ai, MediaType.ApplicationOctet },
                { FileFormat.Apk, MediaType.ApplicationAndroidPackageArchive },
                { FileFormat.AppleScript, MediaType.TextPlain },
                { FileFormat.Binary, MediaType.ApplicationOctet },
                { FileFormat.Bmp, MediaType.ImageBmp },
                { FileFormat.BoxNote, MediaType.ApplicationOctet },
                { FileFormat.C, MediaType.TextC },
                { FileFormat.CSharp, MediaType.TextCSharp },
                { FileFormat.Cpp, MediaType.TextC },
                { FileFormat.Css, MediaType.TextCss },
                { FileFormat.Csv, MediaType.TextCsv },
                { FileFormat.Clojure, MediaType.TextPlain },
                { FileFormat.CoffeeScript, MediaType.TextPlain },
                { FileFormat.Cfm, MediaType.TextPlain },
                { FileFormat.D, MediaType.TextPlain },
                { FileFormat.Dart, MediaType.TextPlain },
                { FileFormat.Diff, MediaType.TextPlain },
                { FileFormat.Doc, MediaType.ApplicationMicrosoftOfficeWord },
                { FileFormat.Docx, MediaType.ApplicationMicrosoftOfficeOoxmlWord },
                { FileFormat.DockerFile, MediaType.TextPlain },
                { FileFormat.Dotx, MediaType.ApplicationMicrosoftOfficeOoxmlWordTemplate },
                { FileFormat.Email, MediaType.MessageEmail },
                { FileFormat.Eps, MediaType.ApplicationPostscript },
                { FileFormat.Epub, MediaType.ApplicationEpub },
                { FileFormat.Erlang, MediaType.TextPlain },
                { FileFormat.Fla, MediaType.ApplicationOctet },
                { FileFormat.Flv, MediaType.VideoFlash },
                { FileFormat.FSharp, MediaType.TextPlain },
                { FileFormat.Fortran, MediaType.TextFortran },
                { FileFormat.GDoc, MediaType.ApplicationGoogleDocs },
                { FileFormat.GDraw, MediaType.ApplicationGoogleDrawing },
                { FileFormat.Gif, MediaType.ImageGif },
                { FileFormat.Go, MediaType.TextPlain },
                { FileFormat.GPres, MediaType.ApplicationGoogleSlides },
                { FileFormat.Groovy, MediaType.TextPlain },
                { FileFormat.GSheet, MediaType.ApplicationGoogleSheets },
                { FileFormat.GZip, MediaType.ApplicationGzip },
                { FileFormat.Html, MediaType.TextHtml },
                { FileFormat.Handlebars, MediaType.TextPlain },
                { FileFormat.Haskell, MediaType.TextPlain },
                { FileFormat.Haxe, MediaType.TextPlain },
                { FileFormat.Indd, MediaType.ApplicationOctet },
                { FileFormat.Java, MediaType.TextJava },
                { FileFormat.JavaScript, MediaType.ApplicationJson },
                { FileFormat.Json, MediaType.TextJavaScript },
                { FileFormat.Jpg, MediaType.ImageJpeg },
                { FileFormat.Keynote, MediaType.ApplicationOctet },
                { FileFormat.Kotlin, MediaType.TextPlain },
                { FileFormat.Latex, MediaType.TextPlain },
                { FileFormat.Lisp, MediaType.TextPlain },
                { FileFormat.Lua, MediaType.TextPlain },
                { FileFormat.M4a, MediaType.AudioMpeg4 },
                { FileFormat.Markdown, MediaType.TextMarkdown },
                { FileFormat.Matlab, MediaType.TextPlain },
                { FileFormat.MHtml, MediaType.MultipartRelated },
                { FileFormat.Mkv, MediaType.VideoMkv },
                { FileFormat.Mov, MediaType.VideoQuickTime },
                { FileFormat.Mp3, MediaType.AudioMpeg },
                { FileFormat.Mp4, MediaType.VideoMpeg4 },
                { FileFormat.Mpg, MediaType.VideoMpeg },
                { FileFormat.Numbers, MediaType.ApplicationOctet },
                { FileFormat.Nzb, MediaType.TextXml },
                { FileFormat.ObjC, MediaType.TextPlain },
                { FileFormat.OCaml, MediaType.TextPlain },
                { FileFormat.Odg, MediaType.ApplicationOpenDocumentGraphics },
                { FileFormat.Odi, MediaType.ApplicationOpenDocumentImage },
                { FileFormat.Odp, MediaType.ApplicationOpenDocumentPresentation },
                { FileFormat.Ods, MediaType.ApplicationOpenDocumentSpreadsheet },
                { FileFormat.Odt, MediaType.ApplicationOpenDocumentText },
                { FileFormat.Ogg, MediaType.AudioOgg },
                { FileFormat.Ogv, MediaType.VideoOgg },
                { FileFormat.Pages, MediaType.ApplicationOctet },
                { FileFormat.Pascal, MediaType.TextPlain },
                { FileFormat.Pdf, MediaType.ApplicationPdf },
                { FileFormat.Perl, MediaType.ApplicationPerl },
                { FileFormat.Php, MediaType.TextPlain },
                { FileFormat.Pig, MediaType.TextPlain },
                { FileFormat.Png, MediaType.ImagePng },
                { FileFormat.Post, MediaType.ApplicationOctet },
                { FileFormat.PowerShell, MediaType.TextPlain },
                { FileFormat.Ppt, MediaType.ApplicationMicrosoftOfficePowerPoint },
                { FileFormat.Pptx, MediaType.ApplicationMicrosoftOfficeOoxmlPowerPoint },
                { FileFormat.Psd, MediaType.ImagePhotoshop },
                { FileFormat.Puppet, MediaType.TextPlain },
                { FileFormat.Python, MediaType.TextPython },
                { FileFormat.Qtz, MediaType.ApplicationOctet },
                { FileFormat.R, MediaType.TextPlain },
                { FileFormat.Rtf, MediaType.ApplicationRtf },
                { FileFormat.Ruby, MediaType.TextPlain },
                { FileFormat.Rust, MediaType.TextPlain },
                { FileFormat.Sql, MediaType.ApplicationSql },
                { FileFormat.Sass, MediaType.TextCss },
                { FileFormat.Scala, MediaType.TextPlain },
                { FileFormat.Scheme, MediaType.TextPlain },
                { FileFormat.Sketch, MediaType.ApplicationZip },
                { FileFormat.Shell, MediaType.ApplicationShellScript },
                { FileFormat.Smalltalk, MediaType.TextPlain },
                { FileFormat.Svg, MediaType.ImageSvg },
                { FileFormat.Swf, MediaType.ApplicationShockwaveFlash },
                { FileFormat.Swift, MediaType.TextPlain },
                { FileFormat.Tar, MediaType.ApplicationTar },
                { FileFormat.Tiff, MediaType.ImageTiff },
                { FileFormat.Tsv, MediaType.TextTsv },
                { FileFormat.Vb, MediaType.TextPlain },
                { FileFormat.VbScript, MediaType.TextPlain },
                { FileFormat.VCard, MediaType.TextVcard },
                { FileFormat.Velocity, MediaType.TextPlain },
                { FileFormat.Verilog, MediaType.TextPlain },
                { FileFormat.Wav, MediaType.AudioWav },
                { FileFormat.WebM, MediaType.VideoWebm },
                { FileFormat.Wmv, MediaType.VideoWmv },
                { FileFormat.Xls, MediaType.ApplicationMicrosoftOfficeExcel },
                { FileFormat.Xlsx, MediaType.ApplicationMicrosoftOfficeOoxmlExcel },
                { FileFormat.Xlsb, MediaType.ApplicationMicrosoftOfficeExcelBinary },
                { FileFormat.Xlsm, MediaType.ApplicationMicrosoftOfficeExcelMacroEnabled },
                { FileFormat.Xltx, MediaType.ApplicationMicrosoftOfficeOoxmlExcelTemplate },
                { FileFormat.Xml, MediaType.TextXml },
                { FileFormat.Yaml, MediaType.TextYaml },
                { FileFormat.Zip, MediaType.ApplicationZip },
            };

        /// <summary>
        /// Gets the typically used file extensions for a specified file format.
        /// </summary>
        /// <param name="fileFormat">The file format.</param>
        /// <returns>
        /// The typically used file extensions for the specified file format, with leading period.
        /// </returns>
        public static IReadOnlyCollection<string> GetTypicalFileExtensions(
            this FileFormat fileFormat)
        {
            if (fileFormat == FileFormat.Unspecified)
            {
                throw new ArgumentOutOfRangeException(Invariant($"{nameof(fileFormat)} is {nameof(FileFormat.Unspecified)}."));
            }

            // Note that a unit test guarantees that all FileFormat values are in the dictionary.
            var result = FileFormatToFileExtensionsMap[fileFormat];

            return result;
        }

        /// <summary>
        /// Converts a <see cref="FileFormat"/> to a <see cref="MediaType"/>.
        /// </summary>
        /// <param name="fileFormat">The file format.</param>
        /// <returns>
        /// The <see cref="MediaType"/> converted from a <see cref="FileFormat"/>.
        /// </returns>
        public static MediaType ToMediaType(
            this FileFormat fileFormat)
        {
            // Note that a unit test guarantees that all FileFormat values are in the dictionary.
            var result = fileFormat == FileFormat.Unspecified
                ? MediaType.ApplicationOctet
                : FileFormatToMediaTypeMap[fileFormat];

            return result;
        }
    }
}
