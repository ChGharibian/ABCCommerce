import { Outlet} from "react-router-dom";

import Navbar from '../components/Navbar';
/**
 * @global
 * @function Layout
 * @author Angel Cortes
 * @since September 30
 * @description Provides the layout for the entire site, making sure
 * the navbar is always displayed.
 * @returns {JSX.Element}
 */
export default function Layout() {
  return (
    <>
      <Navbar />

      {/* An <Outlet> renders whatever child route is currently active,
          so you can think about this <Outlet> as a placeholder for
          the child routes we defined above. */}
      <Outlet />
    </>
  )
}