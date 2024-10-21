import { useState, useRef } from "react";
import './DropdownList.css';
export default function DropdownList({placeholder, list, name, width, required}) {
    const [userInput, setUserInput] = useState("");
    const [listShown, setListShown] = useState(false);
    const [listLength, setListLength] = useState(0);
    const dropdown = useRef(null)
    return (
    <div className="dropdown-list-wrapper">
    {required ? 
    <div className="required-mark" style={{
        width,
        left: "calc((100% - " + width + ") / 2 + 0.7rem)"
        }}>*</div> 
    : <></>}
    <input style={{width}} required={required} placeholder={placeholder} name={name} value={userInput} 
        onChange={e => {
        if(!listShown) setListShown(true);
        setUserInput(e.target.value)
        }} 
        onBlur={(e) => {
        if(e.relatedTarget !== dropdown.current) setListShown(false);
        }}
    />
    <select style={{width,
        left: "calc(50% - " + width + " / 2)",
        maxHeight: listLength === 0 ? "0" : "6rem",
        borderLeft: listLength > 0 ? "1px solid white" : "none",
        borderRight: listLength > 0 ? "1px solid white" : "none",
        borderBottom: listLength > 0 ? "1px solid white" : "none",
        borderTop: "none"
    }} size={Math.min(Math.max(1, listLength), 5)} ref={dropdown} multiple className={"dropdown-list" + (
        !listShown ? " hidden" :
        "" 
    )}
    onChange={(e) => {
        setUserInput(e.target.value);
        setListShown(false);
        }}>
        {getFilteredList()}
    </select>
    </div>
    )

    function getFilteredList() {
        let options = list.filter(item => item.toLowerCase().includes(userInput.toLowerCase())).map(item => 
            <option className="dropdown-item" key={item} >
            {item}
            </option>
        )

        if(listLength !== options.length) setListLength(options.length);
        return options;
    }
}