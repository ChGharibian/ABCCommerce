import Tag from "./Tag"
import './TagList.css';
export function TagList({tags, maxTags, maxTagWidth}) {
    return <div className="tag-list">
        {tags.length > maxTags ?
        <>
            {tags.slice(0, maxTags).map((tag, i) =>
                <Tag key={i} tag={tag} maxWidth={maxTagWidth} />
            )}
            <p className="small-text">{' + ' + (tags.length - 2 > 4 ? '' : tags.length - 2) + ' more'}</p>
            
        </>
        : 
            tags.map((tag, i) => 
                <Tag key={i} tag={tag} maxWidth={maxTagWidth} />
            ) 
        }
    </div>
}