import Tag from "./Tag"
import './TagList.css';
export default function TagList({tags, maxTags, maxTagWidth, fontSize=".7rem"
}) {
    const maxMoreTagsAmt = 4;
    return <div className="tag-list">
        {tags.length > maxTags ?
        <>
            {tags.slice(0, maxTags).map((tag, i) =>
                <Tag key={i} tag={tag} maxWidth={maxTagWidth} fontSize={fontSize}/>
            )}
            <p style={{fontSize}}>{' + ' + (tags.length - maxTags > maxMoreTagsAmt ? '' : tags.length - maxTags) + ' more'}</p>
            
        </>
        : 
            tags.map((tag, i) => 
                <Tag key={i} tag={tag} maxWidth={maxTagWidth} fontSize={fontSize}/>
            ) 
        }
    </div>
}