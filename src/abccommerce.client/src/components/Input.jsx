import React from 'react';
import './Input.css';

/**
 * @category component
 * @function Input
 * @author Angel Cortes
 * @since October 24
 * @description Abstraction of default html input component containing default input styling for the site.
 * @param {React.HTMLAttributes<HTMLInputElement>} props
 * @returns {JSX.Element}
 */
export default function Input({
  type='text', 
  label,
  name, 
  value, 
  onChange,
  onBlur,
  placeholder, 
  error, 
  required=false,
  ...rest}) {
    return (
  <div className="input-wrapper">
    {label && 
    <label htmlFor={name}>
      {label}
      {required && ' *'}
      </label>}
    <input 
      name= {name}
      style= {{width: '40%'}}
      type={type}
      value={value}
      onChange={onChange}
      onBlur={onBlur}
      placeholder={placeholder}
      required={required}
      {...rest}
     />
     {error && <p className="error-message">{error}</p>}
  </div>
    )
}