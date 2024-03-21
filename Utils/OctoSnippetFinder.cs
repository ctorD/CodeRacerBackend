using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Octokit;

namespace CodeRacerBackend.Utils;

public class OctoSnippetFinder : ISnippetFinder
{
    private GitHubClient _client;
    public OctoSnippetFinder(IConfiguration configuration)
    {
        var gitKey = configuration.GetValue<string>("GitKey");
        _client = new GitHubClient(new ProductHeaderValue("Code-Racer"))
        {
            Credentials = new Credentials(gitKey)
        };
    }
    
    private List<int> GetAllIndexes(string source, string matchString)
    {
        var test = new List<int>();
        matchString = Regex.Escape(matchString);
        foreach (Match match in Regex.Matches(source, matchString)) test.Add(match.Index);

        return test;
    }

    private string GetKeyword(Language lang)
    {
        switch (lang)
        {
            case Language.JavaScript:
                return "function";
            case Language.CSharp:
                return "public";
        }

        return "";
    }

    public string GetSnippet(string lang)
    {
        throw new NotImplementedException();
    }

    public string GetSnippet(Language lang)
    {
        var test2 = _client.Search.SearchCode(new SearchCodeRequest(GetKeyword(lang))
        {
            Language = lang,
        }).Result.Items[3];

        var testCont = _client.Repository.Content.GetAllContents(repositoryId: test2.Repository.Id, test2.Path).Result;
        var file = testCont[0].Content;
        file = file.Trim();
        var keyword = GetKeyword(lang);

        var allIndexes = GetAllIndexes(file, keyword);

        var randomIndex = new Random().Next(0, allIndexes.Count);

        file = file.Substring(allIndexes[randomIndex], file.Length - allIndexes[randomIndex]);

        var beforeFunctionBlock = file.Substring(0, file.IndexOf('{'));

        var matches = Regex.Matches(file, @"{((?>[^{}]+|{(?<c>)|}(?<-c>))*(?(c)(?!)))");

        var functionBlock = matches[0].Value + "\n }";

        var fullFunction = beforeFunctionBlock + functionBlock;

        return fullFunction;
    }
} 