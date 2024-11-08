import { useEffect, useState, useRef } from "react";
import Arrow from './Arrow';
import './ImageList.css';
export default function ImageList({images}) {
    const [imagesPerScroll, setImagesPerScroll] = useState(1);
    const [range, setRange] = useState([0,0]) // inclusive both ends
    const imageListWrapper = useRef(null);

    const getInBoundIndex = (arr, i) => {
        // js modulo is actually a remainder
        if(i < 0) {
            return arr.length + (i % arr.length);
        }
        return i % arr.length;
    }   

    const scrollLeft = () => {
        setRange([getInBoundIndex(images, range[0] - 1),
                  getInBoundIndex(images, range[1] - 1)]);
    }

    const scrollRight = () => {
        setRange([getInBoundIndex(images, range[0] + 1),
                  getInBoundIndex(images, range[1] + 1)]);
    }

    const getFromRange = (arr, range) => {
        let newArr = [];
        for(let i = range[0]; i !== range[1]; i++) {
            // loop back to start if i is out of array bound
            if(i >= arr.length) {
                if(range[1] === 0) break;
                i = 0;    
            }

            newArr.push(arr[i]);
        }
        if(arr[range[1]]) newArr.push(arr[range[1]]);
        return newArr;
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
        images.length > imagesPerScroll ? 
        setRange([range[0], getInBoundIndex(images, range[0] + imagesPerScroll - 1)])
        :
        setRange([range[0], getInBoundIndex(images, range[0] + images.length - 1)]);
    }, [imagesPerScroll])

    useEffect(() => {
        handleResize();
        window.addEventListener('resize', handleResize)

        return () => window.removeEventListener('resize', handleResize)
    }, [])

    return (
        <div className="image-list-wrapper" ref={imageListWrapper}>
            {images.length === 0 ?
                <p>No Images</p>
            : images.length <= imagesPerScroll ?
                    
                    getFromRange(images, range).map((i, index) => 
                        <img src={i} key={index}/>
                    )
            :   
                <>
                <div className="control-left">
                    <Arrow size={20} direction="left" onClick={scrollLeft}/>
                </div>
                    {
                    getFromRange(images, range).map((i, index) => 
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