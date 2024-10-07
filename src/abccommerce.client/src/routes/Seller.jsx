// import {useLocation} from 'react-router-dom';
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
                tags: ["rareeeeeeeeeeeeeee", "vintage"],
                date: new Date(2024, 7, 27, 18, 24)
            },
            {
                id: 1,
                name: "random item",
                description: "this item is random or something",
                pricePerUnit: 12.69,
                quantity: 7,
                tags: ["unusedklllllllllllllll", "real", "tag", "tag2", "tagggg", "tagtest", "again"],
                date: new Date(2023, 11, 2, 1, 3)
            },
            {
                id: 2,
                name: "more random item with a very long name",
                description: "this item is even more random than the last",
                pricePerUnit: 1.35,
                quantity: 10,
                tags: ["tag123", "tag321311"],
                date: new Date(2023, 7, 17, 12, 52)
            },
            {
                id: 3,
                name: "no tags",
                description: "tagsss",
                pricePerUnit: 235235,
                quantity: 1,
                tags: [],
                date: new Date(2024, 7, 17, 12, 52)
            },
            {
                id: 4,
                name: "Rare signed basketball",
                description: "basketball signed by {insert basketball player}",
                pricePerUnit: 505.23,
                quantity: 1,
                tags: ["rare", "vintage"],
                date: new Date(2024, 7, 27, 18, 24)
            },
            {
                id: 5,
                name: "random item",
                description: "this item is random or something",
                pricePerUnit: 12.69,
                quantity: 7,
                tags: ["unused", "real", "tag", "tag2", "tagggg"],
                date: new Date(2023, 11, 2, 1, 3)
            },
            {
                id: 6,
                name: "more random item with a very long name",
                description: "this item is even more random than the last",
                pricePerUnit: 1.35,
                quantity: 10,
                tags: ["tag123", "tag321311"],
                date: new Date(2023, 7, 17, 12, 52)
            },
            {
                id: 7,
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
        <div id="seller-name-wrapper">
            <h1 id="seller-name">{seller.name}</h1>
        </div>
        <div id="seller-listings-wrapper">
            <ul className="seller-listings">
                {seller.listings.map(l => 
                    <Listing listing={l} key={l.id}/>
                )}
            </ul>
        </div>
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
    

    return <li className="listing-wrapper" key={props.listing.id}>
        <div className="listing">
        {/* image would go first */}
        <div className="listing-image-wrapper">
            <img className="listing-image" src="http://localhost:5147/images/calculator-fill.svg" />
        </div>
            <div className="listing-details-wrapper">
                <p className="listing-price">{'$' + props.listing.pricePerUnit}</p>
                <p className="listing-quantity">{props.listing.quantity}</p>
                <p className="listing-name">{props.listing.name}</p>
                    <div className="listing-tags-date-wrapper">
                        <div className="listing-tags">
                            {getTagElements(props.listing.tags, props.listing.id)}
                        </div>
                        {getDateElement(props.listing.date)}
                    </div>
            </div>
        </div>
    </li>;

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
}



export default Seller;