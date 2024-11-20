/**
 * @module paging
 */

/**
 * Provides basic logic for pagination and protection against 
 * page numbers less than 1. 
 * @function
 * @author Thomas Scott, Angel Cortes
 * @since November 8
 * @param {Number} newPage - Proposed new page to switch to
 * @param {Number} currentPage - Current page
 * @param {Array} list - List of items on the current page 
 * @param {Function} set - Callback function containing more specific
 * logic for paging
 */
export function handlePageChange(newPage, currentPage, list, set) {
    if(list?.length === 0 && newPage > currentPage) return;
    set(newPage);
}