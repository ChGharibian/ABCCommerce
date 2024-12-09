import { useEffect, useState } from "react"
import { useCookies } from 'react-cookie';
import Input from "../components/Input";
import generalHandleChange from '../util/generalHandleChange';
import generalHandleBlur from '../util/generalHandleBlur'
import { ValidationUtil } from "../util/validation";
import './Checkout.css';
import ListingTable from "../components/listingTable";
import { CurrencyUtil } from "../util/currency";


export default function Checkout(){

  const [cartItems, setCartItems] = useState([]);
  const [totalPrice, setTotalPrice] = useState(0);
  const [delivDate, setDelivDate] = useState('');
  const [paymentInfo, setPaymentInfo] = useState({
    cardNumber: '',
    expirationDate: '',
    SecurityCode: '',
  })
  const [mailingAddr, setMailingAddr] = useState({
    street: '',
    streetPlus: '',
    city: '',
    state: '',
    zip: '',
  })

  const [errors, setErrors] = useState({
    cardNumber:'',
    expirationDate:'',
    SecurityCode: '',
    street: '',
    city: '',
    state: '',
    zip: '',

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
        const totalAmount = data.reduce((a,currentListing)=>{
          a+= (currentListing.listing.pricePerUnit*currentListing.quantity);
          return a
        },0)

        setTotalPrice(totalAmount);
      } }catch(error) {
        console.error('Error getting cart data:', error);
      }
    }
    
    getCartData();
    setDelivDate(getDeleveiryDate(4));
  }, []);

  const handleSubmit = async e => {
      e.preventDefault()

      function correctPaymentFormat(expireString,cardState){
        const monthSt = expireString.slice(0,2)
        const yearSt = expireString.slice(3,5)

        const month = parseInt(monthSt, 10);
        const year = parseInt('20' + yearSt, 10);

        const returnedState = {...cardState};
        delete returnedState.expirationDate;

        returnedState['year'] = year;
        returnedState['month'] = month;

        return returnedState;
      }
      const formatedPaymentState = correctPaymentFormat(paymentInfo.expirationDate, paymentInfo);

      try {
        let response = await fetch("http://localhost:5147/user/login", {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON. stringify(formatedPaymentState)
      });
  }

  const expireHandleChange = (element) => {
    const { name, value } = element.target;

    // Validate the input
    const validationResult = ValidationUtil.validateExpirDateOnChange(value);

    if (validationResult === true) {
      // Remove any non-numeric characters except '/'
      let cleanedValue = value.replace(/[^\d]/g, '');

      // Add a slash after the first two digits
      if (cleanedValue.length > 2) {
        cleanedValue = cleanedValue.slice(0, 2) + '/' + cleanedValue.slice(2);
      }

      // Update the state with the formatted value
      setPaymentInfo((prevState) => ({
        ...prevState,
        [name]: cleanedValue,
      }));
    }

  };

  const getDeleveiryDate = (daysToDeliver) => {
    const delivDate = new Date();
    delivDate.setDate(delivDate.getDate() + daysToDeliver);

    const formatter = new Intl.DateTimeFormat('en-US', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
    const readableDate = formatter.format(delivDate);
    return readableDate;
  }
  


  return (
    <div id="checkout-wrapper">
      <form action="checkout-Form">
        <h2>Checkout</h2>
        <h2>Mailing Address</h2>
        <Input
        label="Address Line 1:"
        name="street"
        type="text"
        value={mailingAddr.street}
        placeholder="Street address"
        onChange={generalHandleChange(setMailingAddr)}
        onBlur={generalHandleBlur(ValidationUtil.validateCreditCard, setErrors)}
        required
        error={errors.street} 
        />
        <Input
        label="Address Line 2:"
        name="streetPlus"
        type="text"
        value={mailingAddr.streetPlus}
        placeholder="Apt,suite,unit,building,floor,etc."
        onChange={generalHandleChange(setMailingAddr)}
        onBlur={generalHandleBlur(ValidationUtil.validateCreditCard, setErrors)}
        />
        <Input
        label="City:"
        name="city"
        type="text"
        value={mailingAddr.city}
        placeholder="City Name"
        onChange={generalHandleChange(setMailingAddr)}
        onBlur={generalHandleBlur(ValidationUtil.validateCreditCard, setErrors)}
        required
        error={errors.city} 
        />
        <Input
        label="State:"
        name="state"
        type="text"
        value={mailingAddr.state}
        placeholder="State"
        onChange={generalHandleChange(setMailingAddr)}
        onBlur={generalHandleBlur(ValidationUtil.validateCreditCard, setErrors)}
        required
        error={errors.state} 
        />
        <Input
        label="ZIP Code:"
        name="zip"
        type="number"
        value={mailingAddr.zip}
        placeholder="5 Digit Zip Code"
        onChange={generalHandleChange(setMailingAddr)}
        onBlur={generalHandleBlur(ValidationUtil.validateCreditCard, setErrors)}
        required
        error={errors.zip} 
        />
        <h2>Credit Card Information</h2>
        <Input
        label="Credit CardNumber"
        name="cardNumber"
        type="text"
        value={paymentInfo.cardNumber}
        placeholder="cardnumber"
        onChange={generalHandleChange(setPaymentInfo)}
        onBlur={generalHandleBlur(ValidationUtil.validateCreditCard, setErrors)}
        required
        error={errors.cardNumber} 
        />
        <Input
        label="Expiration Date:"
        name="expirationDate"
        type="text"
        value={paymentInfo.expirationDate}
        placeholder="MM/YY"
        onChange={expireHandleChange}
        onBlur={generalHandleBlur(ValidationUtil.validateExpirDateOnBlur, setErrors)}
        required 
        error={errors.expirationDate}
        />
        <Input
        label="CCV"
        name="SecurityCode"
        type="number"
        value={paymentInfo.SecurityCode}
        placeholder="000"
        onChange={generalHandleChange(setPaymentInfo,4)}

        maxLength={2}
        />
        <h2>Delevery Date:</h2>
        <h2>{delivDate}</h2>
        <ListingTable
          cartItems={cartItems}
          totalPrice={totalPrice}
          CurrencyUtil={CurrencyUtil}
        />
      </form>
    </div>
  )
}