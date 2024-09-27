import {useLocation} from 'react-router-dom';
import { useEffect, useState } from 'react';
import './Seller.css';
function Seller() {
    // const location = useLocation();
    const [seller, setSeller] = useState();
    
    useEffect(() => {
        getSellerData();
    }, [])
    const fakeSeller = {
        name: "Bob",
        listings: [
            {
                id: 0,
                name: "Rare signed basketball",
                description: "basketball signed by {insert basketball player}",
                pricePerUnit: 505.23,
                quantity: 1,
                tags: ["rare", "vintage"],
                date: new Date(2024, 4, 24, 18, 24)
            },
            {
                id: 1,
                name: "random item",
                description: "this item is random or something",
                pricePerUnit: 12.69,
                quantity: 7,
                tags: ["unused", "real", "tag", "tag2", "tagggg"],
                date: new Date(2024, 11, 2, 1, 3)
            },
            {
                id: 2,
                name: "more random item with a very long name",
                description: "this item is even more random than the last",
                pricePerUnit: 1.35,
                quantity: 10,
                tags: ["tag123", "tag321311"],
                date: new Date(2024, 7, 17, 12, 52)
            },
            {
                id: 3,
                name: "no tags",
                description: "tagsss",
                pricePerUnit: 235235,
                quantity: 1,
                tags: [],
                date: new Date(2024, 7, 17, 12, 52)
            }
        ]
    }


    let contents = seller ?
    <div id="seller-wrapper">
        <h1 id="seller-name">{seller.name}</h1>
        <ul className="seller-listings">
            {seller.listings.map(l => 
                <Listing listing={l} key={l.id}/>
            )}
        </ul>
    </div>
    :
    <p>loading</p>
    return contents;

    async function getSellerData() {
        // implement backend call later
        //const sellerId = location.pathname.split('/seller/')[1];
        // let response = await fetch(`http://localhost:5147/Seller/${sellerId}`);
        // let data = await response.json();
        setSeller(fakeSeller);

    }

    
}

function Listing(props) {
    

    return <li className="listing" key={props.listing.id}>
        {/* image would go first */}
        <div className="listing-image-wrapper">image here</div>
        <div className="listing-details-wrapper">
            <p className="listing-price">{'$' + props.listing.pricePerUnit}</p>
            <p className="listing-quantity">{props.listing.quantity}</p>
            <p className="listing-name">{props.listing.name}</p>
                <div className="listing-tags-date-wrapper">
                    <div className="listing-tags">
                        {getTagItems(props.listing.tags, props.listing.id)}
                    </div>
                    <p className="listing-date small-text">
                        {
                        props.listing.date.toDateString().slice(3) + ' ' + 
                        props.listing.date.toLocaleTimeString('en-US').slice(0, -6) + ' ' + 
                        props.listing.date.toLocaleTimeString('en-US').slice(-2)
                        }
                    </p>
                </div>
        </div>
    </li>;

    function getTagItems(tagList, lid) {
        return tagList.length > 2 ?
        <>
            <p className="listing-tag small-text">{tagList[0]}</p>
            <p className="listing-tag small-text">{tagList[1]}</p>
            <p className="small-text">{' + ' + (tagList.length - 2) + ' more'}</p>
            
        </>
        : tagList.length > 0 ?
            tagList.map(t => <p key={t + lid} className="listing-tag small-text">{t}</p>) 
        :
            <></>
        
    }
}



export default Seller;