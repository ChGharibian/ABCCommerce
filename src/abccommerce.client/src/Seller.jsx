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
                name: "Rare signed basketball",
                description: "basketball signed by {insert basketball player}",
                pricePerUnit: 505.23,
                quantity: 1,
                tags: ["rare", "vintage"],
                date: new Date(2024, 4, 24, 18, 24)
            },
            {
                name: "random item",
                description: "this item is random or something",
                pricePerUnit: 12.69,
                quantity: 7,
                tags: ["unused", "real", "tag", "tag2", "tagggg"],
                date: new Date(2024, 11, 2, 1, 3)
            },
            {
                name: "more random item with a very long name",
                description: "this item is even more random than the last",
                pricePerUnit: 1.35,
                quantity: 10,
                tags: ["tag123", "tag321311"],
                date: new Date(2024, 7, 17, 12, 52)
            }
        ]
    }


    let contents = seller ?
    <div id="seller-wrapper">
        <h1 id="seller-name">{seller.name}</h1>
        <ul class="seller-listings">
            {seller.listings.map(l => 
                <li className="listing">
                    {/* image would go first */}
                    <div class="listing-image-wrapper">image here</div>
                    <div class="listing-details-wrapper">
                        <p class="listing-price">{'$' + l.pricePerUnit}</p>
                        <p class="listing-quantity">{l.quantity}</p>
                        <p class="listing-name">{l.name}</p>
                            <div class="listing-tags-date-wrapper">
                                <div class="listing-tags">
                                    {getTagItems(l.tags)}
                                </div>
                                <p class="listing-date small-text">
                                    {
                                    l.date.toDateString().slice(3) + ' ' + 
                                    l.date.toLocaleTimeString('en-US').slice(0, -6) + ' ' + 
                                    l.date.toLocaleTimeString('en-US').slice(-2)
                                    }
                                </p>
                            </div>
                    </div>
                </li>
            )}
        </ul>
    </div>
    :
    <p>loading</p>
    return contents;

    function getSellerData() {
        // implement backend call later
        //const sellerId = location.pathname.split('/seller/')[1];
        setSeller(fakeSeller);

    }

    function getTagItems(tagList) {
        return tagList.length <= 2 ?
        tagList.map(t => <p class="listing-tag small-text">{t}</p>) 
        : <>
            <p class="listing-tag small-text">{tagList[0]}</p>
            <p class="listing-tag small-text">{tagList[1]}</p>
            <p class="small-text">{' + ' + (tagList.length - 2) + ' more'}</p>
            
        </>
    }
}

export default Seller;