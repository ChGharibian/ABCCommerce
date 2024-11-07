import './Navbar.css';
import { Link } from 'react-router-dom';
import { useCookies } from 'react-cookie';
import Logout from './Logout';
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
            <li>
              <Logout/>
            </li>
          }
        </ul>
      </nav>
    )
}