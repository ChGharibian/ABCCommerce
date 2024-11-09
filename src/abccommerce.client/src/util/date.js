export function monthsSince(date) {
    let currentDate = new Date();
    let yearOffset = (currentDate.getFullYear() - date.getFullYear()) * 12;
    let monthOffset = currentDate.getMonth() - date.getMonth();
    let dayOffset = currentDate.getDate() >= date.getDate() ? 0 : -1;

    return yearOffset + monthOffset + dayOffset;
}

export function getDateText(date) {
    let months = monthsSince(date);
    let yearsAgo = Math.floor(months / 12);
    return  months >= 12 ?
            (yearsAgo !== 1 ? yearsAgo + ' years ago' : yearsAgo + ' year ago')
            : months >= 1 ?
            (months !== 1 ? months + ' months ago' : months + ' month ago')
            :
            date.toDateString().slice(3) + ' ' + 
            date.toLocaleTimeString('en-US').slice(0, -6) + ' ' + 
            date.toLocaleTimeString('en-US').slice(-2)
        
    
}