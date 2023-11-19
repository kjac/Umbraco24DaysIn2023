import {Link} from "react-router-dom";
import { tagUrl } from "./Helpers";

function PostTags(props) {
    const tags = props.post?.properties.tags ?? [];
    
    if(tags.length === 0){
        return <></>
    }

    return <>
        <span key={tags[0]}>
            <Link to={tagUrl(tags[0])} className="hover:underline">{tags[0]}</Link>
        </span>
        {tags.slice(1).map(tag =>
            <span key={tag}> | <Link to={tagUrl(tag)} className="hover:underline">{tag + ''}</Link></span>
        )}
    </>
}

export default PostTags;
