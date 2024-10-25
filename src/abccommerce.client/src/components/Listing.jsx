import { useState } from 'react';
import './Listing.css';
function Listing({listing}) {
    const listingDate = new Date(listing.listingDate)
    const [currentImage, setCurrentImage] = useState(0);
    
    const scrollLeft = () => {
        if(currentImage === 0) {
            setCurrentImage(listing.images.length - 1);
        } else {
            setCurrentImage(currentImage - 1);
        }
    }

    const scrollRight = () => {
        if(currentImage === listing.images.length - 1) {
            setCurrentImage(0);
        } else {
            setCurrentImage(currentImage + 1);
        }
    }

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

    function getDollarString(price) {
        return '$' + Number.parseFloat(price).toFixed(2);
    }
    
    return <li className="listing-wrapper" key={listing.id}>
        <a href={`/seller/${listing.item.seller.id}/items/${listing.item.id}`} className="listing-redirect"></a>
        <div className="listing">
        {/* image would go first */}
        <div className="listing-image-wrapper">
            {listing.images.length > 1 ?
                <div className="listing-left-controls">
                    <div onClick={scrollLeft} className="arrow left"></div>
                </div>
            :
                <></>
            }
            {listing.images.length > 0 ?
                <img className="listing-image" src={listing.images[currentImage]} />
            :
                "No images"
            }
            {listing.images.length > 1 ?
                <div className="listing-right-controls">
                    <div onClick={scrollRight} className="arrow right"></div>
                </div>
            :
                <></>
            }
        </div>
            <div className="listing-details-wrapper">
                <p className="listing-price">{getDollarString(listing.pricePerUnit)}</p>
                <p className="listing-quantity">{listing.quantity.toLocaleString()}</p>
                <div className="listing-top-info">
                    <p className="listing-name">{listing.item.name}</p>
                    <a className="listing-seller-name" 
                        href={listing.item.seller.id ? 
                        `/seller/${listing.item.seller.id}` : "#"}>
                        {listing.item.seller.name ?? "Unknown Seller"}
                    </a>
                </div>
                
                    <div className="listing-bottom-info">
                        <div className="listing-tags">
                            {getTagElements(listing.tags, listing.id)}
                        </div>
                        {getDateElement(listingDate)}
                    </div>
            </div>
        </div>
    </li>;
}

export default Listing;