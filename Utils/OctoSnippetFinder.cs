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
        // var gitKey = configuration.GetValue<string>("GitKey");
        var envKey = Environment.GetEnvironmentVariable("GitApiKey");
        var gitKey = envKey ?? configuration.GetSection("GitApiKey").Value;
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
            case Language.TypeScript:
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
        var codeSample = _client.Search.SearchCode(new SearchCodeRequest(GetKeyword(lang))
        {
            Language = lang,
        }).Result.Items[3];

        var testCont = _client.Repository.Content.GetAllContents(repositoryId: codeSample.Repository.Id, codeSample.Path).Result;
        var rawCodeString = testCont[0].Content;
        //Trim out function block
        rawCodeString = rawCodeString.Trim();
        var keyword = GetKeyword(lang);
        var allIndexes = GetAllIndexes(rawCodeString, keyword);
        var randomIndex = new Random().Next(0, allIndexes.Count);
        rawCodeString = rawCodeString.Substring(allIndexes[randomIndex], rawCodeString.Length - allIndexes[randomIndex]);
        var beforeFunctionBlock = rawCodeString.Substring(0, rawCodeString.IndexOf('{'));
        var matches = Regex.Matches(rawCodeString, @"{((?>[^{}]+|{(?<c>)|}(?<-c>))*(?(c)(?!)))");
        var functionBlock = matches[0].Value + "\n }";
        var fullFunction = beforeFunctionBlock + functionBlock;

        return fullFunction;
    }
} 