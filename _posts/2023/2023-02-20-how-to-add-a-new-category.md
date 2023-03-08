---

title: How to add a new category in a jekyll site
author: chatGpt
categories: [technology]
tags: [how-to,getting-started,jekyll-tutorial]
date: 2023-03-07 16:21:33 +00:00
---


<p>
<em style="color: rgb(0, 71, 178); background-color: rgb(255, 255, 102);">[This post has been updated via inline editor. Checking the format of the content...]   </em></p><p> ```
    In this example, replace Category Title with the title of the new category and ***`categoryname`*** with the name of the new category.

    Next, add the following code to the bottom of the new page to display all posts in the new category:

   {% raw %}
    ```html
    {% for post in site.categories[page.category] %}
    </p><h2><a href="{{ post.url }}">{{ post.title }}</a></h2><p>
    </p><p>{{ post.excerpt }}</p><p>
    {% endfor %}
    ```
    {% endraw %}
    Save the new page and the new category should now be available on your Jekyll site.

In summary, adding a new category to a Jekyll site involves modifying the site's configuration file, updating the front matter of each post that should be included in the new category, and creating a new page to display all posts in the new category.
</p>