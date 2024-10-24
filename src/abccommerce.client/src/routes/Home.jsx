import './Home.css';
import { useEffect, useState } from 'react';
import Listing from '../components/Listing';
import searchImg from '../assets/search.svg';
import PageSelector from '../components/PageSelector';

export default function Home() {
    const [listings, setListings] = useState([]);
    const [query, setQuery] = useState('');
    const [searchInput, setSearchInput] = useState('')
    const [pageNumber, setPageNumber] = useState(1);

    // max 50
    const listingsPerPage = 24;
    useEffect(() => {
        search(query, pageNumber - 1, listingsPerPage);
    }, [query, pageNumber])
    const handleKeyDown = (e) => {
        if(e.key === "Enter") {
            setQuery(searchInput);
        }
    }
  return (
    <div id="home-page-wrapper">
        <div id="search-controls-wrapper">
            <div id="search-controls">
                <input onKeyDown={handleKeyDown} id="search-bar" placeholder="Search" onChange={e => setSearchInput(e.target.value)}/>
                <button onClick={() => setQuery(searchInput)} id="search-button">
                  <img id="search-img" src={searchImg} />
                </button>
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
        <PageSelector width={200} height={50} handlePageChange={(page) => setPageNumber(page)}/>
    </div>
    )


  async function search(query, step, count) {
    if(query === '' || typeof query === undefined) return;
    try {
      let response = await fetch(`http://localhost:5147/search?q=${query}&skip=${(step) * count}&count=${count}`);
      setListings(await response.json());
    } catch(error) {
      console.error(error);
    }
    
  }
}