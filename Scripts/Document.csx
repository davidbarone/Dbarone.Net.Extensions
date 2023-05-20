/* ------------------------------------------------------
    Name: C# XML Comments to Markdown
    Purpose: Converts C# XML comments file to markdown.
    Notes: Based on https://gist.github.com/lontivero/593fc51f1208555112e0
   ------------------------------------------------------ */

#r "System.Xml.XDocument"
#r "System.Text.RegularExpressions"
#r "System.Console"

using System;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Dynamic;

var xmlFilePath = Args[0];
var mdFilePath = Args[1];
Main(xmlFilePath, mdFilePath);

static void Main(string xmlFilePath, string mdFilePath)
{
    Console.WriteLine(xmlFilePath);
    var xml = xmlFilePath;
    var doc = XDocument.Load(xml);
    var md = doc.Root.ToMarkDown();
    if (File.Exists(mdFilePath))
    {
        File.Delete(mdFilePath);
    }
    File.WriteAllText(mdFilePath, md);
}

static var fxNameAndText = new Func<string, XElement, string[]>((att, node) => new[] {
                    node.Attribute(att).Value,
                    node.Nodes().ToMarkDown()
});

static var templates = new Dictionary<string, string>  {
                    {"doc", "# {0}\n{1}"},
                    {"type", "\n\n>## {0}\n\n{1}\n---\n"},
                    {"field", "### {0}\n{1}\n---\n"},
                    {"property", "### {0}\n{1}\n---\n"},
                    {"method", "### {0}\n{1}{2}{3}{4}\n---\n"},
                    {"event", "### {0}\n{1}\n---\n"},
                    {"summary", "{0}\n"},
                    {"remarks", "\n>{0}\n"},
                    {"example", "_C# code_\n```c#\n{0}\n```\n"},
                    {"seePage", "[[{1}|{0}]]"},
                    {"seeAnchor", "[{1}]({0})"},
                    {"param", "|{0}: |{1}|\n" },
                    {"exception", "\nException thrown: [{0}](#{0}): {1}\n" },
                    {"returns", "Returns: {0}\n"},
                    {"none", ""}  };

static var methods = new Dictionary<string, Func<XElement, IEnumerable<string>>>
                {
                    {"doc", x=> new[]{
                        x.Element("assembly").Element("name").Value,
                        x.Element("members").Elements("member").ToMarkDown()
                    }},
                    {"type", x=>fxNameAndText("name", x)},
                    {"field", x=> fxNameAndText("name", x)},
                    {"property", x=> fxNameAndText("name", x)},
                    //{"method",x=>d("name", x)},
                    {"method", x=> new[]{
                        x.Attribute("name").Value,
                        x.Elements("summary").ToMarkDown(),
                        "|Name | Description |\n|-----|------|\n",
                        x.Elements("param").Any() ? x.Elements("param").ToMarkDown() : "",
                        (x.Element("exception") != null) ? x.Element("exception").ToMarkDown() : ""
                    }},
                    {"event", x=>fxNameAndText("name", x)},
                    {"summary", x=> new[]{ x.Nodes().ToMarkDown() }},
                    {"remarks", x => new[]{x.Nodes().ToMarkDown()}},
                    {"example", x => new[]{x.Value.ToCodeBlock()}},
                    {"seePage", x=> fxNameAndText("cref", x) },
                    {"seeAnchor", x=> { var xx = fxNameAndText("cref", x); xx[0] = xx[0].ToLower(); return xx; }},
                    {"param", x => fxNameAndText("name", x) },
                    {"exception", x => fxNameAndText("cref", x) },
                    {"returns", x => new[]{x.Nodes().ToMarkDown()}},
                    {"none", x => new string[0]}
                };

/// <summary>
/// Represents the parts making up a comment document id.
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
internal class IdParts
{
    public string Id { get; set; }
    public string MemberName { get; set; }
    public string FullyQualifiedName { get; set; }
    public string Namespace { get; set; }
    public string Name { get; set; }

    public IdParts(string id)
    {
        this.Id = id;
        var splits = id.Split(':');
        switch (splits[0])
        {
            case "N": this.MemberName = "namespace"; break;
            case "F": this.MemberName = "field"; break;
            case "P": this.MemberName = "property"; break;
            case "T": this.MemberName = "type"; break;
            case "E": this.MemberName = "event"; break;
            case "M": this.MemberName = "method"; break;
            case "!": this.MemberName = "error"; break;
            default: this.MemberName = "none"; break;
        }
        this.FullyQualifiedName = splits[1];
        var fqnParts = id.Split('(');  // look for first '('. Required for Methods and properties with arguments.
        var nameParts = fqnParts[0].Split('.');
        this.Name = fqnParts[fqnParts.Length - 1];
        this.Namespace = string.Join('.', nameParts.Take(nameParts.Length - 1));
    }
}

/// <summary>
/// Represents an XML comments fragment containing an outer element with
/// a 'name' attribute (usually containing an 'id' value), and inner
/// elements containing text descriptions. As so many of the XML comments
/// are in this format, we create a special structure for this.
/// </summary>
internal class IdAndText
{
    public IdParts IdParts { get; set; }
    public string Text { get; set; }

    public IdAndText(string id, string text)
    {
        this.IdParts = new IdParts(id);
        this.Text = text;
    }
}

internal static string ToMarkDown(this XNode e)
{
    string name;
    if (e.NodeType == XmlNodeType.Element)
    {
        var el = (XElement)e;

        // Get the name of the element (or if the
        // element is a 'member' element, get the name attribute).
        name = el.Name.LocalName;

        if (name == "member")
        {
            var idParts = new IdParts(el.Attribute("name").Value);
            name = idParts.MemberName;
        }
        if (name == "see")
        {
            var anchor = el.Attribute("cref").Value.StartsWith("!:#");
            name = anchor ? "seeAnchor" : "seePage";
        }
        var vals = methods[name](el).ToArray();
        return string.Format(templates[name], vals);
    }

    if (e.NodeType == XmlNodeType.Text)
        return Regex.Replace(((XText)e).Value.Replace('\n', ' '), @"\s+", " ");

    return "";
}

internal static string ToMarkDown(this IEnumerable<XNode> es)
{
    if (es != null && es.Any())
    {
        return es.Aggregate("", (current, x) => current + x.ToMarkDown());
    }
    else
    {
        return "";
    }
}

static string ToCodeBlock(this string s)
{
    var lines = s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
    var blank = lines[0].TakeWhile(x => x == ' ').Count() - 4;
    return string.Join("\n", lines.Select(x => new string(x.SkipWhile((y, i) => i < blank).ToArray())));
}