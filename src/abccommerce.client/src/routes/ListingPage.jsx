import './ListingPage.css';
import TagList from '../components/TagList';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useCookies } from 'react-cookie';
import ImageList from '../components/ImageList';
import Input from '../components/Input';
import { getDateText } from '../util/date';
import { getDollarString } from '../util/currency';
export default function ListingPage() {
    const [listing, setListing] = useState();
    const [quantity, setQuantity] = useState(1);
    const [error, setError] = useState('');
    const {sellerId, listingId} = useParams();
    const [cookies ,setCookie,] = useCookies(['userToken', 'refreshToken']);
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

    async function addToCart() {
      // refresh token method here

      try {
        let response = await fetch("http://localhost:5147/cart", {
          method: "POST",
          headers: {
            "Authorization": "Bearer " + cookies.userToken,
            "Content-Type": "application/json"
          },
          body: JSON.stringify({
            listingId: listing.id,
            quantity
          })
        })
        
        if(response.ok) {
          setError('');
          console.log(await response.json());
        } else if(response.status === 401) {
          // unauthorized
          setError('Log in to add items to cart');
        } else {
          // other issue
          setError('Something went wrong, try again later');
        }
      }
      catch(error) {
        setError('Something went wrong, try again later');
        console.error(error);
      }
      
    }

    function handleQuantityChange(e) {
      e.preventDefault();
      if(e.target.value >= 1 && e.target.value <= listing.quantity) setQuantity(e.target.value);
    }

    return (
    <div id="listing-page-wrapper">
        {listing ?
        <>
        <div id="listing-page-image-section">
          <div>
              <ImageList images={listing.images}/>
          </div>
        </div>
        <div id="listing-page-details-section">
            <div id="listing-page-top-info" className="listing-detail">
              {listing.name ? listing.name : <i>No name</i>} · {getDollarString(listing.pricePerUnit)} · {listing.quantity} available
              </div>
            <a className="listing-detail" href={`/seller/${listing.item.seller.id}`}>{listing.item.seller.name}</a>
            <p className="listing-detail">{'Posted ' + getDateText(new Date(listing.listingDate)).toLowerCase()}</p>
            <p className="listing-detail">{listing.description}</p>
            <div className="listing-detail">
                <TagList tags={listing.tags} maxTags={35} maxTagWidth="6rem" fontSize="1rem" />
            </div>
        </div>
        <Input value={quantity} type="number" placeholder="Quantity" style={{width: "8rem", marginLeft: "1.25rem", marginTop: "5px"}} 
          onChange={handleQuantityChange}/>
        <button onClick={addToCart}>Add to Cart</button>
        {error &&
        <p className="error">{error}</p>
        }
        </>
        :
        <p>Loading</p>
        }
        
    </div>
    
    )
}