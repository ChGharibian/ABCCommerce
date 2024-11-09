import './Listing.css';
import TagList from './TagList';
import ImageScroller from './ImageScroller';
import { getDateText } from '../util/date';
import { getDollarString } from '../util/currency';
function Listing({listing}) {
    const listingDate = new Date(listing.listingDate)
    
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
                        <p className="listing-date small-text">{getDateText(listingDate)}</p>
                    </div>
            </div>
        </div>
    </li>;
}

export default Listing;