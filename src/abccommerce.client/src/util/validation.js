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
}
