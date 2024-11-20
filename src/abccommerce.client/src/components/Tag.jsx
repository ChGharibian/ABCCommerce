import './Tag.css';
/**
 * @category component
 * @function Tag
 * @author Thomas Scott
 * @since October 25
 * @description Displays a tag with styles matching the site.
 * @param {Object} props
 * @param {String} props.tag Tag text to be displayed
 * @param {String} props.maxWidth Max width of the tag (CSS format)
 * @param {String} props.fontSize Font size of the tag (CSS format)
 * @returns {JSX.Element}
 */
export default function Tag({tag, maxWidth, fontSize}) {

    return <p className="tag small-text"
    style={{
        maxWidth,
        fontSize
    }}>{tag}</p>
}