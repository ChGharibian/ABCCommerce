/**
 * @module arrays
 */


/**
 * Provides utility functions for arrays.
 * @class ArrayUtil
 */
export class ArrayUtil {
    /**
     * Returns an index that is in bounds of the input array based on
     * an input index that can have any value.
     * @function 
     * @author Thomas Scott, Angel Cortes
     * @since November 8
     * @param {Array} arr - Array to be indexed
     * @param {Number} i  - Index
     * @returns {Number} In bound index
     */
    static getInBoundIndex(arr, i) {
        // js modulo is actually a remainder
        if(i < 0) {
            return arr.length + (i % arr.length);
        }
        return i % arr.length;
    }  


    /**
     * Returns a subarray based on a flexible range. This range allows for looping past the end of the array.
     * @function
     * @author Thomas Scott, Angel Cortes
     * @since November 8
     * @param {Array} arr - Input array
     * @param {Array} range - Range array; the first element in the array is the starting point (inclusive)
     * , while the second is the end point (inclusive)
     * @returns {Array} Subarray
     */
    static getFromRange(arr, range) {
        let newArr = [];
        for(let i = range[0]; i !== range[1]; i++) {
            // loop back to start if i is out of array bound
            if(i >= arr.length) {
                if(range[1] === 0) break;
                i = 0;    
            }

            newArr.push(arr[i]);
        }
        if(arr[range[1]]) newArr.push(arr[range[1]]);
        return newArr;
    }
}
