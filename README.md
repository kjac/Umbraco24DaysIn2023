# 24 Days In Umbraco 2023

This repo contains the code samples for the article "Do once, use twice with the Delivery API" from 24 Days in Umbraco 2023

There are two projects:

- The React Blog
- The Razor Blog

The Razor Blog acts as a server for the React Blog; in other words, The Razor Blog must run for the React Blog to function.

Please note that these are _not_ production grade implementations. They are purely samples that showcase different aspects of how data from the Delivery API can be applied.

## The React Blog

This is a single page application built with [React](https://react.dev/) using [React Router](https://reactrouter.com/).

To try this sample out, run `npm install` and `npm start` in the sample root directory.

Among other things, this sample showcases how to use [custom filters](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api/extension-api-for-querying#custom-filter) to query blog posts by author and by tag.

## The Razor Blog

This is first and foremost the Umbraco server that serves blog data through the Delivery API. The Umbraco database is included in this repo, so you should be able to "just" clone the repo and run the site.

The credentials for the backoffice are:

- Email: admin@localhost
- Password: SuperSecret123

...these are also displayed on the login screen :smile:

This is also a Razor implementation of a blog, where Umbraco acts as head of the request. In other words, this showcases how Umbraco can be used as a hybrid CMS, serving data both headlessly to other channels _and_ serving a website on its own.

The sample shows how the Delivery API can be applied to solve various use cases:

- Fetching content items asyncronously using a web component on the client.
- Utilizing the Delivery API querying service to query for content items, reusing the custom filtering built for The React Blog.
