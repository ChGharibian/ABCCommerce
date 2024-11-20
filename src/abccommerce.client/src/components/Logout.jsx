import { useCookies } from 'react-cookie';
import { useNavigate } from 'react-router-dom';

/**
 * @category component
 * @function Logout
 * @author Angel Cortes
 * @since October 29
 * @description Displays a button that removes a client's userToken and refreshToken cookies,
 * logging them out of their account and redirecting them to the login page.
 * @returns {JSX.Element}
 */
export default function Logout(){
  const [, , removeCookie] = useCookies(['token', 'refreshToken']);
  const navigate = useNavigate();

  const handleClick = () => {
    removeCookie('userToken', { path: "/" });
    removeCookie('refreshToken', { path: '/'});
    navigate('/login');
  }
  return (
      <a onClick={handleClick}>Logout</a>
  )
}