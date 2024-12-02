
import { describe, it, expect } from 'vitest';
import { ValidationUtil } from '../src/util/validation';

describe('ValidationUtil class tests', () => {
  describe('validateUserName', ( ()=>{
    it('should return a error if username is empty', () => {
      const result = ValidationUtil.validateUsername('');
      expect(result).toBe('Username is required');

    })
    it('should not return error if username is not empty', () => {
      const result = ValidationUtil.validateUsername('username');
      expect(result).toBe('');

    })
  }))
  describe('validateRegistrationPassword', () => {
    it('should return isValid true and no errors for a strong password', () => {
      const result = ValidationUtil.validateRegistrationPassword('StrongPass1!');
      expect(result.isValid).toBe(true);
      expect(result.errors).toHaveLength(0);
    });

    it('will return errors if password is too short', () => {
      const result = ValidationUtil.validateRegistrationPassword('Short1!');
      expect(result.isValid).toBe(false);
      expect(result.errors).toContain('Password must be at least 8 characters.');
    });

    it('will return errors if password is too long', () => {
      const longPassword = 'ThisPasswordIsWayTooLongToBeValid123!';
      const result = ValidationUtil.validateRegistrationPassword(longPassword);
      expect(result.isValid).toBe(false);
      expect(result.errors).toContain('Password must be no more than 20 characters.');
    });

    it('will return errors if password lacks uppercase letters', () => {
      const result = ValidationUtil.validateRegistrationPassword('lowercase1!');
      expect(result.isValid).toBe(false);
      expect(result.errors).toContain('Password must include at least one uppercase character.');
    });

    it('will return errors if password lacks numbers', () => {
      const result = ValidationUtil.validateRegistrationPassword('NoNumbers!');
      expect(result.isValid).toBe(false);
      expect(result.errors).toContain('Password must include at least one number.');
    });

    it('will return multiple errors for multiple validation failures', () => {
      const result = ValidationUtil.validateRegistrationPassword('weakp');
      expect(result.isValid).toBe(false);
      expect(result.errors).toContain('Password must be at least 8 characters.');
      expect(result.errors).toContain('Password must include at least one uppercase character.');
      expect(result.errors).toContain('Password must include at least one number.'); 
    });
  });
})