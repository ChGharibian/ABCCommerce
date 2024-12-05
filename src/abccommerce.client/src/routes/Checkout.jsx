import { useEffect, useState } from "react"
import { useCookies } from 'react-cookie';
import Input from "../components/Input";
import './Checkout.css';

export default function Checkout(){

  const [cartItems, setCartItems] = useState([]);
  const [paymentInfo, setPaymentInfo] = useState({
    cardNumber: '',
    expirationDate: '',
    cvv: '',
  })
  const [cookies] = useCookies(['userToken']);
  

  useEffect(()=> {
      
    const getCartData = async () => {
      try {
        let response = await fetch("http://localhost:5147/cart", {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${cookies.userToken}`,
            "Content-Type": "application/json"
          }
        })
      const data = await response.json(); 
      console.log(data);
      if(response.ok) {
        setCartItems(data) 
      } }catch(error) {
        console.error('Error getting cart data:', error);
      }
    }
    
    getCartData();
  }, []);

  return (
    <div id="checkout-wrapper">
      <form action="checkout-Form">
        <h2>Checkout</h2>
        {/* <Input
        label="CardNumber"
        type="text"
        placeholder="cardnumber"
        onChange={handleChange}
        onBlur={handleBlur}
        required 
        />
        <Input
        label="Month"
        type="text"
        placeholder="carcardnumber"
        onChange={handleChange}
        onBlur={handleBlur}
        required 
        />
        <Input
        label="Year"
        type
        /> */}
      </form>
    </div>
  )
}