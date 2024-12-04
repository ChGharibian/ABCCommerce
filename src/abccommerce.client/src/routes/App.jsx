import { Routes, Route } from "react-router-dom";
import Login from "./Login";
//import ErrorPage from "../error-page";
import Layout from "./Layout";
import Registration from "./Registration";
import Home from "./Home";
import Cart from "./Cart";
import WrongLink from "./WrongLink";
import Seller from "./Seller";
import ListingPage from "./ListingPage";
import AddListing from "./AddListing";
import '../App.css'
import '../variables.css';
import Checkout from "./Checkout";
/**
 * @global
 * @function App
 * @author Angel Cortes
 * @since September 30
 * @description Handles routing within the frontend, serving the correct 
 * pages for each route.
 * @returns {JSX.Element} Router
 */
export default function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Home/>}/>
        <Route path="login" element={<Login/>}/>
        <Route path="registration" element={<Registration/>}/>
        <Route path="seller/:sellerId" element={<Seller/>}/>
        <Route path="seller/:sellerId/addlisting" element={<AddListing />} />
        <Route path="seller/:sellerId/listing/:listingId" element={<ListingPage />} />
        <Route path="cart" element={<Cart/>}/>
        <Route path="checkout" element={<Checkout/>}/>
        <Route path="*" element={<WrongLink/>}/>
        <Route/>
      </Route>
    </Routes>
  )
} 