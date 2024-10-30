import {useParams} from 'react-router-dom';
import { useEffect, useState } from 'react';
import './Seller.css';
import Listing from '../components/Listing';
import PageSelector from '../components/PageSelector';
function Seller() {
    // const location = useLocation();
    const [seller, setSeller] = useState();
    const [listings, setListings] = useState();
    const [pageNumber, setPageNumber] = useState(1);
    const { sellerId } = useParams();
    const listingsPerPage = 24;

    useEffect(() => {
        getPageData(pageNumber, listingsPerPage)
    }, [pageNumber, seller])

    function handlePageChange(page) {
        if(listings?.length === 0 && page > pageNumber) return;
        setPageNumber(page);
    }

    let contents = 
    <div id="seller-wrapper">
        {seller && !seller.error ?
        <>
        <div id="seller-name-wrapper">
            <h1 id="seller-name">{seller.exists ? seller.name : "Seller not found"}</h1>
        </div>
        <div className="listings-wrapper">
            {listings?.length > 0 ? 
            <ul className="listings">
                {listings.map(l => 
                    <Listing listing={l} key={l.id} />
                )}
            </ul>
            : seller.exists && listings?.loading ?
            <p id="no-listings">Loading</p>
            : seller.exists && pageNumber === 1 ?
            <p id="no-listings">This seller has no listings</p>
            : seller.exists && pageNumber > 1 ?
            <p id="no-listings">No items on this page</p>
            :
            <></>
            }
        </div>
        {
        seller.exists && listings && (listings.length > 0 || pageNumber > 1) &&
          <PageSelector width={160} height={40} handlePageChange={handlePageChange} page={pageNumber}/>
        }
        </>
        : seller?.error ?
        <p>An error occured</p>
        :
        <p>Loading</p>
        }
        
        
    </div>
    
    return contents;


    async function getSellerListingData(page, listingsPerPage) {
        try {
            setListings({
                loading: true
            })
            let response = await fetch(`http://localhost:5147/seller/${sellerId}/listings?skip=${(page - 1) * listingsPerPage}&count=${listingsPerPage}`);
            if(!response.ok) {
                return;
            }
            let listingData = await response.json();
            listingData = listingData.map(l => {
                l.item.seller = {
                    id: seller?.id,
                    name: seller?.name,
                    image: seller?.image
                }
                return l;
            })

            setListings(listingData)
        }
        catch(error) {
            setListings({
                error: true
            })
        }
    }
    
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
                
                setSeller({
                    exists: true,
                    ...sellerData
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

    function getPageData(page, listingsPerPage) {
        if(seller === undefined) {
            // seller not instantiated, make fetch
            getSellerData();
        } else if(seller?.exists) {
            getSellerListingData(page, listingsPerPage);
        }
    }

}



export default Seller;