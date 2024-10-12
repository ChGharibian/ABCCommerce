import './Home.css';
import { useEffect, useState } from 'react';
import Listing from '../components/Listing';
const fakeListings = [
      {
          id: 0,
          item: {
            name: "Rare signed basketball",
          },
          description: "basketball signed by {insert basketball player}",
          pricePerUnit: 505.23,
          quantity: 1,
          tags: ["rareeeeeeeeeeeeeee", "vintage"],
          listingDate: new Date(2024, 7, 27, 18, 24).toDateString()
      },
      {
          id: 1,
          item: {
            name: "random item",
          },
          description: "this item is random or something",
          pricePerUnit: 12.69,
          quantity: 7,
          tags: ["unusedklllllllllllllll", "real", "tag", "tag2", "tagggg", "tagtest", "again"],
          listingDate: new Date(2023, 11, 2, 1, 3).toDateString()
      },
      {
          id: 2,
          item: {
            name: "more random item with a very long name",
          },
          description: "this item is even more random than the last",
          pricePerUnit: 1.35,
          quantity: 10,
          tags: ["tag123", "tag321311"],
          listingDate: new Date(2023, 7, 17, 12, 52).toDateString()
      },
      {
          id: 3,
          item: {
            name: "no tags",
          },
          description: "tagsss",
          pricePerUnit: 235235,
          quantity: 1,
          tags: [],
          listingDate: new Date(2024, 7, 17, 12, 52).toDateString()
      },
      {
          id: 4,
          item: {
            name: "Rare signed basketball",
          },
          description: "basketball signed by {insert basketball player}",
          pricePerUnit: 505.23,
          quantity: 1,
          tags: ["rare", "vintage"],
          listingDate: new Date(2024, 7, 27, 18, 24).toDateString()
      },
      {
          id: 5,
          item: {
            name: "random item",
          },
          description: "this item is random or something",
          pricePerUnit: 12.69,
          quantity: 7,
          tags: ["unused", "real", "tag", "tag2", "tagggg"],
          listingDate: new Date(2023, 11, 2, 1, 3).toDateString()
      },
      {
          id: 6,
          item: {
            name: "more random item with a very long name",
          },
          description: "this item is even more random than the last",
          pricePerUnit: 1.35,
          quantity: 10,
          tags: ["tag123", "tag321311"],
          listingDate: new Date(2023, 7, 17, 12, 52).toDateString()
      },
      {
          id: 7,
          item: {
            name: "no tags",
          },
          description: "tagsss",
          pricePerUnit: 235235,
          quantity: 1,
          tags: [],
          listingDate: new Date(2024, 7, 17, 12, 52).toDateString()
      }
  ]


export default function Home() {
    const [listings, setListings] = useState([]);

    // useEffect(() => {
    //     search();
    // })
    const handleKeyDown = (e) => {
        if(e.key === "Enter") {
            search();
        }
    }
  return (
    <div id="home-page-wrapper">
        <div id="search-controls-wrapper">
            <div id="search-controls">
                <input onKeyDown={handleKeyDown} id="search-bar" placeholder="Search" />
                <button onClick={search} id="search-button">S</button>
            </div>
        </div>
        <div className="listings-wrapper">  
            <div className="listings">
            {listings ?
                listings.map(l => <Listing listing={l} key={l.id} />)
            :
                <></>
            }
            </div>
        </div>
    </div>
    )


  async function search() {
    // implement backend call later

    // fake search
    setListings(fakeListings)
  }
}