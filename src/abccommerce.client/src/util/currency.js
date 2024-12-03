/**
 * @module currency
 */

/**
 * Provides utility functions for currency
 * @class CurrencyUtil
 */
export class CurrencyUtil {
    /**
     * Takes in a number and returns a string formatted to represent
     * a dollar amount.
     * @function
     * @static 
     * @author Thomas Scott, Angel Cortes
     * @since November 8
     * @param {Number} price - Price value to be formatted
     * @returns {string} Dollar formatted price
     */
    static getDollarString(price) {
        let parsed = Number.parseFloat(price);
        if(isNaN(parsed)) return "Not a valid price";
        return '$' + parsed.toFixed(2);
    }
}