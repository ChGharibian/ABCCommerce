import './Arrow.css';
import { useState } from 'react';
export default function Arrow({direction = "left", onClick = ()=>{}, 
                               size = 30, thickness = 5, 
                               color = "var(--main-text-color)",
                               secondaryColor = "transparent",
                               dimAmount="50%",
                               ...rest}) {

    const [brightness, setBrightness] = useState("100%");
    const outlineSizeMultiplier = 2;
    const outlineRadius = (size + thickness) * outlineSizeMultiplier / 2;
    return ( 
    <div className={`arrow-click-area ${direction}`}
    onClick={onClick} 
    onMouseOver={() => setBrightness(dimAmount)}
    onMouseOut={() => setBrightness("100%")}
    style={{
        width: `${outlineRadius * 2}px`,
        height: `${outlineRadius * 2}px`,
        backgroundColor: `${secondaryColor}`,
        borderRadius: `${outlineRadius}px`, 
        ...rest
    }}>
        <div className="arrow" 
            style={{
                width: `${size}px`,
                height: `${size}px`,
                borderTop: `${thickness}px solid ${color}`,
                borderLeft: `${thickness}px solid ${color}`,
                filter: `brightness(${brightness})`,
                transform: `translate(${outlineRadius - size / 2 + (((size - thickness) / 2) * Math.pow(Math.sin(Math.PI / 4), 2))}px,
                 ${outlineRadius - size / 2 + (((size - thickness) / 2) * Math.pow(Math.sin(Math.PI / 4), 2))}px)`,
            }}
            ></div>
    </div>
    
        )
}