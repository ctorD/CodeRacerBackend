using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace CodeRacerBackend.Utils
{
    public class SnippetFinder : ISnippetFinder
    {

        private readonly string _gitApiKey;
        public SnippetFinder(IConfiguration configuration)
        {
            _gitApiKey = configuration.GetSection("GitApiKey").Value;
        }

        private string GetRepo(string lang)
        {
            var urlParams = $"repositories?q=tetris+language:{lang}&sort=stars&order=desc";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com/search/");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "CodeRacer");

            //Accept JSON

            HttpResponseMessage response = client.GetAsync(urlParams).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsStringAsync().Result;
                var jo = JObject.Parse(dataObjects);
                var randomRepo = new Random().Next(0, jo.Count);
                var repoName = jo["items"][randomRepo]["full_name"].ToString();

                return repoName;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }
        }

        private string FindSnippetParamBuilder(string lang, string repo, string keyword)
        {
            var urlParams = $"code?q={keyword}+in:file+language:{lang}+repo:{repo}";

            return urlParams;
        }

        public string GetSnippet(string lang)
        {
            //Search for keyword depending on language

            var repo = GetRepo(lang);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com/search/");
            client.DefaultRequestHeaders.Add("User-Agent", "request");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _gitApiKey);
            
            //client.DefaultRequestHeaders.Add("Authorization", "5c394c85f95a938bf97e7f2a49af448883317f3f");
           //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

            var urlParams = "";

            var keyword = "function";

            switch (lang)
            {
                case "javascript":
                    //Javascript keywords = function
                    urlParams = FindSnippetParamBuilder(lang, repo, keyword);
                    break;

                case "java":
                    //Java keywords = "public void", "private void", "private string"

                    break;

                case "csharp":
                    //csharp keywords = "public void", "private void", "private string"

                    break;
            }

            HttpResponseMessage response = client.GetAsync(urlParams).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsStringAsync().Result;
                var jo = JObject.Parse(dataObjects);
                var randomFile = new Random().Next(0, jo.Count);
                var fileUrl = jo["items"][randomFile]["url"].ToString();

                var dlLink = GetDownloadLink(fileUrl, keyword);

                if (dlLink != null)
                {
                    var snippet = ExtractSnippet(dlLink, keyword);
                    return snippet;
                }
                else
                {
                    throw new Exception("Could not find download link");
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }
        }

        private List<int> GetAllIndexes(string source, string matchString)
        {
            List<int> test = new List<int>();
            matchString = Regex.Escape(matchString);
            foreach (Match match in Regex.Matches(source, matchString))
            {
                test.Add(match.Index);
            }

            return test;
        }

        private string ExtractSnippet(string downloadLink, string keyword)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(downloadLink);
            HttpResponseMessage response = client.GetAsync("").Result;
            if (response.IsSuccessStatusCode)
            {
                string file = response.Content.ReadAsStringAsync().Result;

                file = file.Trim();

                var allIndexes = GetAllIndexes(file, keyword);

                var randomIndex = new Random().Next(0, allIndexes.Count);

                file = file.Substring(allIndexes[randomIndex], file.Length - allIndexes[randomIndex]);

                var beforeFunctionBlock = file.Substring(0, file.IndexOf('{'));

                var matches = Regex.Matches(file, @"{((?>[^{}]+|{(?<c>)|}(?<-c>))*(?(c)(?!)))");

                var functionBlock = matches[0].Value + "\n }";

                var fullFunction = beforeFunctionBlock + functionBlock;

                return fullFunction;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

                return null;
            }
        }

        private static string GetDownloadLink(string gitUrl, string keyWord)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(gitUrl);
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "CodeRacer");
            HttpResponseMessage response = client.GetAsync("").Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsStringAsync().Result;
                var jo = JObject.Parse(dataObjects);
                var downloadUrl = jo["download_url"]?.ToString();

                return downloadUrl;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }
        }
    }
}