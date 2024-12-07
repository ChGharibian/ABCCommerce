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

    static validateCreditCard(cardNumber) {
      if(!cardNumber){
        return 'Card information is required';
      }
      //checks that it is a valid length and that it follows starting card patterns
      const cardRegex = /^(?:4\d{12}(?:\d{3})?|5[1-5]\d{14}|3[47]\d{13}|3(?:0[0-5]|[68]\d)\d{11}|6(?:011|5\d{2})\d{12}|(?:2131|1800|35\d{3})\d{11})$/;

      // takes away spaces and dashes
      let sanitizedCardNumber = cardNumber.replace(/[\s-]/g, '');
      //checks if card is valid
      const isCardValid = cardRegex.test(sanitizedCardNumber);
      if(!isCardValid) {
        return 'You have not entered a valid Credit Card';
      }
      //return empty string
      return '';


    }
    static validateExpirDateOnChange(expirDate) {
      
      //if longer then 5 then return false
      if(expirDate.length > 5) {
        return false;
      }

      const cleanedValue = expirDate.replace(/\//g, '');
      if (cleanedValue.length === 0) {
        return true; // empty is still considered valid until more input
      }

      const lastChar = cleanedValue[cleanedValue.length - 1];
      let validNumber = Number(lastChar);

      if(Number.isNaN(validNumber)){
        return false
      }

      return true;

    }

    static validateExpirDateOnBlur(expirDate) {
        if(expirDate.length == 5) {
          const inputMonth = Number(expirDate.slice(0,2))
          const inputYear = Number(expirDate.slice(3,5));
          if(inputMonth >12 || inputYear < 24) {
            return 'Invalid Expiration Date check if month is valid or if card expired';
          }
        }
        return '';
      
    
    }
  }

    
    

