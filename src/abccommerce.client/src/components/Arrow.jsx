import './Arrow.css';
import { useState } from 'react';
export default function Arrow({direction = "left", onClick = ()=>{}, 
                               size = "30px", thickness = "5px", 
                               color = "var(--c-white-100)",
                               dimAmount="50%",
                               ...rest}) {

    const [brightness, setBrightness] = useState("100%");
    return <div className={"arrow " + direction} onClick={onClick} 
            style={{
                width: size,
                height: size,
                borderTop: `${thickness} solid ${color}`,
                borderLeft: `${thickness} solid ${color}`,
                filter: `brightness(${brightness})`,
                ...rest
            }}
            onMouseOver={() => setBrightness(dimAmount)}
            onMouseOut={() => setBrightness("100%")}
            ></div>
}            