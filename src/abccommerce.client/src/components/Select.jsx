import './Select.css';
/**
 * @category component
 * @function Select
 * @description Abstraction of the default select component with some default styling and boilerplate for 
 * inner option elements.
 * @author Thomas Scott
 * @since November 19
 * @param {Object} props
 * @param {Array<String>} props.options Options to be selected from
 * @param {Array<String>} props.values Values for respective options
 * @param {Function} props.onChange Function to be called when an option is picked
 * @returns {JSX.Element}
 */
export default function Select({options, values, onChange, ...rest}) {

    return (
        <div className="select-wrapper">
            <select onChange={(e) => onChange(e.target.value)} {...rest}>
                {options.map((o, i) => 
                    <option value={values[i] ?? ""} key={o + i}>{o}</option>
                )}
            </select>
        </div>
        
    )
}