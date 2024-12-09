import './ListingPage.css';
import TagList from '../components/TagList';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useCookies } from 'react-cookie';
import ImageList from '../components/ImageList';
import Input from '../components/Input';
import { DateUtil } from '../util/date';
import { CurrencyUtil } from '../util/currency';
import { TokenUtil } from '../util/tokens';
/**
 * @category route
 * @function ListingPage
 * @author Thomas Scott
 * @since October 30
 * @description Displays a single listing and its details, allowing a user to add the listing to their cart.
 * @returns {JSX.Element} Listing page
 */
export default function ListingPage() {
    const [listing, setListing] = useState();
    const [quantity, setQuantity] = useState(1);
    const [addedQuantity, setAddedQuantity] = useState(0);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState(false);
    const [refreshed, setRefreshed] = useState(false);
    const {sellerId, listingId} = useParams();
    const [cookies] = useCookies(['userToken', 'refreshToken', 'seller']);
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
      setSuccess(false);
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
          setSuccess(true);
          setAddedQuantity(quantity);
        } else if(response.status === 401 && !refreshed) {
          // unauthorized
          let refSuccess = await TokenUtil.refresh(cookies.refreshToken);
          console.log(refSuccess);
          setRefreshed(true);
          if(refSuccess) {
            setError('Try again');
          } else {
            setError('Please log in to add to cart');
          }
        } else if(response.status === 401) {
          setError('Please log in to add to cart');
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
              {listing.name ? listing.name : <i>No name</i>} · {CurrencyUtil.getDollarString(listing.pricePerUnit)} · {listing.quantity} in stock
              {Number(cookies.seller) === Number(sellerId) && <> · <a href={`/seller/${sellerId}/editlisting/${listingId}`}>Edit</a></>}
              </div>
            <a className="listing-detail" href={`/seller/${listing.item.seller.id}`}>{listing.item.seller.name}</a>
            <p className="listing-detail">{'Posted ' + DateUtil.getDateText(new Date(listing.listingDate)).toLowerCase()}</p>
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
        {success &&
        <p className="success">{`You added ${addedQuantity} item${addedQuantity !== 1 ? 's' : ''} to cart`}</p>
        }
        
    </div>
    
    )
}