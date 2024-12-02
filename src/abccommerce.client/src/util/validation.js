export class ValidationUtil {
    static validateFiles(typesAccepted, files) {
        for(const file in files) {
            if(!this.validateFile(typesAccepted, file)) return false;
        }
    }
    
    static validateFile(typesAccepted, file) {
        return typesAccepted.indexOf(file.type.slice(file.type.indexOf('/') + 1)) !== -1;
    }

    static validateUsername(username) {
        let error = '';
        if(!username.trim()) {
          error = 'Username is required';
        }
        return error;
    }

    static validatePassword(password) {
        let error = '';
        if(!password) {
          error = 'Password is required.';
        } else if (password.length < 10) {
          error = 'Password must be at least 10 characters long.';
        }
        return error;
    }
    
    static validateRegistrationPassword(password) {
      const minLength = 8;
      const maxLength = 20;
      
      const patterns = {
        uppercase: /.*[A-Z].*/,
        number: /.*[0-9].*/,
      };
      let errors = [];
    
      if (password.length < minLength) {
        errors.push(`Password must be at least ${minLength} characters.`);
      }

      if (password.length > maxLength) {
        errors.push(`Password must be no more than ${maxLength} characters.`);
      
      }
      if(!patterns.uppercase.test(password)) {
        errors.push('Password must include at least one uppercase character.');
      }

      if(!patterns.number.test(password)) {
        errors.push('Password must include at least one number.');
      }
      
      return {
        isValid: errors.length === 0,
        errors
      };
    }
    
}
