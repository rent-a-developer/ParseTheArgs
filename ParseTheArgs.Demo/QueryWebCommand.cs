using System;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using ParseTheArgs.Errors;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup;

namespace ParseTheArgs.Demo
{
    public static class QueryWebCommand
    {
        public static Int32 QueryWeb(QueryWebCommandArguments arguments)
        {
            try
            {
                String queryUrl = "";

                if (!String.IsNullOrEmpty(arguments.WebsiteAddress))
                {
                    queryUrl = arguments.WebsiteAddress;
                }
                else if (!String.IsNullOrEmpty(arguments.SearchEngineQuery))
                {
                    queryUrl = $"https://www.google.com/search?q={WebUtility.UrlEncode(arguments.SearchEngineQuery)}";
                }

                String queryResult;

                using (var webClient = new WebClient())
                {
                    queryResult = webClient.DownloadString(queryUrl);
                }

                var websiteText = HtmlToTextConverter.ConvertHtmlToText(queryResult);
                Console.WriteLine(websiteText);

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception:");
                Console.Error.WriteLine(ex.ToString());
                return 1;
            }
        }

        public static void SetupCommand(ParserSetup parserSetup)
        {
            var defaultCommand = parserSetup
                .DefaultCommand<QueryWebCommandArguments>()
                .Help("Downloads and shows a website on the console and performs a search engine search.")
                .ExampleUsage("Toolbox --search What is life");

            defaultCommand
                .Argument(a => a.WebsiteAddress)
                .Name("website")
                .ShortName('w')
                .Help("The HTTP address of a website to download an show.");

            defaultCommand
                .Argument(a => a.SearchEngineQuery)
                .Name("search")
                .ShortName('s')
                .Help("A search query to ask a search engine.");

            defaultCommand.Validate(ValidateArguments);
        }

        private static void ValidateArguments(CommandValidatorContext<QueryWebCommandArguments> context)
        {
            if (String.IsNullOrEmpty(context.CommandArguments.SearchEngineQuery) && String.IsNullOrWhiteSpace(context.CommandArguments.WebsiteAddress))
            {
                context.AddError(new ArgumentMissingError(context.GetArgumentName(a => a.WebsiteAddress)));
            }

            if (!String.IsNullOrWhiteSpace(context.CommandArguments.WebsiteAddress))
            {
                if (
                    !Uri.TryCreate(context.CommandArguments.WebsiteAddress, UriKind.Absolute, out var uri) ||
                    (
                        uri.Scheme != Uri.UriSchemeHttp &&
                        uri.Scheme != Uri.UriSchemeHttps
                    )
                )
                {
                    context.AddError(
                        new InvalidArgumentError(
                            context.GetArgumentName(a => a.WebsiteAddress),
                            $"The website address '{context.CommandArguments.WebsiteAddress}' is not a valid web URL."
                        )
                    );
                }
            }
        }

        private static class HtmlToTextConverter
        {
            public static String ConvertHtmlToText(String html)
            {
                var document = new HtmlDocument();
                document.LoadHtml(html);

                var writer = new StringWriter();

                ConvertNodeChildrenToText(document.DocumentNode, writer);

                writer.Flush();
                return writer.ToString();
            }

            private static void ConvertNodeChildrenToText(HtmlNode node, StringWriter outputWriter)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    ConvertNodeToText(childNode, outputWriter);
                }
            }

            private static void ConvertNodeToText(HtmlNode node, StringWriter outputWriter)
            {
                switch (node.NodeType)
                {
                    case HtmlNodeType.Comment:
                        break;

                    case HtmlNodeType.Document:
                        ConvertNodeChildrenToText(node, outputWriter);
                        break;

                    case HtmlNodeType.Text:
                        var parentName = node.ParentNode.Name;

                        if ((parentName == "script") || (parentName == "style"))
                        {
                            break;
                        }

                        var html = ((HtmlTextNode)node).Text;

                        if (HtmlNode.IsOverlappedClosingElement(html))
                        {
                            break;
                        }

                        if (html.Trim().Length > 0)
                        {
                            outputWriter.Write(HtmlEntity.DeEntitize(html));
                        }

                        break;

                    case HtmlNodeType.Element:
                        switch (node.Name)
                        {
                            case "p":
                                outputWriter.Write("\r\n");
                                break;
                        }

                        if (node.HasChildNodes)
                        {
                            ConvertNodeChildrenToText(node, outputWriter);
                        }
                        break;
                }
            }
        }
    }
}
