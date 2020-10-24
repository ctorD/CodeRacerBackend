
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeRacerBackend.Utils
{
    public class SnippetFinder
    {

        public SnippetFinder()
        {

        }

        private string getRepo(string lang)
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

        private string findSnippetParamBuilder(string lang, string repo, string keyword)
        {
            var urlParams = $"code?q={keyword}+in:file+language:{lang}+repo:{repo}";

            return urlParams;
        }


        public string getSnippet(string lang)
        {

            //Search for keyword depending on language

            var repo = getRepo(lang);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com/search/");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "CodeRacer");

            string urlParams = "";

            string keyword = "function";

            switch (lang)
            {
                case "javascript":
                    //Javascript keywords = function 
                    urlParams = findSnippetParamBuilder(lang, repo, keyword);
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


                var dlLink = getDownloadLink(fileUrl, keyword);

                if (dlLink != null)
                {
                    var snippet = extractSnippet(dlLink, keyword);
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
        public List<int> GetAllIndexes(string source, string matchString)
        {
            List<int> test = new List<int>();
            matchString = Regex.Escape(matchString);
            foreach (Match match in Regex.Matches(source, matchString))
            {
                test.Add(match.Index);
            }

            return test;
        }
        private string extractSnippet(string downloadLink, string keyword)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(downloadLink);
            HttpResponseMessage response = client.GetAsync("").Result;
            if (response.IsSuccessStatusCode)
            {

                string file = response.Content.ReadAsStringAsync().Result;

                file = file.Trim();

                var AllIndexes = GetAllIndexes(file, keyword);

                var randomIndex = new Random().Next(0, AllIndexes.Count);


                file = file.Substring(AllIndexes[randomIndex], file.Length - AllIndexes[randomIndex]);

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

        private string getDownloadLink(string gitUrl, string keyWord)
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
                var downloadUrl = jo["download_url"].ToString();

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
