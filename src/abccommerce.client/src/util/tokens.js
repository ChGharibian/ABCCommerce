/**
 * @typedef {Object} TokenData
 * @property {String} token User token
 * @property {String} refreshToken Refresh token
 * @property {String} tokenType Type of token
 * @property {String} expirationDate Token expiration date
 */

/**
 * @module tokens
 * 
 */

/**
 * Provides utility functions for tokens.
 * @class TokenUtil
 */
export class TokenUtil {
    /**
         * Sets the userToken cookie with the appropriate details specified
         * by the input tokenData object.
         * @async
         * @static
         * @function
         * @author Thomas Scott, Angel Cortes
         * @since November 16
         * @param {TokenData} tokenData General token data
         */
    static async setUserToken(tokenData) {
        let expDate = new Date(tokenData.expirationDate);
        document.cookie = `userToken=${tokenData.token}; expires=${expDate}; path=/`;
    } 

    /**
     * Sets the refreshToken cookie with the appropriate details specified
     * by the input tokenData object.
     * @async
     * @static
     * @function
     * @author Thomas Scott, Angel Cortes
     * @since November 16
     * @param {TokenData} tokenData General token data
     */
    static async setRefreshToken(tokenData) {
        const refreshTokenMonths = 6;
        let expDate = new Date(Date.now() + refreshTokenMonths * 30 * 24 * 60 * 60 * 1000);
        document.cookie = `refreshToken=${tokenData.refreshToken}; expires=${expDate}; path=/`;
    }


    /**
     * Sets both userToken and refreshToken cookies with appropriate details 
     * specified by the input tokenData object.
     * @async
     * @static
     * @function
     * @author Thomas Scott, Angel Cortes
     * @since November 16
     * @param {TokenData} tokenData General token data
     */
    static async setTokens(tokenData) {
        this.setUserToken(tokenData);
        this.setRefreshToken(tokenData);
    }

    /**
     * Attempts to refresh the userToken cookie using the refreshToken cookie.
     * @async
     * @static
     * @function
     * @author Thomas Scott, Angel Cortes
     * @since November 16
     * @param {String} refreshToken Refresh token cookie string
     * @returns {Boolean} Boolean representing the success of 
     * refreshing the userToken cookie
     */
    static async refresh(refreshToken) {
        if(!refreshToken) return false;
        try {
            let response = await fetch('http://localhost:5147/user/refresh', {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    refreshToken
                })
            })

            if(!response.ok) return false;

            let tokenData = await response.json();
            this.setUserToken(tokenData);
            return true;
        }
        catch(error) {
            console.error(error);
            return false;
        }
    }
}
    
