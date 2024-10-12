import { useState } from 'react';
import './Registration.css';
export default function Registration() {
  return(
    <div id="registration-wrapper">
      <form id="registration-form">
        <h1>Create an Account</h1> 
        <div id="registration-fields">
        <div class="registration-field">
            <label></label>
            <input required type="email" placeholder="Email" name="email" />
          </div>
          <div class="registration-field">
            <label></label>
            <input required type="password" placeholder="Password" name="password" />
          </div>
          <div class="registration-field">
            <label></label>
            <input required placeholder="Address Line 1" name="street" />
          </div>
          <div class="registration-field">
            <label></label>
            <input placeholder="Address Line 2" />
          </div>
          <div class="registration-field">
            <label></label>
            <input required placeholder="City" name="city" />
          </div>
          <div class="registration-field">
            <label></label>
            <input required placeholder="State" name="state" />
          </div>
          <div class="registration-field">
            <label></label>
            <input required placeholder="Zip or Postal Code" name="zip" />
          </div>
          <div class="registration-field">
            <input required className="register-button" type="submit" value="Create Account" />
          </div>
        </div>
      </form>
    </div>
  )
}