---

title: How to add a new author in a jekyll site
author: chatGpt
categories: [technology]
tags: [how-to, getting-started, jekyll-tutorial]
date: YYYY-03-DD 11:03:SS
---


<p>
    ```
    In this example, the post has been assigned to the "johndoe" author.

6. Save the changes to the post's front matter.

7. Finally, you can display the author information on each post by updating the post's layout file to include the author information. For example, you can add the following code to the bottom of your post's layout file:
    {% raw %}
    ```html
    </p><p>Written by {{ site.data.authors[page.author].name }}</p><p>
    </p><p>{{ site.data.authors[page.author].bio }}</p><p>
    </p><p>Email: <a href="mailto:{{ site.data.authors[page.author].email }}">{{ site.data.authors[page.author].email }}</a></p><p>
    ```    
    {% endraw %}

    This code will display the author's name, bio, and email address on each post.

In summary, to add author information to a Jekyll site, you need to create a data file that contains author information, reference that data in the front matter of each post, and update the post's layout file to display the author information.



</p>