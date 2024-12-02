import React, { useState } from 'react';
import '../components/Input';
import Input from '../components/Input';
import { useNavigate } from 'react-router-dom';
import './login.css';
import { TokenUtil } from '../util/tokens';
import { ValidationUtil } from '../util/validation';
/**
 * @category route
 * @function Login
 * @author Angel Cortes
 * @since October 24
 * @description Displays a form that allows users to enter their user credentials and 
 * log into their account.
 * @returns {JSX.Element} Login page
 */
export default function Login(){

  //set state
  const [formData, setFormData] = useState({
    username: '',
    password: '',
  })

  const [errors, setErrors] = useState({
    username:'',
    password:'',
    submit:'',
  })

  const navigate = useNavigate();

  const [serverMessage, setServerMessage] = useState ('');

  const handleSubmit = async e => {
    e.preventDefault();

    try {
      let response = await fetch("http://localhost:5147/user/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON. stringify(formData)
    });

    const data = await response.json();

    if(response.ok) {
      setServerMessage('We in boys');
      

      TokenUtil.setTokens(data);
      navigate('/');
    } else{
      setServerMessage(data.error || "Authentication failed");
      setErrors({...errors, submit: "Username and/or password do not match system"});
    }
      
    } catch(error) {
      console.error('Fetch error',error);
      setServerMessage('Network error, try again later');
    }

  }

  //onChange
  const handleChange = (e) => {
    const {name, value} = e.target;

    setFormData({...formData, [name]: value});

    setErrors({...errors, [name]: ''});
  };

  //onBlur
  const handleBlur = (e) => {
    const {name, value} = e.target;
    let error = '';

    if(name === 'username') {
      error = ValidationUtil.validateUsername(value);
    } else if (name === 'password'){
      error = ValidationUtil.validatePassword(value);
    }

    setErrors({...errors, [name]: error});
  };




  return(
    <div id="login-wrapper">
      <form id="login-form"onSubmit={handleSubmit} >
        <h2>Login</h2>
        <Input
        label="Username"
        name="username"
        type="text"
        value={formData.username}
        onChange={handleChange}
        onBlur={handleBlur}
        placeholder="username"
        required
        error={errors.username}
        />
        <Input
        label="Password"
        type="password"
        name="password"
        value={formData.password}
        onChange={handleChange}
        onBlur={handleBlur}
        placeholder="password"
        required
        error={errors.password}
        />
        <button type="submit" disabled={!!errors.username || !!errors.password}>
          Login
        </button>
      </form>
    </div>
  )
}