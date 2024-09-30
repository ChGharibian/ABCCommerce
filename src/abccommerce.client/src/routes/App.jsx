import { Routes, Route } from "react-router-dom";
import Login from "./Login";
//import ErrorPage from "../error-page";
import Layout from "./Layout";
import Registration from "./Registration";
import Home from "./Home";
import WrongLink from "./WrongLink";
import Seller from "./Seller";

export default function App() {
  return (
    
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Home/>}/>
        <Route path="login" element={<Login/>}/>
        <Route path="registration" element={<Registration/>}/>
        <Route path="seller" element={<Seller/>}/>
        <Route path="*" element={<WrongLink/>}/>
        <Route/>
      </Route>
    </Routes>
  )
} 