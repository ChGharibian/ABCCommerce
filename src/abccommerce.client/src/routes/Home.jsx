import './Home.css';
import { useEffect, useState } from 'react';
import Listing from '../components/Listing';
import searchImg from '../assets/search.svg';
import PageSelector from '../components/PageSelector';
import { handlePageChange } from '../util/paging';
import { useCookies } from 'react-cookie';

/**
 * @category route
 * @function Home
 * @author Thomas Scott
 * @since October 9
 * @description Displays the home page, allowing users to use the 
 * search bar to search for listings.
 * @returns {JSX.Element} Home page
 */
export default function Home() {
    const [listings, setListings] = useState([]);
    const [query, setQuery] = useState('');
    const [searchInput, setSearchInput] = useState('')
    const [pageNumber, setPageNumber] = useState(1);
    const [loading, setLoading] = useState(false);
    const [searched, setSearched] = useState(false);
    const [cookies] = useCookies(['seller']);
    // const [prevSearch, setPrevSearch] = useState('');

    // max 50
    const listingsPerPage = 24;

    useEffect(() => {
        search(query, pageNumber - 1, listingsPerPage);
    }, [query, pageNumber])

    const handleKeyDown = (e) => {
        if(e.key === "Enter") {
            handleSearchSubmit();
        }
    }

    const handleSearchSubmit = () => {
      setQuery(searchInput.trim());
      setPageNumber(1);
    }

    async function search(query, step, count) {
      if(query === '' || typeof query === undefined) return;
      if(!searched) setSearched(true);
      try {
        setLoading(true);
        let response = await fetch(`http://localhost:5147/search?q=${query}&skip=${(step) * count}&count=${count}`);
        setListings(await response.json());
        setLoading(false);
      } catch(error) {
        console.error(error);
      }
      
    }

  return (
    <div id="home-page-wrapper">
        <div id="search-controls-wrapper">
            <div id="search-controls">
                <input onKeyDown={handleKeyDown} id="search-bar" placeholder="Search" onChange={e => setSearchInput(e.target.value)}/>
                <button onClick={handleSearchSubmit} id="search-button">
                  <img id="search-img" src={searchImg} />
                </button>
            </div>
        </div>
        <div className="listings-wrapper">  
            <div className="listings">
            {
              loading ?
                <p className="no-listings">Loading</p>
              :
              listings?.length > 0 ?
                listings.map(l => <Listing listing={l} key={l.id} editable={Number(cookies.seller) === Number(l.item.seller.id)} />)
              : 
              listings?.length === 0 && searched ?
                pageNumber > 1 ?
                  <p className="no-listings">No items on this page</p>
                :
                  <p className="no-listings">No results for {query}</p>
              :  
                <></>
            }
            </div>
        </div>
        {
        (listings?.length > 0 || listings?.length === 0 && searched && pageNumber > 1) &&
          <PageSelector width={160} height={40} 
          handlePageChange={(page) => handlePageChange(page, pageNumber, listings, setPageNumber)}
          page={pageNumber}/>
        }
    </div>
    )

}