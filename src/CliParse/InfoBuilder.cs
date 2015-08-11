using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CliParse
{
    public static class InfoBuilder
    {
        public static string GetHelpInfoFromAssemblyInfo(Parsable parsable, Assembly asm, string template, string argumentTemplate, string argumentPrefix)
        {
            if (parsable == null) throw new ArgumentNullException("parsable");
            if(asm == null) throw new ArgumentNullException("asm");

            var title = GetAssemblyAttribute(asm, typeof (AssemblyTitleAttribute));
            template = template.Replace("{title}", title);

            var description = GetAssemblyAttribute(asm, typeof (AssemblyDescriptionAttribute));
            template = template.Replace("{description}", description);

            var longdescription = GetAssemblyMetadataAttribute(asm, "longdescription");
            template = template.Replace("{longdescription}", longdescription);

            var syntax = GetSyntaxInfo(parsable, argumentTemplate, argumentPrefix);
            template = template.Replace("{syntax}", syntax);

            var company = GetAssemblyAttribute(asm, typeof (AssemblyCompanyAttribute));
            template = template.Replace("{company}", company);

            var copyright = GetAssemblyAttribute(asm, typeof (AssemblyCopyrightAttribute));
            template = template.Replace("{copyright}", copyright);

            return template;
        }

        private static string GetSyntaxInfo(Parsable parsable, string argumentTemplate, string prefix)
        {
            var arguments = GetListArgumentAttributes(parsable);

            var sb = new StringBuilder();
            foreach (var argument in arguments)
            {
                sb.AppendLine(argument.GetSyntax(argumentTemplate, prefix));
            }

            return sb.ToString();
        }

        private static IEnumerable<Argument> GetListArgumentAttributes(Parsable parsable)
        {
            if (parsable == null) throw new ArgumentNullException("parsable");

            var parsableType = parsable.GetType();
            var properties = parsableType.GetProperties();

            var arguments = new List<Argument>();
            foreach (var prop in properties)
            {
                foreach (var argument in prop.GetCustomAttributes(true).OfType<Argument>())
                {
                    arguments.Add(argument);
                }
            }
            return arguments;
        }

        private static string GetAssemblyMetadataAttribute(Assembly asm, string key)
        {
            var customAttributes = asm.GetCustomAttributes(typeof (AssemblyMetadataAttribute));

            var t = (from AssemblyMetadataAttribute attribute in customAttributes
                where attribute.Key.Equals(key)
                select attribute).FirstOrDefault();

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
    }
}