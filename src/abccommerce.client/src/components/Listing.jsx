import './Listing.css';
import TagList from './TagList';
import ImageScroller from './ImageScroller';
function Listing({listing}) {
    const listingDate = new Date(listing.listingDate)
    
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

    function getDollarString(price) {
        return '$' + Number.parseFloat(price).toFixed(2);
    }
    
    return <li className="listing-wrapper" key={listing.id}>
        <a href={`/seller/${listing.item.seller.id}/listing/${listing.id}`} className="listing-redirect"></a>
        <div className="listing">
        {/* image would go first */}
        <div className="listing-image-wrapper">
            <ImageScroller images={listing.images} />
        </div>
            <div className="listing-details-wrapper">
                <p className="listing-price">{getDollarString(listing.pricePerUnit)}</p>
                <p className="listing-quantity">{listing.quantity.toLocaleString()}</p>
                <div className="listing-top-info">
                    <p className="listing-name">{listing.name}</p>
                    <a className="listing-seller-name" 
                        href={listing.item.seller.id ? 
                        `/seller/${listing.item.seller.id}` : "#"}>
                        {listing.item.seller.name ?? "Unknown Seller"}
                    </a>
                </div>
                
                    <div className="listing-bottom-info">
                        <div className="listing-tags">
                            <TagList tags={listing.tags} maxTags={2} maxTagWidth="4rem" />
                        </div>
                        {getDateElement(listingDate)}
                    </div>
            </div>
        </div>
    </li>;
}

export default Listing;