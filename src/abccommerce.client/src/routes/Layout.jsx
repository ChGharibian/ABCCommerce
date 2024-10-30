import { Outlet, Link } from "react-router-dom";
import './Layout.css';
import { useCookies } from 'react-cookie';
import Login from './Login'
import Logout from "../components/Logout";
export default function Layout() {
  const [cookies] = useCookies(['userToken']);
  return (
    <>
      <nav className="navbar">
        <ul>
          <li>
            <Link to="/">Home</Link>
          </li>
          {(!cookies.userToken && cookies.userToken !=="") && 
            <li>
              <Link to="/Login">Login</Link>
            </li>
          }
          <li>
            <Link to="/registration">Registration</Link>
          </li>
          <li>
            <Link to="/seller">Seller</Link>
          </li>
          {(cookies.userToken && cookies.userToken !== "") &&
            <li>
              <Logout/>
            </li>
          }
          <li>
            <Link to="/wronglink">wronglink</Link>
          </li>
        </ul>
      </nav>

      <hr />

      {/* An <Outlet> renders whatever child route is currently active,
          so you can think about this <Outlet> as a placeholder for
          the child routes we defined above. */}
      <Outlet />
    </>
  )
}