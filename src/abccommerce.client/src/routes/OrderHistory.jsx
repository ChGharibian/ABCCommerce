import { useState, useEffect } from 'react';
import { useCookies } from 'react-cookie';
export default function OrderHistory(){

  const [transactions, setTransactions] = useState([]);


  //upon navigation get request
  useEffect( () => {

    const getCartData = async () => {
      try {
        let response = await fetch("http://localhost:5147/transaction", {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${cookies.userToken}`,
            "Content-Type": "application/json"
          }
        })
  
        const data = await response.json();
        console.log(data);
        if(response.ok) {
          setTransactions(data)  
          console.log('we in'); 
        } else {
          console.log(data.error || "Authentication failed");
        }
        
      } catch(error){
        console.error('Fetch error', error);
      }
    }
    getCartData();
  })

  return (
    <div id="cart-page-wrapper">
      <table>
        <thead>
          <tr>
            <th scope="col">Purchase Date</th>
            <th scope="col">Total Cost</th>
            <th scope="col">Items purchased</th>
          </tr>
        </thead>
          <tbody>
            {transactions.map((currentTransaction) => (
              <tr key={currentTransaction.id}>
                <td data-label="Purchase Date">{currentTransaction.purchaseDate}</td>
                <td data-label="Total Cost">{currentTransaction.items.map((currentItem)=>{
                    <p>{currentItem.item.name +':' + currentItem.quantity}</p>
                  }
                )}</td>
                <td data-label="Items">{currentTransaction.quantity}</td>
              </tr>
            ))}
          </tbody>
      </table>
    </div>
  )
}