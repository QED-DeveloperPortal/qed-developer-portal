---
title: How to raise a pull request to GitHub repo using Octokit library
author: jeny
categories: technology, documentation
tags: how-to,overview
date: 3/1/2023 12:00:00 AM
---


<p>To raise a pull request in a GitHub repository using the Octokit library in C#, you can use the <code style="color: var(--tw-prose-code);">PullRequestCreate</code> method of the <code style="color: var(--tw-prose-code);">PullRequestsClient</code> class. This method creates a new pull request in the specified repository with the specified title, body, and head branch.</p><p><br></p><p>Here's an example of how to use this method to create a new pull request:</p><p><br></p><pre class="ql-syntax" spellcheck="false">using Octokit;

var client = new GitHubClient(new ProductHeaderValue("MyApp"));
var tokenAuth = new Credentials("YOUR_PERSONAL_ACCESS_TOKEN");
client.Credentials = tokenAuth;

var newPullRequest = new NewPullRequest("My new pull request", "my-feature-branch", "main")
{
    Body = "This is my pull request body"
};

var pullRequest = await client.PullRequest.Create("octocat", "hello-world", newPullRequest);

Console.WriteLine(pullRequest.HtmlUrl);
</pre><p><br></p><p>In this example, we create a new instance of the <code style="color: var(--tw-prose-code);">GitHubClient</code> class with a personal access token, and then use the <code style="color: var(--tw-prose-code);">PullRequest.Create</code> method of the <code style="color: var(--tw-prose-code);">PullRequestsClient</code> object to create a new pull request in the <code style="color: var(--tw-prose-code);">hello-world</code> repository owned by the user <code style="color: var(--tw-prose-code);">octocat</code>. The <code style="color: var(--tw-prose-code);">NewPullRequest</code> object specifies the title, head branch, and base branch of the pull request, as well as an optional body.</p><p><br></p><p>The <code style="color: var(--tw-prose-code);">PullRequest.Create</code> method returns an object of type <code style="color: var(--tw-prose-code);">PullRequest</code>, which contains information about the newly created pull request, including the URL, which we access using <code style="color: var(--tw-prose-code);">pullRequest.HtmlUrl</code>.</p><p><br></p><p>Note that in the example above, we are passing the personal access token as the <code style="color: var(--tw-prose-code);">Credentials</code> property of the <code style="color: var(--tw-prose-code);">GitHubClient</code> object. However, for more secure authentication, it's recommended to use the <code style="color: var(--tw-prose-code);">GitHubClient.Credentials.GetCredentials()</code> method to retrieve credentials from the user's system credential store.</p>