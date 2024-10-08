// import {useLocation} from 'react-router-dom';
import { useEffect, useState } from 'react';
import './Seller.css';
import Listing from './Listing';
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
        <div className="seller-listings-wrapper">
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
        // let response = await fetch(`http://localhost:5147/Seller/1`);
        // let data = await response.json();
        // console.log(data);
        setSeller(fakeSeller);

    }

    
}



export default Seller;