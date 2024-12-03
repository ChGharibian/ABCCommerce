import { useState, useEffect } from "react";
import { useCookies } from "react-cookie";
import { UserUtil } from "../util/users";
import Select from "./Select";
/**
 * @category component
 * @function SellerSelector
 * @description Specialized Select element for selecting between different sellers a user is associated with.
 * @author Thomas Scott
 * @since November 19
 * @returns {JSX.Element}
 */
export default function SellerSelector() {
    const [sellers, setSellers] = useState([]);
    const [cookies, setCookie] = useCookies(['userToken', 'seller']);
    useEffect(() => {
        manageSellerData();
      }, [cookies.userToken])
  
      async function manageSellerData() {
        let data = await UserUtil.getSellerData(cookies.userToken);
        if(data) {
          setSellers(data);
          setCookie('seller', data[0].id ?? "", {path: "/"});
        } else {
          setSellers([]);
        }
        
      }
    return (
        <Select options={[sellers.map(s => s.name)]} values={sellers.map(s => s.id)} onChange={(value) => setCookie('seller', value)} />
    )
}