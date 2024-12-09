import { useState, useEffect } from 'react';
import { useCookies } from 'react-cookie';
import { CurrencyUtil } from '../util/currency';
import { useNavigate } from 'react-router-dom';
import './cart.css';

/**
 * 
 * @category route
 * @function Cart
 * @author Angel Cortes
 * @since November 12
 * @description Displays the items that a user has in their cart along with their prices/total price. Allows user to proceed
 * to checkout.
 * @returns {JSX.Element} Cart page
 */
export default function Cart() {

  const [cartItems, setCartItems] = useState([]);
  const [totalPrice, setTotalPrice] = useState(0);
  const [cookies] = useCookies(['userToken']);
  const navigate = useNavigate();

  //when component is rerendered rerender cart list
  //useEffect will be used
  useEffect(() => {
    //define function to request cart info from back end
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
          
          const totalAmount = data.reduce((a,currentListing)=>{
            a+= (currentListing.listing.pricePerUnit*currentListing.quantity);
            return a
          },0)

          //setting total price
          setTotalPrice(totalAmount);
          
        } else {
          console.log(data.error || "Authentication failed");
        }
        
      } catch(error){
        console.error('Fetch error', error);
      }
    }

    //invoke function
    getCartData();
  }, []);

  
  const handleButtonClick = () => {
    navigate('/checkout')
  }

  //map the list of cartItems from the Cart
    //each list will be displayed as a row
    //
  return (
    <div id="cart-page-wrapper">
      <table>
        <thead>
          <tr>
            <th scope="col">Image</th>
            <th scope="col">Name</th>
            <th scope="col">Description</th>
            <th scope="col">Quantity</th>
            <th scope="col">Cost</th>
          </tr>
        </thead>
          <tbody>
            {cartItems.map((currentListing) => (
              <tr key={currentListing.id}>
                <td data-label="Image">
                  <img src={currentListing.listing.images[0]}/>
                </td>
                <td data-label="Name">{currentListing.listing.name}</td>
                <td data-label="Description">{currentListing.listing.description}</td>
                <td data-label="Quantity">{currentListing.quantity}</td>
                <td data-label="Cost">{CurrencyUtil.getDollarString(currentListing.listing.pricePerUnit*currentListing.quantity)}</td>
              </tr>
            ))}
          </tbody>
          <tfoot>
            <tr>
              <th scope="row" colSpan="4">Total Cost</th>
              <td>{CurrencyUtil.getDollarString(totalPrice)}</td>
            </tr>
          </tfoot>
          
      </table>
      <button onClick={handleButtonClick} style={{backgroundColor: 'white'}}>Proceed to Checkout</button>
    </div>
  )
}