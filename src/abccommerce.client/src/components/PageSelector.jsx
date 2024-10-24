import { useEffect, useState } from "react";
import './PageSelector.css';
export default function PageSelector({width, height, handlePageChange}) {
    // width and height passed in as integers representing pixels
    const [pageNumber, setPageNumber] = useState(1);

    const downPage = () => {
        if(pageNumber > 1) setPageNumber(pageNumber - 1);
    }

    const upPage = () => {
        setPageNumber(pageNumber + 1);
    }

    useEffect(() => {
        handlePageChange(pageNumber);
    }, [pageNumber])

    return (
        <div className="page-selector-wrapper" style={{
            width,
            height,
            top: "calc(95% - " + height + "px)",
            left: "calc(50% - " + (width / 2) + "px)" 
        }}>
            <div className="arrow left" style={{
                width: (height / 4) + "px",
                height: (height / 4) + "px"
            }} onClick={downPage}></div>
            <div className="page-number" style={{
                fontSize: (height / 2) + "px"
            }}>{pageNumber}</div>
            <div className="arrow right" style={{
                width: (height / 4) + "px",
                height: (height / 4) + "px"
            }} onClick={upPage}></div>
        </div>
    )
}   