import { useState, useEffect } from 'react';
import { useCookies } from 'react-cookie';
import './Registration.css';
import DropdownList from '../components/DropdownList';
import { setTokens } from '../util/tokens';
export default function Registration() {
  const [states, setStates] = useState([]);
  useEffect(() => {
    getStateData();
  }, [])

  const refreshTokenMonths = 6;

  const handleSubmit = async e => {
    e.preventDefault();
    const formData = new FormData(e.currentTarget);
    let formObject = {};
    formData.forEach((value, key) => formObject[key] = value)

    try {
      let response = await fetch("http://localhost:5147/user/register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(formObject)
      });
  
      let data = await response.json();
      setTokens(data);
    } catch(error) {
      console.error(error);
    }
    
  }

  return(
    <div id="registration-wrapper">
      <form id="registration-form" onSubmit={handleSubmit} autoComplete="off">
        <h1>Create an Account</h1> 
        <div id="registration-fields">
          <div className="registration-field">
            <label></label>
            <input className="registration-field-input" required type="email" placeholder="Email" name="Email" />
          </div>
          <div className="registration-field">
            <label></label>
            <input className="registration-field-input" placeholder="Username" name="Username" />
          </div>
          <div className="registration-field">
            <label></label>
            <input className="registration-field-input" required type="password" placeholder="Password" name="Password" />
          </div>
          <div className="registration-field">
            <label></label>
            <input className="registration-field-input" required placeholder="Address Line 1" name="Street" />
          </div>
          <div className="registration-field">
            <label></label>
            <input className="registration-field-input" placeholder="Address Line 2" name="StreetPlus" />
          </div>
          <div className="registration-field">
            <label></label>
            <input className="registration-field-input" required placeholder="City" name="City" />
          </div>
          <div className="registration-field">
            <DropdownList placeholder="State" list={states} name="State" width="40%" required={true} />
          </div>
          <div className="registration-field">
            <label></label>
            <input className="registration-field-input" required placeholder="Zip or Postal Code" name="Zip" />
          </div>
          <div className="registration-field">
            <input required className="register-button" type="submit" value="Create Account" />
          </div>
        </div>
      </form>
    </div>
  )

  async function getStateData() {
    let response = await fetch('/src/assets/states.json');
    setStates(await response.json());
  }
}