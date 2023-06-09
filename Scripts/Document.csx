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

static var fxIdAndText = new Func<string, XElement, IDictionary<string, object>>((att, node) => new Dictionary<string, object> {
                    {"IdParts", new IdParts(node.Attribute(att).Value)},
                    {"Text", node.Nodes().ToMarkDown()}
});

static var templates = new Dictionary<string, Func<dynamic, string>>  {

                    // Document / Assembly
                    {"doc", (model) =>
$@"# Assembly: {model.assembly}
## Contents
{model.toc}

{model.members}"},

                    // Type
                    {"type", (model) =>
$@"
---
## {model.IdParts.FullyQualifiedName}
Namespace: `{model.IdParts.Namespace}`

{model.Text}
"},

                    // Field
                    {"field", (model) => $"### {model.Name}\n{model.Text}\n---\n"},
                    
                    // Property
                    {"property", (model) => $"### {model.Name}\n{model.Text}\n---\n"},

                    // Method
                    {"method", (model) =>
$@"### {model.IdParts.MemberName}: {model.IdParts.Parent}.{model.IdParts.Name}
id: `{model.IdParts.Id}`

{model.summary}
{model.typeparametersHeader}
{model.typeparameters}
{model.paramHeader}
{model.parameters}
{model.exceptions}
{model.examples}
"},
                    {"event", (model) => $"### {model.Name}\n{model.Text}\n---\n"},
                    {"summary", (model) => $"{model.Text}\n"},
                    {"remarks", (model) => $"\n>{model.Name}\n"},
                    {"example", (model) => $"\n{model.Text}\n"},
                    {"seePage", (model) => $"[[{model.Text}|{model.Name}]]"},
                    {"seeAnchor", (model) => $"[{model.Text}]({model.Name})"},
                    {"param", (model) => $"|{model.Name}: |{model.Text}|\n" },
                    {"typeparam", (model) => $"|{model.Name}: |{model.Text}|\n" },
                    {"exception", (model) => $"\nException thrown: [{model.Name}](#{model.Name}): {model.Text}\n" },
                    {"returns", (model) => $"Returns: {model.Name}\n"},
                    {"none", (model) => ""}  };

static var methods = new Dictionary<string, Func<XElement, IDictionary<string, object>>>
                {
                    {"doc", x=> new Dictionary<string, object> {
                        {"assembly", x.Element("assembly").Element("name").Value},
                        {"members", x.Element("members").Elements("member").ToMarkDown()},
                        {"toc", x.Element("members").Elements("member")
                        .Select(toc => new IdParts(toc.Attribute("name").Value))
                        .Where(toc => toc.MemberName == "type")
                        .Select(toc => $"- [{toc.FullyQualifiedName}](#{toc.FullyQualifiedNameLink})\n")
                        .Aggregate("", (current, next) => current + "" + next)}
                    }},
                    {"type", x=> fxIdAndText("name", x)},
                    {"field", x=> fxNameAndText("name", x)},
                    {"property", x=> fxNameAndText("name", x)},
                    //{"method",x=>d("name", x)},
                    {"method", x=> new Dictionary<string, object>{
                        {"IdParts", new IdParts(x.Attribute("name").Value)},
                        {"summary", x.Elements("summary").ToMarkDown()},
                        {"typeparametersHeader", x.Elements("typeparam").Any() ? "|Param | Description |\n|-----|-----|" : ""},
                        {"typeparameters", x.Elements("typeparam").Any() ? x.Elements("typeparam").ToMarkDown() : ""},
                        {"paramHeader", x.Elements("param").Any() ? "|Name | Description |\n|-----|------|" : ""},
                        {"parameters", x.Elements("param").Any() ? x.Elements("param").ToMarkDown() : ""},
                        {"exceptions", (x.Element("exception") != null) ? x.Element("exception").ToMarkDown() : ""},
                        {"examples", x.Elements("example").Any() ? $"\n#### Examples:\n{x.Elements("example").ToMarkDown()}\n" : ""},
                    }},
                    {"event", x=>fxNameAndText("name", x)},
                    {"summary", x=> new Dictionary<string, object> {{"Text", x.Nodes().ToMarkDown()}}},
                    {"remarks", x => new Dictionary<string, object> {{"Text", x.Nodes().ToMarkDown()}}},
                    {"example", x => new Dictionary<string, object> {{"Text", x.ToCodeBlock()}}},
                    {"seePage", x=> fxNameAndText("cref", x) },
                    {"seeAnchor", x=> {
                        var xx = fxNameAndText("cref", x);
                        xx["Name"] = xx["Name"].ToString().ToLower(); return new Dictionary<string, object> {{"Text", xx}}; }},
                    {"param", x => fxNameAndText("name", x) },
                    {"typeparam", x => fxNameAndText("name", x) },
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
    public string FullyQualifiedNameLink { get; set; }
    public string Namespace { get; set; }
    public string Parent { get; set; }
    public string Name { get; set; }

    public IdParts(string id)
    {
        this.Id = id;
        Console.WriteLine(id);
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
        this.FullyQualifiedNameLink = this.FullyQualifiedName.Replace(".", "").ToLower();
        var fqnParts = this.FullyQualifiedName.Split('(');  // look for first '('. Required for Methods and properties with arguments.
        var nameParts = fqnParts[0].Split('.');
        this.Name = nameParts[nameParts.Length - 1];
        if ("FPEM".Contains(splits[0]))
        {
            this.Parent = nameParts[nameParts.Length - 2];
            this.Namespace = string.Join('.', nameParts.Take(nameParts.Length - 2));
        }
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

/// <summary>
/// Converts an IDictionary to a dynamic type.
/// </summary>
/// <param name="dict"></param>
/// <returns></returns>
internal static dynamic DictionaryToExpando(IDictionary<string, object> dict)
{
    var expando = new ExpandoObject();
    var expandoDict = (IDictionary<string, object>)expando;
    foreach (var kvp in dict)
    {
        if (kvp.Value is IDictionary<string, object>)
        {
            var expandoValue = DictionaryToExpando((IDictionary<string, object>)kvp.Value);
            expandoDict.Add(kvp.Key, expandoValue);
        }
        else if (kvp.Value is System.Collections.ICollection)
        {
            // iterate through the collection and convert any string-object dictionaries
            // along the way into expando objects
            var itemList = new List<object>();
            foreach (var item in (System.Collections.ICollection)kvp.Value)
            {
                if (item is IDictionary<string, object>)
                {
                    var expandoItem = DictionaryToExpando((IDictionary<string, object>)item);
                    itemList.Add(expandoItem);
                }
                else
                {
                    itemList.Add(item);
                }
            }
            expandoDict.Add(kvp.Key, itemList);
        }
        else
        {
            expandoDict.Add(kvp);
        }
    }
    return expando;
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
        var model = DictionaryToExpando(methods[name](el));
        Console.WriteLine($"About to render template for [{name}]...");
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

static string ToCodeBlock(this XElement el)
{
    string s = "";
    foreach (var childNode in el.Nodes())
    {
        if (childNode.NodeType == XmlNodeType.Text)
        {
            s = s + $"\n{childNode.ToString().Trim()}\n";
        }
        else if (childNode.NodeType == XmlNodeType.Element && ((XElement)childNode).Name == "code")
        {
            var code = (childNode as XElement).Value;
            var lines = code.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var blank = lines[0].TakeWhile(x => x == ' ').Count() - 4;
            code = string.Join("\n", lines.Select(x => new string(x.SkipWhile((y, i) => i < blank).ToArray())));
            code = $"``` c#\n{code}\n```";
            s = s + code;
        }
    }
    return s;
}