import { Outlet} from "react-router-dom";

import Navbar from '../components/Navbar';

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