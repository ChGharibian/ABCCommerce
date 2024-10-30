import Tag from "./Tag"
import './TagList.css';
export default function TagList({tags, maxTags, maxTagWidth}) {
    const maxMoreTagsAmt = 4;
    return <div className="tag-list">
        {tags.length > maxTags ?
        <>
            {tags.slice(0, maxTags).map((tag, i) =>
                <Tag key={i} tag={tag} maxWidth={maxTagWidth} />
            )}
            <p className="small-text">{' + ' + (tags.length - maxTags > maxMoreTagsAmt ? '' : tags.length - maxTags) + ' more'}</p>
            
        </>
        : 
            tags.map((tag, i) => 
                <Tag key={i} tag={tag} maxWidth={maxTagWidth} />
            ) 
        }
    </div>
}