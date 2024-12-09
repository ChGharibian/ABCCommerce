import ImageScroller from "./ImageScroller";
import Input from "./Input";
import './ImageScrollerDelete.css';
import { useEffect, useState } from "react";
export default function ImageScrollerDelete({images, handleChange}) {
    const [currentImage, setCurrentImage] = useState(0);
    const [removeList, setRemoveList] = useState([]);
    const [visible, setVisible] = useState(true);
    useEffect(() => {
        handleChange(removeList);
    }, [removeList])

    function handleImageChange(index) {
        setCurrentImage(index);
    }

    function handleRemove(e) {
        let index = removeList.indexOf(currentImage);
        if(e.target.checked) {
            // removing image
            if(index === -1) {
                // not in remove list
                setRemoveList([...removeList, currentImage])
            }
        } else {
            // unremoving image
            if(index !== -1) {
                // in remove list
                setRemoveList([...removeList.slice(0, index), ...removeList.slice(index + 1, removeList.length)])
            }
        }
    }

    return (
        <div className={"image-scroller-delete-wrapper" + (!visible ? " hidden" : "")}>
            <ImageScroller images={images} handleImageChange={handleImageChange} onActiveImages={(list) => list.length === 0 ? setVisible(false) : setVisible(true)}/>
            <div className="delete-controls">
                <label htmlFor="deleteImage">Remove? 
                    <input name="deleteImage" type="checkbox" checked={removeList.includes(currentImage)} onChange={handleRemove} /></label>
            </div>
        </div>
    )
}