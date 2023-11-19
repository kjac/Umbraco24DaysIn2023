import {UMBRACO_SERVER_URL} from "./Constants";

export const postUrl = (post) => post.route.path;

export const imageUrl = (image) => `${UMBRACO_SERVER_URL}/${image.url}`;

export const authorUrl = (author) => author.route.path;

export const tagUrl = (tag) => `/tags/${tag.toLowerCase()}`;

export const imageAltText = (image) => image.properties.altText;
