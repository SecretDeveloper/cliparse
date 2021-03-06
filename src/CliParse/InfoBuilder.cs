using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CliParse
{
    /// <summary>
    /// Information content builder for help screens.
    /// </summary>
    public static class InfoBuilder
    {
        /// <summary>
        /// Returns help content derived from the provided assembly and parsable object.
        /// </summary>
        /// <param name="parsable"></param>
        /// <param name="asm"></param>
        /// <param name="template"></param>
        /// <param name="argumentTemplate"></param>
        /// <param name="maxLineLength">The maximum number of characters in a line before it is wrapped</param>
        /// <returns></returns>
        public static string GetHelpInfoFromAssembly(Parsable parsable, Assembly asm, string template, string argumentTemplate, int maxLineLength = 80)
        {
            if (parsable == null) throw new ArgumentNullException("parsable");
            if(asm == null) throw new ArgumentNullException("asm");
            if (string.IsNullOrEmpty(template)) return "";

            var parsableClass = Helper.GetObjectAttribute(parsable, typeof(ParsableClassAttribute)) as ParsableClassAttribute;
            if (parsableClass == null)
                throw new CliParseException("Unable to find 'ParsableClass' attribute on provided object.");

            var title = GetAssemblyAttribute(asm, typeof (AssemblyTitleAttribute));
            template = template.Replace("{title}", title);
            
            var version = asm.GetName().Version.ToString();
            template = template.Replace("{version}", version);

            var company = GetAssemblyAttribute(asm, typeof(AssemblyCompanyAttribute));
            template = template.Replace("{company}", company);

            var description = GetAssemblyAttribute(asm, typeof (AssemblyDescriptionAttribute));
            template = template.Replace("{description}", description);

            var syntax = GetSyntaxInfo(parsable, argumentTemplate, parsableClass.AllowedPrefixes);
            template = template.Replace("{syntax}", syntax);
            
            var copyright = GetAssemblyAttribute(asm, typeof (AssemblyCopyrightAttribute));
            template = template.Replace("{copyright}", copyright);
            
            var footer = GetAssemblyMetadataAttribute(asm, "footer");
            template = template.Replace("{footer}", footer);

            return FormatTextForScreen(template.Trim(), maxLineLength);
        }

        /// <summary>
        /// Returns help content derived from the provided parsable object.
        /// </summary>
        /// <param name="parsable"></param>
        /// <param name="template"></param>
        /// <param name="argumentTemplate"></param>
        /// <param name="maxLineLength">The maximum number of characters in a line before it is wrapped</param>
        /// <returns></returns>
        public static string GetHelpInfo(Parsable parsable, string template, string argumentTemplate, int maxLineLength = 80)
        {
            if (parsable == null) throw new ArgumentNullException("parsable");
            if (string.IsNullOrEmpty(template)) return "";

            var parsableClass = Helper.GetObjectAttribute(parsable, typeof(ParsableClassAttribute)) as ParsableClassAttribute;
            if(parsableClass == null)
                throw new CliParseException("Unable to find 'ParsableClass' attribute on provided object.");

            template = template.Replace("{title}", parsableClass.Title);
            template = template.Replace("{description}", parsableClass.Description);

            template = template.Replace("{copyright}", string.IsNullOrEmpty(parsableClass.Copyright) ? "" : parsableClass.Copyright);
            template = template.Replace("{version}", parsableClass.Version);

            var syntax = GetSyntaxInfo(parsable, argumentTemplate, parsableClass.AllowedPrefixes);
            template = template.Replace("{syntax}", syntax);

            template = template.Replace("{example}", parsableClass.ExampleText);
            template = template.Replace("{footer}", parsableClass.FooterText);

            return FormatTextForScreen(template.Trim(), maxLineLength);
        }

        private static string GetSyntaxInfo(Parsable parsable, string argumentTemplate, ICollection<char> prefixes)
        {
            var arguments = GetListArgumentAttributes(parsable);

            var sb = new StringBuilder();

            var prefix = "-"; // default
            if (prefixes.Count > 1)
            {
                prefix = prefixes.FirstOrDefault().ToString();
                var allowedPrefixes = "";
                prefixes.ToList().ForEach(x => allowedPrefixes += "'" + x + "',");
                allowedPrefixes = allowedPrefixes.Substring(0, allowedPrefixes.Length - 1);
                sb.AppendLine("The following argument prefix characters can be used: "+allowedPrefixes);
            }

            foreach (var argument in arguments)
            {
                sb.AppendLine(argument.GetSyntax(argumentTemplate, prefix));
            }

            return sb.ToString();
        }

        private static IEnumerable<ParsableArgumentAttribute> GetListArgumentAttributes(Parsable parsable)
        {
            var parsableType = parsable.GetType();
            var properties = parsableType.GetProperties();

            var arguments = new List<ParsableArgumentAttribute>();
            foreach (var prop in properties)
            {
                arguments.AddRange(prop.GetCustomAttributes(true).OfType<ParsableArgumentAttribute>());
            }
            return arguments;
        }

        private static string GetAssemblyMetadataAttribute(Assembly asm, string key)
        {
            var customAttributes = asm.GetCustomAttributes(typeof (AssemblyMetadataAttribute));

            var t = (from AssemblyMetadataAttribute attribute in customAttributes
                     select attribute).FirstOrDefault(x => x.Key.Equals(key));

            return t == null ? "" : t.Value;
        }

        private static string GetAssemblyAttribute(Assembly asm, Type type)
        {
            var customAttribute = asm.GetCustomAttributes(type).FirstOrDefault(x => x.GetType() == type);
            if (customAttribute == null) return "";

            return GetAssemblyAttributeValue(type, customAttribute);
        }

        private static string GetAssemblyAttributeValue(Type type, Attribute customAttribute)
        {
            if (type == typeof (AssemblyTitleAttribute))
            {
                var t = customAttribute as AssemblyTitleAttribute;
                return t == null ? "" : t.Title;
            }

            if (type == typeof (AssemblyDescriptionAttribute))
            {
                var t = customAttribute as AssemblyDescriptionAttribute;
                return t == null ? "" : t.Description;
            }

            if (type == typeof (AssemblyCompanyAttribute))
            {
                var t = customAttribute as AssemblyCompanyAttribute;
                return t == null ? "" : t.Company;
            }

            if (type == typeof (AssemblyCopyrightAttribute))
            {
                var t = customAttribute as AssemblyCopyrightAttribute;
                return t == null ? "" : t.Copyright;
            }
            return "";
        }

        private static string FormatTextForScreen(string text, int maxLineLength)
        {
            var sb = new StringBuilder();
            var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                sb.AppendLine(BreakStringToLength(line, maxLineLength));

            }
            return sb.ToString();
        }

        public static string BreakStringToLength(string line, int maximumLineLength)
        {
            if (string.IsNullOrEmpty(line)) return "";
            if (maximumLineLength <= 1) throw new ArgumentOutOfRangeException("maximumLineLength");
            if (line.Length <= maximumLineLength - 1) return line;

            var maxLineLength = maximumLineLength;

            var sb = new StringBuilder();
            var startingWhiteSpace = GetLeadingWhitespaceAsSpaces(line);
            var startingWhiteSpaceLength = startingWhiteSpace.Length;

            var currentIndex = 0;
            var possibleIndex = 0;

            var keepGoing = true;
            while (keepGoing)
            {
                var scanIndex = line.IndexOf(' ', possibleIndex);
                if (scanIndex != -1) scanIndex += 1;  // move to location after the space so we wrap at start of word.

                if (scanIndex - currentIndex + startingWhiteSpaceLength > maxLineLength)
                {
                    sb.Append(line.Substring(currentIndex, possibleIndex - currentIndex));
                    sb.AppendLine();
                    sb.Append(startingWhiteSpace);
                    currentIndex = possibleIndex;
                }
                // no more spaces
                if (scanIndex == -1)
                {
                    var lengthRemaining = line.Length - currentIndex;
                    if (currentIndex == 0)
                    {
                        if (lengthRemaining > maxLineLength)
                        {
                            sb.AppendLine(line.Substring(currentIndex, maxLineLength));
                            sb.Append(startingWhiteSpace);
                            currentIndex += maxLineLength;
                        }
                        else
                        {
                            sb.Append(line.Substring(currentIndex, lengthRemaining));
                            keepGoing = false;
                        }
                    }
                    else
                    {
                        if (lengthRemaining + startingWhiteSpaceLength > maxLineLength)
                        {
                            sb.AppendLine(line.Substring(currentIndex, maxLineLength - startingWhiteSpaceLength));
                            sb.Append(startingWhiteSpace);
                            currentIndex += maxLineLength - startingWhiteSpaceLength;
                        }
                        else
                        {
                            sb.Append(line.Substring(currentIndex, lengthRemaining));
                            keepGoing = false;
                        }
                    }
                }
                else
                {
                    possibleIndex = scanIndex;
                }
            }

            return sb.ToString();
        }

        private static string GetLeadingWhitespaceAsSpaces(string line)
        {
            int count = 0;
            foreach (var c in line)
            {
                if (!Char.IsWhiteSpace(c)) break;
                if (c == ' ') count++;
                if (c.Equals('\t')) count += 4;
            }
            return new string(' ', count);
        }
    }
}