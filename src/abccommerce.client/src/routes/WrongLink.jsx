import './WrongLink.css';
/**
 * @category route
 * @function WrongLink
 * @author Angel Cortes
 * @since September 30
 * @description Displays a message letting the user know
 * that the current route they are on is invalid.
 * @returns {JSX.Element} Wrong link page
 */
export default function WrongLink(){
  return(
    <div id="wrong-link-wrapper">
      <h1>Page not found</h1>
    </div>
  )
}