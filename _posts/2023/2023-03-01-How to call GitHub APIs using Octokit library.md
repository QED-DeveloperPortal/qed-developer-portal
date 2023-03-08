---
title: How to call GitHub APIs using Octokit library
author: jeny
categories: [technology,documentation]
tags: [how-to,tutorial]

date: 3/1/2023 12:00:00 AM
---


<p>To call the GitHub API using the <strong>Octokit </strong>library, you first need to install it in your project by running the following command in your terminal:</p><p><br></p><pre class="ql-syntax" spellcheck="false">npm install @octokit/rest
</pre><p><br></p><p>Once you have installed <strong>Octokit</strong>, you can create a new instance of the <strong>Octokit </strong>REST client and use it to make API requests. Here's an example of how to create a client and get information about a user:</p><pre class="ql-syntax" spellcheck="false">const { Octokit } = require("@octokit/rest");

const octokit = new Octokit();

octokit.users.getByUsername({
&nbsp; username: "octocat"
}).then(({ data }) =&gt; {
&nbsp; console.log(data);
}).catch((error) =&gt; {
&nbsp; console.error(error);
});
</pre><p><br></p><p>In this example, we create a new instance of the <strong>Octokit </strong>client, and then use the <code style="color: var(--tw-prose-code);">getByUsername</code> method to get information about a user with the username "octocat". The result of the API call is returned as a Promise, which we handle using the <code style="color: var(--tw-prose-code);">then</code> and <code style="color: var(--tw-prose-code);">catch</code> methods.</p><p><br></p><p>You can use other methods provided by <strong>Octokit </strong>to interact with other parts of the GitHub API, such as creating repositories, managing issues, and more. The <strong>Octokit </strong>documentation provides detailed information about all available methods and how to use them.</p>