import './Listing.css';
import { useEffect, useState } from 'react';
import TagList from './TagList';
import ImageScroller from './ImageScroller';
import editSymbol from '../assets/edit-symbol.png';
import activateSymbol from '../assets/activate-symbol.png';
import { useCookies } from 'react-cookie';
import { DateUtil } from '../util/date';
import { CurrencyUtil } from '../util/currency';
/**
 * @typedef {Object} SellerObject
 * @property {Number} id Seller ID
 * @property {String} name Seller name
 * @property {String} image Seller image
 */

/** 
 * @typedef {Object} ItemObject
 * @property {Number} id Item ID
 * @property {String} name Item name
 * @property {String} sku Item SKU
 * @property {SellerObject} seller Seller that made the item
 */

/**
 * @typedef {Object} ListingObject
 * @property {Number} id Listing ID
 * @property {String} name Listing name
 * @property {ItemObject} item Item associated with listing
 * @property {String} listingDate Date listed
 * @property {Number} quantity Listing quantity
 * @property {Number} pricePerUnit Listing price
 * @property {String} description Listing description
 * @property {Boolean} active Active status of listing
 * @property {Array<String>} images Listing images
 * @property {Array<String>} tags Listing tags
 */

/**
 * @category component
 * @author Thomas Scott
 * @function Listing
 * @description Displays a listing's name, seller, tags, date, price, quantity, and images in a card style design.
 * @since October 7
 * @param {ListingObject} props
 * @returns {JSX.Element}
 */
function Listing({listing, editable=false}) {
    const listingDate = new Date(listing.listingDate)
    const [cookies] = useCookies(['seller', 'userToken']);
    const [active, setActive] = useState(true);
    const [listingEditStyle, setListingEditStyle] = useState({
        visibility: "hidden"
    })

    useEffect(() => {
        setActive(listing.active);
    }, [])

    async function activateListing() {
        try {
            let response = await fetch(`http://localhost:5147/seller/${listing.item.seller.id}/listings/${listing.id}`, {
                method: "PATCH",
                headers: {
                    "Authorization": "Bearer " + cookies.userToken,
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({active: true})
            })

            if(response.ok) setActive(true);
        }   
        catch(error) {
            console.error(error);
        }
    }

    return <li className="listing-wrapper" key={listing.id} onMouseOver={() => setListingEditStyle({
        visibility: "visible"
    })}
    onMouseOut={() => setListingEditStyle({
        visibility: "hidden"
    })}>
        <a href={active ? `/seller/${listing.item.seller.id}/listing/${listing.id}`
                : '#'} className="listing-redirect"></a>
        <div className="listing">
            {editable && active ?
                <div className="listing-edit-button-wrapper"
                style={listingEditStyle}>
                    <div>
                        <a href={`/seller/${cookies.seller}/editlisting/${listing.id}`}>
                            <img src={editSymbol} />
                        </a>
                    </div>
                </div>
            : editable &&
                <div className="listing-edit-button-wrapper"
                style={listingEditStyle}>
                    <div>
                        <a href='#' onClick={activateListing}>
                            <img className='activate' src={activateSymbol} />
                        </a>
                    </div>
                </div>
            }
            <div className={"listing-image-wrapper" + (!active ? " inactive" : "")}>
                <ImageScroller images={listing.images} />
            </div>
            <div className={"listing-details-wrapper" + (!active ? " inactive" : "")}>
                <p className="listing-price">{CurrencyUtil.getDollarString(listing.pricePerUnit)}</p>
                <p className="listing-quantity">{listing.quantity.toLocaleString()}</p>
                <div className="listing-top-info">
                    <p className="listing-name">{listing.name ? listing.name : <i>No name</i>}</p>
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
                        <p className="listing-date small-text">{DateUtil.getDateText(listingDate)}</p>
                    </div>
            </div>
        </div>
    </li>;
}

export default Listing;