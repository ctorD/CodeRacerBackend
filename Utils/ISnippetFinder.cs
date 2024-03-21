using Octokit;

namespace CodeRacerBackend.Utils;

public interface ISnippetFinder
{
    string GetSnippet(string lang);
    string GetSnippet(Language lang);

}