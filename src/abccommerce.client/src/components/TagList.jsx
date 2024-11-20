import Tag from "./Tag"
import './TagList.css';
/**
 * @category component
 * @function TagList
 * @author Thomas Scott, Angel Cortes
 * @since October 25
 * @description Displays a list of tags with limits on the the
 * amount of tags to be displayed, their max width, and their 
 * font size. Displays text that indicates the amount of tags that
 * aren't shown.
 * @param {Object} props
 * @param {Array<String>} props.tags List of text to turn into tags
 * @param {Number} props.maxTags Max amount of tags to be displayed in the list
 * @param {String} props.maxTagWidth Max width of the tags (CSS format)
 * @param {String} [props.fontSize=".7rem"] Font size of the tags (CSS format)
 * @returns {JSX.Element}
 */
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