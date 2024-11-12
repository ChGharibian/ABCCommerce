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
import '../App.css'
import '../variables.css';

export default function App() {
  return (
    
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Home/>}/>
        <Route path="login" element={<Login/>}/>
        <Route path="registration" element={<Registration/>}/>
        <Route path="seller/:sellerId" element={<Seller/>}/>
        <Route path="seller/:sellerId/listing/:listingId" element={<ListingPage />} />
        <Route path="cart" element={<Cart/>}/>
        <Route path="*" element={<WrongLink/>}/>
        <Route/>
      </Route>
    </Routes>
  )
} 