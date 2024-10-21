import {useParams} from 'react-router-dom';
import { useEffect, useState } from 'react';
import './Seller.css';
import Listing from '../components/Listing';
function Seller() {
    // const location = useLocation();
    const [seller, setSeller] = useState();
    const { sellerId } = useParams();
    useEffect(() => {
        getSellerData();
    }, [])

    let contents = 
    <div id="seller-wrapper">
        {seller && !seller.error ?
        <>
        <div id="seller-name-wrapper">
            <h1 id="seller-name">{seller.exists ? seller.name : "Seller not found"}</h1>
        </div>
        <div className="listings-wrapper">
            {seller.listings?.length > 0 ? 
            <ul className="listings">
                {seller.listings.map(l => 
                    <Listing listing={l} key={l.id} />
                )}
            </ul>
            : seller.exists ?
            <p id="no-listings">This seller has no listings</p>
            :
            <></>
            }
        </div>
        </>
        : seller?.error ?
        <p>An error occured</p>
        :
        <p>Loading</p>
        }
        
        
    </div>
    
    return contents;

    async function getSellerData() {
        if(sellerId) {
            try {
                let response = await fetch(`http://localhost:5147/seller/${sellerId}`);
                if(!response.ok) {
                    setSeller({
                        exists: false
                    })
                    return;
                }
                let sellerData = await response.json();
                
                response = await fetch(`http://localhost:5147/listing/${sellerId}`);
                if(!response.ok) {
                    setSeller({
                        exists: false
                    })
                    return;
                }
                let listingData = await response.json();
                listingData = listingData.map(l => ({
                    ...l,
                    sellerName: sellerData.name 
                }))
                
        
                // console.log(sellerData,listingData);
                
                setSeller({
                    exists: true,
                    id: sellerData.id,
                    name: sellerData.name,
                    image: sellerData.image,
                    listings: listingData
                });
            } 
            catch(error) {
                console.error(error);
                setSeller({
                    exists: false,
                    error: true
                })
            }
            
        } else {
            setSeller({
                exists: false
            })
        }
        

    }

}



export default Seller;