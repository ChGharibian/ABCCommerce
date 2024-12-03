/**
 * @module date
 */

/**
 * Provides utility functions for dates.
 * @class DateUtil
 */
export class DateUtil {
    /**
     * Returns the amount of months that have passed since the given date.
     * @function
     * @static
     * @author Thomas Scott, Angel Cortes
     * @since November 8
     * @param {Date} date - Date to compare
     * @returns {Number} Months since given date
     */
    static monthsSince(date) {
        let currentDate = new Date();
        let yearOffset = (currentDate.getFullYear() - date.getFullYear()) * 12;
        let monthOffset = currentDate.getMonth() - date.getMonth();
        let dayOffset = currentDate.getDate() >= date.getDate() ? 0 : -1;

        return yearOffset + monthOffset + dayOffset;
    }

    /**
     * Converts a date into a string representing the time 
     * elapsed since the date given.
     * @function
     * @static
     * @author Thomas Scott, Angel Cortes
     * @since November 8
     * @param {Date} date - Date to convert
     * @returns {string} Time elapsed since date
     */
    static getDateText(date) {
        let ms = Date.now().valueOf() - date.valueOf();
        let seconds = Math.floor(ms / 1000);
        let years = Math.floor(seconds / 31540000);
        if(years > 0) {
            return years + (years > 1 ? ' years ago' : ' year ago');
        }
        let months = this.monthsSince(date);
        if(months > 0) {
            return months + (months > 1 ? ' months ago' : ' month ago');
        }
        let minutes = Math.floor(seconds / 60);
        seconds = seconds % 60;
        let hours = Math.floor(minutes / 60)
        minutes = minutes % 60;
        let days = Math.floor(hours / 24);
        hours = hours % 24;
        if(days > 0) {
            return days + (days > 1 ? ' days ago'  : ' day ago');
        }
        if(hours > 0) {
            return hours + (hours > 1 ? ' hours ago' : ' hour ago')
        }
        if(minutes > 0) {
            return minutes + (minutes > 1 ? ' minutes ago' : ' minute ago');
        }
        return 'Just now';
    }
}
