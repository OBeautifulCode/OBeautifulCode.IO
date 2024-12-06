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
                    { FileFormat.AppleScript, new[] { ".scpt", ".applescript" } },
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
                    { FileFormat.Fortran, new[] { ".f", ".for", ".f90" } },
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
                    { FileFormat.JavaScript, new[] { ".js", ".json" } },
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
                    { FileFormat.Mumps, new[] { ".mps" } },
                    { FileFormat.Numbers, new[] { ".numbers" } },
                    { FileFormat.Nzb, new[] { ".nzb" } },
                    { FileFormat.ObjC, new[] { ".m", ".mm" } },
                    { FileFormat.OCaml, new[] { ".ml", ".mli" } },
                    { FileFormat.Odg, new[] { ".odg" } },
                    { FileFormat.Odi, new[] { ".odi" } },
                    { FileFormat.Odp, new[] { ".odp" } },
                    { FileFormat.Ods, new[] { ".ods" } },
                    { FileFormat.Odt, new[] { ".odt" } },
                    { FileFormat.Ogg, new[] { ".ogg" } },
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

            var result = FileFormatToFileExtensionsMap[fileFormat];

            return result;
        }
    }
}
