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
 * Sets the userToken cookie with the appropriate details specified
 * by the input tokenData object.
 * @function
 * @author Thomas Scott, Angel Cortes
 * @since November 16
 * @param {TokenData} tokenData General token data
 */
export async function setUserToken(tokenData) {
    let expDate = new Date(tokenData.expirationDate);
    document.cookie = `userToken=${tokenData.token}; expires=${expDate}; path=/`;
} 

/**
 * Sets the refreshToken cookie with the appropriate details specified
 * by the input tokenData object.
 * @function
 * @author Thomas Scott, Angel Cortes
 * @since November 16
 * @param {TokenData} tokenData General token data
 */
export async function setRefreshToken(tokenData) {
    const refreshTokenMonths = 6;
    let expDate = new Date(Date.now() + refreshTokenMonths * 30 * 24 * 60 * 60 * 1000);
    document.cookie = `refreshToken=${tokenData.refreshToken}; expires=${expDate}; path=/`;
}


/**
 * Sets both userToken and refreshToken cookies with appropriate details 
 * specified by the input tokenData object.
 * @function
 * @author Thomas Scott, Angel Cortes
 * @since November 16
 * @param {TokenData} tokenData General token data
 */
export async function setTokens(tokenData) {
    setUserToken(tokenData);
    setRefreshToken(tokenData);
}

/**
 * Attempts to refresh the userToken cookie using the refreshToken cookie.
 * @function
 * @author Thomas Scott, Angel Cortes
 * @since November 16
 * @param {string} refreshToken Refresh token cookie string
 * @returns {Boolean} Boolean representing the success of 
 * refreshing the userToken cookie
 */
export async function refresh(refreshToken) {
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
        setUserToken(tokenData);
        return true;
    }
    catch(error) {
        console.error(error);
        return false;
    }
}
