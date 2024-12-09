import { useEffect, useState, useRef, act } from "react";
import Arrow from './Arrow';
import './ImageList.css';
import { ArrayUtil } from "../util/arrays";
import { FileUtil } from "../util/files";
/**
 * @category component
 * @function ImageList
 * @author Thomas Scott, Angel Cortes
 * @since November 1
 * @description Creates a dynamically sized list of images that is scrollable when necessary.
 * @param {Object} props
 * @param {Array<String>} props.images List of image sources to display
 * @returns {JSX.Element}
 */
export default function ImageList({images}) {
    const [imagesPerScroll, setImagesPerScroll] = useState(1);
    const [range, setRange] = useState([0,0]) // inclusive both ends
    const [activeImages, setActiveImages] = useState([]);
    const [loadingImgs, setLoadingImgs] = useState(false);
    const imageListWrapper = useRef(null);

    const getActiveImages = async () => {
        setLoadingImgs(true);
        let validList = [];
        for(let i = 0; i < images.length; i++) {
            console.log('checking img');
            if(await FileUtil.isValidImgSrc(images[i])) validList.push(images[i]);
        }
        console.log(validList);
        setActiveImages(validList);
        setLoadingImgs(false);
    }

    const scrollLeft = () => {
        setRange([ArrayUtil.getInBoundIndex(activeImages, range[0] - 1),
                  ArrayUtil.getInBoundIndex(activeImages, range[1] - 1)]);
    }

    const scrollRight = () => {
        setRange([ArrayUtil.getInBoundIndex(activeImages, range[0] + 1),
                  ArrayUtil.getInBoundIndex(activeImages, range[1] + 1)]);
    }

    const handleResize = () => {
        if(imageListWrapper.current.clientWidth < 750) {
            // < 750 1 image
            setImagesPerScroll(1);
        } else if(imageListWrapper.current.clientWidth < 1050) {
            // < 1050 2 images
            setImagesPerScroll(2);
        } else if(imageListWrapper.current.clientWidth < 1370) {
            // < 1370 3 images
            setImagesPerScroll(3)
        } else {
            // 4 images
            setImagesPerScroll(4);
        }
    }

    useEffect(() => {
        if(!loadingImgs) {
            activeImages.length > imagesPerScroll ? 
            setRange([range[0], ArrayUtil.getInBoundIndex(activeImages, range[0] + imagesPerScroll - 1)])
            :
            setRange([range[0], ArrayUtil.getInBoundIndex(activeImages, range[0] + activeImages.length - 1)]);
        } else {
            setRange([0, 0]);
        }
    }, [imagesPerScroll, loadingImgs])

    useEffect(() => {
        getActiveImages();
    }, [images])

    useEffect(() => {
        handleResize();
        window.addEventListener('resize', handleResize)

        return () => window.removeEventListener('resize', handleResize)
    }, [])

    return (
        <div className="image-list-wrapper" ref={imageListWrapper}>
            {activeImages.length === 0 ?
                <p>No Images</p>
            : activeImages.length <= imagesPerScroll ?
                    
                    ArrayUtil.getFromRange(activeImages, range).map((i, index) => 
                        <img src={i} key={index}/>
                    )
            :   
                <>
                <div className="control-left">
                    <Arrow size={20} direction="left" onClick={scrollLeft}/>
                </div>
                    {
                    ArrayUtil.getFromRange(activeImages, range).map((i, index) => 
                        <img src={i} key={index}/>
                    )
                    }
                <div className="control-right">
                    <Arrow size={20} direction="right" onClick={scrollRight}/>
                </div>
                </>
            }
        </div>
    )
}