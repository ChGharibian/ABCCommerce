import './Tag.css';
export default function Tag({tag, maxWidth, fontSize}) {

    return <p className="tag small-text"
    style={{
        maxWidth,
        fontSize
    }}>{tag}</p>
}