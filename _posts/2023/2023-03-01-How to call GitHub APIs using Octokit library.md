---

title: How to call GitHub APIs using Octokit library
author: chatGpt
share: false
categories: [technology,documentation]
tags: [tutorial,how-to]
date: 2023-03-08 16:11:13 +00:00
---


<p>


</p><p><em style="background-color: rgb(255, 255, 102);">This page has been edited by the author</em>

</p><p>To call the GitHub API using the <strong>Octokit </strong>library, you first need to install it in your project by running the following command in your terminal:</p><p><br></p><pre class="ql-syntax" spellcheck="false">npm install @octokit/rest
</pre><p><br></p><p>Once you have installed <strong>Octokit</strong>, you can create a new instance of the <strong>Octokit </strong>REST client and use it to make API requests. Here's an example of how to create a client and get information about a user:</p><pre class="ql-syntax" spellcheck="false"><span class="hljs-keyword">const</span> { <span class="hljs-title class_">Octokit</span> } = <span class="hljs-built_in">require</span>(<span class="hljs-string">"@octokit/rest"</span>);

<span class="hljs-keyword">const</span> octokit = <span class="hljs-keyword">new</span> <span class="hljs-title class_">Octokit</span>();

octokit.<span class="hljs-property">users</span>.<span class="hljs-title function_">getByUsername</span>({
&nbsp; <span class="hljs-attr">username</span>: <span class="hljs-string">"octocat"</span>
}).<span class="hljs-title function_">then</span>(<span class="hljs-function">(<span class="hljs-params">{ data }</span>) =&gt;</span> {
&nbsp; <span class="hljs-variable language_">console</span>.<span class="hljs-title function_">log</span>(data);
}).<span class="hljs-title function_">catch</span>(<span class="hljs-function">(<span class="hljs-params">error</span>) =&gt;</span> {
&nbsp; <span class="hljs-variable language_">console</span>.<span class="hljs-title function_">error</span>(error);
});
</pre><p><br></p><p>In this example, we create a new instance of the <strong>Octokit </strong>client, and then use the <code style="color: var(--tw-prose-code);">getByUsername</code> method to get information about a user with the username "octocat". The result of the API call is returned as a Promise, which we handle using the <code style="color: var(--tw-prose-code);">then</code> and <code style="color: var(--tw-prose-code);">catch</code> methods.</p><p><br></p><p>You can use other methods provided by <strong>Octokit </strong>to interact with other parts of the GitHub API, such as creating repositories, managing issues, and more. The <strong>Octokit </strong>documentation provides detailed information about all available methods and how to use them.</p>