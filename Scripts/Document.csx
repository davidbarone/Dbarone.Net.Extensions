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

Main(Args.ToArray());

static void Main(string[] args)
{
    Console.WriteLine("Begin: Document.csx...");
    var xmlFilePath = args[0];
    var mdFilePath = args[1];
    XmlToMarkdown(xmlFilePath, mdFilePath);
}
static void XmlToMarkdown(string xmlFilePath, string mdFilePath)
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

static var fxNameAndText = new Func<string, XElement, IDictionary<string, object>>((att, node) => new Dictionary<string, object> {
                    {"Name", node.Attribute(att).Value},
                    {"Text", node.Nodes().ToMarkDown()}
});

static var templates = new Dictionary<string, Func<dynamic, string>>  {
                    {"doc", (model) => $"# {model.Name}\n{model.Text}"},
                    {"type", (model) => $"\n\n>## {model.Name}\n\n{model.Text}\n---\n"},
                    {"field", (model) => $"### {model.Name}\n{model.Text}\n---\n"},
                    {"property", (model) => $"### {model.Name}\n{model.Text}\n---\n"},
                    {"method", (model) =>
$@"### {model.id}
Namespace: {model.IdParts.Namespaceb}
{model.summary}
{model.paramHeader}
{model.parameters}
{model.exceptions}
---"},
                    {"event", (model) => $"### {model.Name}\n{model.Text}\n---\n"},
                    {"summary", (model) => $"{model.Name}\n"},
                    {"remarks", (model) => $"\n>{model.Name}\n"},
                    {"example", (model) => $"_C# code_\n```c#\n{model.Name}\n```\n"},
                    {"seePage", (model) => $"[[{model.Text}|{model.Name}]]"},
                    {"seeAnchor", (model) => $"[{model.Text}]({model.Name})"},
                    {"param", (model) => $"|{model.Name}: |{model.Text}|\n" },
                    {"exception", (model) => $"\nException thrown: [{model.Name}](#{model.Name}): {model.Text}\n" },
                    {"returns", (model) => $"Returns: {model.Name}\n"},
                    {"none", (model) => ""}  };

static var methods = new Dictionary<string, Func<XElement, IDictionary<string, object>>>
                {
                    {"doc", x=> new Dictionary<string, object> {
                        {"Name", x.Element("assembly").Element("name").Value},
                        {"Text", x.Element("members").Elements("member").ToMarkDown()}
                    }},
                    {"type", x=> fxNameAndText("name", x)},
                    {"field", x=> fxNameAndText("name", x)},
                    {"property", x=> fxNameAndText("name", x)},
                    //{"method",x=>d("name", x)},
                    {"method", x=> new Dictionary<string, object>{
                        {"IdParts", new IdParts(x.Attribute("name").Value)},
                        {"summary", x.Elements("summary").ToMarkDown()},
                        {"paramHeader", "|Name | Description |\n|-----|------|\n"},
                        {"parameters", x.Elements("param").Any() ? x.Elements("param").ToMarkDown() : ""},
                        {"exceptions", (x.Element("exception") != null) ? x.Element("exception").ToMarkDown() : ""}
                    }},
                    {"event", x=>fxNameAndText("name", x)},
                    {"summary", x=> new Dictionary<string, object> {{"Text", x.Nodes().ToMarkDown()}}},
                    {"remarks", x => new Dictionary<string, object> {{"Text", x.Nodes().ToMarkDown()}}},
                    {"example", x => new Dictionary<string, object> {{"Text", x.Value.ToCodeBlock()}}},
                    {"seePage", x=> fxNameAndText("cref", x) },
                    {"seeAnchor", x=> {
                        var xx = fxNameAndText("cref", x);
                        xx["Name"] = xx["Name"].ToString().ToLower(); return new Dictionary<string, object> {{"Text", xx}}; }},
                    {"param", x => fxNameAndText("name", x) },
                    {"exception", x => fxNameAndText("cref", x) },
                    {"returns", x => new Dictionary<string, object> {{"Text", x.Nodes().ToMarkDown()}}},
                    {"none", x => new Dictionary<string, object> {}}
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
        var model = methods[name](el);
        return templates[name](model);
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