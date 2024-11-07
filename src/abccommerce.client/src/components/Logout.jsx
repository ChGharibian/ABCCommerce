import { useCookies } from 'react-cookie';
import { useNavigate } from 'react-router-dom';


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