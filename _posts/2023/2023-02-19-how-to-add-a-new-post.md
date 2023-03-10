---

title: How to add a new post in a jekyll site
author: chatGpt
share: false
categories: [technology]
tags: [how-to,getting-started,jekyll-tutorial]
date: 2023-03-09 17:02:28 +00:00
---


<p>


</p><p><em style="background-color: rgb(255, 255, 102);">[This file has been updated by the author.]</em></p><p><em><span class="ql-cursor">﻿﻿</span></em></p><p>Jekyll is a powerful static site generator that makes it easy to create a website with a simple and easy-to-use interface. One of the most common tasks when working with Jekyll is adding new posts to your site. In this article, we will take a look at how to add a new post to your Jekyll site.</p><p><br></p><p>### Step 1: Create a new post file</p><p>The first step to adding a new post to your Jekyll site is to create a new post file. You can do this by navigating to the ***`_posts`*** directory in your Jekyll project and creating a new file with the following format:</p><p><br></p><p>```sql</p><p>YEAR-MONTH-DAY-title.md</p><p>```</p><p><br></p><p>For example, if you want to create a post with the title "My First Post", you would create a file with the following name:</p><p><br></p><p>```</p><p>2023-02-20-my-first-post.md</p><p>```</p><p><br></p><p>### Step 2: Front Matter</p><p>Once you have created your new post file, you will need to add front matter to it. Front matter is a section of YAML metadata that Jekyll uses to determine how to handle your post.</p><p><br></p><p>At a minimum, your front matter should include the title of your post, as well as the date and any categories or tags that you want to associate with your post. Here is an example of a basic front matter section:</p><p><br></p><p>```yaml</p><p>
---
</p><p><br></p><p>layout: post</p><p>title: "My First Post"</p><p>date: 2023-02-20 12:00:00 -0500</p><p>categories: jekyll</p><p>tags: [jekyll, tutorial]</p><p>
---
</p><p><br></p><p>```</p><p><br></p><p>The ***`layout`*** field specifies which layout file to use for your post, while the ***`title`***, ***`date`***, ***`categories`***, and ***`tags`*** fields provide metadata about your post.</p><p><br></p><p>### Step 3: Add Content</p><p>With your front matter in place, you can now add the content of your post. Jekyll uses Markdown syntax to format your post, so you can include headings, lists, images, and more.</p><p><br></p><p>### Step 4: Save and Build</p><p>Once you have added your post content, save the file and build your Jekyll site. You can do this by running the following command from the terminal:</p><p><br></p><p>```</p><p>jekyll build</p><p>```</p><p><br></p><p>This command will generate your site and output it to the ***`_site`*** directory.</p><p><br></p><p>### Step 5: Preview your post</p><p>To preview your post, you can run the following command from the terminal:</p><p><br></p><p>```</p><p>jekyll serve</p><p>```</p><p><br></p><p>This will start a local server that you can use to view your Jekyll site. You can then navigate to your post by entering its URL in your web browser.</p><p><br></p><p>In conclusion, adding a new post to your Jekyll site is a simple process that involves creating a new post file, adding front matter, adding content, saving and building, and previewing your post. With these steps, you can quickly and easily create new content for your Jekyll site.</p><p><br></p><p>

</p>