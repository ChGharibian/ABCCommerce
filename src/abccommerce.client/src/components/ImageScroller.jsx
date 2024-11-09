import { useState } from "react";
import Arrow from "./Arrow";
import './ImageScroller.css';
import { getInBoundIndex } from "../util/arrays";

export default function ImageScroller({images}) {
    const [currentImage, setCurrentImage] = useState(0);

    const scrollLeft = () => {
        setCurrentImage(getInBoundIndex(images, currentImage - 1));
    }

    const scrollRight = () => {
        setCurrentImage(getInBoundIndex(images, currentImage + 1));
    }

    return (
        <div className="image-scroller-wrapper">
            {images.length > 1 &&
            <div className="image-scroller-left-controls">
                <Arrow size={13} thickness={3} onClick={scrollLeft} direction="left" zIndex={2000} />
            </div>
            }
            {images.length > 0 ?
            <img className="image-scroller-image" src={images[currentImage]} />
            :
            "No images"
            }
            {images.length > 1 &&
            <div className="image-scroller-right-controls">
                <Arrow size={13} thickness={3} onClick={scrollRight} direction="right" zIndex={2000} />
            </div>
            }
        </div>
    )
}