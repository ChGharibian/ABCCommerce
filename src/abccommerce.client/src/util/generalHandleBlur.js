
export default function generalHandleBlur(validationFunction, innerFunc) {

  return function handleBlur(event){
    const {name, value} = event.target;

    //check if value is valid is input and returns error
    const error = validationFunction(value);

    //update error state
    innerFunc((prevErrState) => ({
      ...prevErrState,
      [name]: error,
    }))
  }
}