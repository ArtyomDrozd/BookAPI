using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using BookAPI.DTO.Interfaces;

namespace BookAPI.DTO.Services
{
    public class BooksService : IBooksService
    {
        public string GetUrl(string bookName)
        {
            var searchRequest = "http://flibusta.site/booksearch?ask=" + bookName.Replace(' ', '+');
            return searchRequest;
        }

        public string GetResponse(string uri)
        {
            var sb = new StringBuilder();
            var buf = new byte[8192];
            var request = (HttpWebRequest) WebRequest.Create(uri);
            var response = (HttpWebResponse) request.GetResponse();
            var resStream = response.GetResponseStream();
            int count;
            do
            {
                count = resStream.Read(buf, 0, buf.Length);
                if (count != 0)
                {
                    sb.Append(Encoding.Default.GetString(buf, 0, count));
                }
            } while (count > 0);

            return sb.ToString();
        }

        public List<string> GetDataFromSearch(string htmlText)
        {
            const string regText = "<li><a href=\"/((b|sequence)/(.+?))\">(.+?)</a>";

            var regex = new Regex(regText);
            var matches = regex.Matches(htmlText);
            var dataBook = new List<string>();

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    var name = match.Groups[4].Value;
                    name = name.Replace("<b>", "");
                    name = name.Replace("<span style=\"background-color: #FFFCBB\">", "");
                    name = name.Replace("</span>", "");

                    name = name.Replace("</b>", "");
                    dataBook.Add(($"https://www.flibusta.site/{match.Groups[1].Value} | {name}"));
                }
            }

            return dataBook;
        }


        public void BookDownload(List<string> dataBook, int key, string userFormat)
        {
            var formats = new Dictionary<string, string>
            {
                ["fb2"] = ".fb2.zip",
                ["epub"] = ".fb2.epub",
                ["mobi"] = ".fb2.mobi",
                ["pdf"] = ".pdf"
            };

            if (key > 0 & key <= dataBook.Count)
            {
                var line = dataBook[key - 1].Split('|');
                var bookLink = line[0].Replace(" ", "");
                var fullBookName = line[1];
                Console.WriteLine(bookLink);

                const string regText = "скачать: (<a(.+?))</div>";

                var regex = new Regex(regText);
                var matches = regex.Matches(GetResponse(bookLink));

                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        var formatBook = match.Groups[1].Value;
                        var containsSearchResult =
                            formatBook.Contains($"{userFormat}", StringComparison.OrdinalIgnoreCase);
                        if (containsSearchResult == true)
                        {
                            var downloadLink = ($"{bookLink}/{userFormat}");
                            WebResponse response = null;
                            try
                            {

                                var request = WebRequest.Create(downloadLink);
                                response = request.GetResponse();
                            }
                            catch (WebException e)
                            {
                                if (e.Message.Contains("302"))
                                {
                                    response = e.Response;

                                    var myWebHeaderCollection = response.Headers;
                                    for (var i = 0; i < response.Headers.Count; i++)
                                    {
                                        var header = myWebHeaderCollection.GetKey(i);
                                        var values = myWebHeaderCollection.GetValues(header);
                                        if (values.Length > 0 && header.Equals("Location",
                                            StringComparison.OrdinalIgnoreCase))
                                        {
                                            using (var client = new WebClient())
                                            {
                                                foreach (var keyValue in formats)
                                                {
                                                    if (keyValue.Key == userFormat)
                                                    {
                                                        var downloadEnding = keyValue.Value;
                                                        client.DownloadFile(values[0],
                                                            $"D:\\test\\{fullBookName}{downloadEnding}");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
