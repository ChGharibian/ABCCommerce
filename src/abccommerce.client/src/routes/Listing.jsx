function Listing(props) {
    return <li className="listing-wrapper" key={props.listing.id}>
        <div className="listing">
        {/* image would go first */}
        <div className="listing-image-wrapper">
            <img className="listing-image" src="http://localhost:5147/images/calculator-fill.svg" />
        </div>
            <div className="listing-details-wrapper">
                <p className="listing-price">{'$' + props.listing.pricePerUnit}</p>
                <p className="listing-quantity">{props.listing.quantity}</p>
                <p className="listing-name">{props.listing.name}</p>
                    <div className="listing-tags-date-wrapper">
                        <div className="listing-tags">
                            {getTagElements(props.listing.tags, props.listing.id)}
                        </div>
                        {getDateElement(props.listing.date)}
                    </div>
            </div>
        </div>
    </li>;

    function getTagElements(tagList, lid) {
        return tagList.length > 2 ?
        <>
            <p className="listing-tag small-text">{tagList[0]}</p>
            <p className="listing-tag small-text">{tagList[1]}</p>
            <p className="small-text">{' + ' + (tagList.length - 2 > 4 ? '' : tagList.length - 2) + ' more'}</p>
            
        </>
        : tagList.length > 0 ?
            tagList.map(t => <p key={t + lid} className="listing-tag small-text">{t}</p>) 
        :
            <></>
        
    }

    function monthsSince(date) {
        let currentDate = new Date();
        let yearOffset = (currentDate.getFullYear() - date.getFullYear()) * 12;
        let monthOffset = currentDate.getMonth() - date.getMonth();
        let dayOffset = currentDate.getDate() >= date.getDate() ? 0 : -1;

        return yearOffset + monthOffset + dayOffset;
    }

    function getDateElement(date) {
        let months = monthsSince(date);
        let yearsAgo = Math.floor(months / 12);
        return <p className="listing-date small-text">
            {
                months >= 12 ?
                (yearsAgo !== 1 ? yearsAgo + ' years ago' : yearsAgo + ' year ago')
                : months >= 1 ?
                (months !== 1 ? months + ' months ago' : months + ' month ago')
                :
                date.toDateString().slice(3) + ' ' + 
                date.toLocaleTimeString('en-US').slice(0, -6) + ' ' + 
                date.toLocaleTimeString('en-US').slice(-2)
            }
        </p>
    } 
}

export default Listing;