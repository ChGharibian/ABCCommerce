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

  const handleSubmit = (e) => {
    e.preventDefault();

    const usernameError = validateUsername(formData.username);
    const passwordError = validatePassword(formData.password);
    setErrors({
      username: usernameError,
      password: passwordError,
    });

    if(!usernameError && !passwordError) {

      console.log('Form submitteed! this is the data', formData);
    }
  }



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