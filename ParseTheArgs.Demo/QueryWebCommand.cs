using System;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using ParseTheArgs.Errors;
using ParseTheArgs.Setup;
using ParseTheArgs.Validation;

namespace ParseTheArgs.Demo
{
    public static class QueryWebCommand
    {
        public static Int32 QueryWeb(QueryWebCommandOptions options)
        {
            try
            {
                String queryUrl = "";

                if (!String.IsNullOrEmpty(options.WebsiteAddress))
                {
                    queryUrl = options.WebsiteAddress;
                }
                else if (!String.IsNullOrEmpty(options.SearchEngineQuery))
                {
                    queryUrl = $"https://www.google.com/search?q={WebUtility.UrlEncode(options.SearchEngineQuery)}";
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
                .DefaultCommand<QueryWebCommandOptions>()
                .Help("Downloads and shows a website on the console and performs a search engine search.")
                .ExampleUsage("Toolbox --search What is life");

            defaultCommand
                .Option(a => a.WebsiteAddress)
                .Name("website")
                .Help("The HTTP address of a website to download an show.");

            defaultCommand
                .Option(a => a.SearchEngineQuery)
                .Name("search")
                .Help("A search query to ask a search engine.");

            defaultCommand.Validate(Validate);
        }

        private static void Validate(CommandValidatorContext<QueryWebCommandOptions> context)
        {
            if (String.IsNullOrEmpty(context.CommandOptions!.SearchEngineQuery) && String.IsNullOrWhiteSpace(context.CommandOptions.WebsiteAddress))
            {
                context.AddError(new OptionMissingError(context.GetOptionName(a => a.WebsiteAddress)));
            }

            if (!String.IsNullOrWhiteSpace(context.CommandOptions.WebsiteAddress))
            {
                if (
                    !Uri.TryCreate(context.CommandOptions.WebsiteAddress, UriKind.Absolute, out var uri) ||
                    (
                        uri.Scheme != Uri.UriSchemeHttp &&
                        uri.Scheme != Uri.UriSchemeHttps
                    )
                )
                {
                    context.AddError(
                        new InvalidOptionError(
                            context.GetOptionName(a => a.WebsiteAddress),
                            $"The website address '{context.CommandOptions.WebsiteAddress}' is not a valid web URL."
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
