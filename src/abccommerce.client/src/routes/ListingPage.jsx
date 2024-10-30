import './ListingPage.css';
import ImageScroller from '../components/ImageScroller';
import TagList from '../components/TagList';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

const fakeListing = {
    "id": 0,
    "name": "Random item",
    "item": {
      "id": 0,
      "name": "string",
      "sku": "string",
      "seller": {
        "id": 0,
        "name": "Random Item Guy",
        "image": "string"
      }
    },
    "listingDate": "2024-10-29T00:22:52.248Z",
    "quantity": 3,
    "pricePerUnit": 13.32,
    "description": "This is a very random item This is a very random item This is a very random item This is a very random item This is a very random item This is a very random item This is a very random item This is a very random item This is a very random item This is a very random item This is a very random item This is a very random item",
    "active": true,
    "images": [
      "http://localhost:5147/images/airplane.svg",
      "http://localhost:5147/images/bank2.svg"
    ],
    "tags": [
      "tag",
      "tsgadsfljasdlfja",
      "sfjsdlfjs",
      "lsjfls",
      "sfjslldfusldfisfskjdf",
      "tag",
      "tsgadsfljasdlfja",
      "sfjsdlfjs",
      "lsjfls",
      "sfjslldfusldfisfskjdf",
      "tag",
      "tsgadsfljasdlfja",
      "sfjsdlfjs",
      "lsjfls",
      "sfjslldfusldfisfskjdf",
      "tag",
      "tsgadsfljasdlfja",
      "sfjsdlfjs",
      "lsjfls",
      "sfjslldfusldfisfskjdf",
      "tag",
      "tsgadsfljasdlfja",
      "sfjsdlfjs",
      "lsjfls",
      "sfjslldfusldfisfskjdf",
      "tag",
      "tsgadsfljasdlfja",
      "sfjsdlfjs",
      "lsjfls",
      "sfjslldfusldfisfskjdf",
      "tag",
      "tsgadsfljasdlfja",
      "sfjsdlfjs",
      "lsjfls",
      "sfjslldfusldfisfskjdf",
    ]
  }

export default function ListingPage() {
    const [listing, setListing] = useState();
    const {sellerId, listingId} = useParams();
    useEffect(() => {
        getListing();
    }, [])

    async function getListing() {
        try {
          let response = await fetch(`http://localhost:5147/listing/${listingId}`);
          let listingData = await response.json();
          
          response = await fetch(`http://localhost:5147/seller/${sellerId}`)
          let sellerData = await response.json()

          listingData.item.seller = {
            id: sellerData.id,
            name: sellerData.name,
            image: sellerData.image
          }
          setListing(listingData);
        }
        catch(error) {
          console.error(error);
        }
    }

    function getDollarString(price) {
        return '$' + Number.parseFloat(price).toFixed(2);
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
      return <p className="listing-detail">
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

    return (
    <div id="listing-page-wrapper">
        {listing ?
        <>
        <div id="listing-page-image-section">
            <ImageScroller images={listing.images}/>
        </div>
        <div id="listing-page-details-section">
            <div id="listing-page-top-info" className="listing-detail">{listing.name} · {getDollarString(listing.pricePerUnit)} · {listing.quantity} available</div>
            <a className="listing-detail" href={`/seller/${listing.item.seller.id}`}>{listing.item.seller.name}</a>
            {getDateElement(new Date(listing.listingDate))}
            <p className="listing-detail">{listing.description}</p>
            <div className="listing-detail">
                <TagList tags={listing.tags} maxTags={35} maxTagWidth="4rem" />
            </div>
        </div>
        <button>Add to Cart</button>
        </>
        :
        <p>Loading</p>
        }
        
    </div>
    
    )
}