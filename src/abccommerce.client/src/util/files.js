/**
 * @module files
 */

/**
 * Provides utility functions for files.
 * @class FileUtil
 */
export class FileUtil {
    /**
     * Returns a base64 encoded string of a file.
     * @function
     * @author Thomas Scott
     * @since December 1
     * @param {File} file 
     * @returns {Promise} Promise resolving to base64 encoded file on success or an error on fail.
     */
    static toBase64(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader(file);
            reader.addEventListener('load', (e) => {
                let b64 = e.target.result;
                resolve(b64.slice(b64.indexOf(',') + 1))
            })

            reader.addEventListener('error', (error) => {
                reject(error);
            })

            if(file) reader.readAsDataURL(file);
        })
        
    }

    static isValidImgSrc(src) {
        return new Promise((resolve, reject) => {
            const img = new Image();
            img.src = src;

            img.onerror = function() {
                resolve(false)
            };

            img.onload = function() {
                resolve(true)
            };
        })
        
    }
}