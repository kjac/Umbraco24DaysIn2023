import { useState, useEffect } from "react"
import { Link, useLocation } from "react-router-dom"
import { UMBRACO_API_URL } from "./Constants";
import {postUrl, imageUrl, authorUrl, imageAltText} from "./Helpers";
import { format } from "date-fns"
import PostTags from "./PostTags";

function Post() {
    const [post, setPost] = useState(null)
    const [morePosts, setMorePosts] = useState([])
    const location = useLocation();

    useEffect(() => {
        async function fetchData(){
            // fetch the post
            // - note that we are slightly over-fetching here, as we don't really need the blog excerpt on this page
            let response = await fetch(
                `${UMBRACO_API_URL}/item/${location.pathname}?expand=properties[coverImage,author[properties[picture]]]&fields=properties[$all[properties[picture]]]`,
                {
                    method: 'GET',
                    headers: {
                        'Start-Item': 'home',
                    }
                }
            );
            if (!response.ok) {
                throw new Error(`Could not fetch the post - response status was: ${response.status}`);
            }
            let data = await response.json();
            setPost(data);

            // fetch "more posts" (the most recent posts)
            // - we don't want to fetch the current post, so we'll use the custom "isNot" filter to omit it (if present) from the result
            response = await fetch(
                `${UMBRACO_API_URL}/?fetch=children:posts&filter=isNot:${data.id}&sort=updateDate:desc&take=3&expand=properties[coverImage]&fields=properties[excerpt,tags,coverImage]`,
                {
                    method: 'GET',
                    headers: {
                        'Start-Item': 'home',
                    }
                }
            );
            if (!response.ok) {
                throw new Error(`Could not fetch more posts - response status was: ${response.status}`);
            }
            data = await response.json();
            setMorePosts(data.items);
        }

        fetchData();
    }, [location.pathname]);

    const renderPost = () => {
        // TODO: add some kind of skeleton loading while fetching the blog post and related posts
        return <>
            {/* header */}
            <header className="w-full m-0 p-0 bg-cover bg-bottom" style={{backgroundColor: '#3F46B1'}}>
                <div className="container max-w-4xl mx-auto p-8 text-center break-normal">
                    <Link to="/" className="text-white font-extrabold text-3xl md:text-5xl">
                        The React Blog
                    </Link>
                </div>
            </header>

            { post &&
                <>
                    {/* title */}
                    <div className="text-center pt-16">
                        <p className="md:text-sm pt-2 text-gray-600 text-xs">{format(new Date(post.updateDate), 'dd/MM/yyyy')}</p>
                        <h1 className="font-bold break-normal text-3xl md:text-5xl">{post.name}</h1>
                        <p className="md:text-sm pt-2 text-gray-600 text-xs"><PostTags post={post} /></p>
                    </div>

                    {/* cover image */}
                    <div className="container w-full max-w-6xl mx-auto bg-white bg-no-repeat bg-center mt-8 rounded" style={{backgroundImage: `url(${imageUrl(post.properties.coverImage[0])})`, height: '75vh'}} role="img" aria-label={imageAltText(post.properties.coverImage[0])}></div>

                    {/* container */}
                    <div className="container max-w-5xl mx-auto -mt-32 mb-4">

                        <div className="mx-0 sm:mx-6 rounded-t rounded-b-none overflow-hidden shadow-lg mb-8">

                            {/* author */}
                            <Link to={authorUrl(post.properties.author)} className="flex w-full items-center font-sans p-8 bg-gray-200">
                                <img className="w-10 h-10 rounded-full mr-4" src={imageUrl(post.properties.author.properties.picture[0])} alt={imageAltText(post.properties.author.properties.picture[0])}></img>
                                <div className="flex-1">
                                    <p className="text-base font-bold text-base md:text-xl leading-none">{post.properties.author.name}</p>
                                </div>
                            </Link>

                            {/* content */}
                            <div className="bg-white w-full p-8 md:p-24 text-lg font-serif text-gray-800 leading-normal">
                                <div dangerouslySetInnerHTML={{__html: post.properties.content.markup}}></div>
                            </div>

                        </div>

                        {/* More posts */}
                        <div className="bg-gray-200 mx-0 sm:mx-6">
                            <div className="text-center pb-6">
                                <h2 className="font-bold break-normal text-3xl md:text-5xl">Keep on reading</h2>
                            </div>
                            <div className="container w-full max-w-6xl mx-auto">
                                <div className="flex flex-wrap -mx-2">
                                    {morePosts.map(post =>
                                        <div className="w-full md:w-1/3 px-2" key={post.id}>
                                            <div className="h-full bg-white rounded overflow-hidden shadow-md hover:shadow-lg relative smooth">
                                                <div className="no-underline hover:no-underline">
                                                    <Link to={postUrl(post)}>
                                                        <img src={imageUrl(post.properties.coverImage[0])} className="w-full rounded-t pt-4" alt={imageAltText(post.properties.coverImage[0])}></img>
                                                    </Link>
                                                    <div className="p-6 h-auto">
                                                        <p className="text-gray-600 text-xs md:text-sm">{format(new Date(post.updateDate), 'dd/MM/yyyy')}</p>
                                                        <div className="font-bold text-xl text-gray-900">{post.name}</div>
                                                        <p className="text-gray-600 text-xs md:text-sm"><PostTags post={post} /></p>
                                                        <p className="text-gray-800 font-serif text-base mb-5 pt-6">{post.properties.excerpt}</p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>
                </>
            }
        </>
    }
    
    return renderPost();
}

export default Post;