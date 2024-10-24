import React from 'react';
import './Input.css';


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