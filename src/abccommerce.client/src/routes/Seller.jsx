import { useParams, useSearchParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { useCookies } from 'react-cookie';
import './Seller.css';
import Listing from '../components/Listing';
import PageSelector from '../components/PageSelector';
import { PagingUtil } from '../util/paging';
import plusImage from '../assets/plus-symbol.svg';
/**
 * @category route
 * @function Seller
 * @author Thomas Scott
 * @since September 26
 * @description Displays a specific seller's name and their inventory of listings.
 * @returns {JSX.Element} Seller page
 */
export default function Seller() {
    // const location = useLocation();
    const [seller, setSeller] = useState();
    const [listings, setListings] = useState();
    const [pageNumber, setPageNumber] = useState();
    const [canEdit, setCanEdit] = useState(false);
    const [cookies] = useCookies(['seller', 'userToken']);
    const { sellerId } = useParams();
    const [searchParams, setSearchParams] = useSearchParams();
    const listingsPerPage = 24;

    async function getSellerListingData(page, listingsPerPage) {
        if(!page || isNaN(Number(page)) || page < 1) {
            setPageNumber(1);
            return;
        } else if(Math.floor(Number(page)) !== Number(page)) {
            setPageNumber(Math.floor(Number(page)));
            return;
        }

        try {
            setListings({
                loading: true
            })
            let response;
            if(canEdit) {
                response = await fetch(`http://localhost:5147/seller/${sellerId}/listings`, {
                    method: "GET",
                    headers: {
                        "Authorization": "Bearer " + cookies.userToken
                    }
                });
                if(!response.ok) {
                    setCanEdit(false);
                    response = await fetch(`http://localhost:5147/listing?sellerId=${sellerId}&skip=${(page - 1) * listingsPerPage}&count=${listingsPerPage}`);
                }
            } else {
                response = await fetch(`http://localhost:5147/listing?sellerId=${sellerId}&skip=${(page - 1) * listingsPerPage}&count=${listingsPerPage}`);
            }
            
            if(!response.ok) {
                setListings({
                    error: true
                })
                return;
            }
            let listingData = await response.json();
            listingData = listingData.map(l => {
                l.item.seller = {
                    id: seller?.id || sellerId,
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

    useEffect(() => {
        setCanEdit(Number(cookies.seller) === Number(sellerId));
    }, [cookies.seller])

    useEffect(() => {
        setSearchParams({ page: pageNumber});
    }, [pageNumber])

    useEffect(() => {
        let pageParam = searchParams.get('page');
        if(seller && pageParam && Number(pageParam)) {
            getSellerListingData(Number(pageParam), listingsPerPage);
        }
    }, [searchParams, seller])

    useEffect(() => {
        // on mount
        setCanEdit(Number(cookies.seller) === Number(sellerId));
        getSellerData();
        let pageParam = searchParams.get('page');
        if(pageParam && Number(pageParam)) {
            setPageNumber(Number(pageParam));
        } else {
            setPageNumber(1);
        }
    }, [])
    
    return (
        <div id="seller-wrapper">
            {seller && !seller.error ?
            <>
            <div id="seller-name-wrapper">
                <h1 id="seller-name">{seller.exists ? seller.name : "Seller not found"}</h1>
                {canEdit && 
                <div>
                    <p>Add listing</p>
                    <a href={`/seller/${sellerId}/addlisting`}>
                        <img src={plusImage}></img>
                    </a>
                </div>
                }
            </div>
            <div className="listings-wrapper">
                {listings?.length > 0 ? 
                <ul className="listings">
                    {listings.map(l => 
                        <Listing listing={l} key={l.id} editable={Number(sellerId) === Number(cookies.seller)}/>
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
            seller.exists && listings && 
            (((listings.length > 0 && pageNumber !== 1) 
            || (listings.length === listingsPerPage && pageNumber === 1)) 
            || pageNumber > 1) &&
            <PageSelector width={160} height={40} 
            handlePageChange={(page) => PagingUtil.handlePageChange(page, pageNumber, listings, setPageNumber)} 
            page={pageNumber}/>
            }
            </>
            : seller?.error ?
            <p>An error occured</p>
            :
            <p>Loading</p>
            }
            
            
        </div>
    );
}