import { useState, useEffect } from 'react';
import { useCookies } from 'react-cookie';
import './cart.css';

export default function Cart() {

  const [cartItems, setCartItems] = useState([]);
  const [totalPrice, setTotalPrice] = useState(0);
  const [cookies] = useCookies(['userToken']);


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
          console.log(totalAmount);

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

  
    //send request to the backend for the 
  

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
                  <img src={currentListing}/>
                </td>
                <td data-label="Name">{currentListing.listing.item.name}</td>
                <td data-label="Description">Description</td>
                <td data-label="Quantity">{currentListing.quantity}</td>
                <td data-label="Cost">{currentListing.listing.pricePerUnit*currentListing.quantity}</td>
              </tr>
            ))}
          </tbody>
          <tfoot>
            <tr>
              <th scope="row" colSpan="4">Total Cost</th>
              <td>{totalPrice}</td>
            </tr>
          </tfoot>
          
      </table>
      <button style={{backgroundColor: 'white'}}>Proceed to Checkout</button>
    </div>
  )
}