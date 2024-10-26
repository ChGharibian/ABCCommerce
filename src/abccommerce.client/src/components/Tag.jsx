import './Tag.css';
export default function Tag({tag, maxWidth}) {

    return <p className="tag small-text"
    style={{
        maxWidth
    }}>{tag}</p>
}