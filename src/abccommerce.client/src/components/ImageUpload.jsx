import './ImageUpload.css';
import deleteSymbol from '../assets/delete-symbol.png';
export default function ImageUpload({previewURL, name, size, onDelete=()=>{}}) {
    return (
        <tr className="image-upload-wrapper">
            <td>
                <img src={previewURL} />
            </td>
            <td>
                {name}
            </td>
            <td>
                {size}
            </td>
            <td>
                <img onClick={onDelete} src={deleteSymbol} className="delete-button"/>
            </td>
        </tr>
    )
}