import React, { useState } from 'react';
import '../components/Input';
import Input from '../components/Input';
import './login.css';

export default function Login(){

  //set state
  const [formData, setFormData] = useState({
    username: '',
    password: '',
  })

  const [errors, setErrors] = useState({
    username:'',
    password:'',
  })
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
      console.log('dab on the haters', data);
      //redirect them to their user profile or listings page
    } else{
      setServerMessage(data.error || "Authentication failed");
      console.log('What did we do wrong when sending a request?')
    }
      
    } catch(error) {
      console.error('Fetch error',error);
    }

  }
  //validation
  const validateUsername = (username) => {
    let error = '';
    if(!username.trim()) {
      error = 'Username is required';
    }
    return error;
  };
  const validatePassword = (password) => {
    let error = '';
    if(!password) {
      error = 'Password is required.';
    } else if (password.length < 10) {
      error = 'Password must be at least 10 characters long.';
    }
    return error;
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
      error = validateUsername(value);
    } else if (name === 'password'){
      error = validatePassword(value);
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