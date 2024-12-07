

//just be sure the input has a name element
export default function generalHandleChange(setStateFunc, charLimit= 0) {
  let handleChange;
  if(charLimit) {
    handleChange = (event) => {
      const {name, value } = event.target;
      console.log(value);
      if(value.length <= charLimit) {
        setStateFunc( (prevState) => ({
          ...prevState,
          [name]: value,
        }))
      }
      // else{
      //   setStateFunc( (prevState) => ({
      //     ...prevState
      //   }))
      // }
    }
  }
  else{
    handleChange = (event) => {
      const { name, value } = event.target;
      setStateFunc( (prevState) => ({
        ...prevState,
        [name]: value,
      }))
    }
  }
  return handleChange;
}