/**
 * @module currency
 */

/**
 * Takes in a number and returns a string formatted to represent
 * a dollar amount.
 * @function 
 * @param {Number} price - Price value to be formatted
 * @returns {string} Dollar formatted price
 */
export function getDollarString(price) {
    return '$' + Number.parseFloat(price).toFixed(2);
}