import './Navbar.css';
import { Link } from 'react-router-dom';
import { useCookies } from 'react-cookie';
import Logout from './Logout';
/**
 * @category component
 * @function Navbar
 * @author Angel Cortes
 * @description Displays a navbar that allows navigation through the site,
 * dynamically changing based on whether or not a client is logged in.
 * @since September 30
 * @returns {JSX.Element}
 */
export default function Navbar() {
    const [cookies] = useCookies(['userToken']);
    return (
        <nav className="navbar">
        <ul>
          <li>
            <Link to="/">Home</Link>
          </li>
          {(!cookies.userToken && cookies.userToken !=="") && 
          <>
            <li>
              <Link to="/Login">Login</Link>
            </li>
            <li>
                <Link to="/registration">Register</Link>
            </li>
          </>
          }
          {(cookies.userToken && cookies.userToken !== "") &&
            <>
              <li>
                <Logout/>
              </li>
              <li>
                <Link to="/cart">Cart</Link>
              </li>
            </>
          }
        </ul>
      </nav>
    )
}