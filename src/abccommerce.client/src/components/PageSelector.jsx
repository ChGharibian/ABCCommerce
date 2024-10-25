import { useEffect, useState } from "react";
import './PageSelector.css';
export default function PageSelector({width, height, handlePageChange, page}) {
    // width and height passed in as integers representing pixels

    const downPage = () => {
        if(page > 1) handlePageChange(page - 1)
    }

    const upPage = () => {
        handlePageChange(page + 1)
    }

    return (
        <div className="page-selector-wrapper" style={{
            width,
            height,
            top: "calc(97.5% - " + height + "px)",
            left: "calc(50% - " + (width / 2) + "px)" 
        }}>
            <div className="arrow left" style={{
                width: (height / 4) + "px",
                height: (height / 4) + "px"
            }} onClick={downPage}></div>
            <div className="page-number" style={{
                fontSize: (height / 2) + "px"
            }}>{page}</div>
            <div className="arrow right" style={{
                width: (height / 4) + "px",
                height: (height / 4) + "px"
            }} onClick={upPage}></div>
        </div>
    )
}   