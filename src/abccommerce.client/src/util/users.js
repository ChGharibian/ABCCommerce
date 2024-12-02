/**
 * @module users 
 */

/**
 * Provies utility functions for handling users.
 * @class UserUtil
 */
export class UserUtil {
  /**
  * @async
  * @function 
  * @description Retrieves the sellers a user is associated with from the backend based on their userToken cookie.
  * @author Thomas Scott
  * @since November 19
  * @param {String} userToken userToken cookie string 
  * @returns {Array<SellerObject>|Boolean} Array of sellers or a false return 
  */
  static async getSellerData(userToken) {
    try {
      let response = await fetch("http://localhost:5147/user/sellers", {
        method: "GET",
        headers: {
          "Authorization": "Bearer " + userToken
        }
      })

      if(response.ok) {
        return await response.json();
      } else {
        return false;
      }
    }
    catch(error) {
      console.error(error);
      return false;
    }
  }
}
