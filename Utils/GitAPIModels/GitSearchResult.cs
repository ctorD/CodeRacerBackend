﻿using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodeRacerBackend.Utils.GitAPIModels;

public class GitSearchResult
{
    [JsonProperty("total_count")] public long TotalCount { get; set; }

    [JsonProperty("incomplete_results")] public bool IncompleteResults { get; set; }

    [JsonProperty("items")] public Item[] Items { get; set; }
}

public class Item
{
    [JsonProperty("id")] public long Id { get; set; }

    [JsonProperty("node_id")] public string NodeId { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("full_name")] public string FullName { get; set; }

    [JsonProperty("private")] public bool Private { get; set; }

    [JsonProperty("owner")] public Owner Owner { get; set; }

    [JsonProperty("html_url")] public Uri HtmlUrl { get; set; }

    [JsonProperty("description")] public string Description { get; set; }

    [JsonProperty("fork")] public bool Fork { get; set; }

    [JsonProperty("url")] public Uri Url { get; set; }

    [JsonProperty("forks_url")] public Uri ForksUrl { get; set; }

    [JsonProperty("keys_url")] public string KeysUrl { get; set; }

    [JsonProperty("collaborators_url")] public string CollaboratorsUrl { get; set; }

    [JsonProperty("teams_url")] public Uri TeamsUrl { get; set; }

    [JsonProperty("hooks_url")] public Uri HooksUrl { get; set; }

    [JsonProperty("issue_events_url")] public string IssueEventsUrl { get; set; }

    [JsonProperty("events_url")] public Uri EventsUrl { get; set; }

    [JsonProperty("assignees_url")] public string AssigneesUrl { get; set; }

    [JsonProperty("branches_url")] public string BranchesUrl { get; set; }

    [JsonProperty("tags_url")] public Uri TagsUrl { get; set; }

    [JsonProperty("blobs_url")] public string BlobsUrl { get; set; }

    [JsonProperty("git_tags_url")] public string GitTagsUrl { get; set; }

    [JsonProperty("git_refs_url")] public string GitRefsUrl { get; set; }

    [JsonProperty("trees_url")] public string TreesUrl { get; set; }

    [JsonProperty("statuses_url")] public string StatusesUrl { get; set; }

    [JsonProperty("languages_url")] public Uri LanguagesUrl { get; set; }

    [JsonProperty("stargazers_url")] public Uri StargazersUrl { get; set; }

    [JsonProperty("contributors_url")] public Uri ContributorsUrl { get; set; }

    [JsonProperty("subscribers_url")] public Uri SubscribersUrl { get; set; }

    [JsonProperty("subscription_url")] public Uri SubscriptionUrl { get; set; }

    [JsonProperty("commits_url")] public string CommitsUrl { get; set; }

    [JsonProperty("git_commits_url")] public string GitCommitsUrl { get; set; }

    [JsonProperty("comments_url")] public string CommentsUrl { get; set; }

    [JsonProperty("issue_comment_url")] public string IssueCommentUrl { get; set; }

    [JsonProperty("contents_url")] public string ContentsUrl { get; set; }

    [JsonProperty("compare_url")] public string CompareUrl { get; set; }

    [JsonProperty("merges_url")] public Uri MergesUrl { get; set; }

    [JsonProperty("archive_url")] public string ArchiveUrl { get; set; }

    [JsonProperty("downloads_url")] public Uri DownloadsUrl { get; set; }

    [JsonProperty("issues_url")] public string IssuesUrl { get; set; }

    [JsonProperty("pulls_url")] public string PullsUrl { get; set; }

    [JsonProperty("milestones_url")] public string MilestonesUrl { get; set; }

    [JsonProperty("notifications_url")] public string NotificationsUrl { get; set; }

    [JsonProperty("labels_url")] public string LabelsUrl { get; set; }

    [JsonProperty("releases_url")] public string ReleasesUrl { get; set; }

    [JsonProperty("deployments_url")] public Uri DeploymentsUrl { get; set; }

    [JsonProperty("created_at")] public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at")] public DateTimeOffset UpdatedAt { get; set; }

    [JsonProperty("pushed_at")] public DateTimeOffset PushedAt { get; set; }

    [JsonProperty("git_url")] public string GitUrl { get; set; }

    [JsonProperty("ssh_url")] public string SshUrl { get; set; }

    [JsonProperty("clone_url")] public Uri CloneUrl { get; set; }

    [JsonProperty("svn_url")] public Uri SvnUrl { get; set; }

    [JsonProperty("homepage")] public string Homepage { get; set; }

    [JsonProperty("size")] public long Size { get; set; }

    [JsonProperty("stargazers_count")] public long StargazersCount { get; set; }

    [JsonProperty("watchers_count")] public long WatchersCount { get; set; }

    [JsonProperty("language")] public Language Language { get; set; }

    [JsonProperty("has_issues")] public bool HasIssues { get; set; }

    [JsonProperty("has_projects")] public bool HasProjects { get; set; }

    [JsonProperty("has_downloads")] public bool HasDownloads { get; set; }

    [JsonProperty("has_wiki")] public bool HasWiki { get; set; }

    [JsonProperty("has_pages")] public bool HasPages { get; set; }

    [JsonProperty("forks_count")] public long ForksCount { get; set; }

    [JsonProperty("mirror_url")] public object MirrorUrl { get; set; }

    [JsonProperty("archived")] public bool Archived { get; set; }

    [JsonProperty("disabled")] public bool Disabled { get; set; }

    [JsonProperty("open_issues_count")] public long OpenIssuesCount { get; set; }

    [JsonProperty("license")] public License License { get; set; }

    [JsonProperty("forks")] public long Forks { get; set; }

    [JsonProperty("open_issues")] public long OpenIssues { get; set; }

    [JsonProperty("watchers")] public long Watchers { get; set; }

    [JsonProperty("default_branch")] public DefaultBranch DefaultBranch { get; set; }

    [JsonProperty("score")] public long Score { get; set; }
}

public class License
{
    [JsonProperty("key")] public string Key { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("spdx_id")] public string SpdxId { get; set; }

    [JsonProperty("url")] public Uri Url { get; set; }

    [JsonProperty("node_id")] public string NodeId { get; set; }
}

public class Owner
{
    [JsonProperty("login")] public string Login { get; set; }

    [JsonProperty("id")] public long Id { get; set; }

    [JsonProperty("node_id")] public string NodeId { get; set; }

    [JsonProperty("avatar_url")] public Uri AvatarUrl { get; set; }

    [JsonProperty("gravatar_id")] public string GravatarId { get; set; }

    [JsonProperty("url")] public Uri Url { get; set; }

    [JsonProperty("html_url")] public Uri HtmlUrl { get; set; }

    [JsonProperty("followers_url")] public Uri FollowersUrl { get; set; }

    [JsonProperty("following_url")] public string FollowingUrl { get; set; }

    [JsonProperty("gists_url")] public string GistsUrl { get; set; }

    [JsonProperty("starred_url")] public string StarredUrl { get; set; }

    [JsonProperty("subscriptions_url")] public Uri SubscriptionsUrl { get; set; }

    [JsonProperty("organizations_url")] public Uri OrganizationsUrl { get; set; }

    [JsonProperty("repos_url")] public Uri ReposUrl { get; set; }

    [JsonProperty("events_url")] public string EventsUrl { get; set; }

    [JsonProperty("received_events_url")] public Uri ReceivedEventsUrl { get; set; }

    [JsonProperty("type")] public TypeEnum Type { get; set; }

    [JsonProperty("site_admin")] public bool SiteAdmin { get; set; }
}

public enum DefaultBranch
{
    GhPages,
    Master
}

public enum Language
{
    JavaScript
}

public enum TypeEnum
{
    Organization,
    User
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new()
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
        {
            DefaultBranchConverter.Singleton,
            LanguageConverter.Singleton,
            TypeEnumConverter.Singleton,
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        }
    };
}

internal class DefaultBranchConverter : JsonConverter
{
    public static readonly DefaultBranchConverter Singleton = new();

    public override bool CanConvert(Type t)
    {
        return t == typeof(DefaultBranch) || t == typeof(DefaultBranch?);
    }

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        switch (value)
        {
            case "gh-pages":
                return DefaultBranch.GhPages;
            case "master":
                return DefaultBranch.Master;
        }

        throw new Exception("Cannot unmarshal type DefaultBranch");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }

        var value = (DefaultBranch)untypedValue;
        switch (value)
        {
            case DefaultBranch.GhPages:
                serializer.Serialize(writer, "gh-pages");
                return;
            case DefaultBranch.Master:
                serializer.Serialize(writer, "master");
                return;
        }

        throw new Exception("Cannot marshal type DefaultBranch");
    }
}

internal class LanguageConverter : JsonConverter
{
    public static readonly LanguageConverter Singleton = new();

    public override bool CanConvert(Type t)
    {
        return t == typeof(Language) || t == typeof(Language?);
    }

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        if (value == "JavaScript") return Language.JavaScript;
        throw new Exception("Cannot unmarshal type Language");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }

        var value = (Language)untypedValue;
        if (value == Language.JavaScript)
        {
            serializer.Serialize(writer, "JavaScript");
            return;
        }

        throw new Exception("Cannot marshal type Language");
    }
}

internal class TypeEnumConverter : JsonConverter
{
    public static readonly TypeEnumConverter Singleton = new();

    public override bool CanConvert(Type t)
    {
        return t == typeof(TypeEnum) || t == typeof(TypeEnum?);
    }

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        switch (value)
        {
            case "Organization":
                return TypeEnum.Organization;
            case "User":
                return TypeEnum.User;
        }

        throw new Exception("Cannot unmarshal type TypeEnum");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }

        var value = (TypeEnum)untypedValue;
        switch (value)
        {
            case TypeEnum.Organization:
                serializer.Serialize(writer, "Organization");
                return;
            case TypeEnum.User:
                serializer.Serialize(writer, "User");
                return;
        }

        throw new Exception("Cannot marshal type TypeEnum");
    }
}