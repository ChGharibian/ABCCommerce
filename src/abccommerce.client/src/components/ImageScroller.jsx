import { useEffect, useState } from "react";
import Arrow from "./Arrow";
import './ImageScroller.css';
import { ArrayUtil } from "../util/arrays";
import { FileUtil } from "../util/files";
/**
 * @category component
 * @function ImageScroller
 * @author Thomas Scott, Angel Cortes
 * @since October 26
 * @description Creates a simple horizontal scrolling display for images with one image displayed at a time.
 * @param {Object} props
 * @param {Array<String>} props.images List of image sources to display
 * @returns {JSX.Element}
 */
export default function ImageScroller({images, handleImageChange = () => {}, onActiveImages = () => {}}) {
    const [currentImage, setCurrentImage] = useState(0);
    const [activeImages, setActiveImages] = useState([]);

    useEffect(() => {
        getActiveImages();
    }, [images])

    const getActiveImages = async () => {
        let validList = [];
        for(let i = 0; i < images.length; i++) {
            if(await FileUtil.isValidImgSrc(images[i])) validList.push({
                src: images[i],
                realIndex: i
            })
        }
        setActiveImages(validList);
        onActiveImages(validList);
    }

    const scrollLeft = async () => {
        let index = ArrayUtil.getInBoundIndex(activeImages, currentImage - 1);
        setCurrentImage(index);
        handleImageChange(activeImages[index].realIndex);
    }

    const scrollRight = async () => {
        let index = ArrayUtil.getInBoundIndex(activeImages, currentImage + 1);
        setCurrentImage(index);
        handleImageChange(activeImages[index].realIndex);
    }

    return (
        <div className="image-scroller-wrapper">
            {activeImages.length > 1 &&
            <div className="image-scroller-left-controls" onClick={scrollLeft}>
                <Arrow size={13} thickness={3} direction="left" zIndex={2000} />
            </div>
            }
            {activeImages.length > 0 ?
            <img className="image-scroller-image" src={activeImages[currentImage].src} alt="Deleted" />
            :
            "No images"
            }
            {activeImages.length > 1 &&
            <div className="image-scroller-right-controls" onClick={scrollRight} >
                <Arrow size={13} thickness={3} direction="right" zIndex={2000} />
            </div>
            }
        </div>
    )
}